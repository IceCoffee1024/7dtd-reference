using System;
using System.Xml.Linq;

// Token: 0x02000671 RID: 1649
public class TargetedCompareRequirementBase : RequirementBase
{
	// Token: 0x06003527 RID: 13607 RVA: 0x00160340 File Offset: 0x0015E540
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

	// Token: 0x06003528 RID: 13608 RVA: 0x001603D0 File Offset: 0x0015E5D0
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

	// Token: 0x04002B31 RID: 11057
	[PublicizedFrom(EAccessModifier.Protected)]
	public TargetedCompareRequirementBase.TargetTypes targetType;

	// Token: 0x04002B32 RID: 11058
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityAlive target;

	// Token: 0x02000672 RID: 1650
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum TargetTypes
	{
		// Token: 0x04002B34 RID: 11060
		self,
		// Token: 0x04002B35 RID: 11061
		other,
		// Token: 0x04002B36 RID: 11062
		instigator
	}
}
