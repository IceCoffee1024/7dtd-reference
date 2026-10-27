using System;
using UnityEngine.Scripting;

// Token: 0x02000D8B RID: 3467
[Preserve]
public class ItemActionEntryWear : BaseItemActionEntry
{
	// Token: 0x0600687D RID: 26749 RVA: 0x00298139 File Offset: 0x00296339
	public ItemActionEntryWear(XUiController _controller)
		: base(_controller, "lblContextActionWear", "ui_game_symbol_shirt", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x0600687E RID: 26750 RVA: 0x00298158 File Offset: 0x00296358
	public override void OnActivated()
	{
		XUiM_PlayerEquipment playerEquipment = base.ItemController.xui.PlayerEquipment;
		ItemStack itemStack = ((IItemStackController)base.ItemController).ItemStack.Clone();
		((IItemStackController)base.ItemController).ItemStack = playerEquipment.EquipItem(itemStack);
	}
}
