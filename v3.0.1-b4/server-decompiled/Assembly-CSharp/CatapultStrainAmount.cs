using System;
using UnityEngine.Scripting;

// Token: 0x0200067B RID: 1659
[Preserve]
public class CatapultStrainAmount : TargetedCompareRequirementBase
{
	// Token: 0x060034C6 RID: 13510 RVA: 0x0015DD14 File Offset: 0x0015BF14
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		ItemValue itemValue = _params.ItemValue;
		if (itemValue == null)
		{
			return false;
		}
		ItemActionCatapult itemActionCatapult = itemValue.ItemClass.Actions[0] as ItemActionCatapult;
		if (itemActionCatapult != null)
		{
			float strainPercent = itemActionCatapult.GetStrainPercent(_params.ItemInventoryData.actionData[0]);
			if (this.invert)
			{
				return !RequirementBase.compareValues(strainPercent, this.operation, this.value);
			}
			return RequirementBase.compareValues(strainPercent, this.operation, this.value);
		}
		else
		{
			ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = _params.ItemActionData as ItemActionLauncher.ItemActionDataLauncher;
			if (itemActionDataLauncher == null)
			{
				return false;
			}
			float lastAttackStrainPercent = itemActionDataLauncher.lastAttackStrainPercent;
			if (this.invert)
			{
				return !RequirementBase.compareValues(lastAttackStrainPercent, this.operation, this.value);
			}
			return RequirementBase.compareValues(lastAttackStrainPercent, this.operation, this.value);
		}
	}
}
