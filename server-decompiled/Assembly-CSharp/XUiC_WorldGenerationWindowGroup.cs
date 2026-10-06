using System;
using UnityEngine.Scripting;

// Token: 0x02000E70 RID: 3696
[Preserve]
public class XUiC_WorldGenerationWindowGroup : XUiC_EditingToolsDialogBase
{
	// Token: 0x0600717A RID: 29050 RVA: 0x002BC05D File Offset: 0x002BA25D
	public override void Init()
	{
		base.Init();
		XUiC_WorldGenerationWindowGroup.ID = this.windowGroup.Id;
	}

	// Token: 0x0600717B RID: 29051 RVA: 0x002BC075 File Offset: 0x002BA275
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void close()
	{
		this.window.StartClose();
	}

	// Token: 0x040055CA RID: 21962
	public static string ID = "";

	// Token: 0x040055CB RID: 21963
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldGenerationWindow window;
}
