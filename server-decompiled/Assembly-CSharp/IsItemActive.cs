using System;
using UnityEngine.Scripting;

// Token: 0x02000673 RID: 1651
[Preserve]
public class IsItemActive : TargetedCompareRequirementBase
{
	// Token: 0x060034AC RID: 13484 RVA: 0x0015D6FC File Offset: 0x0015B8FC
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
		if (_params.ItemValue.Activated > 0)
		{
			return !this.invert;
		}
		return this.invert;
	}
}
