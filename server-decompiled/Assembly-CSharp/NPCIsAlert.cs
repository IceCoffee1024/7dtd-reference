using System;
using System.Collections.Generic;

// Token: 0x02000697 RID: 1687
public class NPCIsAlert : TargetedCompareRequirementBase
{
	// Token: 0x0600358B RID: 13707 RVA: 0x00161D06 File Offset: 0x0015FF06
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

	// Token: 0x0600358C RID: 13708 RVA: 0x00161D44 File Offset: 0x0015FF44
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Entity {0}Alert", this.invert ? "NOT " : ""));
	}
}
