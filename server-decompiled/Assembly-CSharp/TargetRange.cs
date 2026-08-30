using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006A8 RID: 1704
[Preserve]
public class TargetRange : TargetedCompareRequirementBase
{
	// Token: 0x06003588 RID: 13704 RVA: 0x00161730 File Offset: 0x0015F930
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

	// Token: 0x06003589 RID: 13705 RVA: 0x001617DA File Offset: 0x0015F9DA
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("TargetRange: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}
}
