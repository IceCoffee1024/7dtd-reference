using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000674 RID: 1652
[Preserve]
public class IsStatAtMax : TargetedCompareRequirementBase
{
	// Token: 0x0600352E RID: 13614 RVA: 0x001604CC File Offset: 0x0015E6CC
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

	// Token: 0x0600352F RID: 13615 RVA: 0x00160618 File Offset: 0x0015E818
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

	// Token: 0x04002B38 RID: 11064
	[PublicizedFrom(EAccessModifier.Protected)]
	public IsStatAtMax.StatTypes stat;

	// Token: 0x02000675 RID: 1653
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum StatTypes
	{
		// Token: 0x04002B3A RID: 11066
		None,
		// Token: 0x04002B3B RID: 11067
		Health,
		// Token: 0x04002B3C RID: 11068
		Stamina,
		// Token: 0x04002B3D RID: 11069
		Food,
		// Token: 0x04002B3E RID: 11070
		Water
	}
}
