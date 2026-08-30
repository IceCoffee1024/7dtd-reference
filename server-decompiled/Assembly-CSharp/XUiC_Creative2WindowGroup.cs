using System;
using UnityEngine.Scripting;

// Token: 0x02000F02 RID: 3842
[Preserve]
public class XUiC_Creative2WindowGroup : XUiController
{
	// Token: 0x0600788A RID: 30858 RVA: 0x002D7CEC File Offset: 0x002D5EEC
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

	// Token: 0x0600788B RID: 30859 RVA: 0x002D5D88 File Offset: 0x002D3F88
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("windowpaging");
	}
}
