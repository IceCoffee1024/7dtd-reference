using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FDD RID: 4061
[Preserve]
public class XUiC_PowerTriggerWindowGroup : XUiController
{
	// Token: 0x06007FD2 RID: 32722 RVA: 0x0030A6E4 File Offset: 0x003088E4
	public override void Init()
	{
		base.Init();
		XUiController xuiController = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		if (xuiController != null)
		{
			this.nonPagingHeader = (XUiC_WindowNonPagingHeader)xuiController;
		}
		xuiController = base.GetChildByType<XUiC_PowerTriggerOptions>();
		if (xuiController != null)
		{
			this.triggerWindow = (XUiC_PowerTriggerOptions)xuiController;
			this.triggerWindow.Owner = this;
		}
		xuiController = base.GetChildByType<XUiC_CameraWindow>();
		if (xuiController != null)
		{
			this.cameraWindow = (XUiC_CameraWindow)xuiController;
		}
	}

	// Token: 0x17000F14 RID: 3860
	// (get) Token: 0x06007FD3 RID: 32723 RVA: 0x0030A745 File Offset: 0x00308945
	// (set) Token: 0x06007FD4 RID: 32724 RVA: 0x0030A74D File Offset: 0x0030894D
	public TileEntityPoweredTrigger TileEntity
	{
		get
		{
			return this.tileEntity;
		}
		set
		{
			this.tileEntity = value;
			this.triggerWindow.TileEntity = this.tileEntity;
			this.cameraWindow.TileEntity = this.tileEntity;
		}
	}

	// Token: 0x06007FD5 RID: 32725 RVA: 0x0030A778 File Offset: 0x00308978
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

	// Token: 0x06007FD6 RID: 32726 RVA: 0x0030A858 File Offset: 0x00308A58
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.nonPagingHeader != null)
		{
			string text = Localization.Get("xuiTrigger", false, null);
			this.nonPagingHeader.SetHeader(text);
		}
		Manager.BroadcastPlayByLocalPlayer(this.TileEntity.ToWorldPos().ToVector3() + Vector3.one * 0.5f, "open_vending");
		this.IsDirty = true;
		this.TileEntity.Destroyed += this.TileEntity_Destroyed;
	}

	// Token: 0x06007FD7 RID: 32727 RVA: 0x0030A8E4 File Offset: 0x00308AE4
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

	// Token: 0x06007FD8 RID: 32728 RVA: 0x0030A958 File Offset: 0x00308B58
	[PublicizedFrom(EAccessModifier.Private)]
	public void TileEntity_Destroyed(ITileEntity te)
	{
		if (this.TileEntity == te)
		{
			if (GameManager.Instance != null)
			{
				this.xui.playerUI.windowManager.Close("powertrigger");
				this.xui.playerUI.windowManager.Close("powercamera");
				return;
			}
		}
		else
		{
			te.Destroyed -= this.TileEntity_Destroyed;
		}
	}

	// Token: 0x04005EFA RID: 24314
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04005EFB RID: 24315
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PowerTriggerOptions triggerWindow;

	// Token: 0x04005EFC RID: 24316
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CameraWindow cameraWindow;

	// Token: 0x04005EFD RID: 24317
	public static string ID = "powertrigger";

	// Token: 0x04005EFE RID: 24318
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005EFF RID: 24319
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;

	// Token: 0x04005F00 RID: 24320
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPoweredTrigger tileEntity;
}
