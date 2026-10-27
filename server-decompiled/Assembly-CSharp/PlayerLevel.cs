using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200069B RID: 1691
[Preserve]
public class PlayerLevel : TargetedCompareRequirementBase
{
	// Token: 0x06003597 RID: 13719 RVA: 0x00161E88 File Offset: 0x00160088
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

	// Token: 0x06003598 RID: 13720 RVA: 0x00161EF2 File Offset: 0x001600F2
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Player level {0} {1} {2}", this.invert ? "NOT" : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}
}
