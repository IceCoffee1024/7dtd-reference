using System;
using UnityEngine.Scripting;

// Token: 0x020006A0 RID: 1696
[Preserve]
public class IsDayNumber : TargetedCompareRequirementBase
{
	// Token: 0x060035B0 RID: 13744 RVA: 0x00161AA8 File Offset: 0x0015FCA8
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		int num = GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime);
		if (this.invert)
		{
			return !RequirementBase.compareValues((float)num, this.operation, this.value);
		}
		return RequirementBase.compareValues((float)num, this.operation, this.value);
	}
}
