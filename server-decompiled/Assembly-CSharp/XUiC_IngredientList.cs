using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F4B RID: 3915
[Preserve]
public class XUiC_IngredientList : XUiController
{
	// Token: 0x17000E57 RID: 3671
	// (get) Token: 0x06007AFB RID: 31483 RVA: 0x002E6044 File Offset: 0x002E4244
	// (set) Token: 0x06007AFC RID: 31484 RVA: 0x002E604C File Offset: 0x002E424C
	public Recipe Recipe
	{
		get
		{
			return this.recipe;
		}
		set
		{
			this.recipe = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000E58 RID: 3672
	// (get) Token: 0x06007AFD RID: 31485 RVA: 0x002E605C File Offset: 0x002E425C
	// (set) Token: 0x06007AFE RID: 31486 RVA: 0x002E6064 File Offset: 0x002E4264
	public int CraftingTier
	{
		get
		{
			return this.craftingTier;
		}
		set
		{
			this.craftingTier = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06007AFF RID: 31487 RVA: 0x002E6074 File Offset: 0x002E4274
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_IngredientEntry>(null);
		XUiController[] array = childrenByType;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				this.ingredientEntries.Add(array[i]);
			}
		}
	}

	// Token: 0x06007B00 RID: 31488 RVA: 0x002E60B2 File Offset: 0x002E42B2
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.PlayerInventory.OnBackpackItemsChanged += this.PlayerInventory_OnBackpackItemsChanged;
		this.xui.PlayerInventory.OnToolbeltItemsChanged += this.PlayerInventory_OnToolbeltItemsChanged;
	}

	// Token: 0x06007B01 RID: 31489 RVA: 0x002E60F2 File Offset: 0x002E42F2
	public override void OnClose()
	{
		base.OnClose();
		this.xui.PlayerInventory.OnBackpackItemsChanged -= this.PlayerInventory_OnBackpackItemsChanged;
		this.xui.PlayerInventory.OnToolbeltItemsChanged -= this.PlayerInventory_OnToolbeltItemsChanged;
	}

	// Token: 0x06007B02 RID: 31490 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnToolbeltItemsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007B03 RID: 31491 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnBackpackItemsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007B04 RID: 31492 RVA: 0x002E6134 File Offset: 0x002E4334
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			if (this.recipe != null)
			{
				int count = this.ingredientEntries.Count;
				int count2 = this.recipe.ingredients.Count;
				for (int i = 0; i < count; i++)
				{
					if (this.ingredientEntries[i] is XUiC_IngredientEntry)
					{
						ItemStack itemStack = ((i < count2) ? this.recipe.ingredients[i].Clone() : null);
						if (itemStack != null && this.recipe.UseIngredientModifier)
						{
							itemStack.count = (int)EffectManager.GetValue(PassiveEffects.CraftingIngredientCount, null, (float)itemStack.count, this.xui.playerUI.entityPlayer, this.recipe, FastTags<TagGroup.Global>.Parse(itemStack.itemValue.ItemClass.GetItemName()), true, true, true, true, true, this.craftingTier, true, false);
							if (itemStack.count > 0)
							{
								itemStack.count = (int)((float)itemStack.count * XUiM_Recipes.GetCraftingInputModifier(this.recipe));
								if (XUiM_Recipes.CraftingInputModifier > 0f)
								{
									itemStack.count = Utils.FastMax(1, itemStack.count);
								}
							}
						}
						if (itemStack == null || (itemStack != null && itemStack.count > 0))
						{
							((XUiC_IngredientEntry)this.ingredientEntries[i]).Ingredient = itemStack;
						}
						else
						{
							((XUiC_IngredientEntry)this.ingredientEntries[i]).Ingredient = null;
						}
					}
				}
			}
			else
			{
				int count3 = this.ingredientEntries.Count;
				for (int j = 0; j < count3; j++)
				{
					if (this.ingredientEntries[j] is XUiC_IngredientEntry)
					{
						((XUiC_IngredientEntry)this.ingredientEntries[j]).Ingredient = null;
					}
				}
			}
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x04005B0F RID: 23311
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;

	// Token: 0x04005B10 RID: 23312
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiController> ingredientEntries = new List<XUiController>();

	// Token: 0x04005B11 RID: 23313
	[PublicizedFrom(EAccessModifier.Private)]
	public int craftingTier = -1;
}
