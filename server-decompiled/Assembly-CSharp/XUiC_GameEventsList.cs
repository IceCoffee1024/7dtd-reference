using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F20 RID: 3872
[Preserve]
public class XUiC_GameEventsList : XUiC_List<XUiC_GameEventsList.GameEventEntry>
{
	// Token: 0x17000E32 RID: 3634
	// (get) Token: 0x0600799F RID: 31135 RVA: 0x002DDBF3 File Offset: 0x002DBDF3
	// (set) Token: 0x060079A0 RID: 31136 RVA: 0x002DDBFB File Offset: 0x002DBDFB
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

	// Token: 0x060079A1 RID: 31137 RVA: 0x002DDC0C File Offset: 0x002DBE0C
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

	// Token: 0x060079A2 RID: 31138 RVA: 0x002DDCD4 File Offset: 0x002DBED4
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x040059CA RID: 22986
	[PublicizedFrom(EAccessModifier.Private)]
	public string category = "";

	// Token: 0x02000F21 RID: 3873
	[Preserve]
	public class GameEventEntry : XUiListEntry<XUiC_GameEventsList.GameEventEntry>
	{
		// Token: 0x060079A4 RID: 31140 RVA: 0x002DDD03 File Offset: 0x002DBF03
		public GameEventEntry(string _name)
		{
			this.name = _name;
		}

		// Token: 0x060079A5 RID: 31141 RVA: 0x002DDD12 File Offset: 0x002DBF12
		public override int CompareTo(XUiC_GameEventsList.GameEventEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.name, _otherEntry.name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060079A6 RID: 31142 RVA: 0x002DDD2B File Offset: 0x002DBF2B
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040059CB RID: 22987
		public readonly string name;

		// Token: 0x040059CC RID: 22988
		public readonly string camelCase;
	}

	// Token: 0x02000F22 RID: 3874
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_GameEventsList.GameEventEntry>.XUiC_ListEntry
	{
		// Token: 0x060079A7 RID: 31143 RVA: 0x002DDD40 File Offset: 0x002DBF40
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_GameEventsList.GameEventEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.name : null) ?? "";
		}
	}
}
