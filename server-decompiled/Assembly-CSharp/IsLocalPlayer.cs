using System;
using UnityEngine.Scripting;

// Token: 0x02000678 RID: 1656
[Preserve]
public class IsLocalPlayer : TargetedCompareRequirementBase
{
	// Token: 0x060034F7 RID: 13559 RVA: 0x0015F76F File Offset: 0x0015D96F
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
