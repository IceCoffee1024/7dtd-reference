using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F55 RID: 3925
[Preserve]
public class XUiC_GameEventsList : XUiC_List<XUiC_GameEventsList.GameEventEntry>
{
	// Token: 0x17000E74 RID: 3700
	// (get) Token: 0x06007A98 RID: 31384 RVA: 0x002E6807 File Offset: 0x002E4A07
	// (set) Token: 0x06007A99 RID: 31385 RVA: 0x002E680F File Offset: 0x002E4A0F
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

	// Token: 0x06007A9A RID: 31386 RVA: 0x002E6820 File Offset: 0x002E4A20
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

	// Token: 0x06007A9B RID: 31387 RVA: 0x002E68E8 File Offset: 0x002E4AE8
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x04005AE8 RID: 23272
	[PublicizedFrom(EAccessModifier.Private)]
	public string category = "";

	// Token: 0x02000F56 RID: 3926
	[Preserve]
	public class GameEventEntry : XUiListEntry<XUiC_GameEventsList.GameEventEntry>
	{
		// Token: 0x06007A9D RID: 31389 RVA: 0x002E6917 File Offset: 0x002E4B17
		public GameEventEntry(string _name)
		{
			this.name = _name;
		}

		// Token: 0x06007A9E RID: 31390 RVA: 0x002E6926 File Offset: 0x002E4B26
		public override int CompareTo(XUiC_GameEventsList.GameEventEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.name, _otherEntry.name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06007A9F RID: 31391 RVA: 0x002E693F File Offset: 0x002E4B3F
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005AE9 RID: 23273
		public readonly string name;

		// Token: 0x04005AEA RID: 23274
		public readonly string camelCase;
	}

	// Token: 0x02000F57 RID: 3927
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_GameEventsList.GameEventEntry>.XUiC_ListEntry
	{
		// Token: 0x06007AA0 RID: 31392 RVA: 0x002E6954 File Offset: 0x002E4B54
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_GameEventsList.GameEventEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.name : null) ?? "";
		}
	}
}
