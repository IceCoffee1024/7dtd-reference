using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200066B RID: 1643
[Preserve]
public class IsStatAtMax : TargetedCompareRequirementBase
{
	// Token: 0x060034DD RID: 13533 RVA: 0x0015EF74 File Offset: 0x0015D174
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		switch (this.stat)
		{
		case IsStatAtMax.StatTypes.Health:
			if (this.target.Stats.Health.Max - this.target.Stats.Health.Value < 0.1f)
			{
				return !this.invert;
			}
			return this.invert;
		case IsStatAtMax.StatTypes.Stamina:
			if (this.target.Stats.Stamina.Max - this.target.Stats.Stamina.Value < 0.1f)
			{
				return !this.invert;
			}
			return this.invert;
		case IsStatAtMax.StatTypes.Food:
			if (this.target.Stats.Food.Max - this.target.Stats.Food.Value < 0.1f)
			{
				return !this.invert;
			}
			return this.invert;
		case IsStatAtMax.StatTypes.Water:
			if (this.target.Stats.Water.Max - this.target.Stats.Water.Value < 0.1f)
			{
				return !this.invert;
			}
			return this.invert;
		default:
			return false;
		}
	}

	// Token: 0x060034DE RID: 13534 RVA: 0x0015F0C0 File Offset: 0x0015D2C0
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "stat")
		{
			this.stat = EnumUtils.Parse<IsStatAtMax.StatTypes>(_attribute.Value, true);
			return true;
		}
		return flag;
	}

	// Token: 0x04002B24 RID: 11044
	[PublicizedFrom(EAccessModifier.Protected)]
	public IsStatAtMax.StatTypes stat;

	// Token: 0x0200066C RID: 1644
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum StatTypes
	{
		// Token: 0x04002B26 RID: 11046
		None,
		// Token: 0x04002B27 RID: 11047
		Health,
		// Token: 0x04002B28 RID: 11048
		Stamina,
		// Token: 0x04002B29 RID: 11049
		Food,
		// Token: 0x04002B2A RID: 11050
		Water
	}
}
