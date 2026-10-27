using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FD2 RID: 4050
[Preserve]
public class XUiC_PowerCameraWindowGroup : XUiController
{
	// Token: 0x06007F50 RID: 32592 RVA: 0x00307318 File Offset: 0x00305518
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

	// Token: 0x17000F03 RID: 3843
	// (get) Token: 0x06007F51 RID: 32593 RVA: 0x00307357 File Offset: 0x00305557
	// (set) Token: 0x06007F52 RID: 32594 RVA: 0x0030735F File Offset: 0x0030555F
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

	// Token: 0x06007F53 RID: 32595 RVA: 0x0030737C File Offset: 0x0030557C
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

	// Token: 0x06007F54 RID: 32596 RVA: 0x0030745C File Offset: 0x0030565C
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

	// Token: 0x06007F55 RID: 32597 RVA: 0x003074DC File Offset: 0x003056DC
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

	// Token: 0x04005E90 RID: 24208
	public bool UseEdgeDetection = true;

	// Token: 0x04005E91 RID: 24209
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04005E92 RID: 24210
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CameraWindow cameraWindow;

	// Token: 0x04005E93 RID: 24211
	public static string ID = "powercamera";

	// Token: 0x04005E94 RID: 24212
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005E95 RID: 24213
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;

	// Token: 0x04005E96 RID: 24214
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowered tileEntity;
}
