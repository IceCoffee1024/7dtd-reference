using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000694 RID: 1684
[Preserve]
public class PlayerLevel : TargetedCompareRequirementBase
{
	// Token: 0x06003589 RID: 13705 RVA: 0x00161294 File Offset: 0x0015F494
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (this.target.Progression == null)
		{
			return false;
		}
		int level = this.target.Progression.GetLevel();
		if (this.invert)
		{
			return !RequirementBase.compareValues((float)level, this.operation, this.value);
		}
		return RequirementBase.compareValues((float)level, this.operation, this.value);
	}

	// Token: 0x0600358A RID: 13706 RVA: 0x001612FE File Offset: 0x0015F4FE
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Player level {0} {1} {2}", this.invert ? "NOT" : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}
}
