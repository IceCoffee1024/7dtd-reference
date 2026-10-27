using System;
using UnityEngine.Scripting;

// Token: 0x02000F4A RID: 3914
[Preserve]
public class XUiC_DroneWindowGroup : XUiController
{
	// Token: 0x17000E66 RID: 3686
	// (get) Token: 0x06007A21 RID: 31265 RVA: 0x002E31D1 File Offset: 0x002E13D1
	// (set) Token: 0x06007A22 RID: 31266 RVA: 0x002E31DC File Offset: 0x002E13DC
	public EntityDrone CurrentVehicleEntity
	{
		get
		{
			return this.currentVehicleEntity;
		}
		set
		{
			this.currentVehicleEntity = value;
			this.assembleWindow.group = this;
			ItemStack itemStack = new ItemStack(this.currentVehicleEntity.GetUpdatedItemValue(), 1);
			this.assembleWindow.ItemStack = itemStack;
			this.grid.AssembleWindow = this.assembleWindow;
			this.grid.CurrentItem = itemStack;
			this.cosmeticGrid.AssembleWindow = this.assembleWindow;
			this.cosmeticGrid.CurrentItem = itemStack;
			XUiM_AssembleItem assembleItem = this.xui.AssembleItem;
			assembleItem.AssembleWindow = this.assembleWindow;
			assembleItem.CurrentItem = itemStack;
			assembleItem.CurrentItemStackController = null;
		}
	}

	// Token: 0x06007A23 RID: 31267 RVA: 0x002E3278 File Offset: 0x002E1478
	public override void Init()
	{
		base.Init();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		this.assembleWindow = base.GetChildByType<XUiC_AssembleDroneWindow>();
		this.grid = base.GetChildByType<XUiC_ItemDronePartStackGrid>();
		this.cosmeticGrid = base.GetChildByType<XUiC_ItemCosmeticStackGrid>();
	}

	// Token: 0x06007A24 RID: 31268 RVA: 0x002E32B0 File Offset: 0x002E14B0
	public override void OnOpen()
	{
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
			return;
		}
		if (this.xui.AssembleItem.CurrentEquipmentStackController != null)
		{
			this.xui.AssembleItem.CurrentEquipmentStackController.IsSelected = false;
			childByType.SetItemStack(this.xui.AssembleItem.CurrentEquipmentStackController, true);
		}
	}

	// Token: 0x06007A25 RID: 31269 RVA: 0x002E3388 File Offset: 0x002E1588
	public override void OnClose()
	{
		base.OnClose();
		XUiM_AssembleItem assembleItem = this.xui.AssembleItem;
		assembleItem.AssembleWindow = null;
		if (assembleItem.CurrentItem.itemValue == this.CurrentVehicleEntity.GetUpdatedItemValue())
		{
			assembleItem.CurrentItem = null;
			assembleItem.CurrentItemStackController = null;
		}
		this.CurrentVehicleEntity.StopUIInteraction();
	}

	// Token: 0x06007A26 RID: 31270 RVA: 0x002E33E1 File Offset: 0x002E15E1
	public void OnItemChanged(ItemStack itemStack)
	{
		this.grid.CurrentItem = itemStack;
		this.cosmeticGrid.CurrentItem = itemStack;
	}

	// Token: 0x04005A77 RID: 23159
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_AssembleDroneWindow assembleWindow;

	// Token: 0x04005A78 RID: 23160
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x04005A79 RID: 23161
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemDronePartStackGrid grid;

	// Token: 0x04005A7A RID: 23162
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemCosmeticStackGrid cosmeticGrid;

	// Token: 0x04005A7B RID: 23163
	public static string ID = "junkDrone";

	// Token: 0x04005A7C RID: 23164
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityDrone currentVehicleEntity;
}
