using System;
using UnityEngine.Scripting;

// Token: 0x02000D78 RID: 3448
[Preserve]
public class ItemActionEntryTake : BaseItemActionEntry
{
	// Token: 0x0600683C RID: 26684 RVA: 0x00295D61 File Offset: 0x00293F61
	public ItemActionEntryTake(XUiController _controller)
		: base(_controller, "lblContextActionTake", "ui_game_symbol_hand", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x0600683D RID: 26685 RVA: 0x00295D80 File Offset: 0x00293F80
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

	// Token: 0x0600683E RID: 26686 RVA: 0x00295DD8 File Offset: 0x00293FD8
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

	// Token: 0x0600683F RID: 26687 RVA: 0x00295E30 File Offset: 0x00294030
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
