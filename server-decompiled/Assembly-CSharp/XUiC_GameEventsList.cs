using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F37 RID: 3895
[Preserve]
public class XUiC_GameEventsList : XUiC_List<XUiC_GameEventsList.GameEventEntry>
{
	// Token: 0x17000E4D RID: 3661
	// (get) Token: 0x06007A58 RID: 31320 RVA: 0x002E1BA7 File Offset: 0x002DFDA7
	// (set) Token: 0x06007A59 RID: 31321 RVA: 0x002E1BAF File Offset: 0x002DFDAF
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

	// Token: 0x06007A5A RID: 31322 RVA: 0x002E1BC0 File Offset: 0x002DFDC0
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

	// Token: 0x06007A5B RID: 31323 RVA: 0x002E1C88 File Offset: 0x002DFE88
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x04005A7D RID: 23165
	[PublicizedFrom(EAccessModifier.Private)]
	public string category = "";

	// Token: 0x02000F38 RID: 3896
	[Preserve]
	public class GameEventEntry : XUiListEntry<XUiC_GameEventsList.GameEventEntry>
	{
		// Token: 0x06007A5D RID: 31325 RVA: 0x002E1CB7 File Offset: 0x002DFEB7
		public GameEventEntry(string _name)
		{
			this.name = _name;
		}

		// Token: 0x06007A5E RID: 31326 RVA: 0x002E1CC6 File Offset: 0x002DFEC6
		public override int CompareTo(XUiC_GameEventsList.GameEventEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.name, _otherEntry.name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06007A5F RID: 31327 RVA: 0x002E1CDF File Offset: 0x002DFEDF
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04005A7E RID: 23166
		public readonly string name;

		// Token: 0x04005A7F RID: 23167
		public readonly string camelCase;
	}

	// Token: 0x02000F39 RID: 3897
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_GameEventsList.GameEventEntry>.XUiC_ListEntry
	{
		// Token: 0x06007A60 RID: 31328 RVA: 0x002E1CF4 File Offset: 0x002DFEF4
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_GameEventsList.GameEventEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.name : null) ?? "";
		}
	}
}
