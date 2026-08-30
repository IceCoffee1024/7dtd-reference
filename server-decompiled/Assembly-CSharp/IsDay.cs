using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000690 RID: 1680
[Preserve]
public class IsDay : TargetedCompareRequirementBase
{
	// Token: 0x0600350B RID: 13579 RVA: 0x0015EDA6 File Offset: 0x0015CFA6
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (!this.invert)
		{
			return GameManager.Instance.World.IsDaytime();
		}
		return !GameManager.Instance.World.IsDaytime();
	}

	// Token: 0x0600350C RID: 13580 RVA: 0x0015EDDD File Offset: 0x0015CFDD
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} night time", this.invert ? "NOT " : ""));
	}
}
