using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200067F RID: 1663
[Preserve]
public class IsMale : TargetedCompareRequirementBase
{
	// Token: 0x06003543 RID: 13635 RVA: 0x00160C0D File Offset: 0x0015EE0D
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

	// Token: 0x06003544 RID: 13636 RVA: 0x00160C3C File Offset: 0x0015EE3C
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0}Male", this.invert ? "NOT " : ""));
	}
}
