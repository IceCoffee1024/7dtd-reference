using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006AF RID: 1711
[Preserve]
public class IsIndoors : TargetedCompareRequirementBase
{
	// Token: 0x060035D3 RID: 13779 RVA: 0x00162BA8 File Offset: 0x00160DA8
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

	// Token: 0x060035D4 RID: 13780 RVA: 0x00162BFA File Offset: 0x00160DFA
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("{0}indoors", this.invert ? "NOT " : ""));
	}
}
