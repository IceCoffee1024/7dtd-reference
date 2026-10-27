using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200069A RID: 1690
[Preserve]
public class IsCorpse : TargetedCompareRequirementBase
{
	// Token: 0x06003594 RID: 13716 RVA: 0x00161E37 File Offset: 0x00160037
	public override bool IsValid(MinEventParams _params)
	{
		if (base.IsValid(_params) && this.target.IsCorpse())
		{
			return !this.invert;
		}
		return this.invert;
	}

	// Token: 0x06003595 RID: 13717 RVA: 0x00161E5F File Offset: 0x0016005F
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Entity {0}IsCorpse", this.invert ? "NOT " : ""));
	}
}
