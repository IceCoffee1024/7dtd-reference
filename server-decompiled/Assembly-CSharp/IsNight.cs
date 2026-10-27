using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006A4 RID: 1700
[Preserve]
public class IsNight : TargetedCompareRequirementBase
{
	// Token: 0x060035B5 RID: 13749 RVA: 0x00162550 File Offset: 0x00160750
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

	// Token: 0x060035B6 RID: 13750 RVA: 0x00162529 File Offset: 0x00160729
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} night time", this.invert ? "NOT " : ""));
	}
}
