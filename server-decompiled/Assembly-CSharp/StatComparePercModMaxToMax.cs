using System;
using UnityEngine.Scripting;

// Token: 0x0200067E RID: 1662
[Preserve]
public class StatComparePercModMaxToMax : StatCompareAbs
{
	// Token: 0x06003541 RID: 13633 RVA: 0x00160B94 File Offset: 0x0015ED94
	public override bool Compare(MinEventParams _params)
	{
		StatCompareAbs.StatTypes stat = this.stat;
		if (stat != StatCompareAbs.StatTypes.Health)
		{
			return stat == StatCompareAbs.StatTypes.Stamina && this.invert != RequirementBase.compareValues(base.Stats.Stamina.ModifiedMaxPercent, this.operation, this.value);
		}
		return this.invert != RequirementBase.compareValues(base.Stats.Health.ModifiedMaxPercent, this.operation, this.value);
	}
}
