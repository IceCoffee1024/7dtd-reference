using System;
using UnityEngine.Scripting;

// Token: 0x02000D54 RID: 3412
[Preserve]
public class ItemActionEntryTake : BaseItemActionEntry
{
	// Token: 0x060067A6 RID: 26534 RVA: 0x0028F81D File Offset: 0x0028DA1D
	public ItemActionEntryTake(XUiController _controller)
		: base(_controller, "lblContextActionTake", "ui_game_symbol_hand", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x060067A7 RID: 26535 RVA: 0x0028F83C File Offset: 0x0028DA3C
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

	// Token: 0x060067A8 RID: 26536 RVA: 0x0028F894 File Offset: 0x0028DA94
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack getItemStack()
	{
		XUiController itemController = base.ItemController;
		XUiC_ItemStack xuiC_ItemStack = itemController as XUiC_ItemStack;
		ItemStack itemStack;
		if (xuiC_ItemStack == null)
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
			itemStack = xuiC_ItemStack.ItemStack;
		}
		return itemStack;
	}

	// Token: 0x060067A9 RID: 26537 RVA: 0x0028F8EC File Offset: 0x0028DAEC
	public override void OnActivated()
	{
		XUiController itemController = base.ItemController;
		XUiC_ItemStack xuiC_ItemStack = itemController as XUiC_ItemStack;
		if (xuiC_ItemStack != null)
		{
			xuiC_ItemStack.HandleMoveToPreferredLocation();
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
