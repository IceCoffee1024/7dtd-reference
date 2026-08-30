using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000686 RID: 1670
[Preserve]
public class WasAlive : TargetedCompareRequirementBase
{
	// Token: 0x060034EA RID: 13546 RVA: 0x0015E685 File Offset: 0x0015C885
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

	// Token: 0x060034EB RID: 13547 RVA: 0x0015E6C4 File Offset: 0x0015C8C4
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Entity Was {0}Alive", this.invert ? "NOT " : ""));
	}
}
