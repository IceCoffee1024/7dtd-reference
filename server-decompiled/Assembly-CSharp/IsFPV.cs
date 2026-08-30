using System;
using UnityEngine.Scripting;

// Token: 0x02000677 RID: 1655
[Preserve]
public class IsFPV : TargetedCompareRequirementBase
{
	// Token: 0x060034F5 RID: 13557 RVA: 0x0015F70C File Offset: 0x0015D90C
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
