using System;
using UnityEngine.Scripting;

// Token: 0x02000679 RID: 1657
[Preserve]
public class IsFPV : TargetedCompareRequirementBase
{
	// Token: 0x06003538 RID: 13624 RVA: 0x00160070 File Offset: 0x0015E270
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (!(this.target as EntityPlayerLocal != null))
		{
			return this.invert;
		}
		if (!this.invert)
		{
			return (this.target as EntityPlayerLocal).bFirstPersonView;
		}
		return !(this.target as EntityPlayerLocal).bFirstPersonView;
	}
}
