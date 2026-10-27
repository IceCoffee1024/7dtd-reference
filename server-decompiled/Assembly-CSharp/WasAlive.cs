using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000699 RID: 1689
[Preserve]
public class WasAlive : TargetedCompareRequirementBase
{
	// Token: 0x06003591 RID: 13713 RVA: 0x00161DD1 File Offset: 0x0015FFD1
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (!(this.target != null))
		{
			return false;
		}
		if (this.invert)
		{
			return !this.target.WasAlive();
		}
		return this.target.WasAlive();
	}

	// Token: 0x06003592 RID: 13714 RVA: 0x00161E10 File Offset: 0x00160010
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Entity Was {0}Alive", this.invert ? "NOT " : ""));
	}
}
