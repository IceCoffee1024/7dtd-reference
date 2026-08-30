using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006A2 RID: 1698
[Preserve]
public class IsAttachedToEntity : TargetedCompareRequirementBase
{
	// Token: 0x06003540 RID: 13632 RVA: 0x0015F895 File Offset: 0x0015DA95
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (!this.invert)
		{
			return this.target.AttachedToEntity != null;
		}
		return !(this.target.AttachedToEntity != null);
	}

	// Token: 0x06003541 RID: 13633 RVA: 0x0015F8D0 File Offset: 0x0015DAD0
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0}Attached To Entity", this.invert ? "NOT " : ""));
	}
}
