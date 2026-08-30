using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02001072 RID: 4210
[Preserve]
public class XUiC_SpawnersList : XUiC_List<XUiC_SpawnersList.SpawnerEntry>
{
	// Token: 0x060085CE RID: 34254 RVA: 0x003307F4 File Offset: 0x0032E9F4
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		foreach (KeyValuePair<string, GameStageGroup> keyValuePair in GameStageGroup.Groups)
		{
			string key = keyValuePair.Key;
			this.allEntries.Add(new XUiC_SpawnersList.SpawnerEntry(key));
		}
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x02001073 RID: 4211
	[Preserve]
	public class SpawnerEntry : XUiListEntry<XUiC_SpawnersList.SpawnerEntry>
	{
		// Token: 0x060085D0 RID: 34256 RVA: 0x00330880 File Offset: 0x0032EA80
		public SpawnerEntry(string _name)
		{
			this.name = _name;
			this.displayName = GameStageGroup.MakeDisplayName(_name);
		}

		// Token: 0x060085D1 RID: 34257 RVA: 0x0033089B File Offset: 0x0032EA9B
		public override int CompareTo(XUiC_SpawnersList.SpawnerEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.name, _otherEntry.name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060085D2 RID: 34258 RVA: 0x003308B4 File Offset: 0x0032EAB4
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04006374 RID: 25460
		public readonly string name;

		// Token: 0x04006375 RID: 25461
		public readonly string displayName;
	}

	// Token: 0x02001074 RID: 4212
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_SpawnersList.SpawnerEntry>.XUiC_ListEntry
	{
		// Token: 0x060085D3 RID: 34259 RVA: 0x003308C9 File Offset: 0x0032EAC9
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_SpawnersList.SpawnerEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.displayName : null) ?? "";
		}
	}
}
