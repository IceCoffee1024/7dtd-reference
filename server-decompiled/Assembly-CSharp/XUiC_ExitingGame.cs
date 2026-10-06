using System;
using UnityEngine.Scripting;

// Token: 0x02000E19 RID: 3609
[Preserve]
public class XUiC_ExitingGame : XUiController
{
	// Token: 0x06006E15 RID: 28181 RVA: 0x002AE7D7 File Offset: 0x002AC9D7
	public override void Init()
	{
		base.Init();
		XUiC_ExitingGame.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006E16 RID: 28182 RVA: 0x002AE7EF File Offset: 0x002AC9EF
	public override void OnOpen()
	{
		base.OnOpen();
		((XUiV_Window)this.viewComponent).ForceVisible(1f);
	}

	// Token: 0x040053FA RID: 21498
	public static string ID;
}
