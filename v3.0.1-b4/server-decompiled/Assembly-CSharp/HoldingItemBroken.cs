using System;
using UnityEngine.Scripting;

// Token: 0x02000696 RID: 1686
[Preserve]
public class HoldingItemBroken : TargetedCompareRequirementBase
{
	// Token: 0x0600351D RID: 13597 RVA: 0x0015F080 File Offset: 0x0015D280
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (this.target == null)
		{
			return false;
		}
		bool flag = this.target.inventory.holdingItemItemValue.PercentUsesLeft <= 0f;
		if (!this.invert)
		{
			return flag;
		}
		return !flag;
	}
}
