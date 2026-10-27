using System;
using UnityEngine.Scripting;

// Token: 0x020006DD RID: 1757
[Preserve]
public class MinEventActionRemoveBuff : MinEventActionBuffModifierBase
{
	// Token: 0x06003679 RID: 13945 RVA: 0x001674AC File Offset: 0x001656AC
	public override void Execute(MinEventParams _params)
	{
		base.Remove(_params);
	}
}
