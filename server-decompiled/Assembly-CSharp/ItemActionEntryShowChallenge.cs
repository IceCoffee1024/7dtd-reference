using System;
using UnityEngine.Scripting;

// Token: 0x02000D51 RID: 3409
[Preserve]
public class ItemActionEntryShowChallenge : BaseItemActionEntry
{
	// Token: 0x0600679D RID: 26525 RVA: 0x0028F5A4 File Offset: 0x0028D7A4
	public ItemActionEntryShowChallenge(XUiController controller, RecipeUnlockData unlockData)
		: base(controller, "Challenge", "ui_game_symbol_challenge", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
		this.UnlockData = unlockData;
		base.ActionName = Localization.Get("challenge", false, null);
		base.IconName = "ui_game_symbol_challenge";
	}

	// Token: 0x0600679E RID: 26526 RVA: 0x0028DCCE File Offset: 0x0028BECE
	public override void RefreshEnabled()
	{
		base.Enabled = true;
	}

	// Token: 0x0600679F RID: 26527 RVA: 0x0028F5F4 File Offset: 0x0028D7F4
	public override void OnActivated()
	{
		XUi xui = base.ItemController.xui;
		xui.playerUI.windowManager.Close(XUiC_LootWindowGroup.ID);
		XUiC_WindowSelector.OpenSelectorAndWindow(xui.playerUI.entityPlayer, "challenges");
		XUiC_ChallengeEntryListWindow childByType = xui.GetChildByType<XUiC_ChallengeEntryListWindow>();
		if (childByType != null)
		{
			childByType.SetSelectedByUnlockData(this.UnlockData);
		}
	}

	// Token: 0x04004F6D RID: 20333
	public RecipeUnlockData UnlockData;
}
