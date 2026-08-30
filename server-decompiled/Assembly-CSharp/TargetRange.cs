using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200069E RID: 1694
[Preserve]
public class TargetRange : TargetedCompareRequirementBase
{
	// Token: 0x06003532 RID: 13618 RVA: 0x0015F558 File Offset: 0x0015D758
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
		if (!(_params.Self != null) || !(_params.Other != null))
		{
			return false;
		}
		if (!(_params.Self != _params.Other))
		{
			return false;
		}
		if (this.invert)
		{
			return !RequirementBase.compareValues(_params.Self.GetDistance(_params.Other), this.operation, this.value);
		}
		return RequirementBase.compareValues(_params.Self.GetDistance(_params.Other), this.operation, this.value);
	}

	// Token: 0x06003533 RID: 13619 RVA: 0x0015F602 File Offset: 0x0015D802
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("TargetRange: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}
}
