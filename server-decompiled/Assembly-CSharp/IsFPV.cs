using System;
using UnityEngine.Scripting;

// Token: 0x02000680 RID: 1664
[Preserve]
public class IsFPV : TargetedCompareRequirementBase
{
	// Token: 0x06003546 RID: 13638 RVA: 0x00160C64 File Offset: 0x0015EE64
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
