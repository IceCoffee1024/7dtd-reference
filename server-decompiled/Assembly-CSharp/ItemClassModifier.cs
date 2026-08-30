using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020005FC RID: 1532
[Preserve]
public class ItemClassModifier : ItemClass
{
	// Token: 0x06003204 RID: 12804 RVA: 0x00145024 File Offset: 0x00143224
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

	// Token: 0x06003205 RID: 12805 RVA: 0x001450C4 File Offset: 0x001432C4
	public override bool HasAnyTags(FastTags<TagGroup.Global> _tags)
	{
		return this.ModifierTags.Test_AnySet(_tags);
	}

	// Token: 0x06003206 RID: 12806 RVA: 0x001450D2 File Offset: 0x001432D2
	public override bool HasAllTags(FastTags<TagGroup.Global> _tags)
	{
		return this.ModifierTags.Test_AllSet(_tags);
	}

	// Token: 0x06003207 RID: 12807 RVA: 0x001450E0 File Offset: 0x001432E0
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

	// Token: 0x06003208 RID: 12808 RVA: 0x001451A8 File Offset: 0x001433A8
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

	// Token: 0x06003209 RID: 12809 RVA: 0x00145268 File Offset: 0x00143468
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

	// Token: 0x0400278F RID: 10127
	public static ItemClassModifier[] modList = new ItemClassModifier[1000];

	// Token: 0x04002790 RID: 10128
	public FastTags<TagGroup.Global> InstallableTags;

	// Token: 0x04002791 RID: 10129
	public FastTags<TagGroup.Global> DisallowedTags;

	// Token: 0x04002792 RID: 10130
	public FastTags<TagGroup.Global> ModifierTags;

	// Token: 0x04002793 RID: 10131
	public ItemClassModifier.ModifierTypes Type;

	// Token: 0x04002794 RID: 10132
	public Dictionary<string, DynamicProperties> PropertyOverrides;

	// Token: 0x04002795 RID: 10133
	public float CosmeticInstallChance;

	// Token: 0x04002796 RID: 10134
	public static FastTags<TagGroup.Global> CosmeticModTypes = FastTags<TagGroup.Global>.Parse("dye,nametag,charm");

	// Token: 0x04002797 RID: 10135
	public static FastTags<TagGroup.Global> CosmeticItemTags = FastTags<TagGroup.Global>.Parse("canHaveCosmetic");

	// Token: 0x04002798 RID: 10136
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<int> modIds = new List<int>();

	// Token: 0x020005FD RID: 1533
	public enum ModifierTypes
	{
		// Token: 0x0400279A RID: 10138
		Mod,
		// Token: 0x0400279B RID: 10139
		Attachment
	}
}
