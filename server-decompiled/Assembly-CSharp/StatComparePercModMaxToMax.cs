using System;
using UnityEngine.Scripting;

// Token: 0x02000675 RID: 1653
[Preserve]
public class StatComparePercModMaxToMax : StatCompareAbs
{
	// Token: 0x060034F0 RID: 13552 RVA: 0x0015F63C File Offset: 0x0015D83C
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
