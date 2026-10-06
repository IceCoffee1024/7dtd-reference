using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000ED0 RID: 3792
[Preserve]
public class XUiC_AssembleWindowGroup : XUiController
{
	// Token: 0x17000E00 RID: 3584
	// (get) Token: 0x06007600 RID: 30208 RVA: 0x002C8338 File Offset: 0x002C6538
	// (set) Token: 0x06007601 RID: 30209 RVA: 0x002C8340 File Offset: 0x002C6540
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
			this.grid.AssembleWindow = this.assembleWindow;
			this.cosmeticGrid.CurrentItem = value;
			this.cosmeticGrid.AssembleWindow = this.assembleWindow;
		}
	}

	// Token: 0x06007602 RID: 30210 RVA: 0x002C839A File Offset: 0x002C659A
	public override void Init()
	{
		base.Init();
		this.assembleWindow = base.GetChildByType<XUiC_AssembleWindow>();
		this.grid = base.GetChildByType<XUiC_ItemPartStackGrid>();
		this.cosmeticGrid = base.GetChildByType<XUiC_ItemCosmeticStackGrid>();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
	}

	// Token: 0x06007603 RID: 30211 RVA: 0x002C83D4 File Offset: 0x002C65D4
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

	// Token: 0x06007604 RID: 30212 RVA: 0x002C84CF File Offset: 0x002C66CF
	public override void OnClose()
	{
		base.OnClose();
		XUiM_AssembleItem assembleItem = this.xui.AssembleItem;
		assembleItem.CurrentItem = null;
		assembleItem.CurrentItemStackController = null;
		assembleItem.CurrentEquipmentStackController = null;
		GameManager.Instance.StartCoroutine(this.showCraftingLater());
	}

	// Token: 0x06007605 RID: 30213 RVA: 0x002C8507 File Offset: 0x002C6707
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

	// Token: 0x06007606 RID: 30214 RVA: 0x002C8516 File Offset: 0x002C6716
	public static XUiC_AssembleWindowGroup GetWindowGroup(XUi _xuiInstance)
	{
		return _xuiInstance.FindWindowGroupByName(XUiC_AssembleWindowGroup.ID) as XUiC_AssembleWindowGroup;
	}

	// Token: 0x0400577E RID: 22398
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_AssembleWindow assembleWindow;

	// Token: 0x0400577F RID: 22399
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemPartStackGrid grid;

	// Token: 0x04005780 RID: 22400
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemCosmeticStackGrid cosmeticGrid;

	// Token: 0x04005781 RID: 22401
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x04005782 RID: 22402
	public static string ID = "assemble";

	// Token: 0x04005783 RID: 22403
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openEquipmentOnClose;

	// Token: 0x04005784 RID: 22404
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack itemStack;
}
