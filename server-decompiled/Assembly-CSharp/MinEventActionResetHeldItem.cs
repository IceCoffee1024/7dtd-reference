using System;
using UnityEngine.Scripting;

// Token: 0x02000700 RID: 1792
[Preserve]
public class MinEventActionResetHeldItem : MinEventActionTargetedBase
{
	// Token: 0x06003702 RID: 14082 RVA: 0x00169FD9 File Offset: 0x001681D9
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
