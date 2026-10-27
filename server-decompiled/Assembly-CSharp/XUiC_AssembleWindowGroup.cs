using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EDB RID: 3803
[Preserve]
public class XUiC_AssembleWindowGroup : XUiController
{
	// Token: 0x17000E06 RID: 3590
	// (get) Token: 0x06007625 RID: 30245 RVA: 0x002C98A4 File Offset: 0x002C7AA4
	// (set) Token: 0x06007626 RID: 30246 RVA: 0x002C98AC File Offset: 0x002C7AAC
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

	// Token: 0x06007627 RID: 30247 RVA: 0x002C9906 File Offset: 0x002C7B06
	public override void Init()
	{
		base.Init();
		this.assembleWindow = base.GetChildByType<XUiC_AssembleWindow>();
		this.grid = base.GetChildByType<XUiC_ItemPartStackGrid>();
		this.cosmeticGrid = base.GetChildByType<XUiC_ItemCosmeticStackGrid>();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
	}

	// Token: 0x06007628 RID: 30248 RVA: 0x002C9940 File Offset: 0x002C7B40
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

	// Token: 0x06007629 RID: 30249 RVA: 0x002C9A3B File Offset: 0x002C7C3B
	public override void OnClose()
	{
		base.OnClose();
		XUiM_AssembleItem assembleItem = this.xui.AssembleItem;
		assembleItem.CurrentItem = null;
		assembleItem.CurrentItemStackController = null;
		assembleItem.CurrentEquipmentStackController = null;
		GameManager.Instance.StartCoroutine(this.showCraftingLater());
	}

	// Token: 0x0600762A RID: 30250 RVA: 0x002C9A73 File Offset: 0x002C7C73
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

	// Token: 0x0600762B RID: 30251 RVA: 0x002C9A82 File Offset: 0x002C7C82
	public static XUiC_AssembleWindowGroup GetWindowGroup(XUi _xuiInstance)
	{
		return _xuiInstance.FindWindowGroupByName(XUiC_AssembleWindowGroup.ID) as XUiC_AssembleWindowGroup;
	}

	// Token: 0x040057A3 RID: 22435
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_AssembleWindow assembleWindow;

	// Token: 0x040057A4 RID: 22436
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemPartStackGrid grid;

	// Token: 0x040057A5 RID: 22437
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemCosmeticStackGrid cosmeticGrid;

	// Token: 0x040057A6 RID: 22438
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x040057A7 RID: 22439
	public static string ID = "assemble";

	// Token: 0x040057A8 RID: 22440
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openEquipmentOnClose;

	// Token: 0x040057A9 RID: 22441
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack itemStack;
}
