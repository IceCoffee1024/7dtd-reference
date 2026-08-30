using System;
using UnityEngine.Scripting;

// Token: 0x02000E95 RID: 3733
[Preserve]
public class XUiC_StringList : XUiC_List<XUiC_StringList.Entry>
{
	// Token: 0x06007439 RID: 29753 RVA: 0x002BF783 File Offset: 0x002BD983
	public void ClearList()
	{
		this.allEntries.Clear();
	}

	// Token: 0x0600743A RID: 29754 RVA: 0x002BF790 File Offset: 0x002BD990
	public void AddEntry(object _entry, object _tag = null)
	{
		this.allEntries.Add(new XUiC_StringList.Entry(this, _entry, _tag));
	}

	// Token: 0x0600743B RID: 29755 RVA: 0x002BF7A5 File Offset: 0x002BD9A5
	public void SortList()
	{
		this.allEntries.Sort();
	}

	// Token: 0x0600743C RID: 29756 RVA: 0x002BF7B4 File Offset: 0x002BD9B4
	public bool SelectByString(string _toSelect)
	{
		int num = this.filteredEntries.FindIndex((XUiC_StringList.Entry _entry) => _entry.DataString.Equals(_toSelect));
		if (num < 0)
		{
			return false;
		}
		base.SelectedEntryIndex = num;
		return true;
	}

	// Token: 0x0600743D RID: 29757 RVA: 0x002BF7F4 File Offset: 0x002BD9F4
	public bool SelectByTag(object _tag)
	{
		if (_tag == null)
		{
			return false;
		}
		int num = this.filteredEntries.FindIndex((XUiC_StringList.Entry _entry) => _tag.Equals(_entry.Tag));
		if (num < 0)
		{
			return false;
		}
		base.SelectedEntryIndex = num;
		return true;
	}

	// Token: 0x04005680 RID: 22144
	public StringComparison SortingMode = StringComparison.OrdinalIgnoreCase;

	// Token: 0x02000E96 RID: 3734
	[Preserve]
	public class Entry : XUiListEntry<XUiC_StringList.Entry>
	{
		// Token: 0x0600743F RID: 29759 RVA: 0x002BF84D File Offset: 0x002BDA4D
		public Entry(XUiC_StringList _parentList, object _data, object _tag)
		{
			this.parentList = _parentList;
			this.Data = _data;
			this.Tag = _tag;
			this.DataString = this.Data.ToString();
		}

		// Token: 0x06007440 RID: 29760 RVA: 0x002BF87B File Offset: 0x002BDA7B
		public override int CompareTo(XUiC_StringList.Entry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.DataString, _otherEntry.DataString, this.parentList.SortingMode);
		}

		// Token: 0x06007441 RID: 29761 RVA: 0x002BF89E File Offset: 0x002BDA9E
		public override bool MatchesSearch(string _searchString)
		{
			return this.DataString.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005681 RID: 22145
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_StringList parentList;

		// Token: 0x04005682 RID: 22146
		public readonly object Data;

		// Token: 0x04005683 RID: 22147
		public readonly string DataString;

		// Token: 0x04005684 RID: 22148
		public readonly object Tag;
	}

	// Token: 0x02000E97 RID: 3735
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_StringList.Entry>.XUiC_ListEntry
	{
		// Token: 0x06007442 RID: 29762 RVA: 0x002BF8B3 File Offset: 0x002BDAB3
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_StringList.Entry entryData = this.entryData;
			return ((entryData != null) ? entryData.DataString : null) ?? "";
		}
	}
}
