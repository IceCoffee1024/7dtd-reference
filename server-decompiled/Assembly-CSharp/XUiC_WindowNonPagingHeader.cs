using System;
using UnityEngine.Scripting;

// Token: 0x020010B7 RID: 4279
[Preserve]
public class XUiC_WindowNonPagingHeader : XUiController
{
	// Token: 0x0600886F RID: 34927 RVA: 0x00341D04 File Offset: 0x0033FF04
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("lblWindowName");
		if (childById != null)
		{
			this.lblWindowName = (XUiV_Label)childById.ViewComponent;
		}
	}

	// Token: 0x06008870 RID: 34928 RVA: 0x00341D37 File Offset: 0x0033FF37
	public void SetHeader(string name)
	{
		if (this.lblWindowName != null)
		{
			this.lblWindowName.Text = name;
		}
	}

	// Token: 0x06008871 RID: 34929 RVA: 0x00341D4D File Offset: 0x0033FF4D
	public override void OnOpen()
	{
		base.OnOpen();
		XUiC_FocusedBlockHealth.SetData(this.xui.playerUI, null, 0f);
		this.xui.DragAndDropWindow.InMenu = true;
	}

	// Token: 0x06008872 RID: 34930 RVA: 0x00341D7C File Offset: 0x0033FF7C
	public override void OnClose()
	{
		base.OnClose();
		this.xui.DragAndDropWindow.InMenu = false;
		if (this.xui.CurrentSelectedEntry != null)
		{
			this.xui.CurrentSelectedEntry.IsSelected = false;
		}
	}

	// Token: 0x040065AC RID: 26028
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblWindowName;
}
