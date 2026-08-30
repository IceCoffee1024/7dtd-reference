using System;
using UnityEngine.Scripting;

// Token: 0x0200066E RID: 1646
[Preserve]
public class IsFPV : TargetedCompareRequirementBase
{
	// Token: 0x060034A2 RID: 13474 RVA: 0x0015D59C File Offset: 0x0015B79C
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
