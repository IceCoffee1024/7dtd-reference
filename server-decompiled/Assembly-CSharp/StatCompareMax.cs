using System;
using UnityEngine.Scripting;

// Token: 0x02000671 RID: 1649
[Preserve]
public class StatCompareMax : StatCompareAbs
{
	// Token: 0x060034E8 RID: 13544 RVA: 0x0015F2E4 File Offset: 0x0015D4E4
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
