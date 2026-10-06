using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FC2 RID: 4034
[Preserve]
public class XUiC_PowerCameraWindowGroup : XUiController
{
	// Token: 0x06007F01 RID: 32513 RVA: 0x003057FC File Offset: 0x003039FC
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

	// Token: 0x17000EF8 RID: 3832
	// (get) Token: 0x06007F02 RID: 32514 RVA: 0x0030583B File Offset: 0x00303A3B
	// (set) Token: 0x06007F03 RID: 32515 RVA: 0x00305843 File Offset: 0x00303A43
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

	// Token: 0x06007F04 RID: 32516 RVA: 0x00305860 File Offset: 0x00303A60
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

	// Token: 0x06007F05 RID: 32517 RVA: 0x00305940 File Offset: 0x00303B40
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

	// Token: 0x06007F06 RID: 32518 RVA: 0x003059C0 File Offset: 0x00303BC0
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

	// Token: 0x04005E55 RID: 24149
	public bool UseEdgeDetection = true;

	// Token: 0x04005E56 RID: 24150
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04005E57 RID: 24151
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CameraWindow cameraWindow;

	// Token: 0x04005E58 RID: 24152
	public static string ID = "powercamera";

	// Token: 0x04005E59 RID: 24153
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005E5A RID: 24154
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;

	// Token: 0x04005E5B RID: 24155
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowered tileEntity;
}
