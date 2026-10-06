using System;
using UnityEngine.Scripting;

// Token: 0x02000D81 RID: 3457
[Preserve]
public class ServiceActionEntryRent : BaseItemActionEntry
{
	// Token: 0x0600685A RID: 26714 RVA: 0x00296C7B File Offset: 0x00294E7B
	public ServiceActionEntryRent(XUiController _controller, TileEntityVendingMachine _vending)
		: base(_controller, "lblContextActionRent", "ui_game_symbol_coin", BaseItemActionEntry.GamepadShortCut.None, "crafting/craft_click_craft", "ui/ui_denied")
	{
		this.vending = _vending;
	}

	// Token: 0x0600685B RID: 26715 RVA: 0x00296CA0 File Offset: 0x00294EA0
	public override void RefreshEnabled()
	{
		base.Enabled = this.vending.CanRent() == TileEntityVendingMachine.RentResult.Allowed;
	}

	// Token: 0x0600685C RID: 26716 RVA: 0x00296CB8 File Offset: 0x00294EB8
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

	// Token: 0x0600685D RID: 26717 RVA: 0x00296D51 File Offset: 0x00294F51
	public override void OnActivated()
	{
		if (this.vending.Rent())
		{
			((XUiC_TraderWindow)base.ItemController).Refresh();
		}
		this.RefreshEnabled();
	}

	// Token: 0x04005068 RID: 20584
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly TileEntityVendingMachine vending;
}
