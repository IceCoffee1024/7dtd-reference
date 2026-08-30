using System;
using UnityEngine.Scripting;

// Token: 0x020006FE RID: 1790
[Preserve]
public class MinEventActionResetHeldItem : MinEventActionTargetedBase
{
	// Token: 0x060036BF RID: 14015 RVA: 0x00169675 File Offset: 0x00167875
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
