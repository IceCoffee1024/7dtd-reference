using System;
using UnityEngine.Scripting;

// Token: 0x02000D5E RID: 3422
[Preserve]
public class ServiceActionEntryRent : BaseItemActionEntry
{
	// Token: 0x060067C6 RID: 26566 RVA: 0x0029079B File Offset: 0x0028E99B
	public ServiceActionEntryRent(XUiController _controller, TileEntityVendingMachine _vending)
		: base(_controller, "lblContextActionRent", "ui_game_symbol_coin", BaseItemActionEntry.GamepadShortCut.None, "crafting/craft_click_craft", "ui/ui_denied")
	{
		this.vending = _vending;
	}

	// Token: 0x060067C7 RID: 26567 RVA: 0x002907C0 File Offset: 0x0028E9C0
	public override void RefreshEnabled()
	{
		base.Enabled = this.vending.CanRent() == TileEntityVendingMachine.RentResult.Allowed;
	}

	// Token: 0x060067C8 RID: 26568 RVA: 0x002907D8 File Offset: 0x0028E9D8
	public override void OnDisabledActivate()
	{
		string text;
		switch (this.vending.CanRent())
		{
		case TileEntityVendingMachine.RentResult.AlreadyRented:
			text = Localization.Get("ttVMAlreadyRented", false, null);
			break;
		case TileEntityVendingMachine.RentResult.AlreadyRentingVM:
			text = Localization.Get("ttAlreadyRentingVM", false, null);
			break;
		case TileEntityVendingMachine.RentResult.NotEnoughMoney:
			text = Localization.Get(this.vending.LocalPlayerIsOwner() ? "ttVMNotEnoughMoneyAddTime" : "ttVMNotEnoughMoneyRent", false, null);
			break;
		default:
			text = null;
			break;
		}
		string text2 = text;
		if (text2 != null)
		{
			GameManager.ShowTooltip(base.ItemController.xui.playerUI.entityPlayer, text2, false, false, 0f);
		}
	}

	// Token: 0x060067C9 RID: 26569 RVA: 0x00290871 File Offset: 0x0028EA71
	public override void OnActivated()
	{
		if (this.vending.Rent())
		{
			((XUiC_TraderWindow)base.ItemController).Refresh();
		}
		this.RefreshEnabled();
	}

	// Token: 0x04004F8F RID: 20367
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly TileEntityVendingMachine vending;
}
