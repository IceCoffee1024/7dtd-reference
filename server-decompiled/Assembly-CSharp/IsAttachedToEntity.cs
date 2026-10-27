using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006B5 RID: 1717
[Preserve]
public class IsAttachedToEntity : TargetedCompareRequirementBase
{
	// Token: 0x060035E7 RID: 13799 RVA: 0x00162FC9 File Offset: 0x001611C9
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

	// Token: 0x060035E8 RID: 13800 RVA: 0x00163004 File Offset: 0x00161204
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0}Attached To Entity", this.invert ? "NOT " : ""));
	}
}
