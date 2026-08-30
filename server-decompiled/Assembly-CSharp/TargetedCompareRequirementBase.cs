using System;
using System.Xml.Linq;

// Token: 0x02000668 RID: 1640
public class TargetedCompareRequirementBase : RequirementBase
{
	// Token: 0x060034D6 RID: 13526 RVA: 0x0015EDE8 File Offset: 0x0015CFE8
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

	// Token: 0x060034D7 RID: 13527 RVA: 0x0015EE78 File Offset: 0x0015D078
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

	// Token: 0x04002B1D RID: 11037
	[PublicizedFrom(EAccessModifier.Protected)]
	public TargetedCompareRequirementBase.TargetTypes targetType;

	// Token: 0x04002B1E RID: 11038
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityAlive target;

	// Token: 0x02000669 RID: 1641
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum TargetTypes
	{
		// Token: 0x04002B20 RID: 11040
		self,
		// Token: 0x04002B21 RID: 11041
		other,
		// Token: 0x04002B22 RID: 11042
		instigator
	}
}
