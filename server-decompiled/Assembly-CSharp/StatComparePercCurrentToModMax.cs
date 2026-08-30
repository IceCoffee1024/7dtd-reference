using System;
using UnityEngine.Scripting;

// Token: 0x0200066B RID: 1643
[Preserve]
public class StatComparePercCurrentToModMax : StatCompareAbs
{
	// Token: 0x0600349B RID: 13467 RVA: 0x0015D414 File Offset: 0x0015B614
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
