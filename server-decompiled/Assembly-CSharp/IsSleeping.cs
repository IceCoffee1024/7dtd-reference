using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006A1 RID: 1697
[Preserve]
public class IsSleeping : TargetedCompareRequirementBase
{
	// Token: 0x06003575 RID: 13685 RVA: 0x001612B0 File Offset: 0x0015F4B0
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		EntityEnemy entityEnemy = this.target as EntityEnemy;
		if (entityEnemy == null)
		{
			return false;
		}
		if (!this.invert)
		{
			return entityEnemy.IsSleeping;
		}
		return !entityEnemy.IsSleeping;
	}

	// Token: 0x06003576 RID: 13686 RVA: 0x001612F7 File Offset: 0x0015F4F7
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("{0}sleeping", this.invert ? "NOT " : ""));
	}
}
