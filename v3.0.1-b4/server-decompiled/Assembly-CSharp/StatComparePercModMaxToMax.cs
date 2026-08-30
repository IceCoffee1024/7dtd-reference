using System;
using UnityEngine.Scripting;

// Token: 0x0200066C RID: 1644
[Preserve]
public class StatComparePercModMaxToMax : StatCompareAbs
{
	// Token: 0x0600349D RID: 13469 RVA: 0x0015D4CC File Offset: 0x0015B6CC
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
