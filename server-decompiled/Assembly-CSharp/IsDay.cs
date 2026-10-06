using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200069C RID: 1692
[Preserve]
public class IsDay : TargetedCompareRequirementBase
{
	// Token: 0x060035A4 RID: 13732 RVA: 0x001618FE File Offset: 0x0015FAFE
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

	// Token: 0x060035A5 RID: 13733 RVA: 0x00161935 File Offset: 0x0015FB35
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} night time", this.invert ? "NOT " : ""));
	}
}
