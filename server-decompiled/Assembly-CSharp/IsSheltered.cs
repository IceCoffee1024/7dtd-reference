using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006B0 RID: 1712
[Preserve]
public class IsSheltered : TargetedCompareRequirementBase
{
	// Token: 0x060035D6 RID: 13782 RVA: 0x00162C24 File Offset: 0x00160E24
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		EntityPlayerLocal entityPlayerLocal = this.target as EntityPlayerLocal;
		return entityPlayerLocal != null && this.invert != entityPlayerLocal.shelterPercent > 0f;
	}

	// Token: 0x060035D7 RID: 13783 RVA: 0x00162C65 File Offset: 0x00160E65
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("{0}sheltered", this.invert ? "NOT " : ""));
	}
}
