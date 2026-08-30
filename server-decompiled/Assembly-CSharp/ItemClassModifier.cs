using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020005F5 RID: 1525
[Preserve]
public class ItemClassModifier : ItemClass
{
	// Token: 0x060031B5 RID: 12725 RVA: 0x001431E0 File Offset: 0x001413E0
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

	// Token: 0x060031B6 RID: 12726 RVA: 0x00143280 File Offset: 0x00141480
	public override bool HasAnyTags(FastTags<TagGroup.Global> _tags)
	{
		return this.ModifierTags.Test_AnySet(_tags);
	}

	// Token: 0x060031B7 RID: 12727 RVA: 0x0014328E File Offset: 0x0014148E
	public override bool HasAllTags(FastTags<TagGroup.Global> _tags)
	{
		return this.ModifierTags.Test_AllSet(_tags);
	}

	// Token: 0x060031B8 RID: 12728 RVA: 0x0014329C File Offset: 0x0014149C
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

	// Token: 0x060031B9 RID: 12729 RVA: 0x00143364 File Offset: 0x00141564
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

	// Token: 0x060031BA RID: 12730 RVA: 0x00143424 File Offset: 0x00141624
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

	// Token: 0x0400272C RID: 10028
	public static ItemClassModifier[] modList = new ItemClassModifier[1000];

	// Token: 0x0400272D RID: 10029
	public FastTags<TagGroup.Global> InstallableTags;

	// Token: 0x0400272E RID: 10030
	public FastTags<TagGroup.Global> DisallowedTags;

	// Token: 0x0400272F RID: 10031
	public FastTags<TagGroup.Global> ModifierTags;

	// Token: 0x04002730 RID: 10032
	public ItemClassModifier.ModifierTypes Type;

	// Token: 0x04002731 RID: 10033
	public Dictionary<string, DynamicProperties> PropertyOverrides;

	// Token: 0x04002732 RID: 10034
	public float CosmeticInstallChance;

	// Token: 0x04002733 RID: 10035
	public static FastTags<TagGroup.Global> CosmeticModTypes = FastTags<TagGroup.Global>.Parse("dye,nametag,charm");

	// Token: 0x04002734 RID: 10036
	public static FastTags<TagGroup.Global> CosmeticItemTags = FastTags<TagGroup.Global>.Parse("canHaveCosmetic");

	// Token: 0x04002735 RID: 10037
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<int> modIds = new List<int>();

	// Token: 0x020005F6 RID: 1526
	public enum ModifierTypes
	{
		// Token: 0x04002737 RID: 10039
		Mod,
		// Token: 0x04002738 RID: 10040
		Attachment
	}
}
