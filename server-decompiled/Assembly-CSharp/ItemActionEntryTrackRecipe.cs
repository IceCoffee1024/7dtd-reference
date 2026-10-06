using System;
using UnityEngine.Scripting;

// Token: 0x02000D79 RID: 3449
[Preserve]
public class ItemActionEntryTrackRecipe : BaseItemActionEntry
{
	// Token: 0x06006840 RID: 26688 RVA: 0x00295E78 File Offset: 0x00294078
	public ItemActionEntryTrackRecipe(XUiController _controller, int _craftingTier)
		: base(_controller, "lblContextActionTrack", "ui_game_symbol_compass", BaseItemActionEntry.GamepadShortCut.DPadLeft, "crafting/craft_click_craft", "ui/ui_denied")
	{
		Recipe recipe = ((XUiC_RecipeEntry)base.ItemController).Recipe;
		recipe.craftingTier = _craftingTier;
		this.selectedCraftingTier = _craftingTier;
	}

	// Token: 0x06006841 RID: 26689 RVA: 0x00295EC4 File Offset: 0x002940C4
	public override void OnActivated()
	{
		Recipe recipe = ((XUiC_RecipeEntry)base.ItemController).Recipe;
		recipe.craftingTier = this.selectedCraftingTier;
		XUi xui = base.ItemController.xui;
		EntityPlayerLocal entityPlayer = xui.playerUI.entityPlayer;
		if (xui.Recipes.TrackedRecipe != recipe || xui.Recipes.TrackedRecipeQuality != this.selectedCraftingTier)
		{
			if (xui.Recipes.TrackedRecipe == null)
			{
				xui.Recipes.SetPreviousTracked(entityPlayer);
			}
			xui.Recipes.TrackedRecipeQuality = this.selectedCraftingTier;
			xui.Recipes.TrackedRecipeCount = 1;
			xui.Recipes.TrackedRecipe = recipe;
			return;
		}
		xui.Recipes.TrackedRecipe = null;
		xui.Recipes.ResetToPreviousTracked(entityPlayer);
	}

	// Token: 0x0400504C RID: 20556
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int selectedCraftingTier;
}
