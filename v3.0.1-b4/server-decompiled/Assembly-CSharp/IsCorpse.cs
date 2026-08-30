using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000687 RID: 1671
[Preserve]
public class IsCorpse : TargetedCompareRequirementBase
{
	// Token: 0x060034ED RID: 13549 RVA: 0x0015E6EB File Offset: 0x0015C8EB
	public override bool IsValid(MinEventParams _params)
	{
		if (base.IsValid(_params) && this.target.IsCorpse())
		{
			return !this.invert;
		}
		return this.invert;
	}

	// Token: 0x060034EE RID: 13550 RVA: 0x0015E713 File Offset: 0x0015C913
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Entity {0}IsCorpse", this.invert ? "NOT " : ""));
	}
}
