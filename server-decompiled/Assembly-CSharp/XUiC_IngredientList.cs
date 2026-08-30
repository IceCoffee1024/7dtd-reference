using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F34 RID: 3892
[Preserve]
public class XUiC_IngredientList : XUiController
{
	// Token: 0x17000E3C RID: 3644
	// (get) Token: 0x06007A42 RID: 31298 RVA: 0x002E2090 File Offset: 0x002E0290
	// (set) Token: 0x06007A43 RID: 31299 RVA: 0x002E2098 File Offset: 0x002E0298
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

	// Token: 0x17000E3D RID: 3645
	// (get) Token: 0x06007A44 RID: 31300 RVA: 0x002E20A8 File Offset: 0x002E02A8
	// (set) Token: 0x06007A45 RID: 31301 RVA: 0x002E20B0 File Offset: 0x002E02B0
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

	// Token: 0x06007A46 RID: 31302 RVA: 0x002E20C0 File Offset: 0x002E02C0
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

	// Token: 0x06007A47 RID: 31303 RVA: 0x002E20FE File Offset: 0x002E02FE
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.PlayerInventory.OnBackpackItemsChanged += this.PlayerInventory_OnBackpackItemsChanged;
		this.xui.PlayerInventory.OnToolbeltItemsChanged += this.PlayerInventory_OnToolbeltItemsChanged;
	}

	// Token: 0x06007A48 RID: 31304 RVA: 0x002E213E File Offset: 0x002E033E
	public override void OnClose()
	{
		base.OnClose();
		this.xui.PlayerInventory.OnBackpackItemsChanged -= this.PlayerInventory_OnBackpackItemsChanged;
		this.xui.PlayerInventory.OnToolbeltItemsChanged -= this.PlayerInventory_OnToolbeltItemsChanged;
	}

	// Token: 0x06007A49 RID: 31305 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnToolbeltItemsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007A4A RID: 31306 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnBackpackItemsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007A4B RID: 31307 RVA: 0x002E2180 File Offset: 0x002E0380
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

	// Token: 0x04005A5C RID: 23132
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;

	// Token: 0x04005A5D RID: 23133
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiController> ingredientEntries = new List<XUiController>();

	// Token: 0x04005A5E RID: 23134
	[PublicizedFrom(EAccessModifier.Private)]
	public int craftingTier = -1;
}
