using System;
using UnityEngine.Scripting;

// Token: 0x02000D70 RID: 3440
[Preserve]
public class ItemActionEntryWear : BaseItemActionEntry
{
	// Token: 0x06006853 RID: 26707 RVA: 0x002936C5 File Offset: 0x002918C5
	public ItemActionEntryWear(XUiController _controller)
		: base(_controller, "lblContextActionWear", "ui_game_symbol_shirt", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x06006854 RID: 26708 RVA: 0x002936E4 File Offset: 0x002918E4
	public override void OnActivated()
	{
		XUiM_PlayerEquipment playerEquipment = base.ItemController.xui.PlayerEquipment;
		ItemStack itemStack = ((XUiC_ItemStack)base.ItemController).ItemStack.Clone();
		((XUiC_ItemStack)base.ItemController).ItemStack = playerEquipment.EquipItem(itemStack);
	}
}
