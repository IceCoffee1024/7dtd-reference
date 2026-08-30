using System;
using UnityEngine.Scripting;

// Token: 0x02000D71 RID: 3441
[Preserve]
public class ServiceActionEntryRent : BaseItemActionEntry
{
	// Token: 0x06006855 RID: 26709 RVA: 0x0029372F File Offset: 0x0029192F
	public ServiceActionEntryRent(XUiController _controller, TileEntityVendingMachine _vending)
		: base(_controller, "lblContextActionRent", "ui_game_symbol_coin", BaseItemActionEntry.GamepadShortCut.None, "crafting/craft_click_craft", "ui/ui_denied")
	{
		this.vending = _vending;
	}

	// Token: 0x06006856 RID: 26710 RVA: 0x00293754 File Offset: 0x00291954
	public override void RefreshEnabled()
	{
		base.Enabled = this.vending.CanRent() == TileEntityVendingMachine.RentResult.Allowed;
	}

	// Token: 0x06006857 RID: 26711 RVA: 0x0029376C File Offset: 0x0029196C
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

	// Token: 0x06006858 RID: 26712 RVA: 0x00293805 File Offset: 0x00291A05
	public override void OnActivated()
	{
		if (this.vending.Rent())
		{
			((XUiC_TraderWindow)base.ItemController).Refresh();
		}
		this.RefreshEnabled();
	}

	// Token: 0x0400502D RID: 20525
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly TileEntityVendingMachine vending;
}
