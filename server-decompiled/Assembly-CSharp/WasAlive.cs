using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000692 RID: 1682
[Preserve]
public class WasAlive : TargetedCompareRequirementBase
{
	// Token: 0x06003583 RID: 13699 RVA: 0x001611DD File Offset: 0x0015F3DD
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

	// Token: 0x06003584 RID: 13700 RVA: 0x0016121C File Offset: 0x0015F41C
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Entity Was {0}Alive", this.invert ? "NOT " : ""));
	}
}
