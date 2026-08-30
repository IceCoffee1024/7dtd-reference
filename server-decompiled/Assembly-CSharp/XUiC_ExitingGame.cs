using System;
using UnityEngine.Scripting;

// Token: 0x02000E0A RID: 3594
[Preserve]
public class XUiC_ExitingGame : XUiController
{
	// Token: 0x06006E1D RID: 28189 RVA: 0x002AB42F File Offset: 0x002A962F
	public override void Init()
	{
		base.Init();
		XUiC_ExitingGame.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006E1E RID: 28190 RVA: 0x002AB447 File Offset: 0x002A9647
	public override void OnOpen()
	{
		base.OnOpen();
		((XUiV_Window)this.viewComponent).ForceVisible(1f);
	}

	// Token: 0x040053C3 RID: 21443
	public static string ID;
}
