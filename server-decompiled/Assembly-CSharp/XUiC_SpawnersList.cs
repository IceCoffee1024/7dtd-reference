using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02001093 RID: 4243
[Preserve]
public class XUiC_SpawnersList : XUiC_List<XUiC_SpawnersList.SpawnerEntry>
{
	// Token: 0x06008667 RID: 34407 RVA: 0x00336484 File Offset: 0x00334684
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

	// Token: 0x02001094 RID: 4244
	[Preserve]
	public class SpawnerEntry : XUiListEntry<XUiC_SpawnersList.SpawnerEntry>
	{
		// Token: 0x06008669 RID: 34409 RVA: 0x00336510 File Offset: 0x00334710
		public SpawnerEntry(string _name)
		{
			this.name = _name;
			this.displayName = GameStageGroup.MakeDisplayName(_name);
		}

		// Token: 0x0600866A RID: 34410 RVA: 0x0033652B File Offset: 0x0033472B
		public override int CompareTo(XUiC_SpawnersList.SpawnerEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.name, _otherEntry.name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600866B RID: 34411 RVA: 0x00336544 File Offset: 0x00334744
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040063F8 RID: 25592
		public readonly string name;

		// Token: 0x040063F9 RID: 25593
		public readonly string displayName;
	}

	// Token: 0x02001095 RID: 4245
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_SpawnersList.SpawnerEntry>.XUiC_ListEntry
	{
		// Token: 0x0600866C RID: 34412 RVA: 0x00336559 File Offset: 0x00334759
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_SpawnersList.SpawnerEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.displayName : null) ?? "";
		}
	}
}
