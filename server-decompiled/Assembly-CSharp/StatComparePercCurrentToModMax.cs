using System;
using UnityEngine.Scripting;

// Token: 0x0200067D RID: 1661
[Preserve]
public class StatComparePercCurrentToModMax : StatCompareAbs
{
	// Token: 0x0600353F RID: 13631 RVA: 0x00160ADC File Offset: 0x0015ECDC
	public override bool Compare(MinEventParams _params)
	{
		StatCompareAbs.StatTypes stat = this.stat;
		if (stat == StatCompareAbs.StatTypes.Health)
		{
			float modifiedMax = base.Stats.Health.ModifiedMax;
			return modifiedMax > 0f && this.invert != RequirementBase.compareValues(base.Stats.Health.Value / modifiedMax, this.operation, this.value);
		}
		if (stat != StatCompareAbs.StatTypes.Stamina)
		{
			return false;
		}
		float modifiedMax2 = base.Stats.Stamina.ModifiedMax;
		return modifiedMax2 > 0f && this.invert != RequirementBase.compareValues(base.Stats.Stamina.Value / modifiedMax2, this.operation, this.value);
	}
}
