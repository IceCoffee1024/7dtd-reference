using System;
using UnityEngine.Scripting;

// Token: 0x02000D75 RID: 3445
[Preserve]
public class ItemActionEntryFavorite : BaseItemActionEntry
{
	// Token: 0x06006833 RID: 26675 RVA: 0x00294D43 File Offset: 0x00292F43
	public ItemActionEntryFavorite(XUiController _controller, Recipe _recipe)
		: base(_controller, "lblContextActionFavorite", "server_favorite", BaseItemActionEntry.GamepadShortCut.DPadRight, "crafting/craft_click_craft", "ui/ui_denied")
	{
		this.recipe = _recipe;
	}

	// Token: 0x06006834 RID: 26676 RVA: 0x00294D68 File Offset: 0x00292F68
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

	// Token: 0x04005055 RID: 20565
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Recipe recipe;
}
