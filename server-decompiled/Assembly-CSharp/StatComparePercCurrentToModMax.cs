using System;
using UnityEngine.Scripting;

// Token: 0x02000676 RID: 1654
[Preserve]
public class StatComparePercCurrentToModMax : StatCompareAbs
{
	// Token: 0x06003531 RID: 13617 RVA: 0x0015FEE8 File Offset: 0x0015E0E8
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
