using System;
using UnityEngine.Scripting;

// Token: 0x02000677 RID: 1655
[Preserve]
public class StatComparePercModMaxToMax : StatCompareAbs
{
	// Token: 0x06003533 RID: 13619 RVA: 0x0015FFA0 File Offset: 0x0015E1A0
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
