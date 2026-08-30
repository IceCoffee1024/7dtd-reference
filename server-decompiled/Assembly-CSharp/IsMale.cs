using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000676 RID: 1654
[Preserve]
public class IsMale : TargetedCompareRequirementBase
{
	// Token: 0x060034F2 RID: 13554 RVA: 0x0015F6B5 File Offset: 0x0015D8B5
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

	// Token: 0x060034F3 RID: 13555 RVA: 0x0015F6E4 File Offset: 0x0015D8E4
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0}Male", this.invert ? "NOT " : ""));
	}
}
