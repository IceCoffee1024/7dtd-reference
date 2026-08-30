using System;
using UnityEngine.Scripting;

// Token: 0x02000D46 RID: 3398
[Preserve]
public class ItemActionEntryFavorite : BaseItemActionEntry
{
	// Token: 0x06006778 RID: 26488 RVA: 0x0028D300 File Offset: 0x0028B500
	public ItemActionEntryFavorite(XUiController _controller, Recipe _recipe)
		: base(_controller, "lblContextActionFavorite", "server_favorite", BaseItemActionEntry.GamepadShortCut.DPadRight, "crafting/craft_click_craft", "ui/ui_denied")
	{
		this.recipe = _recipe;
	}

	// Token: 0x06006779 RID: 26489 RVA: 0x0028D328 File Offset: 0x0028B528
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

	// Token: 0x04004F53 RID: 20307
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Recipe recipe;
}
