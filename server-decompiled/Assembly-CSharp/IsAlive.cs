using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200068F RID: 1679
[Preserve]
public class IsAlive : TargetedCompareRequirementBase
{
	// Token: 0x0600353D RID: 13629 RVA: 0x001607F7 File Offset: 0x0015E9F7
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
			return !this.target.IsAlive();
		}
		return this.target.IsAlive();
	}

	// Token: 0x0600353E RID: 13630 RVA: 0x00160836 File Offset: 0x0015EA36
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Entity {0}Alive", this.invert ? "NOT " : ""));
	}
}
