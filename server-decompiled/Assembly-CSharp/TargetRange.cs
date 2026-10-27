using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006B1 RID: 1713
[Preserve]
public class TargetRange : TargetedCompareRequirementBase
{
	// Token: 0x060035D9 RID: 13785 RVA: 0x00162C8C File Offset: 0x00160E8C
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

	// Token: 0x060035DA RID: 13786 RVA: 0x00162D36 File Offset: 0x00160F36
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("TargetRange: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}
}
