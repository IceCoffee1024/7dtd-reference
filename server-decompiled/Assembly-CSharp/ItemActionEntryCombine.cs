using System;
using UnityEngine.Scripting;

// Token: 0x02000D54 RID: 3412
[Preserve]
public class ItemActionEntryCombine : BaseItemActionEntry
{
	// Token: 0x060067F7 RID: 26615 RVA: 0x0028F6E0 File Offset: 0x0028D8E0
	public ItemActionEntryCombine(XUiController _controller)
		: base(_controller, "xuiCombine", "ui_game_symbol_add", BaseItemActionEntry.GamepadShortCut.DPadUp, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x060067F8 RID: 26616 RVA: 0x0028F700 File Offset: 0x0028D900
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

	// Token: 0x060067F9 RID: 26617 RVA: 0x0028F73C File Offset: 0x0028D93C
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
