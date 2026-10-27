using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000694 RID: 1684
[Preserve]
public class RequirementItemTier : RequirementBase
{
	// Token: 0x06003580 RID: 13696 RVA: 0x001619B8 File Offset: 0x0015FBB8
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

	// Token: 0x06003581 RID: 13697 RVA: 0x00161A20 File Offset: 0x0015FC20
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Item tier {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}
}
