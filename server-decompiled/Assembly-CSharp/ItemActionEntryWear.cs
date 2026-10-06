using System;
using UnityEngine.Scripting;

// Token: 0x02000D80 RID: 3456
[Preserve]
public class ItemActionEntryWear : BaseItemActionEntry
{
	// Token: 0x06006858 RID: 26712 RVA: 0x00296C11 File Offset: 0x00294E11
	public ItemActionEntryWear(XUiController _controller)
		: base(_controller, "lblContextActionWear", "ui_game_symbol_shirt", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x06006859 RID: 26713 RVA: 0x00296C30 File Offset: 0x00294E30
	public override void OnActivated()
	{
		XUiM_PlayerEquipment playerEquipment = base.ItemController.xui.PlayerEquipment;
		ItemStack itemStack = ((IItemStackController)base.ItemController).ItemStack.Clone();
		((IItemStackController)base.ItemController).ItemStack = playerEquipment.EquipItem(itemStack);
	}
}
