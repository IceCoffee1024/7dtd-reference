using System;
using UnityEngine.Scripting;

// Token: 0x020006A5 RID: 1701
[Preserve]
public class IsSecondaryAttack : TargetedCompareRequirementBase
{
	// Token: 0x060035BD RID: 13757 RVA: 0x00161D6C File Offset: 0x0015FF6C
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
			return !this.target.inventory.holdingItemItemValue.ItemClass.Actions[1].IsActionRunning(this.target.inventory.holdingItemData.actionData[1]);
		}
		return this.target.inventory.holdingItemItemValue.ItemClass.Actions[1].IsActionRunning(this.target.inventory.holdingItemData.actionData[1]);
	}
}
