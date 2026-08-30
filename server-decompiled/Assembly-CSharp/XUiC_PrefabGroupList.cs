using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FB1 RID: 4017
[Preserve]
public class XUiC_PrefabGroupList : XUiC_List<XUiC_PrefabGroupList.PrefabGroupEntry>
{
	// Token: 0x06007EC1 RID: 32449 RVA: 0x00301E9C File Offset: 0x0030009C
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

	// Token: 0x06007EC2 RID: 32450 RVA: 0x00301F88 File Offset: 0x00300188
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

	// Token: 0x06007EC3 RID: 32451 RVA: 0x00302054 File Offset: 0x00300254
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

	// Token: 0x04005DE6 RID: 24038
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<string> groupsResult = new List<string>();

	// Token: 0x02000FB2 RID: 4018
	[Preserve]
	public class PrefabGroupEntry : XUiListEntry<XUiC_PrefabGroupList.PrefabGroupEntry>
	{
		// Token: 0x06007EC5 RID: 32453 RVA: 0x003020B6 File Offset: 0x003002B6
		public PrefabGroupEntry(string _name, string _filterString)
		{
			this.name = _name;
			this.filterString = _filterString;
		}

		// Token: 0x06007EC6 RID: 32454 RVA: 0x003020CC File Offset: 0x003002CC
		public override int CompareTo(XUiC_PrefabGroupList.PrefabGroupEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.name, _otherEntry.name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06007EC7 RID: 32455 RVA: 0x003020E5 File Offset: 0x003002E5
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005DE7 RID: 24039
		public readonly string name;

		// Token: 0x04005DE8 RID: 24040
		public readonly string filterString;
	}

	// Token: 0x02000FB3 RID: 4019
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabGroupList.PrefabGroupEntry>.XUiC_ListEntry
	{
		// Token: 0x06007EC8 RID: 32456 RVA: 0x003020FA File Offset: 0x003002FA
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PrefabGroupList.PrefabGroupEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.name : null) ?? "";
		}
	}
}
