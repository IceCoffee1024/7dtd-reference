using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000690 RID: 1680
[Preserve]
public class WasAlive : TargetedCompareRequirementBase
{
	// Token: 0x06003540 RID: 13632 RVA: 0x0016085D File Offset: 0x0015EA5D
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

	// Token: 0x06003541 RID: 13633 RVA: 0x0016089C File Offset: 0x0015EA9C
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Entity Was {0}Alive", this.invert ? "NOT " : ""));
	}
}
