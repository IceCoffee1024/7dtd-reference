using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200068B RID: 1675
[Preserve]
public class RequirementItemTier : RequirementBase
{
	// Token: 0x0600352F RID: 13615 RVA: 0x00160440 File Offset: 0x0015E640
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

	// Token: 0x06003530 RID: 13616 RVA: 0x001604A8 File Offset: 0x0015E6A8
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Item tier {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}
}
