using System;
using UnityEngine.Scripting;

// Token: 0x02000D80 RID: 3456
[Preserve]
public class ItemActionEntryShowChallenge : BaseItemActionEntry
{
	// Token: 0x06006858 RID: 26712 RVA: 0x00297010 File Offset: 0x00295210
	public ItemActionEntryShowChallenge(XUiController controller, RecipeUnlockData unlockData)
		: base(controller, "Challenge", "ui_game_symbol_challenge", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
		this.UnlockData = unlockData;
		base.ActionName = Localization.Get("challenge", false, null);
		base.IconName = "ui_game_symbol_challenge";
	}

	// Token: 0x06006859 RID: 26713 RVA: 0x0029570E File Offset: 0x0029390E
	public override void RefreshEnabled()
	{
		base.Enabled = true;
	}

	// Token: 0x0600685A RID: 26714 RVA: 0x00297060 File Offset: 0x00295260
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

	// Token: 0x0400506F RID: 20591
	public RecipeUnlockData UnlockData;
}
