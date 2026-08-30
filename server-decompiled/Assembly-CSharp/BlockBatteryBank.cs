using System;
using UnityEngine.Scripting;

// Token: 0x02000143 RID: 323
[Preserve]
public class BlockBatteryBank : BlockPowerSource
{
	// Token: 0x060008DF RID: 2271 RVA: 0x0003E68B File Offset: 0x0003C88B
	public override TileEntityPowerSource CreateTileEntity(Chunk chunk)
	{
		if (this.slotItem == null)
		{
			this.slotItem = ItemClass.GetItemClass(this.SlotItemName, false);
		}
		return new TileEntityPowerSource(chunk)
		{
			PowerItemType = PowerItem.PowerItemTypes.BatteryBank,
			SlotItem = this.slotItem
		};
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x0003E6C0 File Offset: 0x0003C8C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string GetPowerSourceIcon()
	{
		return "battery";
	}
}
