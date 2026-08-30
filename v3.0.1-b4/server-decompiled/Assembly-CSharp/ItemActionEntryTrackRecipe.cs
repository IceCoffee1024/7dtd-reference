using System;
using UnityEngine.Scripting;

// Token: 0x02000D55 RID: 3413
[Preserve]
public class ItemActionEntryTrackRecipe : BaseItemActionEntry
{
	// Token: 0x060067AA RID: 26538 RVA: 0x0028F934 File Offset: 0x0028DB34
	public ItemActionEntryTrackRecipe(XUiController _controller, int _craftingTier)
		: base(_controller, "lblContextActionTrack", "ui_game_symbol_compass", BaseItemActionEntry.GamepadShortCut.DPadLeft, "crafting/craft_click_craft", "ui/ui_denied")
	{
		Recipe recipe = ((XUiC_RecipeEntry)base.ItemController).Recipe;
		recipe.craftingTier = _craftingTier;
		this.selectedCraftingTier = _craftingTier;
	}

	// Token: 0x060067AB RID: 26539 RVA: 0x0028F980 File Offset: 0x0028DB80
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

	// Token: 0x04004F6F RID: 20335
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int selectedCraftingTier;
}
