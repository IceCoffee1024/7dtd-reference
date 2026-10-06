using System;
using UnityEngine.Scripting;

// Token: 0x02000F28 RID: 3880
[Preserve]
public class XUiC_Creative2WindowGroup : XUiController
{
	// Token: 0x0600793D RID: 31037 RVA: 0x002DF014 File Offset: 0x002DD214
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

	// Token: 0x0600793E RID: 31038 RVA: 0x002DD0A8 File Offset: 0x002DB2A8
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("windowpaging");
	}
}
