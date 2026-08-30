using System;
using UnityEngine.Scripting;

// Token: 0x02000D59 RID: 3417
[Preserve]
public class ItemActionEntryFavorite : BaseItemActionEntry
{
	// Token: 0x06006807 RID: 26631 RVA: 0x0029029C File Offset: 0x0028E49C
	public ItemActionEntryFavorite(XUiController _controller, Recipe _recipe)
		: base(_controller, "lblContextActionFavorite", "server_favorite", BaseItemActionEntry.GamepadShortCut.DPadRight, "crafting/craft_click_craft", "ui/ui_denied")
	{
		this.recipe = _recipe;
	}

	// Token: 0x06006808 RID: 26632 RVA: 0x002902C4 File Offset: 0x0028E4C4
	public override void OnActivated()
	{
		XUiC_RecipeEntry xuiC_RecipeEntry = (XUiC_RecipeEntry)base.ItemController;
		if (((xuiC_RecipeEntry != null) ? xuiC_RecipeEntry.Recipe : null) == null)
		{
			if (this.recipe != null)
			{
				CraftingManager.ToggleFavoriteRecipe(this.recipe);
			}
		}
		else
		{
			CraftingManager.ToggleFavoriteRecipe(xuiC_RecipeEntry.Recipe);
		}
		if (xuiC_RecipeEntry != null)
		{
			XUiC_RecipeList childByType = xuiC_RecipeEntry.WindowGroup.Controller.GetChildByType<XUiC_RecipeList>();
			if (childByType == null)
			{
				return;
			}
			childByType.RefreshCurrentRecipes();
		}
	}

	// Token: 0x04004FF1 RID: 20465
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Recipe recipe;
}
