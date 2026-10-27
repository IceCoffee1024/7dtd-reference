using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D73 RID: 3443
[Preserve]
public class ItemActionEntryDrop : BaseItemActionEntry
{
	// Token: 0x0600682C RID: 26668 RVA: 0x00294A58 File Offset: 0x00292C58
	public ItemActionEntryDrop(XUiController _controller)
		: base(_controller, "lblContextActionDrop", "ui_game_symbol_drop", BaseItemActionEntry.GamepadShortCut.DPadDown, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x0600682D RID: 26669 RVA: 0x00294A78 File Offset: 0x00292C78
	public override void OnActivated()
	{
		LocalPlayerUI playerUI = base.ItemController.xui.playerUI;
		IItemStackController itemStackController = (IItemStackController)base.ItemController;
		base.ItemController.xui.CollectedItemList.RemoveItemStack(itemStackController.ItemStack);
		GameManager.Instance.ItemDropServer(itemStackController.ItemStack, playerUI.entityPlayer.GetDropPosition(), Vector3.zero, playerUI.entityPlayer.entityId, 60f, false);
		playerUI.entityPlayer.PlayOneShot("itemdropped", false, false, false, null, 1f);
		itemStackController.ItemStack = ItemStack.Empty;
	}

	// Token: 0x0600682E RID: 26670 RVA: 0x00294B14 File Offset: 0x00292D14
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

	// Token: 0x0600682F RID: 26671 RVA: 0x00294B80 File Offset: 0x00292D80
	public override void OnDisabledActivate()
	{
		GameManager.ShowTooltip(base.ItemController.xui.playerUI.entityPlayer, "This item cannot be dropped.", false, false, 0f);
	}
}
