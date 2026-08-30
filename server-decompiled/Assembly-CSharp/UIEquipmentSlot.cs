using System;
using UnityEngine;

// Token: 0x0200003C RID: 60
[AddComponentMenu("NGUI/Examples/UI Equipment Slot")]
public class UIEquipmentSlot : UIItemSlot
{
	// Token: 0x17000017 RID: 23
	// (get) Token: 0x0600015F RID: 351 RVA: 0x0000ED38 File Offset: 0x0000CF38
	public override InvGameItem observedItem
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (!(this.equipment != null))
			{
				return null;
			}
			return this.equipment.GetItem(this.slot);
		}
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000ED5B File Offset: 0x0000CF5B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override InvGameItem Replace(InvGameItem item)
	{
		if (!(this.equipment != null))
		{
			return item;
		}
		return this.equipment.Replace(this.slot, item);
	}

	// Token: 0x040001F0 RID: 496
	public InvEquipment equipment;

	// Token: 0x040001F1 RID: 497
	public InvBaseItem.Slot slot;
}
