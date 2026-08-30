using System;
using UnityEngine.Scripting;

// Token: 0x020010B6 RID: 4278
[Preserve]
public class XUiC_VehicleWindowGroup : XUiController
{
	// Token: 0x17000FEA RID: 4074
	// (get) Token: 0x06008866 RID: 34918 RVA: 0x003419A3 File Offset: 0x0033FBA3
	// (set) Token: 0x06008867 RID: 34919 RVA: 0x003419AC File Offset: 0x0033FBAC
	public EntityVehicle CurrentVehicleEntity
	{
		get
		{
			return this.currentVehicleEntity;
		}
		set
		{
			this.xui.Vehicle.CurrentVehicle = value;
			this.currentVehicleEntity = value;
			this.frameWindow.Vehicle = value;
			this.frameWindow.group = this;
			Vehicle vehicle = value.GetVehicle();
			ItemValue updatedItemValue = vehicle.GetUpdatedItemValue();
			ItemStack itemStack = new ItemStack(updatedItemValue, 1);
			this.partGrid.AssembleWindow = this.frameWindow;
			this.partGrid.CurrentVehicle = vehicle;
			this.partGrid.CurrentItem = itemStack;
			this.partGrid.SetMods(updatedItemValue.Modifications);
			this.cosmeticGrid.AssembleWindow = this.frameWindow;
			this.cosmeticGrid.CurrentItem = itemStack;
			this.cosmeticGrid.SetParts(updatedItemValue.CosmeticMods);
			XUiM_AssembleItem assembleItem = this.xui.AssembleItem;
			assembleItem.AssembleWindow = this.frameWindow;
			assembleItem.CurrentItem = itemStack;
			assembleItem.CurrentItemStackController = null;
		}
	}

	// Token: 0x06008868 RID: 34920 RVA: 0x00341A8C File Offset: 0x0033FC8C
	public override void Init()
	{
		base.Init();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		this.headerName = Localization.Get("xuiVehicle", false, null);
		this.frameWindow = base.GetChildByType<XUiC_VehicleFrameWindow>();
		this.partGrid = base.GetChildByType<XUiC_VehiclePartStackGrid>();
		this.cosmeticGrid = base.GetChildByType<XUiC_ItemCosmeticStackGrid>();
	}

	// Token: 0x06008869 RID: 34921 RVA: 0x00341AE4 File Offset: 0x0033FCE4
	public override void Update(float _dt)
	{
		if (this.windowGroup.isShowing)
		{
			if (!this.xui.playerUI.playerInput.PermanentActions.Activate.IsPressed)
			{
				this.wasReleased = true;
			}
			if (this.wasReleased)
			{
				if (this.xui.playerUI.playerInput.PermanentActions.Activate.IsPressed)
				{
					this.activeKeyDown = true;
				}
				if (this.xui.playerUI.playerInput.PermanentActions.Activate.WasReleased && this.activeKeyDown)
				{
					this.activeKeyDown = false;
					if (!this.xui.playerUI.windowManager.IsInputActive())
					{
						this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, false);
					}
				}
			}
		}
		if (this.currentVehicleEntity != null && !this.currentVehicleEntity.CheckUIInteraction())
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		}
		base.Update(_dt);
	}

	// Token: 0x0600886A RID: 34922 RVA: 0x00341BF9 File Offset: 0x0033FDF9
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.nonPagingHeaderWindow != null)
		{
			this.nonPagingHeaderWindow.SetHeader(this.headerName);
		}
	}

	// Token: 0x0600886B RID: 34923 RVA: 0x00341C1C File Offset: 0x0033FE1C
	public override void OnClose()
	{
		base.OnClose();
		XUiM_AssembleItem assembleItem = this.xui.AssembleItem;
		assembleItem.AssembleWindow = null;
		if (assembleItem.CurrentItem.itemValue == this.xui.Vehicle.CurrentVehicle.GetVehicle().GetUpdatedItemValue())
		{
			assembleItem.CurrentItem = null;
			assembleItem.CurrentItemStackController = null;
		}
		this.wasReleased = false;
		this.activeKeyDown = false;
		this.CurrentVehicleEntity.StopUIInteraction();
		this.xui.Vehicle.CurrentVehicle = null;
	}

	// Token: 0x0600886C RID: 34924 RVA: 0x00341CA4 File Offset: 0x0033FEA4
	public void OnItemChanged(ItemStack itemStack)
	{
		this.partGrid.CurrentItem = itemStack;
		this.partGrid.SetMods(itemStack.itemValue.Modifications);
		this.cosmeticGrid.CurrentItem = itemStack;
		this.cosmeticGrid.SetParts(itemStack.itemValue.CosmeticMods);
	}

	// Token: 0x040065A3 RID: 26019
	public static string ID = "vehicle";

	// Token: 0x040065A4 RID: 26020
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_VehicleFrameWindow frameWindow;

	// Token: 0x040065A5 RID: 26021
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x040065A6 RID: 26022
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_VehiclePartStackGrid partGrid;

	// Token: 0x040065A7 RID: 26023
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemCosmeticStackGrid cosmeticGrid;

	// Token: 0x040065A8 RID: 26024
	[PublicizedFrom(EAccessModifier.Private)]
	public string headerName;

	// Token: 0x040065A9 RID: 26025
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityVehicle currentVehicleEntity;

	// Token: 0x040065AA RID: 26026
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x040065AB RID: 26027
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}
