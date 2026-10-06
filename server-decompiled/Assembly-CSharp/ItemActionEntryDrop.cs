using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D68 RID: 3432
[Preserve]
public class ItemActionEntryDrop : BaseItemActionEntry
{
	// Token: 0x06006807 RID: 26631 RVA: 0x00293530 File Offset: 0x00291730
	public ItemActionEntryDrop(XUiController _controller)
		: base(_controller, "lblContextActionDrop", "ui_game_symbol_drop", BaseItemActionEntry.GamepadShortCut.DPadDown, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x06006808 RID: 26632 RVA: 0x00293550 File Offset: 0x00291750
	public override void OnActivated()
	{
		LocalPlayerUI playerUI = base.ItemController.xui.playerUI;
		IItemStackController itemStackController = (IItemStackController)base.ItemController;
		base.ItemController.xui.CollectedItemList.RemoveItemStack(itemStackController.ItemStack);
		GameManager.Instance.ItemDropServer(itemStackController.ItemStack, playerUI.entityPlayer.GetDropPosition(), Vector3.zero, playerUI.entityPlayer.entityId, 60f, false);
		playerUI.entityPlayer.PlayOneShot("itemdropped", false, false, false, null, 1f);
		itemStackController.ItemStack = ItemStack.Empty;
	}

	// Token: 0x06006809 RID: 26633 RVA: 0x002935EC File Offset: 0x002917EC
	public override void RefreshEnabled()
	{
		IItemStackController itemStackController = (IItemStackController)base.ItemController;
		bool flag;
		if (!itemStackController.ItemStack.IsEmpty() && itemStackController.ItemStack.itemValue.ItemClass.CanDrop(itemStackController.ItemStack.itemValue))
		{
			XUiC_ItemStack xuiC_ItemStack = base.ItemController as XUiC_ItemStack;
			flag = xuiC_ItemStack == null || !xuiC_ItemStack.StackLock;
		}
		else
		{
			flag = false;
		}
		base.Enabled = flag;
	}

	// Token: 0x0600680A RID: 26634 RVA: 0x00293658 File Offset: 0x00291858
	public override void OnDisabledActivate()
	{
		GameManager.ShowTooltip(base.ItemController.xui.playerUI.entityPlayer, "This item cannot be dropped.", false, false, 0f);
	}
}
