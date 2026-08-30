using System;
using UnityEngine.Scripting;

// Token: 0x02000DF5 RID: 3573
[Preserve]
public class XUiC_ExitingGame : XUiController
{
	// Token: 0x06006D7D RID: 28029 RVA: 0x002A7F1B File Offset: 0x002A611B
	public override void Init()
	{
		base.Init();
		XUiC_ExitingGame.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006D7E RID: 28030 RVA: 0x002A7F33 File Offset: 0x002A6133
	public override void OnOpen()
	{
		base.OnOpen();
		((XUiV_Window)this.viewComponent).ForceVisible(1f);
	}

	// Token: 0x04005320 RID: 21280
	public static string ID;
}
