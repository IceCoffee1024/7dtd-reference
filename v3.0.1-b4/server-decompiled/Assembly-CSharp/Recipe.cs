using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x020002D8 RID: 728
public class Recipe
{
	// Token: 0x1700024E RID: 590
	// (get) Token: 0x0600150A RID: 5386 RVA: 0x0007EDAF File Offset: 0x0007CFAF
	// (set) Token: 0x0600150B RID: 5387 RVA: 0x0007EDB7 File Offset: 0x0007CFB7
	public bool IsLearnable
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600150C RID: 5388 RVA: 0x0007EDC0 File Offset: 0x0007CFC0
	public List<ItemStack> GetIngredientsSummedUp()
	{
		return this.ingredients;
	}

	// Token: 0x0600150E RID: 5390 RVA: 0x0007EDF8 File Offset: 0x0007CFF8
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

	// Token: 0x0600150F RID: 5391 RVA: 0x0007EECC File Offset: 0x0007D0CC
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

	// Token: 0x06001510 RID: 5392 RVA: 0x0007EF3B File Offset: 0x0007D13B
	public void AddIngredients(List<ItemStack> _items)
	{
		this.ingredients.AddRange(_items);
	}

	// Token: 0x06001511 RID: 5393 RVA: 0x0007EF4C File Offset: 0x0007D14C
	public string GetName()
	{
		ItemClass forId = ItemClass.GetForId(this.itemValueType);
		if (forId == null)
		{
			return string.Empty;
		}
		return forId.GetItemName();
	}

	// Token: 0x06001512 RID: 5394 RVA: 0x0007EF74 File Offset: 0x0007D174
	public string GetIcon()
	{
		ItemClass forId = ItemClass.GetForId(this.itemValueType);
		if (forId == null)
		{
			return string.Empty;
		}
		return forId.GetIconName();
	}

	// Token: 0x06001513 RID: 5395 RVA: 0x0007EF9C File Offset: 0x0007D19C
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

	// Token: 0x06001514 RID: 5396 RVA: 0x0007F0C8 File Offset: 0x0007D2C8
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

	// Token: 0x06001515 RID: 5397 RVA: 0x0007F210 File Offset: 0x0007D410
	public bool IsUnlocked(EntityPlayer _ep)
	{
		return !XUiM_Recipes.CraftingProgression || !this.IsLearnable || EffectManager.GetValue(PassiveEffects.RecipeTagUnlocked, null, _ep.GetCVar(this.GetName()), _ep, null, this.tags, true, true, true, true, true, 1, true, false) > 0f;
	}

	// Token: 0x06001516 RID: 5398 RVA: 0x0007F25C File Offset: 0x0007D45C
	public int GetCraftingTier(EntityPlayer _ep)
	{
		if (!XUiM_Recipes.CraftingProgression)
		{
			return 6;
		}
		return (int)EffectManager.GetValue(PassiveEffects.CraftingTier, null, 1f, _ep, this, this.tags, true, true, true, true, true, 1, true, false);
	}

	// Token: 0x06001517 RID: 5399 RVA: 0x0007F290 File Offset: 0x0007D490
	public ItemClass GetOutputItemClass()
	{
		return ItemClass.GetForId(this.itemValueType);
	}

	// Token: 0x06001518 RID: 5400 RVA: 0x0007F2A0 File Offset: 0x0007D4A0
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

	// Token: 0x06001519 RID: 5401 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool CanBeCraftedWith(Dictionary<ItemValue, int> _items)
	{
		return false;
	}

	// Token: 0x0600151A RID: 5402 RVA: 0x0007F2F4 File Offset: 0x0007D4F4
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

	// Token: 0x0600151B RID: 5403 RVA: 0x0007F398 File Offset: 0x0007D598
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

	// Token: 0x0600151C RID: 5404 RVA: 0x0007F430 File Offset: 0x0007D630
	public override string ToString()
	{
		return string.Format("[Recipe: " + this.GetName() + "]", Array.Empty<object>());
	}

	// Token: 0x0600151D RID: 5405 RVA: 0x0007F451 File Offset: 0x0007D651
	public void ModifyValue(PassiveEffects _passiveEffect, ref float _base_val, ref float _perc_val, FastTags<TagGroup.Global> tags, int _craftingTier = 1)
	{
		if (this.Effects != null)
		{
			this.Effects.ModifyValue(null, _passiveEffect, ref _base_val, ref _perc_val, (float)_craftingTier, tags, 1);
		}
	}

	// Token: 0x04000E08 RID: 3592
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort Version = 1;

	// Token: 0x04000E09 RID: 3593
	public static FastTags<TagGroup.Global> MaterialBased = FastTags<TagGroup.Global>.Parse("materialbased");

	// Token: 0x04000E0A RID: 3594
	public static FastTags<TagGroup.Global> LearnableRecipe = FastTags<TagGroup.Global>.Parse("learnable");

	// Token: 0x04000E0B RID: 3595
	public int itemValueType;

	// Token: 0x04000E0C RID: 3596
	public int count;

	// Token: 0x04000E0D RID: 3597
	public List<ItemStack> ingredients = new List<ItemStack>();

	// Token: 0x04000E0E RID: 3598
	public bool wildcardForgeCategory;

	// Token: 0x04000E0F RID: 3599
	public bool wildcardCampfireCategory;

	// Token: 0x04000E10 RID: 3600
	public bool materialBasedRecipe;

	// Token: 0x04000E11 RID: 3601
	public int craftingToolType;

	// Token: 0x04000E12 RID: 3602
	public float craftingTime;

	// Token: 0x04000E13 RID: 3603
	public string craftingArea;

	// Token: 0x04000E14 RID: 3604
	public string tooltip;

	// Token: 0x04000E15 RID: 3605
	public int unlockExpGain;

	// Token: 0x04000E16 RID: 3606
	public int craftExpGain;

	// Token: 0x04000E17 RID: 3607
	public bool UseIngredientModifier = true;

	// Token: 0x04000E18 RID: 3608
	public FastTags<TagGroup.Global> tags;

	// Token: 0x04000E19 RID: 3609
	public bool IsTrackable = true;

	// Token: 0x04000E1A RID: 3610
	public bool isQuest;

	// Token: 0x04000E1B RID: 3611
	public bool isChallenge;

	// Token: 0x04000E1C RID: 3612
	public bool IsTracked;

	// Token: 0x04000E1D RID: 3613
	public bool IsScrap;

	// Token: 0x04000E1E RID: 3614
	public int craftingTier = -1;

	// Token: 0x04000E1F RID: 3615
	public MinEffectController Effects;
}
