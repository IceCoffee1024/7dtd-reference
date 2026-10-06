using System;
using UnityEngine.Scripting;

// Token: 0x02000FC4 RID: 4036
[Preserve]
public class XUiC_PoweredSpotlightWindowGroup : XUiC_PoweredGenericWindowGroup
{
	// Token: 0x06007F10 RID: 32528 RVA: 0x00305C3C File Offset: 0x00303E3C
	public override void Init()
	{
		base.Init();
		XUiController xuiController = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		if (xuiController != null)
		{
			this.nonPagingHeader = (XUiC_WindowNonPagingHeader)xuiController;
		}
		xuiController = base.GetChildById("windowPowerCameraControlPreview");
		if (xuiController != null)
		{
			this.cameraWindowPreview = (XUiC_CameraWindow)xuiController;
			this.cameraWindowPreview.Owner = this;
			this.cameraWindowPreview.UseEdgeDetection = false;
		}
	}

	// Token: 0x06007F11 RID: 32529 RVA: 0x00305C98 File Offset: 0x00303E98
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void setupWindowTileEntities()
	{
		base.setupWindowTileEntities();
		if (this.cameraWindowPreview != null)
		{
			this.cameraWindowPreview.TileEntity = this.tileEntity;
		}
	}

	// Token: 0x06007F12 RID: 32530 RVA: 0x00305CBC File Offset: 0x00303EBC
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.nonPagingHeader != null)
		{
			World world = GameManager.Instance.World;
			string localizedBlockName = this.tileEntity.GetChunk().GetBlock(this.tileEntity.localChunkPos).Block.GetLocalizedBlockName();
			this.nonPagingHeader.SetHeader(localizedBlockName);
		}
		base.TileEntity.Destroyed += this.TileEntity_Destroyed;
	}

	// Token: 0x06007F13 RID: 32531 RVA: 0x00305D34 File Offset: 0x00303F34
	public override void OnClose()
	{
		base.TileEntity.Destroyed -= this.TileEntity_Destroyed;
		base.OnClose();
	}

	// Token: 0x06007F14 RID: 32532 RVA: 0x00305D54 File Offset: 0x00303F54
	[PublicizedFrom(EAccessModifier.Private)]
	public void TileEntity_Destroyed(ITileEntity te)
	{
		if (base.TileEntity == te)
		{
			if (GameManager.Instance != null)
			{
				this.xui.playerUI.windowManager.Close("powerspotlight");
				this.xui.playerUI.windowManager.Close("powercamera");
				return;
			}
		}
		else
		{
			te.Destroyed -= this.TileEntity_Destroyed;
		}
	}

	// Token: 0x04005E5F RID: 24159
	public static string ID = "powerspotlight";

	// Token: 0x04005E60 RID: 24160
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04005E61 RID: 24161
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CameraWindow cameraWindowPreview;
}
