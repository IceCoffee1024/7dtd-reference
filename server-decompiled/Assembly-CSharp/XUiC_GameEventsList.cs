using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F46 RID: 3910
[Preserve]
public class XUiC_GameEventsList : XUiC_List<XUiC_GameEventsList.GameEventEntry>
{
	// Token: 0x17000E6A RID: 3690
	// (get) Token: 0x06007A55 RID: 31317 RVA: 0x002E4F17 File Offset: 0x002E3117
	// (set) Token: 0x06007A56 RID: 31318 RVA: 0x002E4F1F File Offset: 0x002E311F
	public string Category
	{
		get
		{
			return this.category;
		}
		set
		{
			this.category = value;
			this.RebuildList(false);
		}
	}

	// Token: 0x06007A57 RID: 31319 RVA: 0x002E4F30 File Offset: 0x002E3130
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		foreach (KeyValuePair<string, GameEventActionSequence> keyValuePair in GameEventManager.GameEventSequences)
		{
			if (keyValuePair.Value.AllowUserTrigger && (this.category == "" || (keyValuePair.Value.CategoryNames != null && keyValuePair.Value.CategoryNames.ContainsCaseInsensitive(this.category))))
			{
				this.allEntries.Add(new XUiC_GameEventsList.GameEventEntry(keyValuePair.Key));
			}
		}
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x06007A58 RID: 31320 RVA: 0x002E4FF8 File Offset: 0x002E31F8
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x04005AB5 RID: 23221
	[PublicizedFrom(EAccessModifier.Private)]
	public string category = "";

	// Token: 0x02000F47 RID: 3911
	[Preserve]
	public class GameEventEntry : XUiListEntry<XUiC_GameEventsList.GameEventEntry>
	{
		// Token: 0x06007A5A RID: 31322 RVA: 0x002E5027 File Offset: 0x002E3227
		public GameEventEntry(string _name)
		{
			this.name = _name;
		}

		// Token: 0x06007A5B RID: 31323 RVA: 0x002E5036 File Offset: 0x002E3236
		public override int CompareTo(XUiC_GameEventsList.GameEventEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.name, _otherEntry.name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06007A5C RID: 31324 RVA: 0x002E504F File Offset: 0x002E324F
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005AB6 RID: 23222
		public readonly string name;

		// Token: 0x04005AB7 RID: 23223
		public readonly string camelCase;
	}

	// Token: 0x02000F48 RID: 3912
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_GameEventsList.GameEventEntry>.XUiC_ListEntry
	{
		// Token: 0x06007A5D RID: 31325 RVA: 0x002E5064 File Offset: 0x002E3264
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_GameEventsList.GameEventEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.name : null) ?? "";
		}
	}
}
