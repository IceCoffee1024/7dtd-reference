using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200069F RID: 1695
[Preserve]
public class TimeOfDay : TargetedCompareRequirementBase
{
	// Token: 0x060035AD RID: 13741 RVA: 0x001619D4 File Offset: 0x0015FBD4
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

	// Token: 0x060035AE RID: 13742 RVA: 0x00161A6A File Offset: 0x0015FC6A
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("time of day {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x04002B50 RID: 11088
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSetup;

	// Token: 0x04002B51 RID: 11089
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong timeValue;
}
