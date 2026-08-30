using System;
using System.Collections.Generic;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000D42 RID: 3394
[Preserve]
public class ItemActionEntryCraft : BaseItemActionEntry
{
	// Token: 0x0600676B RID: 26475 RVA: 0x0028C7FC File Offset: 0x0028A9FC
	public ItemActionEntryCraft(XUiController _controller, XUiC_RecipeCraftCount _recipeCraftCount, int _craftingTier)
		: base(_controller, "lblContextActionCraft", "ui_game_symbol_hammer", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
		this.craftCountControl = _recipeCraftCount;
		this.craftingTier = _craftingTier;
		WorkstationData workstationData = CraftingManager.GetWorkstationData(((XUiC_RecipeEntry)base.ItemController).Recipe.craftingArea);
		if (workstationData != null)
		{
			base.ActionName = workstationData.CraftActionName;
			base.IconName = workstationData.CraftIcon;
			base.SoundName = workstationData.CraftSound;
		}
	}

	// Token: 0x0600676C RID: 26476 RVA: 0x0028C88C File Offset: 0x0028AA8C
	public override void RefreshEnabled()
	{
		base.Enabled = true;
		XUiC_RecipeEntry xuiC_RecipeEntry = (XUiC_RecipeEntry)base.ItemController;
		Recipe recipe = xuiC_RecipeEntry.Recipe;
		if (xuiC_RecipeEntry.Recipe == null)
		{
			this.state = ItemActionEntryCraft.StateTypes.Other;
			base.Enabled = false;
			return;
		}
		if (!xuiC_RecipeEntry.IsCurrentWorkstation)
		{
			this.state = ItemActionEntryCraft.StateTypes.WrongWorkStation;
			base.Enabled = false;
			return;
		}
		if (!XUiM_Recipes.GetRecipeIsUnlocked(base.ItemController.xui, ((XUiC_RecipeEntry)base.ItemController).Recipe))
		{
			this.state = ItemActionEntryCraft.StateTypes.RecipeLocked;
			base.Enabled = false;
			return;
		}
		List<XUiC_CraftingWindowGroup> childrenByType = base.ItemController.xui.GetChildrenByType<XUiC_CraftingWindowGroup>();
		int i = 0;
		while (i < childrenByType.Count)
		{
			if (childrenByType[i].WindowGroup != null && childrenByType[i].WindowGroup.isShowing)
			{
				XUiC_CraftingWindowGroup xuiC_CraftingWindowGroup = childrenByType[i];
				if (!xuiC_CraftingWindowGroup.CraftingRequirementsValid(((XUiC_RecipeEntry)base.ItemController).Recipe))
				{
					this.state = ItemActionEntryCraft.StateTypes.Other;
					this.otherMessage = xuiC_CraftingWindowGroup.CraftingRequirementsInvalidMessage(((XUiC_RecipeEntry)base.ItemController).Recipe);
					base.Enabled = false;
					return;
				}
				break;
			}
			else
			{
				i++;
			}
		}
		if (!this.hasItems(xuiC_RecipeEntry.xui, recipe))
		{
			this.state = ItemActionEntryCraft.StateTypes.NotEnoughMaterials;
			base.Enabled = false;
			return;
		}
		if (this.craftingTier > recipe.GetCraftingTier(base.ItemController.xui.playerUI.entityPlayer))
		{
			this.state = ItemActionEntryCraft.StateTypes.RecipeLocked;
			base.Enabled = false;
			return;
		}
		ItemAction holdingPrimary = base.ItemController.xui.playerUI.entityPlayer.inventory.GetHoldingPrimary();
		if (holdingPrimary != null && holdingPrimary.IsActionRunning(base.ItemController.xui.playerUI.entityPlayer.inventory.holdingItemData.actionData[0]))
		{
			this.state = ItemActionEntryCraft.StateTypes.Other;
			base.Enabled = false;
			return;
		}
		if (base.ItemController.xui.IsUsingItemActionEntryUse)
		{
			this.state = ItemActionEntryCraft.StateTypes.Other;
			base.Enabled = false;
		}
	}

