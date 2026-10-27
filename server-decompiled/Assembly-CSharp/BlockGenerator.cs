using System;
using UnityEngine.Scripting;

// Token: 0x02000144 RID: 324
[Preserve]
public class BlockGenerator : BlockPowerSource
{
	// Token: 0x060008E5 RID: 2277 RVA: 0x0003E870 File Offset: 0x0003CA70
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

	// Token: 0x060008E6 RID: 2278 RVA: 0x0003E8A5 File Offset: 0x0003CAA5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string GetPowerSourceIcon()
	{
		return "electric_generator";
	}

	// Token: 0x04000996 RID: 2454
	public static FastTags<TagGroup.Global> tag = FastTags<TagGroup.Global>.Parse("gasoline");
}
