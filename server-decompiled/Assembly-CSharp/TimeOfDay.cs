using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006A6 RID: 1702
[Preserve]
public class TimeOfDay : TargetedCompareRequirementBase
{
	// Token: 0x060035BB RID: 13755 RVA: 0x001625C8 File Offset: 0x001607C8
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

	// Token: 0x060035BC RID: 13756 RVA: 0x0016265E File Offset: 0x0016085E
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("time of day {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x04002B6B RID: 11115
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSetup;

	// Token: 0x04002B6C RID: 11116
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong timeValue;
}
