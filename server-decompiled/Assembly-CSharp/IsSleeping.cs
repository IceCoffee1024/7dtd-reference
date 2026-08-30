using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000697 RID: 1687
[Preserve]
public class IsSleeping : TargetedCompareRequirementBase
{
	// Token: 0x0600351F RID: 13599 RVA: 0x0015F0D8 File Offset: 0x0015D2D8
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

	// Token: 0x06003520 RID: 13600 RVA: 0x0015F11F File Offset: 0x0015D31F
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("{0}sleeping", this.invert ? "NOT " : ""));
	}
}
