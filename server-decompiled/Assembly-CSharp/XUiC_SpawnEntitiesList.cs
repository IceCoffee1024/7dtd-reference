using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02001080 RID: 4224
[Preserve]
public class XUiC_SpawnEntitiesList : XUiC_List<XUiC_SpawnEntitiesList.SpawnEntityEntry>
{
	// Token: 0x0600860C RID: 34316 RVA: 0x003342C0 File Offset: 0x003324C0
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

	// Token: 0x0600860D RID: 34317 RVA: 0x00334360 File Offset: 0x00332560
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x02001081 RID: 4225
	[Preserve]
	public class SpawnEntityEntry : XUiListEntry<XUiC_SpawnEntitiesList.SpawnEntityEntry>
	{
		// Token: 0x0600860F RID: 34319 RVA: 0x00334384 File Offset: 0x00332584
		public SpawnEntityEntry(string _name, int _key)
		{
			this.name = _name;
			this.key = _key;
			this.camelCase = this.name.SeparateCamelCase();
		}

		// Token: 0x06008610 RID: 34320 RVA: 0x003343AB File Offset: 0x003325AB
		public override int CompareTo(XUiC_SpawnEntitiesList.SpawnEntityEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			return string.Compare(this.name, _otherEntry.name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06008611 RID: 34321 RVA: 0x003343C4 File Offset: 0x003325C4
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040063B6 RID: 25526
		public readonly string name;

		// Token: 0x040063B7 RID: 25527
		public readonly int key;

		// Token: 0x040063B8 RID: 25528
		public readonly string camelCase;
	}

	// Token: 0x02001082 RID: 4226
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_SpawnEntitiesList.SpawnEntityEntry>.XUiC_ListEntry
	{
		// Token: 0x06008612 RID: 34322 RVA: 0x003343D9 File Offset: 0x003325D9
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_SpawnEntitiesList.SpawnEntityEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.name : null) ?? "";
		}
	}
}
