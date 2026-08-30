using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000692 RID: 1682
[Preserve]
public class IsBloodMoon : TargetedCompareRequirementBase
{
	// Token: 0x06003511 RID: 13585 RVA: 0x0015EE3B File Offset: 0x0015D03B
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params) && (SkyManager.IsBloodMoonVisible() ^ this.invert);
	}

	// Token: 0x06003512 RID: 13586 RVA: 0x0015EE54 File Offset: 0x0015D054
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} blood moon", this.invert ? "NOT " : ""));
	}
}
