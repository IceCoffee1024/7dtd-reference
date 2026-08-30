using System;
using UnityEngine.Scripting;

// Token: 0x02000FB5 RID: 4021
[Preserve]
public class XUiC_PoweredSpotlightWindowGroup : XUiC_PoweredGenericWindowGroup
{
	// Token: 0x06007F01 RID: 32513 RVA: 0x00302C64 File Offset: 0x00300E64
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

	// Token: 0x06007F02 RID: 32514 RVA: 0x00302CC0 File Offset: 0x00300EC0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void setupWindowTileEntities()
	{
		base.setupWindowTileEntities();
		if (this.cameraWindowPreview != null)
		{
			this.cameraWindowPreview.TileEntity = this.tileEntity;
		}
	}

	// Token: 0x06007F03 RID: 32515 RVA: 0x00302CE4 File Offset: 0x00300EE4
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

	// Token: 0x06007F04 RID: 32516 RVA: 0x00302D5C File Offset: 0x00300F5C
	public override void OnClose()
	{
		base.TileEntity.Destroyed -= this.TileEntity_Destroyed;
		base.OnClose();
	}

	// Token: 0x06007F05 RID: 32517 RVA: 0x00302D7C File Offset: 0x00300F7C
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

	// Token: 0x04005E36 RID: 24118
	public static string ID = "powerspotlight";

	// Token: 0x04005E37 RID: 24119
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04005E38 RID: 24120
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CameraWindow cameraWindowPreview;
}
