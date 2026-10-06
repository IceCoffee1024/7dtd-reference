using System;
using UnityEngine.Scripting;

// Token: 0x02000F3B RID: 3899
[Preserve]
public class XUiC_DroneWindowGroup : XUiController
{
	// Token: 0x17000E5C RID: 3676
	// (get) Token: 0x060079DE RID: 31198 RVA: 0x002E18E1 File Offset: 0x002DFAE1
	// (set) Token: 0x060079DF RID: 31199 RVA: 0x002E18EC File Offset: 0x002DFAEC
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

	// Token: 0x060079E0 RID: 31200 RVA: 0x002E1988 File Offset: 0x002DFB88
	public override void Init()
	{
		base.Init();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		this.assembleWindow = base.GetChildByType<XUiC_AssembleDroneWindow>();
		this.grid = base.GetChildByType<XUiC_ItemDronePartStackGrid>();
		this.cosmeticGrid = base.GetChildByType<XUiC_ItemCosmeticStackGrid>();
	}

	// Token: 0x060079E1 RID: 31201 RVA: 0x002E19C0 File Offset: 0x002DFBC0
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

	// Token: 0x060079E2 RID: 31202 RVA: 0x002E1A98 File Offset: 0x002DFC98
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

	// Token: 0x060079E3 RID: 31203 RVA: 0x002E1AF1 File Offset: 0x002DFCF1
	public void OnItemChanged(ItemStack itemStack)
	{
		this.grid.CurrentItem = itemStack;
		this.cosmeticGrid.CurrentItem = itemStack;
	}

	// Token: 0x04005A44 RID: 23108
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_AssembleDroneWindow assembleWindow;

	// Token: 0x04005A45 RID: 23109
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x04005A46 RID: 23110
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemDronePartStackGrid grid;

	// Token: 0x04005A47 RID: 23111
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemCosmeticStackGrid cosmeticGrid;

	// Token: 0x04005A48 RID: 23112
	public static string ID = "junkDrone";

	// Token: 0x04005A49 RID: 23113
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityDrone currentVehicleEntity;
}
