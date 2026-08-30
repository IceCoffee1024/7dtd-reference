using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FCA RID: 4042
[Preserve]
public class XUiC_PrefabGroupList : XUiC_List<XUiC_PrefabGroupList.PrefabGroupEntry>
{
	// Token: 0x06007FB3 RID: 32691 RVA: 0x00306670 File Offset: 0x00304870
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

	// Token: 0x06007FB4 RID: 32692 RVA: 0x0030675C File Offset: 0x0030495C
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

	// Token: 0x06007FB5 RID: 32693 RVA: 0x00306828 File Offset: 0x00304A28
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

	// Token: 0x04005EAD RID: 24237
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<string> groupsResult = new List<string>();

	// Token: 0x02000FCB RID: 4043
	[Preserve]
	public class PrefabGroupEntry : XUiListEntry<XUiC_PrefabGroupList.PrefabGroupEntry>
	{
		// Token: 0x06007FB7 RID: 32695 RVA: 0x0030688A File Offset: 0x00304A8A
		public PrefabGroupEntry(string _name, string _filterString)
		{
			this.name = _name;
			this.filterString = _filterString;
		}

		// Token: 0x06007FB8 RID: 32696 RVA: 0x003068A0 File Offset: 0x00304AA0
		public override int CompareTo(XUiC_PrefabGroupList.PrefabGroupEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.name, _otherEntry.name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06007FB9 RID: 32697 RVA: 0x003068B9 File Offset: 0x00304AB9
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005EAE RID: 24238
		public readonly string name;

		// Token: 0x04005EAF RID: 24239
		public readonly string filterString;
	}

	// Token: 0x02000FCC RID: 4044
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PrefabGroupList.PrefabGroupEntry>.XUiC_ListEntry
	{
		// Token: 0x06007FBA RID: 32698 RVA: 0x003068CE File Offset: 0x00304ACE
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PrefabGroupList.PrefabGroupEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.name : null) ?? "";
		}
	}
}
