using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000693 RID: 1683
[Preserve]
public class IsCorpse : TargetedCompareRequirementBase
{
	// Token: 0x06003586 RID: 13702 RVA: 0x00161243 File Offset: 0x0015F443
	public override bool IsValid(MinEventParams _params)
	{
		if (base.IsValid(_params) && this.target.IsCorpse())
		{
			return !this.invert;
		}
		return this.invert;
	}

	// Token: 0x06003587 RID: 13703 RVA: 0x0016126B File Offset: 0x0015F46B
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Entity {0}IsCorpse", this.invert ? "NOT " : ""));
	}
}
