using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000680 RID: 1664
[Preserve]
public class IsHeldItem : TargetedCompareRequirementBase
{
	// Token: 0x0600350A RID: 13578 RVA: 0x0015FB00 File Offset: 0x0015DD00
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

	// Token: 0x0600350B RID: 13579 RVA: 0x0015FB44 File Offset: 0x0015DD44
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} Held Item", this.invert ? "NOT " : ""));
	}
}
