using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006A3 RID: 1699
[Preserve]
public class IsSleeping : TargetedCompareRequirementBase
{
	// Token: 0x060035B8 RID: 13752 RVA: 0x00161C30 File Offset: 0x0015FE30
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

	// Token: 0x060035B9 RID: 13753 RVA: 0x00161C77 File Offset: 0x0015FE77
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("{0}sleeping", this.invert ? "NOT " : ""));
	}
}
