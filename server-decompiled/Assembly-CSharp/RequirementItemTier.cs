using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200068D RID: 1677
[Preserve]
public class RequirementItemTier : RequirementBase
{
	// Token: 0x06003572 RID: 13682 RVA: 0x00160DC4 File Offset: 0x0015EFC4
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

	// Token: 0x06003573 RID: 13683 RVA: 0x00160E2C File Offset: 0x0015F02C
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Item tier {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}
}
