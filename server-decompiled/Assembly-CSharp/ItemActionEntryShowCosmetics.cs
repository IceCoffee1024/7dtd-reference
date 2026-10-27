using System;
using UnityEngine.Scripting;

// Token: 0x02000D81 RID: 3457
[Preserve]
public class ItemActionEntryShowCosmetics : BaseItemActionEntry
{
	// Token: 0x0600685B RID: 26715 RVA: 0x002970B7 File Offset: 0x002952B7
	public ItemActionEntryShowCosmetics(XUiController _controller)
		: base(_controller, "xuiCosmetics", "ui_game_symbol_wardrobe", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x0600685C RID: 26716 RVA: 0x0029570E File Offset: 0x0029390E
	public override void RefreshEnabled()
	{
		base.Enabled = true;
	}

	// Token: 0x0600685D RID: 26717 RVA: 0x002970D8 File Offset: 0x002952D8
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
