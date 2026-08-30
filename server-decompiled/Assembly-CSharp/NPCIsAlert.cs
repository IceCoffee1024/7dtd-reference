using System;
using System.Collections.Generic;

// Token: 0x02000684 RID: 1668
public class NPCIsAlert : TargetedCompareRequirementBase
{
	// Token: 0x060034E4 RID: 13540 RVA: 0x0015E5BA File Offset: 0x0015C7BA
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (!this.target.IsAlive())
		{
			return false;
		}
		if (this.invert)
		{
			return !this.target.IsAlert;
		}
		return this.target.IsAlert;
	}

	// Token: 0x060034E5 RID: 13541 RVA: 0x0015E5F8 File Offset: 0x0015C7F8
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Entity {0}Alert", this.invert ? "NOT " : ""));
	}
}
