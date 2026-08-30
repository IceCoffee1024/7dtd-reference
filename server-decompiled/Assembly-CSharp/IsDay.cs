using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200069A RID: 1690
[Preserve]
public class IsDay : TargetedCompareRequirementBase
{
	// Token: 0x06003561 RID: 13665 RVA: 0x00160F7E File Offset: 0x0015F17E
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

	// Token: 0x06003562 RID: 13666 RVA: 0x00160FB5 File Offset: 0x0015F1B5
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} night time", this.invert ? "NOT " : ""));
	}
}
