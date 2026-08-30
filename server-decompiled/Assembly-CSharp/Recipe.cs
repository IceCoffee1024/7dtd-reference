using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x020002DC RID: 732
public class Recipe
{
	// Token: 0x1700024F RID: 591
	// (get) Token: 0x0600151D RID: 5405 RVA: 0x0007F46B File Offset: 0x0007D66B
	// (set) Token: 0x0600151E RID: 5406 RVA: 0x0007F473 File Offset: 0x0007D673
	public bool IsLearnable
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600151F RID: 5407 RVA: 0x0007F47C File Offset: 0x0007D67C
	public List<ItemStack> GetIngredientsSummedUp()
	{
		return this.ingredients;
	}

	// Token: 0x06001521 RID: 5409 RVA: 0x0007F4B4 File Offset: 0x0007D6B4
	public void Init()
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < this.ingredients.Count; i++)
		{
			ItemStack itemStack = this.ingredients[i];
			if (itemStack.itemValue.ItemClass != null)
			{
				num += itemStack.itemValue.ItemClass.CraftComponentExp * (float)itemStack.count;
				num2 += itemStack.itemValue.ItemClass.CraftComponentTime * (float)itemStack.count;
			}
		}
		if (this.unlockExpGain < 0)
		{
			this.unlockExpGain = (int)(num * 2f);
		}
		if (this.craftExpGain < 0)
		{
			this.craftExpGain = (int)num;
		}
		if (this.craftingTime < 0f)
		{
			this.craftingTime = num2;
		}
		this.IsLearnable = this.tags.Test_AnySet(Recipe.LearnableRecipe);
	}

	// Token: 0x06001522 RID: 5410 RVA: 0x0007F588 File Offset: 0x0007D788
	public void AddIngredient(ItemValue _itemValue, int _count)
	{
		for (int i = 0; i < this.ingredients.Count - 1; i++)
		{
			if (this.ingredients[i].itemValue.type == _itemValue.type)
			{
				this.ingredients[i].count += _count;
				return;
			}
		}
		this.ingredients.Add(new ItemStack(_itemValue, _count));
	}

	// Token: 0x06001523 RID: 5411 RVA: 0x0007F5F7 File Offset: 0x0007D7F7
	public void AddIngredients(List<ItemStack> _items)
	{
		this.ingredients.AddRange(_items);
	}

	// Token: 0x06001524 RID: 5412 RVA: 0x0007F608 File Offset: 0x0007D808
	public string GetName()
	{
		ItemClass forId = ItemClass.GetForId(this.itemValueType);
		if (forId == null)
		{
			return string.Empty;
		}
		return forId.GetItemName();
	}

	// Token: 0x06001525 RID: 5413 RVA: 0x0007F630 File Offset: 0x0007D830
	public string GetIcon()
	{
		ItemClass forId = ItemClass.GetForId(this.itemValueType);
		if (forId == null)
		{
			return string.Empty;
		}
		return forId.GetIconName();
	}

	// Token: 0x06001526 RID: 5414 RVA: 0x0007F658 File Offset: 0x0007D858
	public bool CanCraft(IList<ItemStack> _itemStack, EntityAlive _ea = null, int _craftingTier = -1)
	{
		this.craftingTier = this.GetCraftingTier((EntityPlayer)_ea);
		if (_craftingTier != -1 && _craftingTier < this.craftingTier)
		{
			this.craftingTier = _craftingTier;
		}
		for (int i = 0; i < this.ingredients.Count; i++)
		{
			ItemStack itemStack = this.ingredients[i];
			int num = itemStack.count;
			if (this.UseIngredientModifier)
			{
				num = (int)EffectManager.GetValue(PassiveEffects.CraftingIngredientCount, null, (float)itemStack.count, _ea, this, FastTags<TagGroup.Global>.Parse(itemStack.itemValue.ItemClass.GetItemName()), true, true, true, true, true, this.craftingTier, true, false);
				if (num > 0)
				{
					num = (int)((float)num * XUiM_Recipes.GetCraftingInputModifier(this));
					num = Utils.FastMax(1, num);
				}
			}
			if (num != 0)
			{
				int num2 = 0;
				while (num > 0 && num2 < _itemStack.Count)
				{
					if ((!_itemStack[num2].itemValue.HasModSlots || !_itemStack[num2].itemValue.HasMods()) && _itemStack[num2].itemValue.type == itemStack.itemValue.type)
					{
						num -= _itemStack[num2].count;
					}
					num2++;
				}
				if (num > 0)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06001527 RID: 5415 RVA: 0x0007F784 File Offset: 0x0007D984
	public bool CanCraftAny(IList<ItemStack> _itemStack, EntityAlive _ea = null)
	{
		for (int i = this.GetCraftingTier((EntityPlayer)_ea); i >= 0; i--)
		{
			bool flag = true;
			for (int j = 0; j < this.ingredients.Count; j++)
			{
				ItemStack itemStack = this.ingredients[j];
				int num = itemStack.count;
				if (this.UseIngredientModifier)
				{
					num = (int)EffectManager.GetValue(PassiveEffects.CraftingIngredientCount, null, (float)itemStack.count, _ea, this, FastTags<TagGroup.Global>.Parse(itemStack.itemValue.ItemClass.GetItemName()), true, true, true, true, true, i, true, false);
					if (num > 0)
					{
						num = (int)((float)num * XUiM_Recipes.GetCraftingInputModifier(this));
						if (XUiM_Recipes.CraftingInputModifier > 0f)
						{
							num = Utils.FastMax(1, num);
						}
					}
				}
				if (num != 0)
				{
					int num2 = 0;
					while (num > 0 && num2 < _itemStack.Count)
					{
						if ((!_itemStack[num2].itemValue.HasModSlots || !_itemStack[num2].itemValue.HasMods()) && _itemStack[num2].itemValue.type == itemStack.itemValue.type)
						{
							num -= _itemStack[num2].count;
						}
						num2++;
					}
					if (num > 0)
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001528 RID: 5416 RVA: 0x0007F8CC File Offset: 0x0007DACC
	public bool IsUnlocked(EntityPlayer _ep)
	{
		return !XUiM_Recipes.CraftingProgression || !this.IsLearnable || EffectManager.GetValue(PassiveEffects.RecipeTagUnlocked, null, _ep.GetCVar(this.GetName()), _ep, null, this.tags, true, true, true, true, true, 1, true, false) > 0f;
	}

	// Token: 0x06001529 RID: 5417 RVA: 0x0007F918 File Offset: 0x0007DB18
	public int GetCraftingTier(EntityPlayer _ep)
	{
		if (!XUiM_Recipes.CraftingProgression)
		{
			return 6;
		}
		return (int)EffectManager.GetValue(PassiveEffects.CraftingTier, null, 1f, _ep, this, this.tags, true, true, true, true, true, 1, true, false);
	}

	// Token: 0x0600152A RID: 5418 RVA: 0x0007F94C File Offset: 0x0007DB4C
	public ItemClass GetOutputItemClass()
	{
		return ItemClass.GetForId(this.itemValueType);
	}

	// Token: 0x0600152B RID: 5419 RVA: 0x0007F95C File Offset: 0x0007DB5C
	public bool ContainsIngredients(ItemValue[] _items)
	{
		for (int i = 0; i < this.ingredients.Count; i++)
		{
			for (int j = 0; j < _items.Length; j++)
			{
				if (this.ingredients[i].itemValue.type == _items[j].type)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600152C RID: 5420 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool CanBeCraftedWith(Dictionary<ItemValue, int> _items)
	{
		return false;
	}

	// Token: 0x0600152D RID: 5421 RVA: 0x0007F9B0 File Offset: 0x0007DBB0
	public void Write(BinaryWriter _bw)
	{
		_bw.Write(this.Version);
		_bw.Write(this.itemValueType);
		_bw.Write(this.count);
		_bw.Write(this.IsScrap);
		_bw.Write(this.craftingTime);
		_bw.Write(this.craftExpGain);
		_bw.Write(this.craftingArea ?? "");
		_bw.Write(this.ingredients.Count);
		for (int i = 0; i < this.ingredients.Count; i++)
		{
			this.ingredients[i].Write(_bw);
		}
	}

	// Token: 0x0600152E RID: 5422 RVA: 0x0007FA54 File Offset: 0x0007DC54
	public static Recipe Read(BinaryReader _br)
	{
		_br.ReadUInt16();
		Recipe recipe = new Recipe();
		recipe.itemValueType = _br.ReadInt32();
		recipe.count = _br.ReadInt32();
		recipe.IsScrap = _br.ReadBoolean();
		recipe.craftingTime = _br.ReadSingle();
		recipe.craftExpGain = _br.ReadInt32();
		recipe.craftingArea = _br.ReadString();
		int num = _br.ReadInt32();
		recipe.ingredients = new List<ItemStack>(num);
		for (int i = 0; i < num; i++)
		{
			recipe.ingredients.Add(new ItemStack().Read(_br));
		}
		return recipe;
	}

	// Token: 0x0600152F RID: 5423 RVA: 0x0007FAEC File Offset: 0x0007DCEC
	public override string ToString()
	{
		return string.Format("[Recipe: " + this.GetName() + "]", Array.Empty<object>());
	}

	// Token: 0x06001530 RID: 5424 RVA: 0x0007FB0D File Offset: 0x0007DD0D
	public void ModifyValue(PassiveEffects _passiveEffect, ref float _base_val, ref float _perc_val, FastTags<TagGroup.Global> tags, int _craftingTier = 1)
	{
		if (this.Effects != null)
		{
			this.Effects.ModifyValue(null, _passiveEffect, ref _base_val, ref _perc_val, (float)_craftingTier, tags, 1);
		}
	}

	// Token: 0x04000E19 RID: 3609
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort Version = 1;

	// Token: 0x04000E1A RID: 3610
	public static FastTags<TagGroup.Global> MaterialBased = FastTags<TagGroup.Global>.Parse("materialbased");

	// Token: 0x04000E1B RID: 3611
	public static FastTags<TagGroup.Global> LearnableRecipe = FastTags<TagGroup.Global>.Parse("learnable");

	// Token: 0x04000E1C RID: 3612
	public int itemValueType;

	// Token: 0x04000E1D RID: 3613
	public int count;

	// Token: 0x04000E1E RID: 3614
	public List<ItemStack> ingredients = new List<ItemStack>();

	// Token: 0x04000E1F RID: 3615
	public bool wildcardForgeCategory;

	// Token: 0x04000E20 RID: 3616
	public bool wildcardCampfireCategory;

	// Token: 0x04000E21 RID: 3617
	public bool materialBasedRecipe;

	// Token: 0x04000E22 RID: 3618
	public int craftingToolType;

	// Token: 0x04000E23 RID: 3619
	public float craftingTime;

	// Token: 0x04000E24 RID: 3620
	public string craftingArea;

	// Token: 0x04000E25 RID: 3621
	public string tooltip;

	// Token: 0x04000E26 RID: 3622
	public int unlockExpGain;

	// Token: 0x04000E27 RID: 3623
	public int craftExpGain;

	// Token: 0x04000E28 RID: 3624
	public bool UseIngredientModifier = true;

	// Token: 0x04000E29 RID: 3625
	public FastTags<TagGroup.Global> tags;

	// Token: 0x04000E2A RID: 3626
	public bool IsTrackable = true;

	// Token: 0x04000E2B RID: 3627
	public bool isQuest;

	// Token: 0x04000E2C RID: 3628
	public bool isChallenge;

	// Token: 0x04000E2D RID: 3629
	public bool IsTracked;

	// Token: 0x04000E2E RID: 3630
	public bool IsScrap;

	// Token: 0x04000E2F RID: 3631
	public int craftingTier = -1;

	// Token: 0x04000E30 RID: 3632
	public MinEffectController Effects;
}
