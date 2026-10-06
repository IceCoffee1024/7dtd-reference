using System;
using UnityEngine.Scripting;

// Token: 0x02000D69 RID: 3433
[Preserve]
public class ItemActionEntryEquip : BaseItemActionEntry
{
	// Token: 0x0600680B RID: 26635 RVA: 0x00293680 File Offset: 0x00291880
	public ItemActionEntryEquip(XUiController _controller)
		: base(_controller, "lblContextActionEquip", "ui_game_symbol_knife", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x0600680C RID: 26636 RVA: 0x002936A0 File Offset: 0x002918A0
	public override void RefreshEnabled()
	{
		XUi xui = base.ItemController.xui;
		EntityPlayerLocal entityPlayer = xui.playerUI.entityPlayer;
		Inventory toolbelt = xui.PlayerInventory.Toolbelt;
		base.Enabled = !entityPlayer.AttachedToEntity && !toolbelt.IsHandBusy();
	}

	// Token: 0x0600680D RID: 26637 RVA: 0x002936F0 File Offset: 0x002918F0
	public override void OnActivated()
	{
		XUi xui = base.ItemController.xui;
		XUiM_PlayerInventory playerInventory = xui.PlayerInventory;
		Inventory toolbelt = playerInventory.Toolbelt;
		int focusedItemIdx = playerInventory.Toolbelt.GetFocusedItemIdx();
		ItemStack itemStack = playerInventory.Toolbelt.GetItem(focusedItemIdx).Clone();
		ItemStack itemStack2 = ((IItemStackController)base.ItemController).ItemStack.Clone();
		if (xui.AssembleItem.CurrentItemStackController != null)
		{
			XUiC_ItemStack currentItemStackController = xui.AssembleItem.CurrentItemStackController;
			if (currentItemStackController.StackLocation == XUiC_ItemStack.StackLocationTypes.ToolBelt && currentItemStackController.SlotNumber == focusedItemIdx)
			{
				return;
			}
		}
		if (itemStack.Equals(itemStack2) || (!itemStack.IsEmpty() && itemStack.itemValue.ItemClass.IsQuestItem))
		{
			return;
		}
		toolbelt.SetItem(focusedItemIdx, new ItemStack(new ItemValue(ItemClass.GetItem("UselessThing", false).GetItemId(), false), 1));
		toolbelt.Hand.OnUpdate();
		((IItemStackController)base.ItemController).ItemStack = ItemStack.Empty;
		if (!xui.PlayerInventory.AddItem(itemStack))
		{
			((IItemStackController)base.ItemController).ItemStack = itemStack;
		}
		toolbelt.SetItem(focusedItemIdx, itemStack2);
		toolbelt.Hand.OnUpdate();
		toolbelt.SetHoldingItemIdx(focusedItemIdx);
	}
}
