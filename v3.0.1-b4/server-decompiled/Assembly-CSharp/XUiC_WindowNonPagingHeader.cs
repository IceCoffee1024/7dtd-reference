using System;
using UnityEngine.Scripting;

// Token: 0x0200109E RID: 4254
[Preserve]
public class XUiC_WindowNonPagingHeader : XUiController
{
	// Token: 0x0600877D RID: 34685 RVA: 0x0033D4DC File Offset: 0x0033B6DC
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("lblWindowName");
		if (childById != null)
		{
			this.lblWindowName = (XUiV_Label)childById.ViewComponent;
		}
	}

	// Token: 0x0600877E RID: 34686 RVA: 0x0033D50F File Offset: 0x0033B70F
	public void SetHeader(string name)
	{
		if (this.lblWindowName != null)
		{
			this.lblWindowName.Text = name;
		}
	}

	// Token: 0x0600877F RID: 34687 RVA: 0x0033D525 File Offset: 0x0033B725
	public override void OnOpen()
	{
		base.OnOpen();
		XUiC_FocusedBlockHealth.SetData(this.xui.playerUI, null, 0f);
		this.xui.DragAndDropWindow.InMenu = true;
	}

	// Token: 0x06008780 RID: 34688 RVA: 0x0033D554 File Offset: 0x0033B754
	public override void OnClose()
	{
		base.OnClose();
		this.xui.DragAndDropWindow.InMenu = false;
		if (this.xui.CurrentSelectedEntry != null)
		{
			this.xui.CurrentSelectedEntry.IsSelected = false;
		}
	}

	// Token: 0x040064E5 RID: 25829
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblWindowName;
}
