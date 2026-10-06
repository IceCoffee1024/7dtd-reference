using System;
using UnityEngine.Scripting;

// Token: 0x02000D65 RID: 3429
[Preserve]
public class ItemActionEntryCombine : BaseItemActionEntry
{
	// Token: 0x060067FE RID: 26622 RVA: 0x00292C48 File Offset: 0x00290E48
	public ItemActionEntryCombine(XUiController _controller)
		: base(_controller, "xuiCombine", "ui_game_symbol_add", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x060067FF RID: 26623 RVA: 0x00292C68 File Offset: 0x00290E68
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

	// Token: 0x06006800 RID: 26624 RVA: 0x00292CA4 File Offset: 0x00290EA4
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
