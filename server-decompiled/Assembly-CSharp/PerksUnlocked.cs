using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000698 RID: 1688
[Preserve]
public class PerksUnlocked : TargetedCompareRequirementBase
{
	// Token: 0x06003559 RID: 13657 RVA: 0x00160D70 File Offset: 0x0015EF70
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

	// Token: 0x0600355A RID: 13658 RVA: 0x00160E26 File Offset: 0x0015F026
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("perks unlocked count {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x0600355B RID: 13659 RVA: 0x00160E64 File Offset: 0x0015F064
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

	// Token: 0x04002B55 RID: 11093
	[PublicizedFrom(EAccessModifier.Private)]
	public string skill_name;
}
