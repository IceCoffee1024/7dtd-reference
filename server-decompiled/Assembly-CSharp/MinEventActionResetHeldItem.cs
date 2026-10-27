using System;
using UnityEngine.Scripting;

// Token: 0x02000708 RID: 1800
[Preserve]
public class MinEventActionResetHeldItem : MinEventActionTargetedBase
{
	// Token: 0x06003719 RID: 14105 RVA: 0x0016AF71 File Offset: 0x00169171
	public override void Execute(MinEventParams _params)
	{
		ItemActionData itemActionData = _params.ItemActionData;
		if (itemActionData == null)
		{
			return;
		}
		itemActionData.invData.item.OnHoldingReset(_params.ItemActionData.invData);
	}
}
