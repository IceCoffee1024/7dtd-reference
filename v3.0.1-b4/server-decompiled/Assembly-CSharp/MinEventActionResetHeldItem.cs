using System;
using UnityEngine.Scripting;

// Token: 0x020006F4 RID: 1780
[Preserve]
public class MinEventActionResetHeldItem : MinEventActionTargetedBase
{
	// Token: 0x06003669 RID: 13929 RVA: 0x0016749D File Offset: 0x0016569D
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
