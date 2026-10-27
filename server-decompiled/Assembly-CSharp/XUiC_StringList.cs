using System;
using UnityEngine.Scripting;

// Token: 0x02000EAF RID: 3759
[Preserve]
public class XUiC_StringList : XUiC_List<XUiC_StringList.Entry>
{
	// Token: 0x06007456 RID: 29782 RVA: 0x002C409F File Offset: 0x002C229F
	public void ClearList()
	{
		this.allEntries.Clear();
	}

	// Token: 0x06007457 RID: 29783 RVA: 0x002C40AC File Offset: 0x002C22AC
	public void AddEntry(object _entry, object _tag = null)
	{
		this.allEntries.Add(new XUiC_StringList.Entry(this, _entry, _tag));
	}

	// Token: 0x06007458 RID: 29784 RVA: 0x002C40C1 File Offset: 0x002C22C1
	public void SortList()
	{
		this.allEntries.Sort();
	}

	// Token: 0x06007459 RID: 29785 RVA: 0x002C40D0 File Offset: 0x002C22D0
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

	// Token: 0x0600745A RID: 29786 RVA: 0x002C4110 File Offset: 0x002C2310
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

	// Token: 0x040056DC RID: 22236
	public StringComparison SortingMode = StringComparison.OrdinalIgnoreCase;

	// Token: 0x02000EB0 RID: 3760
	[Preserve]
	public class Entry : XUiListEntry<XUiC_StringList.Entry>
	{
		// Token: 0x0600745C RID: 29788 RVA: 0x002C4169 File Offset: 0x002C2369
		public Entry(XUiC_StringList _parentList, object _data, object _tag)
		{
			this.parentList = _parentList;
			this.Data = _data;
			this.Tag = _tag;
			this.DataString = this.Data.ToString();
		}

		// Token: 0x0600745D RID: 29789 RVA: 0x002C4197 File Offset: 0x002C2397
		public override int CompareTo(XUiC_StringList.Entry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.DataString, _otherEntry.DataString, this.parentList.SortingMode);
		}

		// Token: 0x0600745E RID: 29790 RVA: 0x002C41BA File Offset: 0x002C23BA
		public override bool MatchesSearch(string _searchString)
		{
			return this.DataString.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040056DD RID: 22237
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_StringList parentList;

		// Token: 0x040056DE RID: 22238
		public readonly object Data;

		// Token: 0x040056DF RID: 22239
		public readonly string DataString;

		// Token: 0x040056E0 RID: 22240
		public readonly object Tag;
	}

	// Token: 0x02000EB1 RID: 3761
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_StringList.Entry>.XUiC_ListEntry
	{
		// Token: 0x0600745F RID: 29791 RVA: 0x002C41CF File Offset: 0x002C23CF
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_StringList.Entry entryData = this.entryData;
			return ((entryData != null) ? entryData.DataString : null) ?? "";
		}
	}
}
