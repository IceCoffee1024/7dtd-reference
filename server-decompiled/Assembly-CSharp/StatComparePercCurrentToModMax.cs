using System;
using UnityEngine.Scripting;

// Token: 0x02000674 RID: 1652
[Preserve]
public class StatComparePercCurrentToModMax : StatCompareAbs
{
	// Token: 0x060034EE RID: 13550 RVA: 0x0015F584 File Offset: 0x0015D784
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
