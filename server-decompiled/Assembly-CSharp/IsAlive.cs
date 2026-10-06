using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000691 RID: 1681
[Preserve]
public class IsAlive : TargetedCompareRequirementBase
{
	// Token: 0x06003580 RID: 13696 RVA: 0x00161177 File Offset: 0x0015F377
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

	// Token: 0x06003581 RID: 13697 RVA: 0x001611B6 File Offset: 0x0015F3B6
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Entity {0}Alive", this.invert ? "NOT " : ""));
	}
}
