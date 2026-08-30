using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FA5 RID: 4005
[Preserve]
public class XUiC_PowerTriggerWindowGroup : XUiController
{
	// Token: 0x06007E82 RID: 32386 RVA: 0x0030141C File Offset: 0x002FF61C
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

	// Token: 0x17000EA9 RID: 3753
	// (get) Token: 0x06007E83 RID: 32387 RVA: 0x0030147D File Offset: 0x002FF67D
	// (set) Token: 0x06007E84 RID: 32388 RVA: 0x00301485 File Offset: 0x002FF685
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

	// Token: 0x06007E85 RID: 32389 RVA: 0x003014B0 File Offset: 0x002FF6B0
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

	// Token: 0x06007E86 RID: 32390 RVA: 0x00301590 File Offset: 0x002FF790
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

	// Token: 0x06007E87 RID: 32391 RVA: 0x0030161C File Offset: 0x002FF81C
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

	// Token: 0x06007E88 RID: 32392 RVA: 0x00301690 File Offset: 0x002FF890
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

	// Token: 0x04005DCF RID: 24015
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04005DD0 RID: 24016
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PowerTriggerOptions triggerWindow;

	// Token: 0x04005DD1 RID: 24017
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CameraWindow cameraWindow;

	// Token: 0x04005DD2 RID: 24018
	public static string ID = "powertrigger";

	// Token: 0x04005DD3 RID: 24019
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005DD4 RID: 24020
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;

	// Token: 0x04005DD5 RID: 24021
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPoweredTrigger tileEntity;
}
