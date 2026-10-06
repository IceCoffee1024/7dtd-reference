using System;
using UnityEngine.Scripting;

// Token: 0x0200067B RID: 1659
[Preserve]
public class IsInstigator : TargetedCompareRequirementBase
{
	// Token: 0x0600353C RID: 13628 RVA: 0x0016010E File Offset: 0x0015E30E
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params) && this.invert != (this.target == _params.Instigator);
	}
}
