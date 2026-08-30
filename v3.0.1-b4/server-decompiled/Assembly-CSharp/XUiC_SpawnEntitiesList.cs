using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02001056 RID: 4182
[Preserve]
public class XUiC_SpawnEntitiesList : XUiC_List<XUiC_SpawnEntitiesList.SpawnEntityEntry>
{
	// Token: 0x060084D4 RID: 34004 RVA: 0x0032BE8C File Offset: 0x0032A08C
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		foreach (KeyValuePair<int, EntityClass> keyValuePair in EntityClass.list.Dict)
		{
			if (keyValuePair.Value.userSpawnType == EntityClass.UserSpawnType.Menu)
			{
				this.allEntries.Add(new XUiC_SpawnEntitiesList.SpawnEntityEntry(keyValuePair.Value.entityClassName, keyValuePair.Key));
			}
		}
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x060084D5 RID: 34005 RVA: 0x0032BF2C File Offset: 0x0032A12C
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x02001057 RID: 4183
	[Preserve]
	public class SpawnEntityEntry : XUiListEntry<XUiC_SpawnEntitiesList.SpawnEntityEntry>
	{
		// Token: 0x060084D7 RID: 34007 RVA: 0x0032BF50 File Offset: 0x0032A150
		public SpawnEntityEntry(string _name, int _key)
		{
			this.name = _name;
			this.key = _key;
			this.camelCase = this.name.SeparateCamelCase();
		}

		// Token: 0x060084D8 RID: 34008 RVA: 0x0032BF77 File Offset: 0x0032A177
		public override int CompareTo(XUiC_SpawnEntitiesList.SpawnEntityEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.name, _otherEntry.name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060084D9 RID: 34009 RVA: 0x0032BF90 File Offset: 0x0032A190
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040062AA RID: 25258
		public readonly string name;

		// Token: 0x040062AB RID: 25259
		public readonly int key;

		// Token: 0x040062AC RID: 25260
		public readonly string camelCase;
	}

	// Token: 0x02001058 RID: 4184
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_SpawnEntitiesList.SpawnEntityEntry>.XUiC_ListEntry
	{
		// Token: 0x060084DA RID: 34010 RVA: 0x0032BFA5 File Offset: 0x0032A1A5
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_SpawnEntitiesList.SpawnEntityEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.name : null) ?? "";
		}
	}
}
