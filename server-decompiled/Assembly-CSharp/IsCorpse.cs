using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000691 RID: 1681
[Preserve]
public class IsCorpse : TargetedCompareRequirementBase
{
	// Token: 0x06003543 RID: 13635 RVA: 0x001608C3 File Offset: 0x0015EAC3
	public override bool IsValid(MinEventParams _params)
	{
		if (base.IsValid(_params) && this.target.IsCorpse())
		{
			return !this.invert;
		}
		return this.invert;
	}

	// Token: 0x06003544 RID: 13636 RVA: 0x001608EB File Offset: 0x0015EAEB
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Entity {0}IsCorpse", this.invert ? "NOT " : ""));
	}
}
