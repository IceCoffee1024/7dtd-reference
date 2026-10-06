using System;
using UnityEngine.Scripting;

// Token: 0x0200067A RID: 1658
[Preserve]
public class IsLocalPlayer : TargetedCompareRequirementBase
{
	// Token: 0x0600353A RID: 13626 RVA: 0x001600D3 File Offset: 0x0015E2D3
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (!this.invert)
		{
			return this.target as EntityPlayerLocal != null;
		}
		return !(this.target as EntityPlayerLocal != null);
	}
}
