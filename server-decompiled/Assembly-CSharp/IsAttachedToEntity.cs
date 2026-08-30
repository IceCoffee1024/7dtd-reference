using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006AC RID: 1708
[Preserve]
public class IsAttachedToEntity : TargetedCompareRequirementBase
{
	// Token: 0x06003596 RID: 13718 RVA: 0x00161A6D File Offset: 0x0015FC6D
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

	// Token: 0x06003597 RID: 13719 RVA: 0x00161AA8 File Offset: 0x0015FCA8
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0}Attached To Entity", this.invert ? "NOT " : ""));
	}
}
