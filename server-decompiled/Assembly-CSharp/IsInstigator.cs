using System;
using UnityEngine.Scripting;

// Token: 0x02000682 RID: 1666
[Preserve]
public class IsInstigator : TargetedCompareRequirementBase
{
	// Token: 0x0600354A RID: 13642 RVA: 0x00160D02 File Offset: 0x0015EF02
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params) && this.invert != (this.target == _params.Instigator);
	}
}
