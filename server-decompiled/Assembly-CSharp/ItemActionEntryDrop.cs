using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D57 RID: 3415
[Preserve]
public class ItemActionEntryDrop : BaseItemActionEntry
{
	// Token: 0x06006800 RID: 26624 RVA: 0x0028FFC8 File Offset: 0x0028E1C8
	public ItemActionEntryDrop(XUiController _controller)
		: base(_controller, "lblContextActionDrop", "ui_game_symbol_drop", BaseItemActionEntry.GamepadShortCut.DPadDown, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x06006801 RID: 26625 RVA: 0x0028FFE8 File Offset: 0x0028E1E8
	public override void OnActivated()
	{
		LocalPlayerUI playerUI = base.ItemController.xui.playerUI;
		XUiC_ItemStack xuiC_ItemStack = (XUiC_ItemStack)base.ItemController;
		base.ItemController.xui.CollectedItemList.RemoveItemStack(xuiC_ItemStack.ItemStack);
		GameManager.Instance.ItemDropServer(xuiC_ItemStack.ItemStack, playerUI.entityPlayer.GetDropPosition(), Vector3.zero, playerUI.entityPlayer.entityId, 60f, false);
		playerUI.entityPlayer.PlayOneShot("itemdropped", false, false, false, null, 1f);
		xuiC_ItemStack.ItemStack = ItemStack.Empty;
	}

	// Token: 0x06006802 RID: 26626 RVA: 0x00290084 File Offset: 0x0028E284
	public override void RefreshEnabled()
	{
		XUiC_ItemStack xuiC_ItemStack = (XUiC_ItemStack)base.ItemController;
		base.Enabled = !xuiC_ItemStack.ItemStack.IsEmpty() && xuiC_ItemStack.ItemStack.itemValue.ItemClass.CanDrop(null) && !xuiC_ItemStack.StackLock;
	}

	// Token: 0x06006803 RID: 26627 RVA: 0x002900D4 File Offset: 0x0028E2D4
	public override void OnDisabledActivate()
	{
		GameManager.ShowTooltip(base.ItemController.xui.playerUI.entityPlayer, "This item cannot be dropped.", false, false, 0f);
	}
}
