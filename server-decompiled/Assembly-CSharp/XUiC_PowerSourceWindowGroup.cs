using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FA3 RID: 4003
[Preserve]
public class XUiC_PowerSourceWindowGroup : XUiController
{
	// Token: 0x06007E67 RID: 32359 RVA: 0x00300438 File Offset: 0x002FE638
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

	// Token: 0x17000EA6 RID: 3750
	// (get) Token: 0x06007E68 RID: 32360 RVA: 0x003004A5 File Offset: 0x002FE6A5
	// (set) Token: 0x06007E69 RID: 32361 RVA: 0x003004AD File Offset: 0x002FE6AD
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

	// Token: 0x06007E6A RID: 32362 RVA: 0x003004D8 File Offset: 0x002FE6D8
	public void SetOn(bool isOn)
	{
		if (this.PowerSourceSlots != null && this.PowerSourceSlots.ViewComponent.IsVisible)
		{
			this.PowerSourceSlots.SetOn(isOn);
		}
	}

	// Token: 0x06007E6B RID: 32363 RVA: 0x00300500 File Offset: 0x002FE700
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

	// Token: 0x06007E6C RID: 32364 RVA: 0x003005E0 File Offset: 0x002FE7E0
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

	// Token: 0x06007E6D RID: 32365 RVA: 0x003006D8 File Offset: 0x002FE8D8
	public override void OnClose()
	{
		base.OnClose();
		this.wasReleased = false;
		this.activeKeyDown = false;
		Manager.BroadcastPlayByLocalPlayer(this.TileEntity.ToWorldPos().ToVector3() + Vector3.one * 0.5f, "close_vending");
		this.TileEntity.Destroyed -= this.TileEntity_Destroyed;
	}

	// Token: 0x06007E6E RID: 32366 RVA: 0x00300744 File Offset: 0x002FE944
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

	// Token: 0x04005DB1 RID: 23985
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04005DB2 RID: 23986
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PowerSourceStats GeneratorStats;

	// Token: 0x04005DB3 RID: 23987
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PowerSourceSlots PowerSourceSlots;

	// Token: 0x04005DB4 RID: 23988
	public static string ID = "powersource";

	// Token: 0x04005DB5 RID: 23989
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005DB6 RID: 23990
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;

	// Token: 0x04005DB7 RID: 23991
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowerSource tileEntity;
}
