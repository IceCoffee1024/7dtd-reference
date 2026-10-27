using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006B6 RID: 1718
[Preserve]
public class IsOnLadder : TargetedCompareRequirementBase
{
	// Token: 0x060035EA RID: 13802 RVA: 0x0016302B File Offset: 0x0016122B
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

	// Token: 0x060035EB RID: 13803 RVA: 0x0016305A File Offset: 0x0016125A
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} On Ladder", this.invert ? "NOT " : ""));
	}
}
