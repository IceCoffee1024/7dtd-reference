using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200069A RID: 1690
[Preserve]
public class PerksUnlocked : TargetedCompareRequirementBase
{
	// Token: 0x0600359C RID: 13724 RVA: 0x001616F0 File Offset: 0x0015F8F0
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

	// Token: 0x0600359D RID: 13725 RVA: 0x001617A6 File Offset: 0x0015F9A6
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("perks unlocked count {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x0600359E RID: 13726 RVA: 0x001617E4 File Offset: 0x0015F9E4
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

	// Token: 0x04002B4E RID: 11086
	[PublicizedFrom(EAccessModifier.Private)]
	public string skill_name;
}
