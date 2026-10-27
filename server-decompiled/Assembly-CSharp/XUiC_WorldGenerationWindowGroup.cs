using System;
using UnityEngine.Scripting;

// Token: 0x02000E7B RID: 3707
[Preserve]
public class XUiC_WorldGenerationWindowGroup : XUiC_EditingToolsDialogBase
{
	// Token: 0x0600719F RID: 29087 RVA: 0x002BD5C9 File Offset: 0x002BB7C9
	public override void Init()
	{
		base.Init();
		XUiC_WorldGenerationWindowGroup.ID = this.windowGroup.Id;
	}

	// Token: 0x060071A0 RID: 29088 RVA: 0x002BD5E1 File Offset: 0x002BB7E1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void close()
	{
		this.window.StartClose();
	}

	// Token: 0x040055EF RID: 21999
	public static string ID = "";

	// Token: 0x040055F0 RID: 22000
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldGenerationWindow window;
}
