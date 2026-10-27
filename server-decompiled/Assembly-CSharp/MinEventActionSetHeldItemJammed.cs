using System;
using UnityEngine.Scripting;

// Token: 0x02000712 RID: 1810
[Preserve]
public class MinEventActionSetHeldItemJammed : MinEventActionBase
{
	// Token: 0x0600373B RID: 14139 RVA: 0x0016BC00 File Offset: 0x00169E00
	public override void Execute(MinEventParams _params)
	{
		ItemValue itemValue = _params.ItemValue;
		if (itemValue != null && !itemValue.IsEmpty())
		{
			itemValue.SetMetadata(ItemActionRanged.scGunIsJammed, 1);
		}
	}
}
