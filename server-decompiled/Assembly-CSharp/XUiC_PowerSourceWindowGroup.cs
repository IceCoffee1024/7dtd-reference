using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FBC RID: 4028
[Preserve]
public class XUiC_PowerSourceWindowGroup : XUiController
{
	// Token: 0x06007F59 RID: 32601 RVA: 0x00304C0C File Offset: 0x00302E0C
	public override void Init()
	{
		base.Init();
		XUiController xuiController = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		if (xuiController != null)
		{
			this.nonPagingHeader = (XUiC_WindowNonPagingHeader)xuiController;
		}
		xuiController = base.GetChildByType<XUiC_PowerSourceStats>();
		if (xuiController != null)
		{
			this.GeneratorStats = (XUiC_PowerSourceStats)xuiController;
			this.GeneratorStats.Owner = this;
		}
		xuiController = base.GetChildByType<XUiC_PowerSourceSlots>();
		if (xuiController != null)
		{
			this.PowerSourceSlots = (XUiC_PowerSourceSlots)xuiController;
			this.PowerSourceSlots.Owner = this;
		}
	}

	// Token: 0x17000EDF RID: 3807
	// (get) Token: 0x06007F5A RID: 32602 RVA: 0x00304C79 File Offset: 0x00302E79
	// (set) Token: 0x06007F5B RID: 32603 RVA: 0x00304C81 File Offset: 0x00302E81
	public TileEntityPowerSource TileEntity
	{
		get
		{
			return this.tileEntity;
		}
		set
		{
			this.tileEntity = value;
			this.GeneratorStats.TileEntity = this.tileEntity;
			this.PowerSourceSlots.TileEntity = this.tileEntity;
		}
	}

	// Token: 0x06007F5C RID: 32604 RVA: 0x00304CAC File Offset: 0x00302EAC
	public void SetOn(bool isOn)
	{
		if (this.PowerSourceSlots != null && this.PowerSourceSlots.ViewComponent.IsVisible)
		{
			this.PowerSourceSlots.SetOn(isOn);
		}
	}

	// Token: 0x06007F5D RID: 32605 RVA: 0x00304CD4 File Offset: 0x00302ED4
	public override void Update(float _dt)
	{
		base.Update(_dt);
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
	}

	// Token: 0x06007F5E RID: 32606 RVA: 0x00304DB4 File Offset: 0x00302FB4
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.nonPagingHeader != null)
		{
			string text = "";
			EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
			switch (this.TileEntity.PowerItemType)
			{
			case PowerItem.PowerItemTypes.Generator:
				text = Localization.Get("generatorbank", false, null);
				break;
			case PowerItem.PowerItemTypes.SolarPanel:
				text = Localization.Get("solarbank", false, null);
				break;
			case PowerItem.PowerItemTypes.BatteryBank:
				text = Localization.Get("batterybank", false, null);
				break;
			}
			this.nonPagingHeader.SetHeader(text);
		}
		if (this.PowerSourceSlots != null && this.TileEntity != null)
		{
			this.PowerSourceSlots.OnOpen();
		}
		Manager.BroadcastPlayByLocalPlayer(this.TileEntity.ToWorldPos().ToVector3() + Vector3.one * 0.5f, "open_vending");
		this.IsDirty = true;
		this.TileEntity.Destroyed += this.TileEntity_Destroyed;
	}

	// Token: 0x06007F5F RID: 32607 RVA: 0x00304EAC File Offset: 0x003030AC
	public override void OnClose()
	{
		base.OnClose();
		this.wasReleased = false;
		this.activeKeyDown = false;
		Manager.BroadcastPlayByLocalPlayer(this.TileEntity.ToWorldPos().ToVector3() + Vector3.one * 0.5f, "close_vending");
		this.TileEntity.Destroyed -= this.TileEntity_Destroyed;
	}

	// Token: 0x06007F60 RID: 32608 RVA: 0x00304F18 File Offset: 0x00303118
	[PublicizedFrom(EAccessModifier.Private)]
	public void TileEntity_Destroyed(ITileEntity te)
	{
		if (this.TileEntity == te)
		{
			if (GameManager.Instance != null)
			{
				this.xui.playerUI.windowManager.Close("powersource");
				return;
			}
		}
		else
		{
			te.Destroyed -= this.TileEntity_Destroyed;
		}
	}

	// Token: 0x04005E78 RID: 24184
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04005E79 RID: 24185
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PowerSourceStats GeneratorStats;

	// Token: 0x04005E7A RID: 24186
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PowerSourceSlots PowerSourceSlots;

	// Token: 0x04005E7B RID: 24187
	public static string ID = "powersource";

	// Token: 0x04005E7C RID: 24188
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005E7D RID: 24189
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;

	// Token: 0x04005E7E RID: 24190
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowerSource tileEntity;
}
