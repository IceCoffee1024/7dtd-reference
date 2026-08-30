using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006A4 RID: 1700
[Preserve]
public class RoundsInMagazine : TargetedCompareRequirementBase
{
	// Token: 0x0600357C RID: 13692 RVA: 0x001614D0 File Offset: 0x0015F6D0
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

	// Token: 0x0600357D RID: 13693 RVA: 0x00161556 File Offset: 0x0015F756
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Rounds in Magazine: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}
}
