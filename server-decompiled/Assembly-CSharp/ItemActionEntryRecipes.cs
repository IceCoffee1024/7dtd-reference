using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000D79 RID: 3449
[Preserve]
public class ItemActionEntryRecipes : BaseItemActionEntry
{
	// Token: 0x0600683D RID: 26685 RVA: 0x002956F0 File Offset: 0x002938F0
	public ItemActionEntryRecipes(XUiController _controller)
		: base(_controller, "lblContextActionRecipes", "ui_game_symbol_hammer", BaseItemActionEntry.GamepadShortCut.DPadLeft, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x0600683E RID: 26686 RVA: 0x0029570E File Offset: 0x0029390E
	public override void RefreshEnabled()
	{
		base.Enabled = true;
	}

	// Token: 0x0600683F RID: 26687 RVA: 0x00295718 File Offset: 0x00293918
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
