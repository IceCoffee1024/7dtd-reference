using System;
using UnityEngine.Scripting;

// Token: 0x02000694 RID: 1684
[Preserve]
public class IsDayNumber : TargetedCompareRequirementBase
{
	// Token: 0x06003517 RID: 13591 RVA: 0x0015EF50 File Offset: 0x0015D150
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
