using System;
using UnityEngine.Scripting;

// Token: 0x02000708 RID: 1800
[Preserve]
public class MinEventActionSetHeldItemJammed : MinEventActionBase
{
	// Token: 0x060036E1 RID: 14049 RVA: 0x0016A304 File Offset: 0x00168504
	public override void Execute(MinEventParams _params)
	{
		ItemValue itemValue = _params.ItemValue;
		if (itemValue != null && !itemValue.IsEmpty())
		{
			itemValue.SetMetadata(ItemActionRanged.scGunIsJammed, 1);
		}
	}
}
