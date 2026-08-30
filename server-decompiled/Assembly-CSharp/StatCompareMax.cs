using System;
using UnityEngine.Scripting;

// Token: 0x02000668 RID: 1640
[Preserve]
public class StatCompareMax : StatCompareAbs
{
	// Token: 0x06003495 RID: 13461 RVA: 0x0015D174 File Offset: 0x0015B374
	public override bool Compare(MinEventParams _params)
	{
		float num;
		switch (this.stat)
		{
		case StatCompareAbs.StatTypes.Health:
			num = base.Stats.Health.Max;
			break;
		case StatCompareAbs.StatTypes.Stamina:
			num = base.Stats.Stamina.Max;
			break;
		case StatCompareAbs.StatTypes.Food:
			num = base.Stats.Food.Max;
			break;
		case StatCompareAbs.StatTypes.Water:
			num = base.Stats.Water.Max;
			break;
		default:
			return false;
		}
		return this.invert != RequirementBase.compareValues(num, this.operation, this.value);
	}
}
