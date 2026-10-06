using System;
using UnityEngine.Scripting;

// Token: 0x02000D76 RID: 3446
[Preserve]
public class ItemActionEntryShowCosmetics : BaseItemActionEntry
{
	// Token: 0x06006836 RID: 26678 RVA: 0x00295B8F File Offset: 0x00293D8F
	public ItemActionEntryShowCosmetics(XUiController _controller)
		: base(_controller, "xuiCosmetics", "ui_game_symbol_wardrobe", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x06006837 RID: 26679 RVA: 0x002941E6 File Offset: 0x002923E6
	public override void RefreshEnabled()
	{
		base.Enabled = true;
	}

	// Token: 0x06006838 RID: 26680 RVA: 0x00295BB0 File Offset: 0x00293DB0
	public override void OnActivated()
	{
		XUiC_EquipmentStack xuiC_EquipmentStack = base.ItemController as XUiC_EquipmentStack;
		if (xuiC_EquipmentStack == null)
		{
			return;
		}
		ItemClassArmor itemClassArmor = xuiC_EquipmentStack.ItemValue.ItemClass as ItemClassArmor;
		if (itemClassArmor == null)
		{
			return;
		}
		XUiC_CharacterCosmeticWindowGroup.Open(base.ItemController.xui, itemClassArmor.EquipSlot);
	}
}
