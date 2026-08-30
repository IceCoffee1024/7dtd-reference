using System;
using UnityEngine.Scripting;

// Token: 0x0200067B RID: 1659
[Preserve]
public class IsSDCS : TargetedCompareRequirementBase
{
	// Token: 0x060034FD RID: 13565 RVA: 0x0015F828 File Offset: 0x0015DA28
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
