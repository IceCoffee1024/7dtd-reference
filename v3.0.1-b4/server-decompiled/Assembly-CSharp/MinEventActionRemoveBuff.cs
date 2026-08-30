using System;
using UnityEngine.Scripting;

// Token: 0x020006C9 RID: 1737
[Preserve]
public class MinEventActionRemoveBuff : MinEventActionBuffModifierBase
{
	// Token: 0x060035CF RID: 13775 RVA: 0x00163BB3 File Offset: 0x00161DB3
	public override void Execute(MinEventParams _params)
	{
		base.Remove(_params);
	}
}
