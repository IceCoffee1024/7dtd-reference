using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200069A RID: 1690
[Preserve]
public class RoundsInMagazine : TargetedCompareRequirementBase
{
	// Token: 0x06003526 RID: 13606 RVA: 0x0015F2F8 File Offset: 0x0015D4F8
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (_params.ItemValue.IsEmpty() || !(_params.ItemValue.ItemClass.Actions[0] is ItemActionRanged))
		{
			return false;
		}
		if (this.invert)
		{
			return !RequirementBase.compareValues((float)_params.ItemValue.Meta, this.operation, this.value);
		}
		return RequirementBase.compareValues((float)_params.ItemValue.Meta, this.operation, this.value);
	}

	// Token: 0x06003527 RID: 13607 RVA: 0x0015F37E File Offset: 0x0015D57E
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Rounds in Magazine: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}
}
