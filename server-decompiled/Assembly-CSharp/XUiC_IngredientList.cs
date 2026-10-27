using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F69 RID: 3945
[Preserve]
public class XUiC_IngredientList : XUiController
{
	// Token: 0x17000E7E RID: 3710
	// (get) Token: 0x06007B3B RID: 31547 RVA: 0x002EACC0 File Offset: 0x002E8EC0
	// (set) Token: 0x06007B3C RID: 31548 RVA: 0x002EACC8 File Offset: 0x002E8EC8
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

	// Token: 0x17000E7F RID: 3711
	// (get) Token: 0x06007B3D RID: 31549 RVA: 0x002EACD8 File Offset: 0x002E8ED8
	// (set) Token: 0x06007B3E RID: 31550 RVA: 0x002EACE0 File Offset: 0x002E8EE0
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

	// Token: 0x06007B3F RID: 31551 RVA: 0x002EACF0 File Offset: 0x002E8EF0
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

	// Token: 0x06007B40 RID: 31552 RVA: 0x002EAD2E File Offset: 0x002E8F2E
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.PlayerInventory.OnBackpackItemsChanged += this.PlayerInventory_OnBackpackItemsChanged;
		this.xui.PlayerInventory.OnToolbeltItemsChanged += this.PlayerInventory_OnToolbeltItemsChanged;
	}

	// Token: 0x06007B41 RID: 31553 RVA: 0x002EAD6E File Offset: 0x002E8F6E
	public override void OnClose()
	{
		base.OnClose();
		this.xui.PlayerInventory.OnBackpackItemsChanged -= this.PlayerInventory_OnBackpackItemsChanged;
		this.xui.PlayerInventory.OnToolbeltItemsChanged -= this.PlayerInventory_OnToolbeltItemsChanged;
	}

	// Token: 0x06007B42 RID: 31554 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnToolbeltItemsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007B43 RID: 31555 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnBackpackItemsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007B44 RID: 31556 RVA: 0x002EADB0 File Offset: 0x002E8FB0
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

	// Token: 0x04005B7A RID: 23418
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;

	// Token: 0x04005B7B RID: 23419
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiController> ingredientEntries = new List<XUiController>();

	// Token: 0x04005B7C RID: 23420
	[PublicizedFrom(EAccessModifier.Private)]
	public int craftingTier = -1;
}
