using System;
using UnityEngine.Scripting;

// Token: 0x020010C7 RID: 4295
[Preserve]
public class XUiC_VehicleWindowGroup : XUiController
{
	// Token: 0x1700101F RID: 4127
	// (get) Token: 0x060088B4 RID: 34996 RVA: 0x003457AF File Offset: 0x003439AF
	// (set) Token: 0x060088B5 RID: 34997 RVA: 0x003457B8 File Offset: 0x003439B8
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

	// Token: 0x060088B6 RID: 34998 RVA: 0x00345874 File Offset: 0x00343A74
	public override void Init()
	{
		base.Init();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		this.headerName = Localization.Get("xuiVehicle", false, null);
		this.frameWindow = base.GetChildByType<XUiC_VehicleFrameWindow>();
		this.partGrid = base.GetChildByType<XUiC_VehiclePartStackGrid>();
		this.cosmeticGrid = base.GetChildByType<XUiC_ItemCosmeticStackGrid>();
	}

	// Token: 0x060088B7 RID: 34999 RVA: 0x003458CC File Offset: 0x00343ACC
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

	// Token: 0x060088B8 RID: 35000 RVA: 0x003459E1 File Offset: 0x00343BE1
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.nonPagingHeaderWindow != null)
		{
			this.nonPagingHeaderWindow.SetHeader(this.headerName);
		}
	}

	// Token: 0x060088B9 RID: 35001 RVA: 0x00345A04 File Offset: 0x00343C04
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

	// Token: 0x060088BA RID: 35002 RVA: 0x00345A8B File Offset: 0x00343C8B
	public void OnItemChanged(ItemStack itemStack)
	{
		this.partGrid.CurrentItem = itemStack;
		this.cosmeticGrid.CurrentItem = itemStack;
	}

	// Token: 0x040065E5 RID: 26085
	public static string ID = "vehicle";

	// Token: 0x040065E6 RID: 26086
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_VehicleFrameWindow frameWindow;

	// Token: 0x040065E7 RID: 26087
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x040065E8 RID: 26088
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_VehiclePartStackGrid partGrid;

	// Token: 0x040065E9 RID: 26089
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemCosmeticStackGrid cosmeticGrid;

	// Token: 0x040065EA RID: 26090
	[PublicizedFrom(EAccessModifier.Private)]
	public string headerName;

	// Token: 0x040065EB RID: 26091
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityVehicle currentVehicleEntity;

	// Token: 0x040065EC RID: 26092
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x040065ED RID: 26093
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}
