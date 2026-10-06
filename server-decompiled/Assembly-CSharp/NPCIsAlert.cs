using System;
using System.Collections.Generic;

// Token: 0x02000690 RID: 1680
public class NPCIsAlert : TargetedCompareRequirementBase
{
	// Token: 0x0600357D RID: 13693 RVA: 0x00161112 File Offset: 0x0015F312
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

	// Token: 0x0600357E RID: 13694 RVA: 0x00161150 File Offset: 0x0015F350
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Entity {0}Alert", this.invert ? "NOT " : ""));
	}
}
