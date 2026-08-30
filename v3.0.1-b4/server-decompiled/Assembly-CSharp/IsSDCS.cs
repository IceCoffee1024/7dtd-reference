using System;
using UnityEngine.Scripting;

// Token: 0x02000672 RID: 1650
[Preserve]
public class IsSDCS : TargetedCompareRequirementBase
{
	// Token: 0x060034AA RID: 13482 RVA: 0x0015D6B8 File Offset: 0x0015B8B8
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		EntityAlive target = this.target;
		bool flag = ((target != null) ? target.emodel : null) as EModelSDCS != null;
		if (!this.invert)
		{
			return flag;
		}
		return !flag;
	}
}
