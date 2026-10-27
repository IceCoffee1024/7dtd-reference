using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006AD RID: 1709
[Preserve]
public class RoundsInMagazine : TargetedCompareRequirementBase
{
	// Token: 0x060035CD RID: 13773 RVA: 0x00162A2C File Offset: 0x00160C2C
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

	// Token: 0x060035CE RID: 13774 RVA: 0x00162AB2 File Offset: 0x00160CB2
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Rounds in Magazine: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}
}
