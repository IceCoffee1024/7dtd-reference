using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200069D RID: 1693
[Preserve]
public class IsSheltered : TargetedCompareRequirementBase
{
	// Token: 0x0600352F RID: 13615 RVA: 0x0015F4F0 File Offset: 0x0015D6F0
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		EntityPlayerLocal entityPlayerLocal = this.target as EntityPlayerLocal;
		return entityPlayerLocal != null && this.invert != entityPlayerLocal.shelterPercent > 0f;
	}

	// Token: 0x06003530 RID: 13616 RVA: 0x0015F531 File Offset: 0x0015D731
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("{0}sheltered", this.invert ? "NOT " : ""));
	}
}
