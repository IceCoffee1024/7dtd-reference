using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000692 RID: 1682
[Preserve]
public class PlayerLevel : TargetedCompareRequirementBase
{
	// Token: 0x06003546 RID: 13638 RVA: 0x00160914 File Offset: 0x0015EB14
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

	// Token: 0x06003547 RID: 13639 RVA: 0x0016097E File Offset: 0x0015EB7E
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Player level {0} {1} {2}", this.invert ? "NOT" : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}
}
