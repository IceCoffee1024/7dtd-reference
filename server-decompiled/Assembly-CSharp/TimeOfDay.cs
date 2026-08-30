using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200069D RID: 1693
[Preserve]
public class TimeOfDay : TargetedCompareRequirementBase
{
	// Token: 0x0600356A RID: 13674 RVA: 0x00161054 File Offset: 0x0015F254
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

	// Token: 0x0600356B RID: 13675 RVA: 0x001610EA File Offset: 0x0015F2EA
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("time of day {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x04002B57 RID: 11095
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSetup;

	// Token: 0x04002B58 RID: 11096
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong timeValue;
}
