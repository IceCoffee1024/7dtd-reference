using System;
using Audio;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D77 RID: 3447
[Preserve]
public class ItemActionEntryMarkup : BaseItemActionEntry
{
	// Token: 0x06006837 RID: 26679 RVA: 0x00294F08 File Offset: 0x00293108
	public ItemActionEntryMarkup(XUiController _controller)
		: base(_controller, "OVERRIDDEN BELOW", "ui_game_symbol_add", BaseItemActionEntry.GamepadShortCut.DPadRight, "", "ui/ui_denied")
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

	// Token: 0x06006838 RID: 26680 RVA: 0x00294FAC File Offset: 0x002931AC
	public override void OnActivated()
	{
		XUiC_TraderItemEntry xuiC_TraderItemEntry = (XUiC_TraderItemEntry)base.ItemController;
		TraderData traderData = base.ItemController.xui.Trader.TraderData;
		traderData.PrimaryInventory[xuiC_TraderItemEntry.SlotIndex].IncreaseMarkup();
		traderData.SetModified(base.ItemController.xui.Trader.Trader);
		xuiC_TraderItemEntry.InfoWindow.RefreshBindings();
		base.ItemController.xui.Trader.TraderWindowGroup.RefreshTraderItems();
		Manager.PlayInsidePlayerHead("ui_tab", -1, 0f, false, false);
	}

	// Token: 0x04005057 RID: 20567
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool isOwner;
}
