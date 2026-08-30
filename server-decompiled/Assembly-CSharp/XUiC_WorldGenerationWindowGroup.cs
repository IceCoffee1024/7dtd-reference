using System;
using UnityEngine.Scripting;

// Token: 0x02000E61 RID: 3681
[Preserve]
public class XUiC_WorldGenerationWindowGroup : XUiC_EditingToolsDialogBase
{
	// Token: 0x06007182 RID: 29058 RVA: 0x002B8CAD File Offset: 0x002B6EAD
	public override void Init()
	{
		base.Init();
		XUiC_WorldGenerationWindowGroup.ID = this.windowGroup.Id;
	}

	// Token: 0x06007183 RID: 29059 RVA: 0x002B8CC5 File Offset: 0x002B6EC5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void close()
	{
		this.window.StartClose();
	}

	// Token: 0x04005593 RID: 21907
	public static string ID = "";

	// Token: 0x04005594 RID: 21908
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldGenerationWindow window;
}
