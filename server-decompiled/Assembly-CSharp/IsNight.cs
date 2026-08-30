using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200069B RID: 1691
[Preserve]
public class IsNight : TargetedCompareRequirementBase
{
	// Token: 0x06003564 RID: 13668 RVA: 0x00160FDC File Offset: 0x0015F1DC
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

	// Token: 0x06003565 RID: 13669 RVA: 0x00160FB5 File Offset: 0x0015F1B5
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} night time", this.invert ? "NOT " : ""));
	}
}
