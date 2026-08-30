using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000662 RID: 1634
[Preserve]
public class IsStatAtMax : TargetedCompareRequirementBase
{
	// Token: 0x0600348A RID: 13450 RVA: 0x0015CE04 File Offset: 0x0015B004
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

	// Token: 0x0600348B RID: 13451 RVA: 0x0015CF50 File Offset: 0x0015B150
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

	// Token: 0x04002AB0 RID: 10928
	[PublicizedFrom(EAccessModifier.Protected)]
	public IsStatAtMax.StatTypes stat;

	// Token: 0x02000663 RID: 1635
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum StatTypes
	{
		// Token: 0x04002AB2 RID: 10930
		None,
		// Token: 0x04002AB3 RID: 10931
		Health,
		// Token: 0x04002AB4 RID: 10932
		Stamina,
		// Token: 0x04002AB5 RID: 10933
		Food,
		// Token: 0x04002AB6 RID: 10934
		Water
	}
}
