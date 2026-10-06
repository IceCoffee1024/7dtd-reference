using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000682 RID: 1666
[Preserve]
public class IsHeldItem : TargetedCompareRequirementBase
{
	// Token: 0x0600354D RID: 13645 RVA: 0x00160484 File Offset: 0x0015E684
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

	// Token: 0x0600354E RID: 13646 RVA: 0x001604C8 File Offset: 0x0015E6C8
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} Held Item", this.invert ? "NOT " : ""));
	}
}
