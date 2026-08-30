using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D44 RID: 3396
[Preserve]
public class ItemActionEntryDrop : BaseItemActionEntry
{
	// Token: 0x06006771 RID: 26481 RVA: 0x0028D02C File Offset: 0x0028B22C
	public ItemActionEntryDrop(XUiController _controller)
		: base(_controller, "lblContextActionDrop", "ui_game_symbol_drop", BaseItemActionEntry.GamepadShortCut.DPadDown, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x06006772 RID: 26482 RVA: 0x0028D04C File Offset: 0x0028B24C
	public override void OnActivated()
	{
		LocalPlayerUI playerUI = base.ItemController.xui.playerUI;
		XUiC_ItemStack xuiC_ItemStack = (XUiC_ItemStack)base.ItemController;
		base.ItemController.xui.CollectedItemList.RemoveItemStack(xuiC_ItemStack.ItemStack);
		GameManager.Instance.ItemDropServer(xuiC_ItemStack.ItemStack, playerUI.entityPlayer.GetDropPosition(), Vector3.zero, playerUI.entityPlayer.entityId, 60f, false);
		playerUI.entityPlayer.PlayOneShot("itemdropped", false, false, false, null, 1f);
		xuiC_ItemStack.ItemStack = ItemStack.Empty;
	}

	// Token: 0x06006773 RID: 26483 RVA: 0x0028D0E8 File Offset: 0x0028B2E8
	public override void RefreshEnabled()
	{
		XUiC_ItemStack xuiC_ItemStack = (XUiC_ItemStack)base.ItemController;
		base.Enabled = !xuiC_ItemStack.ItemStack.IsEmpty() && xuiC_ItemStack.ItemStack.itemValue.ItemClass.CanDrop(null) && !xuiC_ItemStack.StackLock;
	}

	// Token: 0x06006774 RID: 26484 RVA: 0x0028D138 File Offset: 0x0028B338
	public override void OnDisabledActivate()
	{
		GameManager.ShowTooltip(base.ItemController.xui.playerUI.entityPlayer, "This item cannot be dropped.", false, false, 0f);
	}
}
