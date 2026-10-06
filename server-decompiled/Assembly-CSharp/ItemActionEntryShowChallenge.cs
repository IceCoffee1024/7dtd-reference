using System;
using UnityEngine.Scripting;

// Token: 0x02000D75 RID: 3445
[Preserve]
public class ItemActionEntryShowChallenge : BaseItemActionEntry
{
	// Token: 0x06006833 RID: 26675 RVA: 0x00295AE8 File Offset: 0x00293CE8
	public ItemActionEntryShowChallenge(XUiController controller, RecipeUnlockData unlockData)
		: base(controller, "Challenge", "ui_game_symbol_challenge", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
		this.UnlockData = unlockData;
		base.ActionName = Localization.Get("challenge", false, null);
		base.IconName = "ui_game_symbol_challenge";
	}

	// Token: 0x06006834 RID: 26676 RVA: 0x002941E6 File Offset: 0x002923E6
	public override void RefreshEnabled()
	{
		base.Enabled = true;
	}

	// Token: 0x06006835 RID: 26677 RVA: 0x00295B38 File Offset: 0x00293D38
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

	// Token: 0x0400504A RID: 20554
	public RecipeUnlockData UnlockData;
}
