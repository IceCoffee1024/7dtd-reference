using System;
using System.Collections.Generic;

// Token: 0x02000693 RID: 1683
public class CompareLightLevel : TargetedCompareRequirementBase
{
	// Token: 0x06003549 RID: 13641 RVA: 0x001609BC File Offset: 0x0015EBBC
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

	// Token: 0x0600354A RID: 13642 RVA: 0x00160A28 File Offset: 0x0015EC28
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
