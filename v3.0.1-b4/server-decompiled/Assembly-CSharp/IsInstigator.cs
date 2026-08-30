using System;
using UnityEngine.Scripting;

// Token: 0x02000670 RID: 1648
[Preserve]
public class IsInstigator : TargetedCompareRequirementBase
{
	// Token: 0x060034A6 RID: 13478 RVA: 0x0015D63A File Offset: 0x0015B83A
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params) && this.invert != (this.target == _params.Instigator);
	}
}
