using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200066D RID: 1645
[Preserve]
public class IsMale : TargetedCompareRequirementBase
{
	// Token: 0x0600349F RID: 13471 RVA: 0x0015D545 File Offset: 0x0015B745
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (!this.invert)
		{
			return this.target.IsMale;
		}
		return !this.target.IsMale;
	}

	// Token: 0x060034A0 RID: 13472 RVA: 0x0015D574 File Offset: 0x0015B774
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0}Male", this.invert ? "NOT " : ""));
	}
}
