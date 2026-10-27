using System;
using UnityEngine.Scripting;

// Token: 0x020006A7 RID: 1703
[Preserve]
public class IsDayNumber : TargetedCompareRequirementBase
{
	// Token: 0x060035BE RID: 13758 RVA: 0x0016269C File Offset: 0x0016089C
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
