using System;
using System.Collections.Generic;

// Token: 0x02000695 RID: 1685
public class CompareLightLevel : TargetedCompareRequirementBase
{
	// Token: 0x0600358C RID: 13708 RVA: 0x0016133C File Offset: 0x0015F53C
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (this.target == null)
		{
			return false;
		}
		if (!this.invert)
		{
			return RequirementBase.compareValues(this.target.GetLightBrightness(), this.operation, this.value);
		}
		return !RequirementBase.compareValues(this.target.GetLightBrightness(), this.operation, this.value);
	}

	// Token: 0x0600358D RID: 13709 RVA: 0x001613A8 File Offset: 0x0015F5A8
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("light level '{0}'% {1}{2} {3}", new object[]
		{
			this.target.GetLightBrightness().ToCultureInvariantString(),
			this.invert ? "NOT " : "",
			this.operation.ToStringCached<RequirementBase.OperationTypes>(),
			this.value.ToCultureInvariantString()
		}));
	}
}
