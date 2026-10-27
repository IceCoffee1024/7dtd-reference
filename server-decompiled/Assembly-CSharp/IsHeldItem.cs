using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000689 RID: 1673
[Preserve]
public class IsHeldItem : TargetedCompareRequirementBase
{
	// Token: 0x0600355B RID: 13659 RVA: 0x00161078 File Offset: 0x0015F278
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		bool flag = this.target.inventory.holdingItemStack.itemValue == _params.ItemValue;
		if (!this.invert)
		{
			return flag;
		}
		return !flag;
	}

	// Token: 0x0600355C RID: 13660 RVA: 0x001610BC File Offset: 0x0015F2BC
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} Held Item", this.invert ? "NOT " : ""));
	}
}
