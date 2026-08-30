using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006A3 RID: 1699
[Preserve]
public class IsOnLadder : TargetedCompareRequirementBase
{
	// Token: 0x06003543 RID: 13635 RVA: 0x0015F8F7 File Offset: 0x0015DAF7
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (!this.invert)
		{
			return this.target.IsInElevator();
		}
		return !this.target.IsInElevator();
	}

	// Token: 0x06003544 RID: 13636 RVA: 0x0015F926 File Offset: 0x0015DB26
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} On Ladder", this.invert ? "NOT " : ""));
	}
}
