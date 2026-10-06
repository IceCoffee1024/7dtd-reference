using System;
using UnityEngine.Scripting;

// Token: 0x02000673 RID: 1651
[Preserve]
public class StatCompareMax : StatCompareAbs
{
	// Token: 0x0600352B RID: 13611 RVA: 0x0015FC48 File Offset: 0x0015DE48
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
