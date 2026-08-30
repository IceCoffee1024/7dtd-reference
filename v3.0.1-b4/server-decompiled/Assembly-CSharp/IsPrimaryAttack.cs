using System;
using UnityEngine.Scripting;

// Token: 0x02000698 RID: 1688
[Preserve]
public class IsPrimaryAttack : TargetedCompareRequirementBase
{
	// Token: 0x06003522 RID: 13602 RVA: 0x0015F148 File Offset: 0x0015D348
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (this.target == null || this.target.inventory.holdingItemItemValue.ItemClass.Actions[0] == null)
		{
			return false;
		}
		if (this.invert)
		{
			return !this.target.inventory.holdingItemItemValue.ItemClass.Actions[0].IsActionRunning(this.target.inventory.GetItemActionDataInSlot(this.target.inventory.holdingItemIdx, 0));
		}
		return this.target.inventory.holdingItemItemValue.ItemClass.Actions[0].IsActionRunning(this.target.inventory.GetItemActionDataInSlot(this.target.inventory.holdingItemIdx, 0));
	}
}
