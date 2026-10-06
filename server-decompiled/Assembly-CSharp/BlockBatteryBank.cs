using System;
using UnityEngine.Scripting;

// Token: 0x02000146 RID: 326
[Preserve]
public class BlockBatteryBank : BlockPowerSource
{
	// Token: 0x060008EF RID: 2287 RVA: 0x0003E9D3 File Offset: 0x0003CBD3
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

	// Token: 0x060008F0 RID: 2288 RVA: 0x0003EA08 File Offset: 0x0003CC08
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string GetPowerSourceIcon()
	{
		return "battery";
	}
}
