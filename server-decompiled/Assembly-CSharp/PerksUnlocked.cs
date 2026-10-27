using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006A1 RID: 1697
[Preserve]
public class PerksUnlocked : TargetedCompareRequirementBase
{
	// Token: 0x060035AA RID: 13738 RVA: 0x001622E4 File Offset: 0x001604E4
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (this.skill_name == null)
		{
			return false;
		}
		ProgressionValue progressionValue = this.target.Progression.GetProgressionValue(this.skill_name);
		int num = 0;
		for (int i = 0; i < progressionValue.ProgressionClass.Children.Count; i++)
		{
			num += this.target.Progression.GetProgressionValue(progressionValue.ProgressionClass.Children[i].Name).Level;
		}
		if (this.invert)
		{
			return !RequirementBase.compareValues((float)num, this.operation, this.value);
		}
		return RequirementBase.compareValues((float)num, this.operation, this.value);
	}

	// Token: 0x060035AB RID: 13739 RVA: 0x0016239A File Offset: 0x0016059A
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("perks unlocked count {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x060035AC RID: 13740 RVA: 0x001623D8 File Offset: 0x001605D8
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "skill_name")
		{
			this.skill_name = _attribute.Value;
			return true;
		}
		return flag;
	}

	// Token: 0x04002B69 RID: 11113
	[PublicizedFrom(EAccessModifier.Private)]
	public string skill_name;
}
