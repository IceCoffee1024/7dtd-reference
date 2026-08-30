using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000685 RID: 1669
[Preserve]
public class IsAlive : TargetedCompareRequirementBase
{
	// Token: 0x060034E7 RID: 13543 RVA: 0x0015E61F File Offset: 0x0015C81F
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

	// Token: 0x060034E8 RID: 13544 RVA: 0x0015E65E File Offset: 0x0015C85E
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Entity {0}Alive", this.invert ? "NOT " : ""));
	}
}
