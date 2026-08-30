using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006A7 RID: 1703
[Preserve]
public class IsSheltered : TargetedCompareRequirementBase
{
	// Token: 0x06003585 RID: 13701 RVA: 0x001616C8 File Offset: 0x0015F8C8
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		EntityPlayerLocal entityPlayerLocal = this.target as EntityPlayerLocal;
		return entityPlayerLocal != null && this.invert != entityPlayerLocal.shelterPercent > 0f;
	}

	// Token: 0x06003586 RID: 13702 RVA: 0x00161709 File Offset: 0x0015F909
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("{0}sheltered", this.invert ? "NOT " : ""));
	}
}
