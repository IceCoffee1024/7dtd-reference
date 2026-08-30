using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FBE RID: 4030
[Preserve]
public class XUiC_PowerTriggerWindowGroup : XUiController
{
	// Token: 0x06007F74 RID: 32628 RVA: 0x00305BF0 File Offset: 0x00303DF0
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

	// Token: 0x17000EE2 RID: 3810
	// (get) Token: 0x06007F75 RID: 32629 RVA: 0x00305C51 File Offset: 0x00303E51
	// (set) Token: 0x06007F76 RID: 32630 RVA: 0x00305C59 File Offset: 0x00303E59
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

	// Token: 0x06007F77 RID: 32631 RVA: 0x00305C84 File Offset: 0x00303E84
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

	// Token: 0x06007F78 RID: 32632 RVA: 0x00305D64 File Offset: 0x00303F64
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

	// Token: 0x06007F79 RID: 32633 RVA: 0x00305DF0 File Offset: 0x00303FF0
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

	// Token: 0x06007F7A RID: 32634 RVA: 0x00305E64 File Offset: 0x00304064
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

	// Token: 0x04005E96 RID: 24214
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04005E97 RID: 24215
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PowerTriggerOptions triggerWindow;

	// Token: 0x04005E98 RID: 24216
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CameraWindow cameraWindow;

	// Token: 0x04005E99 RID: 24217
	public static string ID = "powertrigger";

	// Token: 0x04005E9A RID: 24218
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005E9B RID: 24219
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;

	// Token: 0x04005E9C RID: 24220
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPoweredTrigger tileEntity;
}
