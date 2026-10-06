using System;
using UnityEngine.Scripting;

// Token: 0x0200070A RID: 1802
[Preserve]
public class MinEventActionSetHeldItemJammed : MinEventActionBase
{
	// Token: 0x06003724 RID: 14116 RVA: 0x0016AC68 File Offset: 0x00168E68
	public override void Execute(MinEventParams _params)
	{
		ItemValue itemValue = _params.ItemValue;
		if (itemValue != null && !itemValue.IsEmpty())
		{
			itemValue.SetMetadata(ItemActionRanged.scGunIsJammed, 1);
		}
	}
}
