using System;
using UnityEngine.Scripting;

// Token: 0x02000F9C RID: 3996
[Preserve]
public class XUiC_PoweredSpotlightWindowGroup : XUiC_PoweredGenericWindowGroup
{
	// Token: 0x06007E0F RID: 32271 RVA: 0x002FE490 File Offset: 0x002FC690
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

	// Token: 0x06007E10 RID: 32272 RVA: 0x002FE4EC File Offset: 0x002FC6EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void setupWindowTileEntities()
	{
		base.setupWindowTileEntities();
		if (this.cameraWindowPreview != null)
		{
			this.cameraWindowPreview.TileEntity = this.tileEntity;
		}
	}

	// Token: 0x06007E11 RID: 32273 RVA: 0x002FE510 File Offset: 0x002FC710
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

	// Token: 0x06007E12 RID: 32274 RVA: 0x002FE588 File Offset: 0x002FC788
	public override void OnClose()
	{
		base.TileEntity.Destroyed -= this.TileEntity_Destroyed;
		base.OnClose();
	}

	// Token: 0x06007E13 RID: 32275 RVA: 0x002FE5A8 File Offset: 0x002FC7A8
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

	// Token: 0x04005D6F RID: 23919
	public static string ID = "powerspotlight";

	// Token: 0x04005D70 RID: 23920
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04005D71 RID: 23921
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CameraWindow cameraWindowPreview;
}
