using System;
using Audio;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D60 RID: 3424
[Preserve]
public class ItemActionEntryResetPrice : BaseItemActionEntry
{
	// Token: 0x0600681D RID: 26653 RVA: 0x00291504 File Offset: 0x0028F704
	public ItemActionEntryResetPrice(XUiController _controller)
		: base(_controller, "lblContextActionReset", "ui_game_symbol_coin", BaseItemActionEntry.GamepadShortCut.None, "", "ui/ui_denied")
	{
		TileEntityVendingMachine tileEntityVendingMachine = _controller.xui.Trader.Trader as TileEntityVendingMachine;
		if (tileEntityVendingMachine != null)
		{
			TraderInfo traderInfo = _controller.xui.Trader.TraderData.TraderInfo;
			bool playerOwned = traderInfo.PlayerOwned;
			bool rentable = traderInfo.Rentable;
			this.isOwner = (playerOwned || rentable) && tileEntityVendingMachine.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier);
			return;
		}
		this.isOwner = false;
	}

	// Token: 0x0600681E RID: 26654 RVA: 0x0029158C File Offset: 0x0028F78C
	public override void OnActivated()
	{
		XUiC_TraderItemEntry xuiC_TraderItemEntry = (XUiC_TraderItemEntry)base.ItemController;
		TraderData traderData = base.ItemController.xui.Trader.TraderData;
		traderData.PrimaryInventory[xuiC_TraderItemEntry.SlotIndex].Markup = 0;
		traderData.SetModified(base.ItemController.xui.Trader.Trader);
		xuiC_TraderItemEntry.InfoWindow.RefreshBindings();
		base.ItemController.xui.Trader.TraderWindowGroup.RefreshTraderItems();
		Manager.PlayInsidePlayerHead("ui_tab", -1, 0f, false, false);
	}

	// Token: 0x04004FFD RID: 20477
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool isOwner;
}
