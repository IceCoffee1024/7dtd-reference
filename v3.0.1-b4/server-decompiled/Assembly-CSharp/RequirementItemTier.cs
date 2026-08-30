using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000681 RID: 1665
[Preserve]
public class RequirementItemTier : RequirementBase
{
	// Token: 0x060034D9 RID: 13529 RVA: 0x0015E264 File Offset: 0x0015C464
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (_params.ItemValue == null)
		{
			return false;
		}
		if (!this.invert)
		{
			return RequirementBase.compareValues((float)_params.ItemValue.Quality, this.operation, this.value);
		}
		return !RequirementBase.compareValues((float)_params.ItemValue.Quality, this.operation, this.value);
	}

	// Token: 0x060034DA RID: 13530 RVA: 0x0015E2CC File Offset: 0x0015C4CC
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Item tier {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}
}
