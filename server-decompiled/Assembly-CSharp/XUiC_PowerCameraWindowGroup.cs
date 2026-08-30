using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FB3 RID: 4019
[Preserve]
public class XUiC_PowerCameraWindowGroup : XUiController
{
	// Token: 0x06007EF2 RID: 32498 RVA: 0x00302824 File Offset: 0x00300A24
	public override void Init()
	{
		base.Init();
		XUiController xuiController = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		if (xuiController != null)
		{
			this.nonPagingHeader = (XUiC_WindowNonPagingHeader)xuiController;
		}
		xuiController = base.GetChildByType<XUiC_CameraWindow>();
		if (xuiController != null)
		{
			this.cameraWindow = (XUiC_CameraWindow)xuiController;
		}
	}

	// Token: 0x17000ED1 RID: 3793
	// (get) Token: 0x06007EF3 RID: 32499 RVA: 0x00302863 File Offset: 0x00300A63
	// (set) Token: 0x06007EF4 RID: 32500 RVA: 0x0030286B File Offset: 0x00300A6B
	public TileEntityPowered TileEntity
	{
		get
		{
			return this.tileEntity;
		}
		set
		{
			this.tileEntity = value;
			this.cameraWindow.TileEntity = this.tileEntity;
		}
	}

	// Token: 0x06007EF5 RID: 32501 RVA: 0x00302888 File Offset: 0x00300A88
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

	// Token: 0x06007EF6 RID: 32502 RVA: 0x00302968 File Offset: 0x00300B68
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.nonPagingHeader != null)
		{
			World world = GameManager.Instance.World;
			string localizedBlockName = this.tileEntity.GetChunk().GetBlock(this.tileEntity.localChunkPos).Block.GetLocalizedBlockName();
			this.nonPagingHeader.SetHeader(localizedBlockName);
		}
		this.IsDirty = true;
		this.xui.playerUI.CursorController.Locked = true;
	}

	// Token: 0x06007EF7 RID: 32503 RVA: 0x003029E8 File Offset: 0x00300BE8
	public override void OnClose()
	{
		base.OnClose();
		bool flag = true;
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		if (entityPlayer.IsDead() || Vector3.Distance(entityPlayer.position, this.TileEntity.ToWorldPos().ToVector3()) > Constants.cDigAndBuildDistance * Constants.cDigAndBuildDistance)
		{
			flag = false;
		}
		this.wasReleased = false;
		this.activeKeyDown = false;
		GUIWindow guiwindow;
		if (flag && this.xui.playerUI.windowManager.TryGetWindow(XUiC_CameraWindow.lastWindowGroup, out guiwindow))
		{
			XUiController controller = ((XUiWindowGroup)guiwindow).Controller;
			if (XUiC_CameraWindow.lastWindowGroup == "powerrangedtrap")
			{
				((XUiC_PowerRangedTrapWindowGroup)controller).TileEntity = (TileEntityPoweredRangedTrap)this.TileEntity;
			}
			else if (XUiC_CameraWindow.lastWindowGroup == "powertrigger")
			{
				((XUiC_PowerTriggerWindowGroup)controller).TileEntity = (TileEntityPoweredTrigger)this.TileEntity;
			}
			else
			{
				((XUiC_PoweredGenericWindowGroup)controller).TileEntity = this.TileEntity;
			}
			this.xui.playerUI.windowManager.Open(XUiC_CameraWindow.lastWindowGroup, true);
		}
		this.xui.playerUI.CursorController.Locked = false;
	}

	// Token: 0x04005E2C RID: 24108
	public bool UseEdgeDetection = true;

	// Token: 0x04005E2D RID: 24109
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04005E2E RID: 24110
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CameraWindow cameraWindow;

	// Token: 0x04005E2F RID: 24111
	public static string ID = "powercamera";

	// Token: 0x04005E30 RID: 24112
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005E31 RID: 24113
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;

	// Token: 0x04005E32 RID: 24114
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowered tileEntity;
}
