using System;
using UnityEngine.Scripting;

// Token: 0x02000E80 RID: 3712
[Preserve]
public class XUiC_StringList : XUiC_List<XUiC_StringList.Entry>
{
	// Token: 0x06007399 RID: 29593 RVA: 0x002BC257 File Offset: 0x002BA457
	public void ClearList()
	{
		this.allEntries.Clear();
	}

	// Token: 0x0600739A RID: 29594 RVA: 0x002BC264 File Offset: 0x002BA464
	public void AddEntry(object _entry, object _tag = null)
	{
		this.allEntries.Add(new XUiC_StringList.Entry(this, _entry, _tag));
	}

	// Token: 0x0600739B RID: 29595 RVA: 0x002BC279 File Offset: 0x002BA479
	public void SortList()
	{
		this.allEntries.Sort();
	}

	// Token: 0x0600739C RID: 29596 RVA: 0x002BC288 File Offset: 0x002BA488
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

	// Token: 0x0600739D RID: 29597 RVA: 0x002BC2C8 File Offset: 0x002BA4C8
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

	// Token: 0x040055DC RID: 21980
	public StringComparison SortingMode = StringComparison.OrdinalIgnoreCase;

	// Token: 0x02000E81 RID: 3713
	[Preserve]
	public class Entry : XUiListEntry<XUiC_StringList.Entry>
	{
		// Token: 0x0600739F RID: 29599 RVA: 0x002BC321 File Offset: 0x002BA521
		public Entry(XUiC_StringList _parentList, object _data, object _tag)
		{
			this.parentList = _parentList;
			this.Data = _data;
			this.Tag = _tag;
			this.DataString = this.Data.ToString();
		}

		// Token: 0x060073A0 RID: 29600 RVA: 0x002BC34F File Offset: 0x002BA54F
		public override int CompareTo(XUiC_StringList.Entry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.DataString, _otherEntry.DataString, this.parentList.SortingMode);
		}

		// Token: 0x060073A1 RID: 29601 RVA: 0x002BC372 File Offset: 0x002BA572
		public override bool MatchesSearch(string _searchString)
		{
			return this.DataString.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040055DD RID: 21981
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_StringList parentList;

		// Token: 0x040055DE RID: 21982
		public readonly object Data;

		// Token: 0x040055DF RID: 21983
		public readonly string DataString;

		// Token: 0x040055E0 RID: 21984
		public readonly object Tag;
	}

	// Token: 0x02000E82 RID: 3714
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_StringList.Entry>.XUiC_ListEntry
	{
		// Token: 0x060073A2 RID: 29602 RVA: 0x002BC387 File Offset: 0x002BA587
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_StringList.Entry entryData = this.entryData;
			return ((entryData != null) ? entryData.DataString : null) ?? "";
		}
	}
}
