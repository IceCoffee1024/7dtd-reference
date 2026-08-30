using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000691 RID: 1681
[Preserve]
public class IsNight : TargetedCompareRequirementBase
{
	// Token: 0x0600350E RID: 13582 RVA: 0x0015EE04 File Offset: 0x0015D004
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (!this.invert)
		{
			return !GameManager.Instance.World.IsDaytime();
		}
		return GameManager.Instance.World.IsDaytime();
	}

	// Token: 0x0600350F RID: 13583 RVA: 0x0015EDDD File Offset: 0x0015CFDD
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} night time", this.invert ? "NOT " : ""));
	}
}
