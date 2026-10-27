using System;
using System.Collections.Generic;

// Token: 0x020006AE RID: 1710
public class BurstRoundCount : TargetedCompareRequirementBase
{
	// Token: 0x060035D0 RID: 13776 RVA: 0x00162AF0 File Offset: 0x00160CF0
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (_params.ItemValue.IsEmpty())
		{
			return false;
		}
		ItemActionRanged itemActionRanged = _params.ItemValue.ItemClass.Actions[0] as ItemActionRanged;
		if (itemActionRanged == null)
		{
			return false;
		}
		if (this.invert)
		{
			return !RequirementBase.compareValues((float)itemActionRanged.GetBurstCount(this.target.inventory.holdingItemData.actionData[0]), this.operation, this.value);
		}
		return RequirementBase.compareValues((float)itemActionRanged.GetBurstCount(this.target.inventory.holdingItemData.actionData[0]), this.operation, this.value);
	}

	// Token: 0x060035D1 RID: 13777 RVA: 0x00162AB2 File Offset: 0x00160CB2
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Rounds in Magazine: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}
}
