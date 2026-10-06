using System;
using UnityEngine.Scripting;

// Token: 0x02000EA4 RID: 3748
[Preserve]
public class XUiC_StringList : XUiC_List<XUiC_StringList.Entry>
{
	// Token: 0x06007431 RID: 29745 RVA: 0x002C2B33 File Offset: 0x002C0D33
	public void ClearList()
	{
		this.allEntries.Clear();
	}

	// Token: 0x06007432 RID: 29746 RVA: 0x002C2B40 File Offset: 0x002C0D40
	public void AddEntry(object _entry, object _tag = null)
	{
		this.allEntries.Add(new XUiC_StringList.Entry(this, _entry, _tag));
	}

	// Token: 0x06007433 RID: 29747 RVA: 0x002C2B55 File Offset: 0x002C0D55
	public void SortList()
	{
		this.allEntries.Sort();
	}

	// Token: 0x06007434 RID: 29748 RVA: 0x002C2B64 File Offset: 0x002C0D64
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

	// Token: 0x06007435 RID: 29749 RVA: 0x002C2BA4 File Offset: 0x002C0DA4
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

	// Token: 0x040056B7 RID: 22199
	public StringComparison SortingMode = StringComparison.OrdinalIgnoreCase;

	// Token: 0x02000EA5 RID: 3749
	[Preserve]
	public class Entry : XUiListEntry<XUiC_StringList.Entry>
	{
		// Token: 0x06007437 RID: 29751 RVA: 0x002C2BFD File Offset: 0x002C0DFD
		public Entry(XUiC_StringList _parentList, object _data, object _tag)
		{
			this.parentList = _parentList;
			this.Data = _data;
			this.Tag = _tag;
			this.DataString = this.Data.ToString();
		}

		// Token: 0x06007438 RID: 29752 RVA: 0x002C2C2B File Offset: 0x002C0E2B
		public override int CompareTo(XUiC_StringList.Entry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.DataString, _otherEntry.DataString, this.parentList.SortingMode);
		}

		// Token: 0x06007439 RID: 29753 RVA: 0x002C2C4E File Offset: 0x002C0E4E
		public override bool MatchesSearch(string _searchString)
		{
			return this.DataString.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040056B8 RID: 22200
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_StringList parentList;

		// Token: 0x040056B9 RID: 22201
		public readonly object Data;

		// Token: 0x040056BA RID: 22202
		public readonly string DataString;

		// Token: 0x040056BB RID: 22203
		public readonly object Tag;
	}

	// Token: 0x02000EA6 RID: 3750
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_StringList.Entry>.XUiC_ListEntry
	{
		// Token: 0x0600743A RID: 29754 RVA: 0x002C2C63 File Offset: 0x002C0E63
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_StringList.Entry entryData = this.entryData;
			return ((entryData != null) ? entryData.DataString : null) ?? "";
		}
	}
}
