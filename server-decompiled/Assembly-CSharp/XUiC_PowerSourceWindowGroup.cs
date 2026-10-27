using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FDB RID: 4059
[Preserve]
public class XUiC_PowerSourceWindowGroup : XUiController
{
	// Token: 0x06007FB7 RID: 32695 RVA: 0x00309700 File Offset: 0x00307900
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

	// Token: 0x17000F11 RID: 3857
	// (get) Token: 0x06007FB8 RID: 32696 RVA: 0x0030976D File Offset: 0x0030796D
	// (set) Token: 0x06007FB9 RID: 32697 RVA: 0x00309775 File Offset: 0x00307975
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

	// Token: 0x06007FBA RID: 32698 RVA: 0x003097A0 File Offset: 0x003079A0
	public void SetOn(bool isOn)
	{
		if (this.PowerSourceSlots != null && this.PowerSourceSlots.ViewComponent.IsVisible)
		{
			this.PowerSourceSlots.SetOn(isOn);
		}
	}

	// Token: 0x06007FBB RID: 32699 RVA: 0x003097C8 File Offset: 0x003079C8
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

	// Token: 0x06007FBC RID: 32700 RVA: 0x003098A8 File Offset: 0x00307AA8
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

	// Token: 0x06007FBD RID: 32701 RVA: 0x003099A0 File Offset: 0x00307BA0
	public override void OnClose()
	{
		base.OnClose();
		this.wasReleased = false;
		this.activeKeyDown = false;
		Manager.BroadcastPlayByLocalPlayer(this.TileEntity.ToWorldPos().ToVector3() + Vector3.one * 0.5f, "close_vending");
		this.TileEntity.Destroyed -= this.TileEntity_Destroyed;
	}

	// Token: 0x06007FBE RID: 32702 RVA: 0x00309A0C File Offset: 0x00307C0C
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

	// Token: 0x04005EDC RID: 24284
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04005EDD RID: 24285
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PowerSourceStats GeneratorStats;

	// Token: 0x04005EDE RID: 24286
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PowerSourceSlots PowerSourceSlots;

	// Token: 0x04005EDF RID: 24287
	public static string ID = "powersource";

	// Token: 0x04005EE0 RID: 24288
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005EE1 RID: 24289
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;

	// Token: 0x04005EE2 RID: 24290
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowerSource tileEntity;
}
