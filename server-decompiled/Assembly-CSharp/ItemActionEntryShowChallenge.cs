using System;
using UnityEngine.Scripting;

// Token: 0x02000D64 RID: 3428
[Preserve]
public class ItemActionEntryShowChallenge : BaseItemActionEntry
{
	// Token: 0x0600682C RID: 26668 RVA: 0x00292538 File Offset: 0x00290738
	public ItemActionEntryShowChallenge(XUiController controller, RecipeUnlockData unlockData)
		: base(controller, "Challenge", "ui_game_symbol_challenge", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
		this.UnlockData = unlockData;
		base.ActionName = Localization.Get("challenge", false, null);
		base.IconName = "ui_game_symbol_challenge";
	}

	// Token: 0x0600682D RID: 26669 RVA: 0x00290C6A File Offset: 0x0028EE6A
	public override void RefreshEnabled()
	{
		base.Enabled = true;
	}

	// Token: 0x0600682E RID: 26670 RVA: 0x00292588 File Offset: 0x00290788
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

	// Token: 0x0400500B RID: 20491
	public RecipeUnlockData UnlockData;
}
