using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02001090 RID: 4240
[Preserve]
public class XUiC_SpawnEntitiesList : XUiC_List<XUiC_SpawnEntitiesList.SpawnEntityEntry>
{
	// Token: 0x0600865F RID: 34399 RVA: 0x00336344 File Offset: 0x00334544
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

	// Token: 0x06008660 RID: 34400 RVA: 0x003363E4 File Offset: 0x003345E4
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x02001091 RID: 4241
	[Preserve]
	public class SpawnEntityEntry : XUiListEntry<XUiC_SpawnEntitiesList.SpawnEntityEntry>
	{
		// Token: 0x06008662 RID: 34402 RVA: 0x00336408 File Offset: 0x00334608
		public SpawnEntityEntry(string _name, int _key)
		{
			this.name = _name;
			this.key = _key;
			this.camelCase = this.name.SeparateCamelCase();
		}

		// Token: 0x06008663 RID: 34403 RVA: 0x0033642F File Offset: 0x0033462F
		public override int CompareTo(XUiC_SpawnEntitiesList.SpawnEntityEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.name, _otherEntry.name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06008664 RID: 34404 RVA: 0x00336448 File Offset: 0x00334648
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040063F5 RID: 25589
		public readonly string name;

		// Token: 0x040063F6 RID: 25590
		public readonly int key;

		// Token: 0x040063F7 RID: 25591
		public readonly string camelCase;
	}

	// Token: 0x02001092 RID: 4242
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_SpawnEntitiesList.SpawnEntityEntry>.XUiC_ListEntry
	{
		// Token: 0x06008665 RID: 34405 RVA: 0x0033645D File Offset: 0x0033465D
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_SpawnEntitiesList.SpawnEntityEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.name : null) ?? "";
		}
	}
}
