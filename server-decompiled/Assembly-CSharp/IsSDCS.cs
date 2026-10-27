using System;
using UnityEngine.Scripting;

// Token: 0x02000684 RID: 1668
[Preserve]
public class IsSDCS : TargetedCompareRequirementBase
{
	// Token: 0x0600354E RID: 13646 RVA: 0x00160D80 File Offset: 0x0015EF80
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
