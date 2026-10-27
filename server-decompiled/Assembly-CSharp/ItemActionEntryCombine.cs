using System;
using UnityEngine.Scripting;

// Token: 0x02000D70 RID: 3440
[Preserve]
public class ItemActionEntryCombine : BaseItemActionEntry
{
	// Token: 0x06006823 RID: 26659 RVA: 0x00294170 File Offset: 0x00292370
	public ItemActionEntryCombine(XUiController _controller)
		: base(_controller, "xuiCombine", "ui_game_symbol_add", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x06006824 RID: 26660 RVA: 0x00294190 File Offset: 0x00292390
	public override void RefreshEnabled()
	{
		XUi xui = base.ItemController.xui;
		if (xui.CurrentCombineGrid == null)
		{
			base.Enabled = false;
			return;
		}
		base.Enabled = xui.CurrentCombineGrid.HasEmptySlot();
	}

	// Token: 0x06006825 RID: 26661 RVA: 0x002941CC File Offset: 0x002923CC
	public override void OnActivated()
	{
		XUi xui = base.ItemController.xui;
		if (xui.CurrentCombineGrid == null)
		{
			return;
		}
		XUiC_ItemStack xuiC_ItemStack = (XUiC_ItemStack)base.ItemController;
		if (xui.CurrentCombineGrid.TryAddItemToSlot(xuiC_ItemStack.ItemStack.itemValue.ItemClass, xuiC_ItemStack.ItemStack))
		{
			xuiC_ItemStack.ItemStack = ItemStack.Empty;
		}
	}
}
