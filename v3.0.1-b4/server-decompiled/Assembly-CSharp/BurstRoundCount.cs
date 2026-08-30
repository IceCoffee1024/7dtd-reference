using System;
using System.Collections.Generic;

// Token: 0x0200069B RID: 1691
public class BurstRoundCount : TargetedCompareRequirementBase
{
	// Token: 0x06003529 RID: 13609 RVA: 0x0015F3BC File Offset: 0x0015D5BC
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

	// Token: 0x0600352A RID: 13610 RVA: 0x0015F37E File Offset: 0x0015D57E
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Rounds in Magazine: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}
}
