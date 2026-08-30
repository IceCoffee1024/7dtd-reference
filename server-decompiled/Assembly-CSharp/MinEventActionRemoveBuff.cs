using System;
using UnityEngine.Scripting;

// Token: 0x020006D3 RID: 1747
[Preserve]
public class MinEventActionRemoveBuff : MinEventActionBuffModifierBase
{
	// Token: 0x06003625 RID: 13861 RVA: 0x00165D8B File Offset: 0x00163F8B
	public override void Execute(MinEventParams _params)
	{
		base.Remove(_params);
	}
}
