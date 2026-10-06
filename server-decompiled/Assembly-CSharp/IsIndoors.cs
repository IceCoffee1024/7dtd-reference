using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006A8 RID: 1704
[Preserve]
public class IsIndoors : TargetedCompareRequirementBase
{
	// Token: 0x060035C5 RID: 13765 RVA: 0x00161FB4 File Offset: 0x001601B4
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (!this.invert)
		{
			return this.target.Stats.AmountEnclosed > 0f;
		}
		return this.target.Stats.AmountEnclosed <= 0f;
	}

	// Token: 0x060035C6 RID: 13766 RVA: 0x00162006 File Offset: 0x00160206
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("{0}indoors", this.invert ? "NOT " : ""));
	}
}
