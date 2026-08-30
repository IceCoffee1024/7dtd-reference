using System;
using System.Collections.Generic;

// Token: 0x0200068E RID: 1678
public class NPCIsAlert : TargetedCompareRequirementBase
{
	// Token: 0x0600353A RID: 13626 RVA: 0x00160792 File Offset: 0x0015E992
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

	// Token: 0x0600353B RID: 13627 RVA: 0x001607D0 File Offset: 0x0015E9D0
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Entity {0}Alert", this.invert ? "NOT " : ""));
	}
}
