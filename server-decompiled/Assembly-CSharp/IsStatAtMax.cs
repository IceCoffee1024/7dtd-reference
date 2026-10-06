using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200066D RID: 1645
[Preserve]
public class IsStatAtMax : TargetedCompareRequirementBase
{
	// Token: 0x06003520 RID: 13600 RVA: 0x0015F8D8 File Offset: 0x0015DAD8
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

	// Token: 0x06003521 RID: 13601 RVA: 0x0015FA24 File Offset: 0x0015DC24
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

	// Token: 0x04002B1D RID: 11037
	[PublicizedFrom(EAccessModifier.Protected)]
	public IsStatAtMax.StatTypes stat;

	// Token: 0x0200066E RID: 1646
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum StatTypes
	{
		// Token: 0x04002B1F RID: 11039
		None,
		// Token: 0x04002B20 RID: 11040
		Health,
		// Token: 0x04002B21 RID: 11041
		Stamina,
		// Token: 0x04002B22 RID: 11042
		Food,
		// Token: 0x04002B23 RID: 11043
		Water
	}
}
