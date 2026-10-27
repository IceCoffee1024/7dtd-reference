using System;
using UnityEngine.Scripting;

// Token: 0x02000681 RID: 1665
[Preserve]
public class IsLocalPlayer : TargetedCompareRequirementBase
{
	// Token: 0x06003548 RID: 13640 RVA: 0x00160CC7 File Offset: 0x0015EEC7
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
