using System;
using System.Xml.Linq;

// Token: 0x0200065F RID: 1631
public class TargetedCompareRequirementBase : RequirementBase
{
	// Token: 0x06003483 RID: 13443 RVA: 0x0015CC78 File Offset: 0x0015AE78
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		this.target = null;
		if (this.targetType == TargetedCompareRequirementBase.TargetTypes.other)
		{
			if (_params.Other != null)
			{
				this.target = _params.Other;
			}
		}
		else if (this.targetType == TargetedCompareRequirementBase.TargetTypes.instigator)
		{
			if (_params.Instigator != null)
			{
				this.target = _params.Instigator;
			}
		}
		else if (_params.Self != null)
		{
			this.target = _params.Self;
		}
		return this.target != null;
	}

	// Token: 0x06003484 RID: 13444 RVA: 0x0015CD08 File Offset: 0x0015AF08
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "target")
		{
			this.targetType = EnumUtils.Parse<TargetedCompareRequirementBase.TargetTypes>(_attribute.Value, true);
			return true;
		}
		return flag;
	}

	// Token: 0x04002AA9 RID: 10921
	[PublicizedFrom(EAccessModifier.Protected)]
	public TargetedCompareRequirementBase.TargetTypes targetType;

	// Token: 0x04002AAA RID: 10922
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityAlive target;

	// Token: 0x02000660 RID: 1632
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum TargetTypes
	{
		// Token: 0x04002AAC RID: 10924
		self,
		// Token: 0x04002AAD RID: 10925
		other,
		// Token: 0x04002AAE RID: 10926
		instigator
	}
}
