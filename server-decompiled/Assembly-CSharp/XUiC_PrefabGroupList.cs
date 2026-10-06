using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FD9 RID: 4057
[Preserve]
public class XUiC_PrefabGroupList : XUiC_List<XUiC_PrefabGroupList.PrefabGroupEntry>
{
	// Token: 0x06007FC2 RID: 32706 RVA: 0x00309648 File Offset: 0x00307848
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

	// Token: 0x06007FC3 RID: 32707 RVA: 0x00309734 File Offset: 0x00307934
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

	// Token: 0x06007FC4 RID: 32708 RVA: 0x00309800 File Offset: 0x00307A00
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

	// Token: 0x04005ED6 RID: 24278
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<string> groupsResult = new List<string>();

	// Token: 0x02000FDA RID: 4058
	[Preserve]
	public class PrefabGroupEntry : XUiListEntry<XUiC_PrefabGroupList.PrefabGroupEntry>
	{
		// Token: 0x06007FC6 RID: 32710 RVA: 0x00309862 File Offset: 0x00307A62
		public PrefabGroupEntry(string _name, string _filterString)
		{
			this.name = _name;
			this.filterString = _filterString;
		}

		// Token: 0x06007FC7 RID: 32711 RVA: 0x00309878 File Offset: 0x00307A78
		public override int CompareTo(XUiC_PrefabGroupList.PrefabGroupEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.name, _otherEntry.name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06007FC8 RID: 32712 RVA: 0x00309891 File Offset: 0x00307A91
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005ED7 RID: 24279
		public readonly string name;

		// Token: 0x04005ED8 RID: 24280
		public readonly string filterString;
	}

	// Token: 0x02000FDB RID: 4059
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabGroupList.PrefabGroupEntry>.XUiC_ListEntry
	{
		// Token: 0x06007FC9 RID: 32713 RVA: 0x003098A6 File Offset: 0x00307AA6
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PrefabGroupList.PrefabGroupEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.name : null) ?? "";
		}
	}
}
