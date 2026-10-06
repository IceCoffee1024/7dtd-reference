using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006AE RID: 1710
[Preserve]
public class IsAttachedToEntity : TargetedCompareRequirementBase
{
	// Token: 0x060035D9 RID: 13785 RVA: 0x001623D5 File Offset: 0x001605D5
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

	// Token: 0x060035DA RID: 13786 RVA: 0x00162410 File Offset: 0x00160610
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0}Attached To Entity", this.invert ? "NOT " : ""));
	}
}
