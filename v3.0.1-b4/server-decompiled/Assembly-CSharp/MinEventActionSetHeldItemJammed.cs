using System;
using UnityEngine.Scripting;

// Token: 0x020006FE RID: 1790
[Preserve]
public class MinEventActionSetHeldItemJammed : MinEventActionBase
{
	// Token: 0x0600368B RID: 13963 RVA: 0x0016812C File Offset: 0x0016632C
	public override void Execute(MinEventParams _params)
	{
		ItemValue itemValue = _params.ItemValue;
		if (itemValue != null && !itemValue.IsEmpty())
		{
			itemValue.SetMetadata(ItemActionRanged.scGunIsJammed, 1);
		}
	}
}
