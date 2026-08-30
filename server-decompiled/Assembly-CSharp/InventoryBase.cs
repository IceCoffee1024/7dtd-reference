using System;
using System.Runtime.CompilerServices;

// Token: 0x0200056D RID: 1389
public abstract class InventoryBase : IInventory
{
	// Token: 0x06002D17 RID: 11543
	public abstract bool AddItem(ItemStack _itemStack);

	// Token: 0x06002D18 RID: 11544
	public abstract bool HasItem(ItemValue _item);

	// Token: 0x06002D19 RID: 11545
	[return: TupleElementNames(new string[] { "anyMoved", "allMoved" })]
	public abstract ValueTuple<bool, bool> TryStackItem(int startIndex, ItemStack _itemStack);

	// Token: 0x06002D1A RID: 11546
	public abstract void PerformActionOnSlots(Action<ItemStack> _action);

	// Token: 0x06002D1B RID: 11547 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public InventoryBase()
	{
	}
}
