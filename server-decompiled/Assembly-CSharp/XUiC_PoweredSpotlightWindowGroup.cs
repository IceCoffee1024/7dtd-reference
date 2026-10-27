using System;
using UnityEngine.Scripting;

// Token: 0x02000FD4 RID: 4052
[Preserve]
public class XUiC_PoweredSpotlightWindowGroup : XUiC_PoweredGenericWindowGroup
{
	// Token: 0x06007F5F RID: 32607 RVA: 0x00307758 File Offset: 0x00305958
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

	// Token: 0x06007F60 RID: 32608 RVA: 0x003077B4 File Offset: 0x003059B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void setupWindowTileEntities()
	{
		base.setupWindowTileEntities();
		if (this.cameraWindowPreview != null)
		{
			this.cameraWindowPreview.TileEntity = this.tileEntity;
		}
	}

	// Token: 0x06007F61 RID: 32609 RVA: 0x003077D8 File Offset: 0x003059D8
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

	// Token: 0x06007F62 RID: 32610 RVA: 0x00307850 File Offset: 0x00305A50
	public override void OnClose()
	{
		base.TileEntity.Destroyed -= this.TileEntity_Destroyed;
		base.OnClose();
	}

	// Token: 0x06007F63 RID: 32611 RVA: 0x00307870 File Offset: 0x00305A70
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

	// Token: 0x04005E9A RID: 24218
	public static string ID = "powerspotlight";

	// Token: 0x04005E9B RID: 24219
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04005E9C RID: 24220
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CameraWindow cameraWindowPreview;
}
