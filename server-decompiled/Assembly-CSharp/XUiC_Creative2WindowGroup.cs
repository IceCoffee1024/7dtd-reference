using System;
using UnityEngine.Scripting;

// Token: 0x02000F19 RID: 3865
[Preserve]
public class XUiC_Creative2WindowGroup : XUiController
{
	// Token: 0x06007942 RID: 31042 RVA: 0x002DBC78 File Offset: 0x002D9E78
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

	// Token: 0x06007943 RID: 31043 RVA: 0x002D9D14 File Offset: 0x002D7F14
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("windowpaging");
	}
}
