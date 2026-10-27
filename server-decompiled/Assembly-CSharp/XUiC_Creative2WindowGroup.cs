using System;
using UnityEngine.Scripting;

// Token: 0x02000F37 RID: 3895
[Preserve]
public class XUiC_Creative2WindowGroup : XUiController
{
	// Token: 0x06007980 RID: 31104 RVA: 0x002E0904 File Offset: 0x002DEB04
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.windowManager.Open("windowpaging", false);
		XUiC_WindowSelector childByType = this.xui.FindWindowGroupByName("windowpaging").GetChildByType<XUiC_WindowSelector>();
		if (childByType == null)
		{
			return;
		}
		childByType.SetSelected("creative2");
	}

	// Token: 0x06007981 RID: 31105 RVA: 0x002DE998 File Offset: 0x002DCB98
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("windowpaging");
	}
}
