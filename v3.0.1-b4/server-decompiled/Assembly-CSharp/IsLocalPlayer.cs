using System;
using UnityEngine.Scripting;

// Token: 0x0200066F RID: 1647
[Preserve]
public class IsLocalPlayer : TargetedCompareRequirementBase
{
	// Token: 0x060034A4 RID: 13476 RVA: 0x0015D5FF File Offset: 0x0015B7FF
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (!this.invert)
		{
			return this.target as EntityPlayerLocal != null;
		}
		return !(this.target as EntityPlayerLocal != null);
	}
}
