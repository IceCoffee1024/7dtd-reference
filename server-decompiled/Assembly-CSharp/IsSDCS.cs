using System;
using UnityEngine.Scripting;

// Token: 0x0200067D RID: 1661
[Preserve]
public class IsSDCS : TargetedCompareRequirementBase
{
	// Token: 0x06003540 RID: 13632 RVA: 0x0016018C File Offset: 0x0015E38C
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
