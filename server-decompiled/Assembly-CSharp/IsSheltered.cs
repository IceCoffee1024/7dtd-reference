using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006A9 RID: 1705
[Preserve]
public class IsSheltered : TargetedCompareRequirementBase
{
	// Token: 0x060035C8 RID: 13768 RVA: 0x00162030 File Offset: 0x00160230
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		EntityPlayerLocal entityPlayerLocal = this.target as EntityPlayerLocal;
		return entityPlayerLocal != null && this.invert != entityPlayerLocal.shelterPercent > 0f;
	}

	// Token: 0x060035C9 RID: 13769 RVA: 0x00162071 File Offset: 0x00160271
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("{0}sheltered", this.invert ? "NOT " : ""));
	}
}
