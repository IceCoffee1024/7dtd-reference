using System;
using UnityEngine.Scripting;

// Token: 0x020006A4 RID: 1700
[Preserve]
public class IsPrimaryAttack : TargetedCompareRequirementBase
{
	// Token: 0x060035BB RID: 13755 RVA: 0x00161CA0 File Offset: 0x0015FEA0
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
			return !this.target.inventory.holdingItemItemValue.ItemClass.Actions[0].IsActionRunning(this.target.inventory.holdingItemData.actionData[0]);
		}
		return this.target.inventory.holdingItemItemValue.ItemClass.Actions[0].IsActionRunning(this.target.inventory.holdingItemData.actionData[0]);
	}
}
