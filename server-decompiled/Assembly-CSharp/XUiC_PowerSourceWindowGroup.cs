using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FCB RID: 4043
[Preserve]
public class XUiC_PowerSourceWindowGroup : XUiController
{
	// Token: 0x06007F68 RID: 32616 RVA: 0x00307BE4 File Offset: 0x00305DE4
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

	// Token: 0x17000F06 RID: 3846
	// (get) Token: 0x06007F69 RID: 32617 RVA: 0x00307C51 File Offset: 0x00305E51
	// (set) Token: 0x06007F6A RID: 32618 RVA: 0x00307C59 File Offset: 0x00305E59
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

	// Token: 0x06007F6B RID: 32619 RVA: 0x00307C84 File Offset: 0x00305E84
	public void SetOn(bool isOn)
	{
		if (this.PowerSourceSlots != null && this.PowerSourceSlots.ViewComponent.IsVisible)
		{
			this.PowerSourceSlots.SetOn(isOn);
		}
	}

	// Token: 0x06007F6C RID: 32620 RVA: 0x00307CAC File Offset: 0x00305EAC
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

	// Token: 0x06007F6D RID: 32621 RVA: 0x00307D8C File Offset: 0x00305F8C
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

	// Token: 0x06007F6E RID: 32622 RVA: 0x00307E84 File Offset: 0x00306084
	public override void OnClose()
	{
		base.OnClose();
		this.wasReleased = false;
		this.activeKeyDown = false;
		Manager.BroadcastPlayByLocalPlayer(this.TileEntity.ToWorldPos().ToVector3() + Vector3.one * 0.5f, "close_vending");
		this.TileEntity.Destroyed -= this.TileEntity_Destroyed;
	}

	// Token: 0x06007F6F RID: 32623 RVA: 0x00307EF0 File Offset: 0x003060F0
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

	// Token: 0x04005EA1 RID: 24225
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04005EA2 RID: 24226
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PowerSourceStats GeneratorStats;

	// Token: 0x04005EA3 RID: 24227
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PowerSourceSlots PowerSourceSlots;

	// Token: 0x04005EA4 RID: 24228
	public static string ID = "powersource";

	// Token: 0x04005EA5 RID: 24229
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005EA6 RID: 24230
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;

	// Token: 0x04005EA7 RID: 24231
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowerSource tileEntity;
}
