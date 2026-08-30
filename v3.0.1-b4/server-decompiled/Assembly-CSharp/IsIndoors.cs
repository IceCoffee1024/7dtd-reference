using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200069C RID: 1692
[Preserve]
public class IsIndoors : TargetedCompareRequirementBase
{
	// Token: 0x0600352C RID: 13612 RVA: 0x0015F474 File Offset: 0x0015D674
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

	// Token: 0x0600352D RID: 13613 RVA: 0x0015F4C6 File Offset: 0x0015D6C6
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("{0}indoors", this.invert ? "NOT " : ""));
	}
}
