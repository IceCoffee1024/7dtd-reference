using System;
using UnityEngine.Scripting;

// Token: 0x02000E4C RID: 3660
[Preserve]
public class XUiC_WorldGenerationWindowGroup : XUiC_EditingToolsDialogBase
{
	// Token: 0x060070E2 RID: 28898 RVA: 0x002B5789 File Offset: 0x002B3989
	public override void Init()
	{
		base.Init();
		XUiC_WorldGenerationWindowGroup.ID = this.windowGroup.Id;
	}

	// Token: 0x060070E3 RID: 28899 RVA: 0x002B57A1 File Offset: 0x002B39A1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void close()
	{
		this.window.StartClose();
	}

	// Token: 0x040054EF RID: 21743
	public static string ID = "";

	// Token: 0x040054F0 RID: 21744
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldGenerationWindow window;
}
