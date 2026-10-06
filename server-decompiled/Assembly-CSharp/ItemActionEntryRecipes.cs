using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000D6E RID: 3438
[Preserve]
public class ItemActionEntryRecipes : BaseItemActionEntry
{
	// Token: 0x06006818 RID: 26648 RVA: 0x002941C8 File Offset: 0x002923C8
	public ItemActionEntryRecipes(XUiController _controller)
		: base(_controller, "lblContextActionRecipes", "ui_game_symbol_hammer", BaseItemActionEntry.GamepadShortCut.DPadLeft, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x06006819 RID: 26649 RVA: 0x002941E6 File Offset: 0x002923E6
	public override void RefreshEnabled()
	{
		base.Enabled = true;
	}

	// Token: 0x0600681A RID: 26650 RVA: 0x002941F0 File Offset: 0x002923F0
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
		IItemStackController itemStackController = base.ItemController as IItemStackController;
		if (itemStackController != null)
		{
			itemStack = itemStackController.ItemStack;
		}
		if (xuiC_RecipeList != null)
		{
			xuiC_RecipeList.SetRecipeDataByIngredientStack(itemStack);
		}
	}
}
