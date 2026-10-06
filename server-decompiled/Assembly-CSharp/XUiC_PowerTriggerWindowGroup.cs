using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FCD RID: 4045
[Preserve]
public class XUiC_PowerTriggerWindowGroup : XUiController
{
	// Token: 0x06007F83 RID: 32643 RVA: 0x00308BC8 File Offset: 0x00306DC8
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

	// Token: 0x17000F09 RID: 3849
	// (get) Token: 0x06007F84 RID: 32644 RVA: 0x00308C29 File Offset: 0x00306E29
	// (set) Token: 0x06007F85 RID: 32645 RVA: 0x00308C31 File Offset: 0x00306E31
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

	// Token: 0x06007F86 RID: 32646 RVA: 0x00308C5C File Offset: 0x00306E5C
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

	// Token: 0x06007F87 RID: 32647 RVA: 0x00308D3C File Offset: 0x00306F3C
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

	// Token: 0x06007F88 RID: 32648 RVA: 0x00308DC8 File Offset: 0x00306FC8
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

	// Token: 0x06007F89 RID: 32649 RVA: 0x00308E3C File Offset: 0x0030703C
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

	// Token: 0x04005EBF RID: 24255
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04005EC0 RID: 24256
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PowerTriggerOptions triggerWindow;

	// Token: 0x04005EC1 RID: 24257
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CameraWindow cameraWindow;

	// Token: 0x04005EC2 RID: 24258
	public static string ID = "powertrigger";

	// Token: 0x04005EC3 RID: 24259
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005EC4 RID: 24260
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;

	// Token: 0x04005EC5 RID: 24261
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPoweredTrigger tileEntity;
}
