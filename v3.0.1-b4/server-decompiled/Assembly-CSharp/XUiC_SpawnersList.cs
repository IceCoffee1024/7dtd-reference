using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02001059 RID: 4185
[Preserve]
public class XUiC_SpawnersList : XUiC_List<XUiC_SpawnersList.SpawnerEntry>
{
	// Token: 0x060084DC RID: 34012 RVA: 0x0032BFCC File Offset: 0x0032A1CC
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

	// Token: 0x0200105A RID: 4186
	[Preserve]
	public class SpawnerEntry : XUiListEntry<XUiC_SpawnersList.SpawnerEntry>
	{
		// Token: 0x060084DE RID: 34014 RVA: 0x0032C058 File Offset: 0x0032A258
		public SpawnerEntry(string _name)
		{
			this.name = _name;
			this.displayName = GameStageGroup.MakeDisplayName(_name);
		}

		// Token: 0x060084DF RID: 34015 RVA: 0x0032C073 File Offset: 0x0032A273
		public override int CompareTo(XUiC_SpawnersList.SpawnerEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.name, _otherEntry.name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060084E0 RID: 34016 RVA: 0x0032C08C File Offset: 0x0032A28C
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040062AD RID: 25261
		public readonly string name;

		// Token: 0x040062AE RID: 25262
		public readonly string displayName;
	}

	// Token: 0x0200105B RID: 4187
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_SpawnersList.SpawnerEntry>.XUiC_ListEntry
	{
		// Token: 0x060084E1 RID: 34017 RVA: 0x0032C0A1 File Offset: 0x0032A2A1
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_SpawnersList.SpawnerEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.displayName : null) ?? "";
		}
	}
}
