using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FE9 RID: 4073
[Preserve]
public class XUiC_PrefabGroupList : XUiC_List<XUiC_PrefabGroupList.PrefabGroupEntry>
{
	// Token: 0x06008011 RID: 32785 RVA: 0x0030B164 File Offset: 0x00309364
	public override void OnOpen()
	{
		base.OnOpen();
		bool flag = false;
		this.groupsResult.Clear();
		PrefabEditModeManager.Instance.GetAllGroups(this.groupsResult, null);
		foreach (string text in this.groupsResult)
		{
			bool flag2 = false;
			using (List<XUiC_PrefabGroupList.PrefabGroupEntry>.Enumerator enumerator2 = this.allEntries.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.name == text)
					{
						flag2 = true;
						break;
					}
				}
			}
			if (!flag2)
			{
				this.allEntries.Add(new XUiC_PrefabGroupList.PrefabGroupEntry(text, text));
				flag = true;
			}
		}
		if (flag)
		{
			this.allEntries.Sort();
			this.RefreshView(false, true);
		}
	}

	// Token: 0x06008012 RID: 32786 RVA: 0x0030B250 File Offset: 0x00309450
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		this.groupsResult.Clear();
		PrefabEditModeManager.Instance.GetAllGroups(this.groupsResult, null);
		foreach (string text in this.groupsResult)
		{
			this.allEntries.Add(new XUiC_PrefabGroupList.PrefabGroupEntry(text, text));
		}
		this.allEntries.Sort();
		this.allEntries.Insert(0, new XUiC_PrefabGroupList.PrefabGroupEntry("<All>", null));
		this.allEntries.Insert(1, new XUiC_PrefabGroupList.PrefabGroupEntry("<Ungrouped>", ""));
		base.RebuildList(_resetFilter);
	}

	// Token: 0x06008013 RID: 32787 RVA: 0x0030B31C File Offset: 0x0030951C
	public bool SelectByName(string _name)
	{
		if (!string.IsNullOrEmpty(_name))
		{
			for (int i = 0; i < this.filteredEntries.Count; i++)
			{
				if (this.filteredEntries[i].name.Equals(_name, StringComparison.OrdinalIgnoreCase))
				{
					base.SelectedEntryIndex = i;
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x04005F11 RID: 24337
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<string> groupsResult = new List<string>();

	// Token: 0x02000FEA RID: 4074
	[Preserve]
	public class PrefabGroupEntry : XUiListEntry<XUiC_PrefabGroupList.PrefabGroupEntry>
	{
		// Token: 0x06008015 RID: 32789 RVA: 0x0030B37E File Offset: 0x0030957E
		public PrefabGroupEntry(string _name, string _filterString)
		{
			this.name = _name;
			this.filterString = _filterString;
		}

		// Token: 0x06008016 RID: 32790 RVA: 0x0030B394 File Offset: 0x00309594
		public override int CompareTo(XUiC_PrefabGroupList.PrefabGroupEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.name, _otherEntry.name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06008017 RID: 32791 RVA: 0x0030B3AD File Offset: 0x003095AD
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005F12 RID: 24338
		public readonly string name;

		// Token: 0x04005F13 RID: 24339
		public readonly string filterString;
	}

	// Token: 0x02000FEB RID: 4075
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabGroupList.PrefabGroupEntry>.XUiC_ListEntry
	{
		// Token: 0x06008018 RID: 32792 RVA: 0x0030B3C2 File Offset: 0x003095C2
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PrefabGroupList.PrefabGroupEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.name : null) ?? "";
		}
	}
}
