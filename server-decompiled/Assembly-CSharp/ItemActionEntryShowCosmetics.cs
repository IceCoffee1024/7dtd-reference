using System;
using UnityEngine.Scripting;

// Token: 0x02000D65 RID: 3429
[Preserve]
public class ItemActionEntryShowCosmetics : BaseItemActionEntry
{
	// Token: 0x0600682F RID: 26671 RVA: 0x002925DF File Offset: 0x002907DF
	public ItemActionEntryShowCosmetics(XUiController _controller)
		: base(_controller, "xuiCosmetics", "ui_game_symbol_wardrobe", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x06006830 RID: 26672 RVA: 0x00290C6A File Offset: 0x0028EE6A
	public override void RefreshEnabled()
	{
		base.Enabled = true;
	}

	// Token: 0x06006831 RID: 26673 RVA: 0x00292600 File Offset: 0x00290800
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
