using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006A3 RID: 1699
[Preserve]
public class IsDay : TargetedCompareRequirementBase
{
	// Token: 0x060035B2 RID: 13746 RVA: 0x001624F2 File Offset: 0x001606F2
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

	// Token: 0x060035B3 RID: 13747 RVA: 0x00162529 File Offset: 0x00160729
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} night time", this.invert ? "NOT " : ""));
	}
}
