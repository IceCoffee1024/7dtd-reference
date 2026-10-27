using System;
using UnityEngine.Scripting;

// Token: 0x020006A9 RID: 1705
[Preserve]
public class HoldingItemBroken : TargetedCompareRequirementBase
{
	// Token: 0x060035C4 RID: 13764 RVA: 0x001627CC File Offset: 0x001609CC
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
