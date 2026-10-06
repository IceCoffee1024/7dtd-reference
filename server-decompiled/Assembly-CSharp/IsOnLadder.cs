using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006AF RID: 1711
[Preserve]
public class IsOnLadder : TargetedCompareRequirementBase
{
	// Token: 0x060035DC RID: 13788 RVA: 0x00162437 File Offset: 0x00160637
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

	// Token: 0x060035DD RID: 13789 RVA: 0x00162466 File Offset: 0x00160666
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} On Ladder", this.invert ? "NOT " : ""));
	}
}
