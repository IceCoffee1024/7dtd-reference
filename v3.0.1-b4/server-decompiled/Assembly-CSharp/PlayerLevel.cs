using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000688 RID: 1672
[Preserve]
public class PlayerLevel : TargetedCompareRequirementBase
{
	// Token: 0x060034F0 RID: 13552 RVA: 0x0015E73C File Offset: 0x0015C93C
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

	// Token: 0x060034F1 RID: 13553 RVA: 0x0015E7A6 File Offset: 0x0015C9A6
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Player level {0} {1} {2}", this.invert ? "NOT" : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}
}
