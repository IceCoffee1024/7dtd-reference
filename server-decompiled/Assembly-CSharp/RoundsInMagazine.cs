using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006A6 RID: 1702
[Preserve]
public class RoundsInMagazine : TargetedCompareRequirementBase
{
	// Token: 0x060035BF RID: 13759 RVA: 0x00161E38 File Offset: 0x00160038
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

	// Token: 0x060035C0 RID: 13760 RVA: 0x00161EBE File Offset: 0x001600BE
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Rounds in Magazine: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}
}
