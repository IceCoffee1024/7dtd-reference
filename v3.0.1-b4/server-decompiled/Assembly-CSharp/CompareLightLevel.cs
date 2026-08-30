using System;
using System.Collections.Generic;

// Token: 0x02000689 RID: 1673
public class CompareLightLevel : TargetedCompareRequirementBase
{
	// Token: 0x060034F3 RID: 13555 RVA: 0x0015E7E4 File Offset: 0x0015C9E4
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

	// Token: 0x060034F4 RID: 13556 RVA: 0x0015E850 File Offset: 0x0015CA50
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
