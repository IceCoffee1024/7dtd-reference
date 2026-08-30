using System;
using UnityEngine.Scripting;

// Token: 0x020006A2 RID: 1698
[Preserve]
public class IsPrimaryAttack : TargetedCompareRequirementBase
{
	// Token: 0x06003578 RID: 13688 RVA: 0x00161320 File Offset: 0x0015F520
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
