using System;
using UnityEngine.Scripting;

// Token: 0x02000D8C RID: 3468
[Preserve]
public class ServiceActionEntryRent : BaseItemActionEntry
{
	// Token: 0x0600687F RID: 26751 RVA: 0x002981A3 File Offset: 0x002963A3
	public ServiceActionEntryRent(XUiController _controller, TileEntityVendingMachine _vending)
		: base(_controller, "lblContextActionRent", "ui_game_symbol_coin", BaseItemActionEntry.GamepadShortCut.None, "crafting/craft_click_craft", "ui/ui_denied")
	{
		this.vending = _vending;
	}

	// Token: 0x06006880 RID: 26752 RVA: 0x002981C8 File Offset: 0x002963C8
	public override void RefreshEnabled()
	{
		base.Enabled = this.vending.CanRent() == TileEntityVendingMachine.RentResult.Allowed;
	}

	// Token: 0x06006881 RID: 26753 RVA: 0x002981E0 File Offset: 0x002963E0
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

	// Token: 0x06006882 RID: 26754 RVA: 0x00298279 File Offset: 0x00296479
	public override void OnActivated()
	{
		if (this.vending.Rent())
		{
			((XUiC_TraderWindow)base.ItemController).Refresh();
		}
		this.RefreshEnabled();
	}

	// Token: 0x0400508D RID: 20621
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly TileEntityVendingMachine vending;
}
