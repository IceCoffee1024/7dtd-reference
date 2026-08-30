using System;
using UnityEngine.Scripting;

// Token: 0x02000699 RID: 1689
[Preserve]
public class IsSecondaryAttack : TargetedCompareRequirementBase
{
	// Token: 0x06003524 RID: 13604 RVA: 0x0015F220 File Offset: 0x0015D420
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (this.target == null || this.target.inventory.holdingItemItemValue.ItemClass.Actions[1] == null)
		{
			return false;
		}
		if (this.invert)
		{
			return !this.target.inventory.holdingItemItemValue.ItemClass.Actions[1].IsActionRunning(this.target.inventory.GetItemActionDataInSlot(this.target.inventory.holdingItemIdx, 1));
		}
		return this.target.inventory.holdingItemItemValue.ItemClass.Actions[1].IsActionRunning(this.target.inventory.GetItemActionDataInSlot(this.target.inventory.holdingItemIdx, 1));
	}
}
