using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020005FE RID: 1534
[Preserve]
public class ItemClassModifier : ItemClass
{
	// Token: 0x06003247 RID: 12871 RVA: 0x001459FC File Offset: 0x00143BFC
	public static ItemClassModifier GetItemModWithAnyTags(FastTags<TagGroup.Global> tags, FastTags<TagGroup.Global> installedModTypes, GameRandom random)
	{
		for (int i = 0; i < ItemClass.list.Length; i++)
		{
			ItemClassModifier itemClassModifier = ItemClass.list[i] as ItemClassModifier;
			if (itemClassModifier != null && !itemClassModifier.HasAnyTags(installedModTypes) && itemClassModifier.InstallableTags.Test_AnySet(tags) && !itemClassModifier.DisallowedTags.Test_AnySet(tags))
			{
				ItemClassModifier.modIds.Add(itemClassModifier.Id);
			}
		}
		if (ItemClassModifier.modIds.Count == 0)
		{
			return null;
		}
		ItemClassModifier itemClassModifier2 = ItemClass.GetForId(ItemClassModifier.modIds[random.RandomRange(ItemClassModifier.modIds.Count)]) as ItemClassModifier;
		ItemClassModifier.modIds.Clear();
		return itemClassModifier2;
	}

	// Token: 0x06003248 RID: 12872 RVA: 0x00145A9C File Offset: 0x00143C9C
	public override bool HasAnyTags(FastTags<TagGroup.Global> _tags)
	{
		return this.ModifierTags.Test_AnySet(_tags);
	}

	// Token: 0x06003249 RID: 12873 RVA: 0x00145AAA File Offset: 0x00143CAA
	public override bool HasAllTags(FastTags<TagGroup.Global> _tags)
	{
		return this.ModifierTags.Test_AllSet(_tags);
	}

	// Token: 0x0600324A RID: 12874 RVA: 0x00145AB8 File Offset: 0x00143CB8
	public static ItemClassModifier GetCosmeticItemMod(FastTags<TagGroup.Global> itemTags, FastTags<TagGroup.Global> installedModTypes, GameRandom random)
	{
		bool isEmpty = installedModTypes.IsEmpty;
		for (int i = 0; i < ItemClass.list.Length; i++)
		{
			ItemClassModifier itemClassModifier = ItemClass.list[i] as ItemClassModifier;
			if (itemClassModifier != null && (isEmpty || !itemClassModifier.HasAnyTags(installedModTypes)) && itemClassModifier.HasAnyTags(ItemClassModifier.CosmeticModTypes) && itemClassModifier.InstallableTags.Test_AnySet(itemTags) && !itemClassModifier.DisallowedTags.Test_AnySet(itemTags) && random.RandomFloat <= itemClassModifier.CosmeticInstallChance)
			{
				ItemClassModifier.modIds.Add(itemClassModifier.Id);
			}
		}
		if (ItemClassModifier.modIds.Count == 0)
		{
			return null;
		}
		ItemClassModifier itemClassModifier2 = ItemClass.GetForId(ItemClassModifier.modIds[random.RandomRange(ItemClassModifier.modIds.Count)]) as ItemClassModifier;
		ItemClassModifier.modIds.Clear();
		return itemClassModifier2;
	}

	// Token: 0x0600324B RID: 12875 RVA: 0x00145B80 File Offset: 0x00143D80
	public static ItemClassModifier GetDesiredItemModWithAnyTags(FastTags<TagGroup.Global> tags, FastTags<TagGroup.Global> installedModTypes, FastTags<TagGroup.Global> desiredModTypes, GameRandom random)
	{
		bool isEmpty = installedModTypes.IsEmpty;
		bool isEmpty2 = desiredModTypes.IsEmpty;
		for (int i = 0; i < ItemClass.list.Length; i++)
		{
			ItemClassModifier itemClassModifier = ItemClass.list[i] as ItemClassModifier;
			if (itemClassModifier != null && (isEmpty || !itemClassModifier.HasAnyTags(installedModTypes)) && (isEmpty2 || itemClassModifier.HasAnyTags(desiredModTypes)) && itemClassModifier.InstallableTags.Test_AnySet(tags) && !itemClassModifier.DisallowedTags.Test_AnySet(tags))
			{
				ItemClassModifier.modIds.Add(itemClassModifier.Id);
			}
		}
		if (ItemClassModifier.modIds.Count == 0)
		{
			return null;
		}
		ItemClassModifier itemClassModifier2 = ItemClass.GetForId(ItemClassModifier.modIds[random.RandomRange(ItemClassModifier.modIds.Count)]) as ItemClassModifier;
		ItemClassModifier.modIds.Clear();
		return itemClassModifier2;
	}

	// Token: 0x0600324C RID: 12876 RVA: 0x00145C40 File Offset: 0x00143E40
	public bool GetPropertyOverride(string _propertyName, string _itemName, ref string _value)
	{
		if (this.PropertyOverrides.ContainsKey(_itemName) && this.PropertyOverrides[_itemName].Values.ContainsKey(_propertyName))
		{
			_value = this.PropertyOverrides[_itemName].Values[_propertyName];
			return true;
		}
		if (this.PropertyOverrides.ContainsKey("*") && this.PropertyOverrides["*"].Values.ContainsKey(_propertyName))
		{
			_value = this.PropertyOverrides["*"].Values[_propertyName];
			return true;
		}
		return false;
	}

	// Token: 0x04002786 RID: 10118
	public static ItemClassModifier[] modList = new ItemClassModifier[1000];

	// Token: 0x04002787 RID: 10119
	public FastTags<TagGroup.Global> InstallableTags;

	// Token: 0x04002788 RID: 10120
	public FastTags<TagGroup.Global> DisallowedTags;

	// Token: 0x04002789 RID: 10121
	public FastTags<TagGroup.Global> ModifierTags;

	// Token: 0x0400278A RID: 10122
	public ItemClassModifier.ModifierTypes Type;

	// Token: 0x0400278B RID: 10123
	public Dictionary<string, DynamicProperties> PropertyOverrides;

	// Token: 0x0400278C RID: 10124
	public float CosmeticInstallChance;

	// Token: 0x0400278D RID: 10125
	public static FastTags<TagGroup.Global> CosmeticModTypes = FastTags<TagGroup.Global>.Parse("dye,nametag,charm");

	// Token: 0x0400278E RID: 10126
	public static FastTags<TagGroup.Global> CosmeticItemTags = FastTags<TagGroup.Global>.Parse("canHaveCosmetic");

	// Token: 0x0400278F RID: 10127
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<int> modIds = new List<int>();

	// Token: 0x020005FF RID: 1535
	public enum ModifierTypes
	{
		// Token: 0x04002791 RID: 10129
		Mod,
		// Token: 0x04002792 RID: 10130
		Attachment
	}
}
