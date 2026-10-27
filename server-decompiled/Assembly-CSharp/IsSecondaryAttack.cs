using System;
using UnityEngine.Scripting;

// Token: 0x020006AC RID: 1708
[Preserve]
public class IsSecondaryAttack : TargetedCompareRequirementBase
{
	// Token: 0x060035CB RID: 13771 RVA: 0x00162960 File Offset: 0x00160B60
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
