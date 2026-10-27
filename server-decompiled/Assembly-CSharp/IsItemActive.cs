using System;
using UnityEngine.Scripting;

// Token: 0x02000685 RID: 1669
[Preserve]
public class IsItemActive : TargetedCompareRequirementBase
{
	// Token: 0x06003550 RID: 13648 RVA: 0x00160DC4 File Offset: 0x0015EFC4
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
