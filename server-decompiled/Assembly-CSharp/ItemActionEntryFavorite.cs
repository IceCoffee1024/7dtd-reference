using System;
using UnityEngine.Scripting;

// Token: 0x02000D6A RID: 3434
[Preserve]
public class ItemActionEntryFavorite : BaseItemActionEntry
{
	// Token: 0x0600680E RID: 26638 RVA: 0x0029381B File Offset: 0x00291A1B
	public ItemActionEntryFavorite(XUiController _controller, Recipe _recipe)
		: base(_controller, "lblContextActionFavorite", "server_favorite", BaseItemActionEntry.GamepadShortCut.DPadRight, "crafting/craft_click_craft", "ui/ui_denied")
	{
		this.recipe = _recipe;
	}

	// Token: 0x0600680F RID: 26639 RVA: 0x00293840 File Offset: 0x00291A40
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

	// Token: 0x04005030 RID: 20528
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Recipe recipe;
}
