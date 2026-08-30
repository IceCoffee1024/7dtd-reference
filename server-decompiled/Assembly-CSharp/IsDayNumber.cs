using System;
using UnityEngine.Scripting;

// Token: 0x0200069E RID: 1694
[Preserve]
public class IsDayNumber : TargetedCompareRequirementBase
{
	// Token: 0x0600356D RID: 13677 RVA: 0x00161128 File Offset: 0x0015F328
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
