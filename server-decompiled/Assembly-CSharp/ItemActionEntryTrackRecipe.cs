using System;
using UnityEngine.Scripting;

// Token: 0x02000D84 RID: 3460
[Preserve]
public class ItemActionEntryTrackRecipe : BaseItemActionEntry
{
	// Token: 0x06006865 RID: 26725 RVA: 0x002973A0 File Offset: 0x002955A0
	public ItemActionEntryTrackRecipe(XUiController _controller, int _craftingTier)
		: base(_controller, "lblContextActionTrack", "ui_game_symbol_compass", BaseItemActionEntry.GamepadShortCut.DPadLeft, "crafting/craft_click_craft", "ui/ui_denied")
	{
		Recipe recipe = ((XUiC_RecipeEntry)base.ItemController).Recipe;
		recipe.craftingTier = _craftingTier;
		this.selectedCraftingTier = _craftingTier;
	}

	// Token: 0x06006866 RID: 26726 RVA: 0x002973EC File Offset: 0x002955EC
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

	// Token: 0x04005071 RID: 20593
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int selectedCraftingTier;
}
