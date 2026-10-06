using System;
using UnityEngine.Scripting;

// Token: 0x0200067E RID: 1662
[Preserve]
public class IsItemActive : TargetedCompareRequirementBase
{
	// Token: 0x06003542 RID: 13634 RVA: 0x001601D0 File Offset: 0x0015E3D0
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
