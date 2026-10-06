using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200069D RID: 1693
[Preserve]
public class IsNight : TargetedCompareRequirementBase
{
	// Token: 0x060035A7 RID: 13735 RVA: 0x0016195C File Offset: 0x0015FB5C
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

	// Token: 0x060035A8 RID: 13736 RVA: 0x00161935 File Offset: 0x0015FB35
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} night time", this.invert ? "NOT " : ""));
	}
}
