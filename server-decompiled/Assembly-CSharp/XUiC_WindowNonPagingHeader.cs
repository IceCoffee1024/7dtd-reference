using System;
using UnityEngine.Scripting;

// Token: 0x020010C8 RID: 4296
[Preserve]
public class XUiC_WindowNonPagingHeader : XUiController
{
	// Token: 0x060088BD RID: 35005 RVA: 0x00345AB4 File Offset: 0x00343CB4
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("lblWindowName");
		if (childById != null)
		{
			this.lblWindowName = (XUiV_Label)childById.ViewComponent;
		}
	}

	// Token: 0x060088BE RID: 35006 RVA: 0x00345AE7 File Offset: 0x00343CE7
	public void SetHeader(string name)
	{
		if (this.lblWindowName != null)
		{
			this.lblWindowName.Text = name;
		}
	}

	// Token: 0x060088BF RID: 35007 RVA: 0x00345AFD File Offset: 0x00343CFD
	public override void OnOpen()
	{
		base.OnOpen();
		XUiC_FocusedBlockHealth.SetData(this.xui.playerUI, null, 0f);
		this.xui.DragAndDropWindow.InMenu = true;
	}

	// Token: 0x060088C0 RID: 35008 RVA: 0x00345B2C File Offset: 0x00343D2C
	public override void OnClose()
	{
		base.OnClose();
		this.xui.DragAndDropWindow.InMenu = false;
		if (this.xui.CurrentSelectedEntry != null)
		{
			this.xui.CurrentSelectedEntry.IsSelected = false;
		}
	}

	// Token: 0x040065EE RID: 26094
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblWindowName;
}
