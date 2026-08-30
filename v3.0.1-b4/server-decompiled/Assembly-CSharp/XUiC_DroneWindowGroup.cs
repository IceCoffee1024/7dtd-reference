using System;
using UnityEngine.Scripting;

// Token: 0x02000F15 RID: 3861
[Preserve]
public class XUiC_DroneWindowGroup : XUiController
{
	// Token: 0x17000E24 RID: 3620
	// (get) Token: 0x06007928 RID: 31016 RVA: 0x002DA561 File Offset: 0x002D8761
	// (set) Token: 0x06007929 RID: 31017 RVA: 0x002DA56C File Offset: 0x002D876C
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

	// Token: 0x0600792A RID: 31018 RVA: 0x002DA62C File Offset: 0x002D882C
	public override void Init()
	{
		base.Init();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		this.assembleWindow = base.GetChildByType<XUiC_AssembleDroneWindow>();
		this.grid = base.GetChildByType<XUiC_ItemDronePartStackGrid>();
		this.cosmeticGrid = base.GetChildByType<XUiC_ItemCosmeticStackGrid>();
	}

	// Token: 0x0600792B RID: 31019 RVA: 0x002DA664 File Offset: 0x002D8864
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

	// Token: 0x0600792C RID: 31020 RVA: 0x002DA73C File Offset: 0x002D893C
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

	// Token: 0x0600792D RID: 31021 RVA: 0x002DA798 File Offset: 0x002D8998
	public void OnItemChanged(ItemStack itemStack)
	{
		this.grid.CurrentItem = itemStack;
		this.grid.SetParts(itemStack.itemValue.Modifications);
		this.cosmeticGrid.CurrentItem = itemStack;
		this.cosmeticGrid.SetParts(itemStack.itemValue.CosmeticMods);
	}

	// Token: 0x04005959 RID: 22873
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_AssembleDroneWindow assembleWindow;

	// Token: 0x0400595A RID: 22874
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x0400595B RID: 22875
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemDronePartStackGrid grid;

	// Token: 0x0400595C RID: 22876
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemCosmeticStackGrid cosmeticGrid;

	// Token: 0x0400595D RID: 22877
	public static string ID = "junkDrone";

	// Token: 0x0400595E RID: 22878
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityDrone currentVehicleEntity;
}