	// Token: 0x0600676D RID: 26477 RVA: 0x0028CA84 File Offset: 0x0028AC84
	public override void OnActivated()
	{
		Recipe recipe = ((XUiC_RecipeEntry)base.ItemController).Recipe;
		XUi xui = base.ItemController.xui;
		List<XUiC_CraftingWindowGroup> childrenByType = xui.GetChildrenByType<XUiC_CraftingWindowGroup>();
		XUiC_CraftingWindowGroup xuiC_CraftingWindowGroup = null;
		for (int i = 0; i < childrenByType.Count; i++)
		{
			if (childrenByType[i].WindowGroup != null && childrenByType[i].WindowGroup.isShowing)
			{
				xuiC_CraftingWindowGroup = childrenByType[i];
				break;
			}
		}
		if (xuiC_CraftingWindowGroup == null)
		{
			return;
		}
		if (!XUiM_Recipes.GetRecipeIsUnlocked(base.ItemController.xui, recipe))
		{
			this.craftCountControl.IsDirty = true;
			xuiC_CraftingWindowGroup.WindowGroup.Controller.SetAllChildrenDirty();
			return;
		}
		if (!xuiC_CraftingWindowGroup.CraftingRequirementsValid(recipe))
		{
			this.craftCountControl.IsDirty = true;
			xuiC_CraftingWindowGroup.WindowGroup.Controller.SetAllChildrenDirty();
			return;
		}
		Recipe recipe2 = new Recipe
		{
			itemValueType = recipe.itemValueType,
			count = XUiM_Recipes.GetRecipeCraftOutputCount(xui, recipe),
			craftingArea = recipe.craftingArea,
			craftExpGain = recipe.craftExpGain,
			craftingTime = XUiM_Recipes.GetRecipeCraftTime(xui, recipe),
			craftingToolType = recipe.craftingToolType,
			craftingTier = this.craftingTier,
			tags = recipe.tags
		};
		if (!this.hasItems(xui, recipe))
		{
			return;
		}
		if (this.tempIngredientList.Count == 0)
		{
			if (!xuiC_CraftingWindowGroup.AddItemToQueue(recipe2))
			{
				this.warnQueueFull();
			}
			return;
		}
		bool flag = false;
		for (int j = 0; j < recipe.ingredients.Count; j++)
		{
			flag |= recipe.ingredients[j].itemValue.HasQuality;
			if (flag)
			{
				break;
			}
		}
		recipe2.AddIngredients(this.tempIngredientList);
		XUiC_WorkstationInputGrid childByType = this.craftCountControl.WindowGroup.Controller.GetChildByType<XUiC_WorkstationInputGrid>();
		if (xuiC_CraftingWindowGroup.AddItemToQueue(recipe2))
		{
			if (flag)
			{
				this.tempIngredientList.Clear();
			}
			if (childByType != null)
			{
				childByType.RemoveItems(recipe2.ingredients, this.craftCountControl.Count, this.tempIngredientList);
			}
			else
			{
				xui.PlayerInventory.RemoveItems(recipe2.ingredients, this.craftCountControl.Count, this.tempIngredientList);
			}
			if (flag)
			{
				recipe2.ingredients.Clear();
				recipe2.AddIngredients(this.tempIngredientList);
			}
			if (recipe == xui.Recipes.TrackedRecipe)
			{
				xui.Recipes.TrackedRecipe = null;
				xui.Recipes.ResetToPreviousTracked(xui.playerUI.entityPlayer);
			}
		}
		else
		{
			this.warnQueueFull();
		}
		this.craftCountControl.IsDirty = true;
		xuiC_CraftingWindowGroup.WindowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x0600676E RID: 26478 RVA: 0x0028CD1C File Offset: 0x0028AF1C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasItems(XUi _xui, Recipe _recipe)
	{
		bool flag = false;
		List<ItemStack> allItemStacks = _xui.PlayerInventory.GetAllItemStacks();
		this.tempIngredientList.Clear();
		float craftingInputModifier = XUiM_Recipes.GetCraftingInputModifier(_recipe);
		if (craftingInputModifier == 0f)
		{
			return true;
		}
		for (int i = 0; i < _recipe.ingredients.Count; i++)
		{
			int num = _recipe.ingredients[i].count;
			int num2 = num;
			if (_recipe.UseIngredientModifier)
			{
				num = (int)EffectManager.GetValue(PassiveEffects.CraftingIngredientCount, null, (float)_recipe.ingredients[i].count, _xui.playerUI.entityPlayer, _recipe, FastTags<TagGroup.Global>.Parse(_recipe.ingredients[i].itemValue.ItemClass.GetItemName()), true, true, true, true, true, this.craftingTier, true, false);
				num2 = num;
				if (num > 0)
				{
					num = (int)((float)num * craftingInputModifier);
					if (XUiM_Recipes.CraftingInputModifier > 0f)
					{
						num = Utils.FastMax(1, num);
					}
				}
			}
			if (!_recipe.ingredients[i].itemValue.HasQuality)
			{
				this.tempIngredientList.Add(new ItemStack(_recipe.ingredients[i].itemValue, num));
			}
			else
			{
				int num3 = ((num2 == 0) ? 1 : num);
				this.tempIngredientList.Add(new ItemStack(_recipe.ingredients[i].itemValue.Clone(), num3));
				for (int j = 0; j < allItemStacks.Count; j++)
				{
					ItemStack itemStack = allItemStacks[j];
					if ((!itemStack.itemValue.HasModSlots || !itemStack.itemValue.HasMods()) && itemStack.itemValue.type == _recipe.ingredients[i].itemValue.type)
					{
						num3--;
						if (num3 == 0)
						{
							break;
						}
					}
				}
				if (num3 > 0)
				{
					return false;
				}
			}
		}
		flag |= _xui.PlayerInventory.HasItems(this.tempIngredientList, this.craftCountControl.Count);
		XUiC_WorkstationInputGrid childByType = this.craftCountControl.WindowGroup.Controller.GetChildByType<XUiC_WorkstationInputGrid>();
		if (childByType != null)
		{
			flag |= childByType.HasItems(this.tempIngredientList, this.craftCountControl.Count);
		}
		return flag;
	}

	// Token: 0x0600676F RID: 26479 RVA: 0x0028CF54 File Offset: 0x0028B154
	public override void OnDisabledActivate()
	{
		EntityPlayerLocal entityPlayer = base.ItemController.xui.playerUI.entityPlayer;
		switch (this.state)
		{
		case ItemActionEntryCraft.StateTypes.RecipeLocked:
			GameManager.ShowTooltip(entityPlayer, Localization.Get("ttMissingCraftingRecipe", false, null), false, false, 0f);
			return;
		case ItemActionEntryCraft.StateTypes.NotEnoughMaterials:
			GameManager.ShowTooltip(entityPlayer, Localization.Get("ttMissingCraftingResources", false, null), false, false, 0f);
			return;
		case ItemActionEntryCraft.StateTypes.WrongWorkStation:
			break;
		case ItemActionEntryCraft.StateTypes.Other:
			GameManager.ShowTooltip(entityPlayer, this.otherMessage, false, false, 0f);
			break;
		default:
			return;
		}
	}

	// Token: 0x06006770 RID: 26480 RVA: 0x0028CFE0 File Offset: 0x0028B1E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void warnQueueFull()
	{
		GameManager.ShowTooltip(base.ItemController.xui.playerUI.entityPlayer, Localization.Get("xuiCraftQueueFull", false, null), false, false, 0f);
		Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
	}

	// Token: 0x04004F48 RID: 20296
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemActionEntryCraft.StateTypes state;

	// Token: 0x04004F49 RID: 20297
	[PublicizedFrom(EAccessModifier.Private)]
	public string otherMessage = "";

	// Token: 0x04004F4A RID: 20298
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int craftingTier;

	// Token: 0x04004F4B RID: 20299
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_RecipeCraftCount craftCountControl;

	// Token: 0x04004F4C RID: 20300
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ItemStack> tempIngredientList = new List<ItemStack>();

	// Token: 0x02000D43 RID: 3395
	[PublicizedFrom(EAccessModifier.Private)]
	public enum StateTypes
	{
		// Token: 0x04004F4E RID: 20302
		Normal,
		// Token: 0x04004F4F RID: 20303
		RecipeLocked,
		// Token: 0x04004F50 RID: 20304
		NotEnoughMaterials,
		// Token: 0x04004F51 RID: 20305
		WrongWorkStation,
		// Token: 0x04004F52 RID: 20306
		Other
	}
}
