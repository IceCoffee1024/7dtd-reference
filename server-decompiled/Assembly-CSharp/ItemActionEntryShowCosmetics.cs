using System;
using UnityEngine.Scripting;

// Token: 0x02000D52 RID: 3410
[Preserve]
public class ItemActionEntryShowCosmetics : BaseItemActionEntry
{
	// Token: 0x060067A0 RID: 26528 RVA: 0x0028F64B File Offset: 0x0028D84B
	public ItemActionEntryShowCosmetics(XUiController _controller)
		: base(_controller, "xuiCosmetics", "ui_game_symbol_wardrobe", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x060067A1 RID: 26529 RVA: 0x0028DCCE File Offset: 0x0028BECE
	public override void RefreshEnabled()
	{
		base.Enabled = true;
	}

	// Token: 0x060067A2 RID: 26530 RVA: 0x0028F66C File Offset: 0x0028D86C
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
