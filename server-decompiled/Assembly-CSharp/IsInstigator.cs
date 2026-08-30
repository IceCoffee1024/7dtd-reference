using System;
using UnityEngine.Scripting;

// Token: 0x02000679 RID: 1657
[Preserve]
public class IsInstigator : TargetedCompareRequirementBase
{
	// Token: 0x060034F9 RID: 13561 RVA: 0x0015F7AA File Offset: 0x0015D9AA
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params) && this.invert != (this.target == _params.Instigator);
	}
}
