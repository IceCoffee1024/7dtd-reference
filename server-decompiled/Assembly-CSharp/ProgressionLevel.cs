using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000683 RID: 1667
[Preserve]
public class ProgressionLevel : TargetedCompareRequirementBase
{
	// Token: 0x060034E0 RID: 13536 RVA: 0x0015E45C File Offset: 0x0015C65C
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

	// Token: 0x060034E1 RID: 13537 RVA: 0x0015E4F8 File Offset: 0x0015C6F8
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

	// Token: 0x060034E2 RID: 13538 RVA: 0x0015E558 File Offset: 0x0015C758
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

	// Token: 0x04002AD7 RID: 10967
	[PublicizedFrom(EAccessModifier.Private)]
	public string progressionName = string.Empty;

	// Token: 0x04002AD8 RID: 10968
	[PublicizedFrom(EAccessModifier.Private)]
	public int progressionId;

	// Token: 0x04002AD9 RID: 10969
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionValue pv;
}
