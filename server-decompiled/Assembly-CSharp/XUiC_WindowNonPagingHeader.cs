using System;
using UnityEngine.Scripting;

// Token: 0x020010D8 RID: 4312
[Preserve]
public class XUiC_WindowNonPagingHeader : XUiController
{
	// Token: 0x06008910 RID: 35088 RVA: 0x00347B38 File Offset: 0x00345D38
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("lblWindowName");
		if (childById != null)
		{
			this.lblWindowName = (XUiV_Label)childById.ViewComponent;
		}
	}

	// Token: 0x06008911 RID: 35089 RVA: 0x00347B6B File Offset: 0x00345D6B
	public void SetHeader(string name)
	{
		if (this.lblWindowName != null)
		{
			this.lblWindowName.Text = name;
		}
	}

	// Token: 0x06008912 RID: 35090 RVA: 0x00347B81 File Offset: 0x00345D81
	public override void OnOpen()
	{
		base.OnOpen();
		XUiC_FocusedBlockHealth.SetData(this.xui.playerUI, null, 0f);
		this.xui.DragAndDropWindow.InMenu = true;
	}

	// Token: 0x06008913 RID: 35091 RVA: 0x00347BB0 File Offset: 0x00345DB0
	public override void OnClose()
	{
		base.OnClose();
		this.xui.DragAndDropWindow.InMenu = false;
		if (this.xui.CurrentSelectedEntry != null)
		{
			this.xui.CurrentSelectedEntry.IsSelected = false;
		}
	}

	// Token: 0x0400662D RID: 26157
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblWindowName;
}
