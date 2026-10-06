using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000678 RID: 1656
[Preserve]
public class IsMale : TargetedCompareRequirementBase
{
	// Token: 0x06003535 RID: 13621 RVA: 0x00160019 File Offset: 0x0015E219
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

	// Token: 0x06003536 RID: 13622 RVA: 0x00160048 File Offset: 0x0015E248
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0}Male", this.invert ? "NOT " : ""));
	}
}
