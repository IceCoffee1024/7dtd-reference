using System;
using UnityEngine.Scripting;

// Token: 0x02000669 RID: 1641
[Preserve]
public class StatCompareModMax : StatCompareAbs
{
	// Token: 0x06003497 RID: 13463 RVA: 0x0015D210 File Offset: 0x0015B410
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
