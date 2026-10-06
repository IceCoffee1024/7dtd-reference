using System;
using UnityEngine.Scripting;

// Token: 0x020006A2 RID: 1698
[Preserve]
public class HoldingItemBroken : TargetedCompareRequirementBase
{
	// Token: 0x060035B6 RID: 13750 RVA: 0x00161BD8 File Offset: 0x0015FDD8
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
