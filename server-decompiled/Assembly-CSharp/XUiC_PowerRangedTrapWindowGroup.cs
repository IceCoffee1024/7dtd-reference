using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FB8 RID: 4024
[Preserve]
public class XUiC_PowerRangedTrapWindowGroup : XUiController
{
	// Token: 0x06007F29 RID: 32553 RVA: 0x00303894 File Offset: 0x00301A94
	public override void Init()
	{
		base.Init();
		XUiController xuiController = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		if (xuiController != null)
		{
			this.nonPagingHeader = (XUiC_WindowNonPagingHeader)xuiController;
		}
		xuiController = base.GetChildByType<XUiC_PowerRangedAmmoSlots>();
		if (xuiController != null)
		{
			this.ammoWindow = (XUiC_PowerRangedAmmoSlots)xuiController;
			this.ammoWindow.Owner = this;
		}
		xuiController = base.GetChildByType<XUiC_PowerRangedTrapOptions>();
		if (xuiController != null)
		{
			this.optionsWindow = (XUiC_PowerRangedTrapOptions)xuiController;
			this.optionsWindow.Owner = this;
		}
		xuiController = base.GetChildById("windowPowerCameraControlPreview");
		if (xuiController != null)
		{
			this.cameraWindowPreview = (XUiC_CameraWindow)xuiController;
			this.cameraWindowPreview.Owner = this;
		}
	}

	// Token: 0x17000ED8 RID: 3800
	// (get) Token: 0x06007F2A RID: 32554 RVA: 0x00303928 File Offset: 0x00301B28
	// (set) Token: 0x06007F2B RID: 32555 RVA: 0x00303930 File Offset: 0x00301B30
	public TileEntityPoweredRangedTrap TileEntity
	{
		get
		{
			return this.tileEntity;
		}
		set
		{
			this.tileEntity = value;
			this.ammoWindow.TileEntity = this.tileEntity;
			if (this.tileEntity.PowerItemType == PowerItem.PowerItemTypes.RangedTrap)
			{
				this.optionsWindow.TileEntity = this.tileEntity;
			}
			this.cameraWindowPreview.TileEntity = this.tileEntity;
		}
	}

	// Token: 0x06007F2C RID: 32556 RVA: 0x00303988 File Offset: 0x00301B88
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

	// Token: 0x06007F2D RID: 32557 RVA: 0x00303A68 File Offset: 0x00301C68
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.OnOpen();
			base.ViewComponent.IsVisible = true;
		}
		if (this.nonPagingHeader != null)
		{
			World world = GameManager.Instance.World;
			string localizedBlockName = this.tileEntity.GetChunk().GetBlock(this.tileEntity.localChunkPos).Block.GetLocalizedBlockName();
			this.nonPagingHeader.SetHeader(localizedBlockName);
		}
		this.xui.RecenterWindowGroup(this.windowGroup, false);
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].OnOpen();
		}
		if (!this.tileEntity.ShowTargeting)
		{
			this.optionsWindow.ViewComponent.IsVisible = false;
		}
		Manager.BroadcastPlayByLocalPlayer(this.TileEntity.ToWorldPos().ToVector3() + Vector3.one * 0.5f, "open_vending");
		this.IsDirty = true;
		this.TileEntity.Destroyed += this.TileEntity_Destroyed;
	}

	// Token: 0x06007F2E RID: 32558 RVA: 0x00303B98 File Offset: 0x00301D98
	public override void OnClose()
	{
		base.OnClose();
		this.wasReleased = false;
		this.activeKeyDown = false;
		Vector3 vector = this.TileEntity.ToWorldPos().ToVector3() + Vector3.one * 0.5f;
		if (!XUiC_CameraWindow.hackyIsOpeningMaximizedWindow)
		{
			Manager.BroadcastPlayByLocalPlayer(vector, "close_vending");
		}
		this.TileEntity.Destroyed -= this.TileEntity_Destroyed;
	}

	// Token: 0x06007F2F RID: 32559 RVA: 0x00303C0C File Offset: 0x00301E0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void TileEntity_Destroyed(ITileEntity te)
	{
		if (this.TileEntity == te)
		{
			if (GameManager.Instance != null)
			{
				this.xui.playerUI.windowManager.Close("powerrangedtrap");
				this.xui.playerUI.windowManager.Close("powercamera");
				return;
			}
		}
		else
		{
			te.Destroyed -= this.TileEntity_Destroyed;
		}
	}

	// Token: 0x04005E4D RID: 24141
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04005E4E RID: 24142
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PowerRangedTrapOptions optionsWindow;

	// Token: 0x04005E4F RID: 24143
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PowerRangedAmmoSlots ammoWindow;

	// Token: 0x04005E50 RID: 24144
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CameraWindow cameraWindowPreview;

	// Token: 0x04005E51 RID: 24145
	public static string ID = "powerrangedtrap";

	// Token: 0x04005E52 RID: 24146
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005E53 RID: 24147
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;

	// Token: 0x04005E54 RID: 24148
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPoweredRangedTrap tileEntity;
}
