using System;
using UnityEngine.Scripting;

// Token: 0x020006D5 RID: 1749
[Preserve]
public class MinEventActionRemoveBuff : MinEventActionBuffModifierBase
{
	// Token: 0x06003668 RID: 13928 RVA: 0x001666FB File Offset: 0x001648FB
	public override void Execute(MinEventParams _params)
	{
		base.Remove(_params);
	}
}
