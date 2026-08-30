using System;
using System.Runtime.CompilerServices;

// Token: 0x02000568 RID: 1384
public abstract class InventoryBase : IInventory
{
	// Token: 0x06002CED RID: 11501
	public abstract bool AddItem(ItemStack _itemStack);

	// Token: 0x06002CEE RID: 11502
	public abstract bool HasItem(ItemValue _item);

	// Token: 0x06002CEF RID: 11503
	[return: TupleElementNames(new string[] { "anyMoved", "allMoved" })]
	public abstract ValueTuple<bool, bool> TryStackItem(int startIndex, ItemStack _itemStack);

	// Token: 0x06002CF0 RID: 11504
	public abstract void PerformActionOnSlots(Action<ItemStack> _action);

	// Token: 0x06002CF1 RID: 11505 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public InventoryBase()
	{
	}
}
