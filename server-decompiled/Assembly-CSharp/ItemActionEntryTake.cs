using System;
using UnityEngine.Scripting;

// Token: 0x02000D83 RID: 3459
[Preserve]
public class ItemActionEntryTake : BaseItemActionEntry
{
	// Token: 0x06006861 RID: 26721 RVA: 0x00297289 File Offset: 0x00295489
	public ItemActionEntryTake(XUiController _controller)
		: base(_controller, "lblContextActionTake", "ui_game_symbol_hand", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x06006862 RID: 26722 RVA: 0x002972A8 File Offset: 0x002954A8
	public override void RefreshEnabled()
	{
		ItemStack itemStack = this.getItemStack();
		if (itemStack == null || itemStack.itemValue.IsEmpty())
		{
			base.Enabled = false;
			return;
		}
		int num = base.ItemController.xui.PlayerInventory.CountAvailableSpaceForItem(itemStack.itemValue, true);
		base.Enabled = num >= 1;
	}

	// Token: 0x06006863 RID: 26723 RVA: 0x00297300 File Offset: 0x00295500
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack getItemStack()
	{
		XUiController itemController = base.ItemController;
		IItemStackController itemStackController = itemController as IItemStackController;
		ItemStack itemStack;
		if (itemStackController == null)
		{
			XUiC_EquipmentStack xuiC_EquipmentStack = itemController as XUiC_EquipmentStack;
			if (xuiC_EquipmentStack == null)
			{
				XUiC_BasePartStack xuiC_BasePartStack = itemController as XUiC_BasePartStack;
				if (xuiC_BasePartStack == null)
				{
					itemStack = null;
				}
				else
				{
					itemStack = xuiC_BasePartStack.ItemStack;
				}
			}
			else
			{
				itemStack = xuiC_EquipmentStack.ItemStack;
			}
		}
		else
		{
			itemStack = itemStackController.ItemStack;
		}
		return itemStack;
	}

	// Token: 0x06006864 RID: 26724 RVA: 0x00297358 File Offset: 0x00295558
	public override void OnActivated()
	{
		XUiController itemController = base.ItemController;
		IItemStackController itemStackController = itemController as IItemStackController;
		if (itemStackController != null)
		{
			itemStackController.HandleMoveToPreferredLocation();
			return;
		}
		XUiC_EquipmentStack xuiC_EquipmentStack = itemController as XUiC_EquipmentStack;
		if (xuiC_EquipmentStack != null)
		{
			xuiC_EquipmentStack.HandleMoveToPreferredLocation();
			return;
		}
		XUiC_BasePartStack xuiC_BasePartStack = itemController as XUiC_BasePartStack;
		if (xuiC_BasePartStack == null)
		{
			return;
		}
		xuiC_BasePartStack.HandleMoveToPreferredLocation();
	}
}
