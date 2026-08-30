using System;
using UnityEngine.Scripting;

// Token: 0x02000D5D RID: 3421
[Preserve]
public class ItemActionEntryWear : BaseItemActionEntry
{
	// Token: 0x060067C4 RID: 26564 RVA: 0x00290731 File Offset: 0x0028E931
	public ItemActionEntryWear(XUiController _controller)
		: base(_controller, "lblContextActionWear", "ui_game_symbol_shirt", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x060067C5 RID: 26565 RVA: 0x00290750 File Offset: 0x0028E950
	public override void OnActivated()
	{
		XUiM_PlayerEquipment playerEquipment = base.ItemController.xui.PlayerEquipment;
		ItemStack itemStack = ((XUiC_ItemStack)base.ItemController).ItemStack.Clone();
		((XUiC_ItemStack)base.ItemController).ItemStack = playerEquipment.EquipItem(itemStack);
	}
}
