using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000698 RID: 1688
[Preserve]
public class IsAlive : TargetedCompareRequirementBase
{
	// Token: 0x0600358E RID: 13710 RVA: 0x00161D6B File Offset: 0x0015FF6B
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

	// Token: 0x0600358F RID: 13711 RVA: 0x00161DAA File Offset: 0x0015FFAA
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Entity {0}Alive", this.invert ? "NOT " : ""));
	}
}
