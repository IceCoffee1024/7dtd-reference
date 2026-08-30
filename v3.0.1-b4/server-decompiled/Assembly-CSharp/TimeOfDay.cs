using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000693 RID: 1683
[Preserve]
public class TimeOfDay : TargetedCompareRequirementBase
{
	// Token: 0x06003514 RID: 13588 RVA: 0x0015EE7C File Offset: 0x0015D07C
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (!this.isSetup)
		{
			this.timeValue = GameUtils.DayTimeToWorldTime(1, (int)this.value / 100, (int)this.value % 100);
			this.isSetup = true;
		}
		ulong num = GameManager.Instance.World.worldTime % 24000UL;
		if (this.invert)
		{
			return !RequirementBase.compareValues(num, this.operation, this.timeValue);
		}
		return RequirementBase.compareValues(num, this.operation, this.timeValue);
	}

	// Token: 0x06003515 RID: 13589 RVA: 0x0015EF12 File Offset: 0x0015D112
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("time of day {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x04002AE3 RID: 10979
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSetup;

	// Token: 0x04002AE4 RID: 10980
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong timeValue;
}
