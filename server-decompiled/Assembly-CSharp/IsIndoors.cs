using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006A6 RID: 1702
[Preserve]
public class IsIndoors : TargetedCompareRequirementBase
{
	// Token: 0x06003582 RID: 13698 RVA: 0x0016164C File Offset: 0x0015F84C
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

	// Token: 0x06003583 RID: 13699 RVA: 0x0016169E File Offset: 0x0015F89E
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("{0}indoors", this.invert ? "NOT " : ""));
	}
}
