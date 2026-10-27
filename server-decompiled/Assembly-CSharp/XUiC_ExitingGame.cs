using System;
using UnityEngine.Scripting;

// Token: 0x02000E24 RID: 3620
[Preserve]
public class XUiC_ExitingGame : XUiController
{
	// Token: 0x06006E3A RID: 28218 RVA: 0x002AFD43 File Offset: 0x002ADF43
	public override void Init()
	{
		base.Init();
		XUiC_ExitingGame.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006E3B RID: 28219 RVA: 0x002AFD5B File Offset: 0x002ADF5B
	public override void OnOpen()
	{
		base.OnOpen();
		((XUiV_Window)this.viewComponent).ForceVisible(1f);
	}

	// Token: 0x0400541F RID: 21535
	public static string ID;
}
