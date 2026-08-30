using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000D4A RID: 3402
[Preserve]
public class ItemActionEntryRecipes : BaseItemActionEntry
{
	// Token: 0x06006782 RID: 26498 RVA: 0x0028DCB0 File Offset: 0x0028BEB0
	public ItemActionEntryRecipes(XUiController _controller)
		: base(_controller, "lblContextActionRecipes", "ui_game_symbol_hammer", BaseItemActionEntry.GamepadShortCut.DPadLeft, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x06006783 RID: 26499 RVA: 0x0028DCCE File Offset: 0x0028BECE
	public override void RefreshEnabled()
	{
		base.Enabled = true;
	}

	// Token: 0x06006784 RID: 26500 RVA: 0x0028DCD8 File Offset: 0x0028BED8
	public override void OnActivated()
	{
		XUi xui = base.ItemController.xui;
		xui.playerUI.windowManager.Close(XUiC_LootWindowGroup.ID);
		List<XUiC_RecipeList> childrenByType = xui.GetChildrenByType<XUiC_RecipeList>();
		XUiC_RecipeList xuiC_RecipeList = null;
		for (int i = 0; i < childrenByType.Count; i++)
		{
			if (childrenByType[i].WindowGroup != null && childrenByType[i].WindowGroup.isShowing)
			{
				xuiC_RecipeList = childrenByType[i];
				break;
			}
		}
		if (xuiC_RecipeList == null)
		{
			XUiC_WindowSelector.OpenSelectorAndWindow(xui.playerUI.entityPlayer, "crafting");
			xuiC_RecipeList = xui.GetChildByType<XUiC_RecipeList>();
		}
		ItemStack itemStack = ItemStack.Empty;
		XUiC_ItemStack xuiC_ItemStack = base.ItemController as XUiC_ItemStack;
		if (xuiC_ItemStack != null)
		{
			itemStack = xuiC_ItemStack.ItemStack;
		}
		if (xuiC_RecipeList != null)
		{
			xuiC_RecipeList.SetRecipeDataByIngredientStack(itemStack);
		}
	}
}
