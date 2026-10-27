using System;
using UnityEngine.Scripting;

// Token: 0x020006AB RID: 1707
[Preserve]
public class IsPrimaryAttack : TargetedCompareRequirementBase
{
	// Token: 0x060035C9 RID: 13769 RVA: 0x00162894 File Offset: 0x00160A94
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
