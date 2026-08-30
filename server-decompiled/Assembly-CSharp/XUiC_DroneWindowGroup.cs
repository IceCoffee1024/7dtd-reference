using System;
using UnityEngine.Scripting;

// Token: 0x02000F2C RID: 3884
[Preserve]
public class XUiC_DroneWindowGroup : XUiController
{
	// Token: 0x17000E3F RID: 3647
	// (get) Token: 0x060079E1 RID: 31201 RVA: 0x002DE515 File Offset: 0x002DC715
	// (set) Token: 0x060079E2 RID: 31202 RVA: 0x002DE520 File Offset: 0x002DC720
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
			ItemValue updatedItemValue = this.currentVehicleEntity.GetUpdatedItemValue();
			ItemStack itemStack = new ItemStack(updatedItemValue, 1);
			this.assembleWindow.ItemStack = itemStack;
			this.grid.AssembleWindow = this.assembleWindow;
			this.grid.CurrentItem = itemStack;
			this.grid.SetParts(updatedItemValue.Modifications);
			this.cosmeticGrid.AssembleWindow = this.assembleWindow;
			this.cosmeticGrid.CurrentItem = itemStack;
			this.cosmeticGrid.SetParts(updatedItemValue.CosmeticMods);
			XUiM_AssembleItem assembleItem = this.xui.AssembleItem;
			assembleItem.AssembleWindow = this.assembleWindow;
			assembleItem.CurrentItem = itemStack;
			assembleItem.CurrentItemStackController = null;
		}
	}

	// Token: 0x060079E3 RID: 31203 RVA: 0x002DE5E0 File Offset: 0x002DC7E0
	public override void Init()
	{
		base.Init();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		this.assembleWindow = base.GetChildByType<XUiC_AssembleDroneWindow>();
		this.grid = base.GetChildByType<XUiC_ItemDronePartStackGrid>();
		this.cosmeticGrid = base.GetChildByType<XUiC_ItemCosmeticStackGrid>();
	}

	// Token: 0x060079E4 RID: 31204 RVA: 0x002DE618 File Offset: 0x002DC818
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

	// Token: 0x060079E5 RID: 31205 RVA: 0x002DE6F0 File Offset: 0x002DC8F0
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

	// Token: 0x060079E6 RID: 31206 RVA: 0x002DE74C File Offset: 0x002DC94C
	public void OnItemChanged(ItemStack itemStack)
	{
		this.grid.CurrentItem = itemStack;
		this.grid.SetParts(itemStack.itemValue.Modifications);
		this.cosmeticGrid.CurrentItem = itemStack;
		this.cosmeticGrid.SetParts(itemStack.itemValue.CosmeticMods);
	}

	// Token: 0x04005A0C RID: 23052
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_AssembleDroneWindow assembleWindow;

	// Token: 0x04005A0D RID: 23053
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x04005A0E RID: 23054
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemDronePartStackGrid grid;

	// Token: 0x04005A0F RID: 23055
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemCosmeticStackGrid cosmeticGrid;

	// Token: 0x04005A10 RID: 23056
	public static string ID = "junkDrone";

	// Token: 0x04005A11 RID: 23057
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityDrone currentVehicleEntity;
}
