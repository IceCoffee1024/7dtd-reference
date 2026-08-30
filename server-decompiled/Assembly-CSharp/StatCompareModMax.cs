using System;
using UnityEngine.Scripting;

// Token: 0x02000672 RID: 1650
[Preserve]
public class StatCompareModMax : StatCompareAbs
{
	// Token: 0x060034EA RID: 13546 RVA: 0x0015F380 File Offset: 0x0015D580
	public override bool Compare(MinEventParams _params)
	{
		float num;
		switch (this.stat)
		{
		case StatCompareAbs.StatTypes.Health:
			num = base.Stats.Health.ModifiedMax;
			break;
		case StatCompareAbs.StatTypes.Stamina:
			num = base.Stats.Stamina.ModifiedMax;
			break;
		case StatCompareAbs.StatTypes.Food:
			num = base.Stats.Food.ModifiedMax;
			break;
		case StatCompareAbs.StatTypes.Water:
			num = base.Stats.Water.ModifiedMax;
			break;
		default:
			return false;
		}
		return this.invert != RequirementBase.compareValues(num, this.operation, this.value);
	}
}
