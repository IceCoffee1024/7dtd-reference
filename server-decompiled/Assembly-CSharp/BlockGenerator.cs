using System;
using UnityEngine.Scripting;

// Token: 0x02000141 RID: 321
[Preserve]
public class BlockGenerator : BlockPowerSource
{
	// Token: 0x060008D5 RID: 2261 RVA: 0x0003E568 File Offset: 0x0003C768
	public override TileEntityPowerSource CreateTileEntity(Chunk chunk)
	{
		if (this.slotItem == null)
		{
			this.slotItem = ItemClass.GetItemClass(this.SlotItemName, false);
		}
		return new TileEntityPowerSource(chunk)
		{
			PowerItemType = PowerItem.PowerItemTypes.Generator,
			SlotItem = this.slotItem
		};
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x0003E59D File Offset: 0x0003C79D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string GetPowerSourceIcon()
	{
		return "electric_generator";
	}

	// Token: 0x04000991 RID: 2449
	public static FastTags<TagGroup.Global> tag = FastTags<TagGroup.Global>.Parse("gasoline");
}
