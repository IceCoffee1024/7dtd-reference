using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02001083 RID: 4227
[Preserve]
public class XUiC_SpawnersList : XUiC_List<XUiC_SpawnersList.SpawnerEntry>
{
	// Token: 0x06008614 RID: 34324 RVA: 0x00334400 File Offset: 0x00332600
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

	// Token: 0x02001084 RID: 4228
	[Preserve]
	public class SpawnerEntry : XUiListEntry<XUiC_SpawnersList.SpawnerEntry>
	{
		// Token: 0x06008616 RID: 34326 RVA: 0x0033448C File Offset: 0x0033268C
		public SpawnerEntry(string _name)
		{
			this.name = _name;
			this.displayName = GameStageGroup.MakeDisplayName(_name);
		}

		// Token: 0x06008617 RID: 34327 RVA: 0x003344A7 File Offset: 0x003326A7
		public override int CompareTo(XUiC_SpawnersList.SpawnerEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.name, _otherEntry.name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06008618 RID: 34328 RVA: 0x003344C0 File Offset: 0x003326C0
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040063B9 RID: 25529
		public readonly string name;

		// Token: 0x040063BA RID: 25530
		public readonly string displayName;
	}

	// Token: 0x02001085 RID: 4229
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_SpawnersList.SpawnerEntry>.XUiC_ListEntry
	{
		// Token: 0x06008619 RID: 34329 RVA: 0x003344D5 File Offset: 0x003326D5
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_SpawnersList.SpawnerEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.displayName : null) ?? "";
		}
	}
}
