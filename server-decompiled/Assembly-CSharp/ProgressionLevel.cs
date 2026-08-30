using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200068D RID: 1677
[Preserve]
public class ProgressionLevel : TargetedCompareRequirementBase
{
	// Token: 0x06003536 RID: 13622 RVA: 0x00160634 File Offset: 0x0015E834
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (this.target.Progression != null)
		{
			this.pv = this.target.Progression.GetProgressionValue(this.progressionId);
			if (this.pv != null)
			{
				if (this.invert)
				{
					return !RequirementBase.compareValues(this.pv.GetCalculatedLevel(this.target), this.operation, this.value);
				}
				return RequirementBase.compareValues(this.pv.GetCalculatedLevel(this.target), this.operation, this.value);
			}
		}
		return false;
	}

	// Token: 0x06003537 RID: 13623 RVA: 0x001606D0 File Offset: 0x0015E8D0
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("'{1}' level {0} {2} {3}", new object[]
		{
			this.invert ? "NOT" : "",
			this.progressionName,
			this.operation.ToStringCached<RequirementBase.OperationTypes>(),
			this.value.ToCultureInvariantString()
		}));
	}

	// Token: 0x06003538 RID: 13624 RVA: 0x00160730 File Offset: 0x0015E930
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "progression_name")
		{
			this.progressionName = _attribute.Value;
			this.progressionId = Progression.CalcId(this.progressionName);
			return true;
		}
		return flag;
	}

	// Token: 0x04002B4B RID: 11083
	[PublicizedFrom(EAccessModifier.Private)]
	public string progressionName = string.Empty;

	// Token: 0x04002B4C RID: 11084
	[PublicizedFrom(EAccessModifier.Private)]
	public int progressionId;

	// Token: 0x04002B4D RID: 11085
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionValue pv;
}
