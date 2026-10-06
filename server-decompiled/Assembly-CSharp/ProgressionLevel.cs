using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200068F RID: 1679
[Preserve]
public class ProgressionLevel : TargetedCompareRequirementBase
{
	// Token: 0x06003579 RID: 13689 RVA: 0x00160FB4 File Offset: 0x0015F1B4
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

	// Token: 0x0600357A RID: 13690 RVA: 0x00161050 File Offset: 0x0015F250
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

	// Token: 0x0600357B RID: 13691 RVA: 0x001610B0 File Offset: 0x0015F2B0
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

	// Token: 0x04002B44 RID: 11076
	[PublicizedFrom(EAccessModifier.Private)]
	public string progressionName = string.Empty;

	// Token: 0x04002B45 RID: 11077
	[PublicizedFrom(EAccessModifier.Private)]
	public int progressionId;

	// Token: 0x04002B46 RID: 11078
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionValue pv;
}
