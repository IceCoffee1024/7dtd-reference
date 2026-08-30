using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EAC RID: 3756
[Preserve]
public class XUiC_AssembleWindowGroup : XUiController
{
	// Token: 0x17000DCB RID: 3531
	// (get) Token: 0x06007567 RID: 30055 RVA: 0x002C1A5C File Offset: 0x002BFC5C
	// (set) Token: 0x06007568 RID: 30056 RVA: 0x002C1A64 File Offset: 0x002BFC64
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

	// Token: 0x06007569 RID: 30057 RVA: 0x002C1AF4 File Offset: 0x002BFCF4
	public override void Init()
	{
		base.Init();
		this.assembleWindow = base.GetChildByType<XUiC_AssembleWindow>();
		this.grid = base.GetChildByType<XUiC_ItemPartStackGrid>();
		this.cosmeticGrid = base.GetChildByType<XUiC_ItemCosmeticStackGrid>();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
	}

	// Token: 0x0600756A RID: 30058 RVA: 0x002C1B2C File Offset: 0x002BFD2C
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

	// Token: 0x0600756B RID: 30059 RVA: 0x002C1C27 File Offset: 0x002BFE27
	public override void OnClose()
	{
		base.OnClose();
		XUiM_AssembleItem assembleItem = this.xui.AssembleItem;
		assembleItem.CurrentItem = null;
		assembleItem.CurrentItemStackController = null;
		assembleItem.CurrentEquipmentStackController = null;
		GameManager.Instance.StartCoroutine(this.showCraftingLater());
	}

	// Token: 0x0600756C RID: 30060 RVA: 0x002C1C5F File Offset: 0x002BFE5F
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

	// Token: 0x0600756D RID: 30061 RVA: 0x002C1C6E File Offset: 0x002BFE6E
	public static XUiC_AssembleWindowGroup GetWindowGroup(XUi _xuiInstance)
	{
		return _xuiInstance.FindWindowGroupByName(XUiC_AssembleWindowGroup.ID) as XUiC_AssembleWindowGroup;
	}

	// Token: 0x040056A2 RID: 22178
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_AssembleWindow assembleWindow;

	// Token: 0x040056A3 RID: 22179
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemPartStackGrid grid;

	// Token: 0x040056A4 RID: 22180
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemCosmeticStackGrid cosmeticGrid;

	// Token: 0x040056A5 RID: 22181
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x040056A6 RID: 22182
	public static string ID = "assemble";

	// Token: 0x040056A7 RID: 22183
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openEquipmentOnClose;

	// Token: 0x040056A8 RID: 22184
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack itemStack;
}
