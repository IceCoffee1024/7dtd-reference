using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200069E RID: 1694
[Preserve]
public class IsBloodMoon : TargetedCompareRequirementBase
{
	// Token: 0x060035AA RID: 13738 RVA: 0x00161993 File Offset: 0x0015FB93
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params) && (SkyManager.IsBloodMoonVisible() ^ this.invert);
	}

	// Token: 0x060035AB RID: 13739 RVA: 0x001619AC File Offset: 0x0015FBAC
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} blood moon", this.invert ? "NOT " : ""));
	}
}
