using System;
using System.Runtime.CompilerServices;

// Token: 0x02000577 RID: 1399
public abstract class InventoryBase : IInventory
{
	// Token: 0x06002D47 RID: 11591
	public abstract bool AddItem(ItemStack _itemStack);

	// Token: 0x06002D48 RID: 11592
	public abstract bool HasItem(ItemValue _item);

	// Token: 0x06002D49 RID: 11593
	[return: TupleElementNames(new string[] { "anyMoved", "allMoved" })]
	public abstract ValueTuple<bool, bool> TryStackItem(int startIndex, ItemStack _itemStack);

	// Token: 0x06002D4A RID: 11594
	public abstract void PerformActionOnSlots(Action<ItemStack> _action);

	// Token: 0x06002D4B RID: 11595 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public InventoryBase()
	{
	}
}
