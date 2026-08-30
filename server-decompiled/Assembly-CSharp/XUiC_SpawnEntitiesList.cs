using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200106F RID: 4207
[Preserve]
public class XUiC_SpawnEntitiesList : XUiC_List<XUiC_SpawnEntitiesList.SpawnEntityEntry>
{
	// Token: 0x060085C6 RID: 34246 RVA: 0x003306B4 File Offset: 0x0032E8B4
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

	// Token: 0x060085C7 RID: 34247 RVA: 0x00330754 File Offset: 0x0032E954
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x02001070 RID: 4208
	[Preserve]
	public class SpawnEntityEntry : XUiListEntry<XUiC_SpawnEntitiesList.SpawnEntityEntry>
	{
		// Token: 0x060085C9 RID: 34249 RVA: 0x00330778 File Offset: 0x0032E978
		public SpawnEntityEntry(string _name, int _key)
		{
			this.name = _name;
			this.key = _key;
			this.camelCase = this.name.SeparateCamelCase();
		}

		// Token: 0x060085CA RID: 34250 RVA: 0x0033079F File Offset: 0x0032E99F
		public override int CompareTo(XUiC_SpawnEntitiesList.SpawnEntityEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.name, _otherEntry.name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060085CB RID: 34251 RVA: 0x003307B8 File Offset: 0x0032E9B8
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04006371 RID: 25457
		public readonly string name;

		// Token: 0x04006372 RID: 25458
		public readonly int key;

		// Token: 0x04006373 RID: 25459
		public readonly string camelCase;
	}

	// Token: 0x02001071 RID: 4209
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_SpawnEntitiesList.SpawnEntityEntry>.XUiC_ListEntry
	{
		// Token: 0x060085CC RID: 34252 RVA: 0x003307CD File Offset: 0x0032E9CD
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_SpawnEntitiesList.SpawnEntityEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.name : null) ?? "";
		}
	}
}
