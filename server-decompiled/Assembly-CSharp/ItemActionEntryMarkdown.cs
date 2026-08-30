using System;
using Audio;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D5A RID: 3418
[Preserve]
public class ItemActionEntryMarkdown : BaseItemActionEntry
{
	// Token: 0x06006809 RID: 26633 RVA: 0x00290328 File Offset: 0x0028E528
	public ItemActionEntryMarkdown(XUiController _controller)
		: base(_controller, "OVERRIDDEN BELOW", "ui_game_symbol_subtract", BaseItemActionEntry.GamepadShortCut.DPadLeft, "", "ui/ui_denied")
	{
		base.ActionName = string.Format(Localization.Get("lblContextActionPrice", false, null), 20);
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

	// Token: 0x0600680A RID: 26634 RVA: 0x002903CC File Offset: 0x0028E5CC
	public override void OnActivated()
	{
		XUiC_TraderItemEntry xuiC_TraderItemEntry = (XUiC_TraderItemEntry)base.ItemController;
		TraderData traderData = base.ItemController.xui.Trader.TraderData;
		traderData.PrimaryInventory[xuiC_TraderItemEntry.SlotIndex].DecreaseMarkup();
		traderData.SetModified(base.ItemController.xui.Trader.Trader);
		xuiC_TraderItemEntry.InfoWindow.RefreshBindings();
		base.ItemController.xui.Trader.TraderWindowGroup.RefreshTraderItems();
		Manager.PlayInsidePlayerHead("ui_tab", -1, 0f, false, false);
	}

	// Token: 0x04004FF2 RID: 20466
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool isOwner;
}
