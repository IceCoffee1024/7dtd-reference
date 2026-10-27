using System;
using UnityEngine.Scripting;

// Token: 0x020010D7 RID: 4311
[Preserve]
public class XUiC_VehicleWindowGroup : XUiController
{
	// Token: 0x1700102A RID: 4138
	// (get) Token: 0x06008907 RID: 35079 RVA: 0x00347833 File Offset: 0x00345A33
	// (set) Token: 0x06008908 RID: 35080 RVA: 0x0034783C File Offset: 0x00345A3C
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
			ItemStack itemStack = new ItemStack(vehicle.GetUpdatedItemValue(), 1);
			this.partGrid.AssembleWindow = this.frameWindow;
			this.partGrid.CurrentVehicle = vehicle;
			this.partGrid.CurrentItem = itemStack;
			this.cosmeticGrid.AssembleWindow = this.frameWindow;
			this.cosmeticGrid.CurrentItem = itemStack;
			XUiM_AssembleItem assembleItem = this.xui.AssembleItem;
			assembleItem.AssembleWindow = this.frameWindow;
			assembleItem.CurrentItem = itemStack;
			assembleItem.CurrentItemStackController = null;
		}
	}

	// Token: 0x06008909 RID: 35081 RVA: 0x003478F8 File Offset: 0x00345AF8
	public override void Init()
	{
		base.Init();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		this.headerName = Localization.Get("xuiVehicle", false, null);
		this.frameWindow = base.GetChildByType<XUiC_VehicleFrameWindow>();
		this.partGrid = base.GetChildByType<XUiC_VehiclePartStackGrid>();
		this.cosmeticGrid = base.GetChildByType<XUiC_ItemCosmeticStackGrid>();
	}

	// Token: 0x0600890A RID: 35082 RVA: 0x00347950 File Offset: 0x00345B50
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

	// Token: 0x0600890B RID: 35083 RVA: 0x00347A65 File Offset: 0x00345C65
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.nonPagingHeaderWindow != null)
		{
			this.nonPagingHeaderWindow.SetHeader(this.headerName);
		}
	}

	// Token: 0x0600890C RID: 35084 RVA: 0x00347A88 File Offset: 0x00345C88
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

	// Token: 0x0600890D RID: 35085 RVA: 0x00347B0F File Offset: 0x00345D0F
	public void OnItemChanged(ItemStack itemStack)
	{
		this.partGrid.CurrentItem = itemStack;
		this.cosmeticGrid.CurrentItem = itemStack;
	}

	// Token: 0x04006624 RID: 26148
	public static string ID = "vehicle";

	// Token: 0x04006625 RID: 26149
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_VehicleFrameWindow frameWindow;

	// Token: 0x04006626 RID: 26150
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x04006627 RID: 26151
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_VehiclePartStackGrid partGrid;

	// Token: 0x04006628 RID: 26152
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemCosmeticStackGrid cosmeticGrid;

	// Token: 0x04006629 RID: 26153
	[PublicizedFrom(EAccessModifier.Private)]
	public string headerName;

	// Token: 0x0400662A RID: 26154
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityVehicle currentVehicleEntity;

	// Token: 0x0400662B RID: 26155
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x0400662C RID: 26156
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}
