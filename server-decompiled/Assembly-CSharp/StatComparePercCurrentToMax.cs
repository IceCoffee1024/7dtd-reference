using System;
using UnityEngine.Scripting;

// Token: 0x02000675 RID: 1653
[Preserve]
public class StatComparePercCurrentToMax : StatCompareAbs
{
	// Token: 0x0600352F RID: 13615 RVA: 0x0015FD80 File Offset: 0x0015DF80
	public override bool Compare(MinEventParams _params)
	{
		switch (this.stat)
		{
		case StatCompareAbs.StatTypes.Health:
		{
			float max = base.Stats.Health.Max;
			return max > 0f && this.invert != RequirementBase.compareValues(base.Stats.Health.Value / max, this.operation, this.value);
		}
		case StatCompareAbs.StatTypes.Stamina:
		{
			float max2 = base.Stats.Stamina.Max;
			return max2 > 0f && this.invert != RequirementBase.compareValues(base.Stats.Stamina.Value / max2, this.operation, this.value);
		}
		case StatCompareAbs.StatTypes.Food:
		{
			float max3 = this.target.Stats.Food.Max;
			return max3 > 0f && this.invert != RequirementBase.compareValues(base.Stats.Food.Value / max3, this.operation, this.value);
		}
		case StatCompareAbs.StatTypes.Water:
		{
			float max4 = this.target.Stats.Water.Max;
			return max4 > 0f && this.invert != RequirementBase.compareValues(base.Stats.Water.Value / max4, this.operation, this.value);
		}
		default:
			return false;
		}
	}
}
