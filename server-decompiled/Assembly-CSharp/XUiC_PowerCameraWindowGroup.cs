using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F9A RID: 3994
[Preserve]
public class XUiC_PowerCameraWindowGroup : XUiController
{
	// Token: 0x06007E00 RID: 32256 RVA: 0x002FE050 File Offset: 0x002FC250
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

	// Token: 0x17000E98 RID: 3736
	// (get) Token: 0x06007E01 RID: 32257 RVA: 0x002FE08F File Offset: 0x002FC28F
	// (set) Token: 0x06007E02 RID: 32258 RVA: 0x002FE097 File Offset: 0x002FC297
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

	// Token: 0x06007E03 RID: 32259 RVA: 0x002FE0B4 File Offset: 0x002FC2B4
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

	// Token: 0x06007E04 RID: 32260 RVA: 0x002FE194 File Offset: 0x002FC394
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

	// Token: 0x06007E05 RID: 32261 RVA: 0x002FE214 File Offset: 0x002FC414
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

	// Token: 0x04005D65 RID: 23909
	public bool UseEdgeDetection = true;

	// Token: 0x04005D66 RID: 23910
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04005D67 RID: 23911
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CameraWindow cameraWindow;

	// Token: 0x04005D68 RID: 23912
	public static string ID = "powercamera";

	// Token: 0x04005D69 RID: 23913
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005D6A RID: 23914
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;

	// Token: 0x04005D6B RID: 23915
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowered tileEntity;
}
