using System;
using UnityEngine;

// Token: 0x0200003F RID: 63
[AddComponentMenu("NGUI/Examples/UI Storage Slot")]
public class UIStorageSlot : UIItemSlot
{
	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000170 RID: 368 RVA: 0x0000F371 File Offset: 0x0000D571
	public override InvGameItem observedItem
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (!(this.storage != null))
			{
				return null;
			}
			return this.storage.GetItem(this.slot);
		}
	}

	// Token: 0x06000171 RID: 369 RVA: 0x0000F394 File Offset: 0x0000D594
	[PublicizedFrom(EAccessModifier.Protected)]
	public override InvGameItem Replace(InvGameItem item)
	{
		if (!(this.storage != null))
		{
			return item;
		}
		return this.storage.Replace(this.slot, item);
	}

	// Token: 0x04000203 RID: 515
	public UIItemStorage storage;

	// Token: 0x04000204 RID: 516
	public int slot;
}
