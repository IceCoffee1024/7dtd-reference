using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000696 RID: 1686
[Preserve]
public class ProgressionLevel : TargetedCompareRequirementBase
{
	// Token: 0x06003587 RID: 13703 RVA: 0x00161BA8 File Offset: 0x0015FDA8
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

	// Token: 0x06003588 RID: 13704 RVA: 0x00161C44 File Offset: 0x0015FE44
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

	// Token: 0x06003589 RID: 13705 RVA: 0x00161CA4 File Offset: 0x0015FEA4
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

	// Token: 0x04002B5F RID: 11103
	[PublicizedFrom(EAccessModifier.Private)]
	public string progressionName = string.Empty;

	// Token: 0x04002B60 RID: 11104
	[PublicizedFrom(EAccessModifier.Private)]
	public int progressionId;

	// Token: 0x04002B61 RID: 11105
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionValue pv;
}
