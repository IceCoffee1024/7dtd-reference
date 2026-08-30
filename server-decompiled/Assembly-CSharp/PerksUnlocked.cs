using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200068E RID: 1678
[Preserve]
public class PerksUnlocked : TargetedCompareRequirementBase
{
	// Token: 0x06003503 RID: 13571 RVA: 0x0015EB98 File Offset: 0x0015CD98
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

	// Token: 0x06003504 RID: 13572 RVA: 0x0015EC4E File Offset: 0x0015CE4E
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("perks unlocked count {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x06003505 RID: 13573 RVA: 0x0015EC8C File Offset: 0x0015CE8C
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

	// Token: 0x04002AE1 RID: 10977
	[PublicizedFrom(EAccessModifier.Private)]
	public string skill_name;
}
