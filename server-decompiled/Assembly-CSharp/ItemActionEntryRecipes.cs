using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000D5D RID: 3421
[Preserve]
public class ItemActionEntryRecipes : BaseItemActionEntry
{
	// Token: 0x06006811 RID: 26641 RVA: 0x00290C4C File Offset: 0x0028EE4C
	public ItemActionEntryRecipes(XUiController _controller)
		: base(_controller, "lblContextActionRecipes", "ui_game_symbol_hammer", BaseItemActionEntry.GamepadShortCut.DPadLeft, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x06006812 RID: 26642 RVA: 0x00290C6A File Offset: 0x0028EE6A
	public override void RefreshEnabled()
	{
		base.Enabled = true;
	}

	// Token: 0x06006813 RID: 26643 RVA: 0x00290C74 File Offset: 0x0028EE74
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
