using System;
using System.Collections.Generic;

// Token: 0x0200069C RID: 1692
public class CompareLightLevel : TargetedCompareRequirementBase
{
	// Token: 0x0600359A RID: 13722 RVA: 0x00161F30 File Offset: 0x00160130
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

	// Token: 0x0600359B RID: 13723 RVA: 0x00161F9C File Offset: 0x0016019C
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
