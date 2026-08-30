using System;
using UnityEngine.Scripting;

// Token: 0x0200067C RID: 1660
[Preserve]
public class IsItemActive : TargetedCompareRequirementBase
{
	// Token: 0x060034FF RID: 13567 RVA: 0x0015F86C File Offset: 0x0015DA6C
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
		if (_params.ItemValue.Activated)
		{
			return !this.invert;
		}
		return this.invert;
	}
}
