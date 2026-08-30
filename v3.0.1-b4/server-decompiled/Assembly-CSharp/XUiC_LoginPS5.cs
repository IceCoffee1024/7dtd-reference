using System;
using UnityEngine.Scripting;

// Token: 0x02000061 RID: 97
[Preserve]
public class XUiC_LoginPS5 : XUiC_LoginBase
{
	// Token: 0x17000029 RID: 41
	// (get) Token: 0x060001DF RID: 479 RVA: 0x00010E00 File Offset: 0x0000F000
	public override string MsgTitleKey
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return "xuiPSNLogin";
		}
	}
}
