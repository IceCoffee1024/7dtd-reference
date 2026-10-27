using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006AA RID: 1706
[Preserve]
public class IsSleeping : TargetedCompareRequirementBase
{
	// Token: 0x060035C6 RID: 13766 RVA: 0x00162824 File Offset: 0x00160A24
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

	// Token: 0x060035C7 RID: 13767 RVA: 0x0016286B File Offset: 0x00160A6B
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("{0}sleeping", this.invert ? "NOT " : ""));
	}
}
