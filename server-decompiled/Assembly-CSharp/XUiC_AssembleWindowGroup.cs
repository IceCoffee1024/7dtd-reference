using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EC1 RID: 3777
[Preserve]
public class XUiC_AssembleWindowGroup : XUiController
{
	// Token: 0x17000DE5 RID: 3557
	// (get) Token: 0x06007608 RID: 30216 RVA: 0x002C4F80 File Offset: 0x002C3180
	// (set) Token: 0x06007609 RID: 30217 RVA: 0x002C4F88 File Offset: 0x002C3188
	public ItemStack ItemStack
	{
		get
		{
			return this.itemStack;
		}
		set
		{
			this.itemStack = value;
			this.assembleWindow.ItemStack = value;
			this.grid.CurrentItem = value;
			this.grid.SetParts(this.itemStack.itemValue.Modifications);
			this.grid.AssembleWindow = this.assembleWindow;
			this.cosmeticGrid.CurrentItem = value;
			this.cosmeticGrid.SetParts(this.itemStack.itemValue.CosmeticMods);
			this.cosmeticGrid.AssembleWindow = this.assembleWindow;
		}
	}

	// Token: 0x0600760A RID: 30218 RVA: 0x002C5018 File Offset: 0x002C3218
	public override void Init()
	{
		base.Init();
		this.assembleWindow = base.GetChildByType<XUiC_AssembleWindow>();
		this.grid = base.GetChildByType<XUiC_ItemPartStackGrid>();
		this.cosmeticGrid = base.GetChildByType<XUiC_ItemCosmeticStackGrid>();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
	}

	// Token: 0x0600760B RID: 30219 RVA: 0x002C5050 File Offset: 0x002C3250
	public override void OnOpen()
	{
		this.ItemStack = this.xui.AssembleItem.CurrentItem;
		base.OnOpen();
		GUIWindowManager windowManager = this.xui.playerUI.windowManager;
		if (this.nonPagingHeaderWindow != null)
		{
			this.nonPagingHeaderWindow.SetHeader(Localization.Get("lblContextActionModify", false, null));
		}
		windowManager.Close("windowpaging");
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		if (this.xui.AssembleItem.CurrentItemStackController != null)
		{
			this.xui.AssembleItem.CurrentItemStackController.IsSelected = true;
			childByType.SetItemStack(this.xui.AssembleItem.CurrentItemStackController, true);
			this.openEquipmentOnClose = false;
			return;
		}
		if (this.xui.AssembleItem.CurrentEquipmentStackController != null)
		{
			this.xui.AssembleItem.CurrentEquipmentStackController.IsSelected = false;
			childByType.SetItemStack(this.xui.AssembleItem.CurrentEquipmentStackController, true);
			this.openEquipmentOnClose = true;
		}
	}

	// Token: 0x0600760C RID: 30220 RVA: 0x002C514B File Offset: 0x002C334B
	public override void OnClose()
	{
		base.OnClose();
		XUiM_AssembleItem assembleItem = this.xui.AssembleItem;
		assembleItem.CurrentItem = null;
		assembleItem.CurrentItemStackController = null;
		assembleItem.CurrentEquipmentStackController = null;
		GameManager.Instance.StartCoroutine(this.showCraftingLater());
	}

	// Token: 0x0600760D RID: 30221 RVA: 0x002C5183 File Offset: 0x002C3383
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator showCraftingLater()
	{
		yield return new WaitForEndOfFrame();
		if (this.xui != null && this.xui.playerUI != null && this.xui.playerUI.entityPlayer != null)
		{
			XUiC_WindowSelector.OpenSelectorAndWindow(this.xui.playerUI.entityPlayer, this.openEquipmentOnClose ? "character" : "crafting");
		}
		yield break;
	}

	// Token: 0x0600760E RID: 30222 RVA: 0x002C5192 File Offset: 0x002C3392
	public static XUiC_AssembleWindowGroup GetWindowGroup(XUi _xuiInstance)
	{
		return _xuiInstance.FindWindowGroupByName(XUiC_AssembleWindowGroup.ID) as XUiC_AssembleWindowGroup;
	}

	// Token: 0x04005747 RID: 22343
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_AssembleWindow assembleWindow;

	// Token: 0x04005748 RID: 22344
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemPartStackGrid grid;

	// Token: 0x04005749 RID: 22345
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemCosmeticStackGrid cosmeticGrid;

	// Token: 0x0400574A RID: 22346
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x0400574B RID: 22347
	public static string ID = "assemble";

	// Token: 0x0400574C RID: 22348
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openEquipmentOnClose;

	// Token: 0x0400574D RID: 22349
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack itemStack;
}
