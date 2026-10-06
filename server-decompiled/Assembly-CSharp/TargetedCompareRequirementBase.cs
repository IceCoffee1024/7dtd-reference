using System;
using System.Xml.Linq;

// Token: 0x0200066A RID: 1642
public class TargetedCompareRequirementBase : RequirementBase
{
	// Token: 0x06003519 RID: 13593 RVA: 0x0015F74C File Offset: 0x0015D94C
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

	// Token: 0x0600351A RID: 13594 RVA: 0x0015F7DC File Offset: 0x0015D9DC
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

	// Token: 0x04002B16 RID: 11030
	[PublicizedFrom(EAccessModifier.Protected)]
	public TargetedCompareRequirementBase.TargetTypes targetType;

	// Token: 0x04002B17 RID: 11031
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityAlive target;

	// Token: 0x0200066B RID: 1643
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum TargetTypes
	{
		// Token: 0x04002B19 RID: 11033
		self,
		// Token: 0x04002B1A RID: 11034
		other,
		// Token: 0x04002B1B RID: 11035
		instigator
	}
}
