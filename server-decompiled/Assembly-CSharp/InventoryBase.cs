using System;
using System.Runtime.CompilerServices;

// Token: 0x0200057E RID: 1406
public abstract class InventoryBase : IInventory
{
	// Token: 0x06002D52 RID: 11602
	public abstract bool AddItem(ItemStack _itemStack);

	// Token: 0x06002D53 RID: 11603
	public abstract bool HasItem(ItemValue _item);

	// Token: 0x06002D54 RID: 11604
	[return: TupleElementNames(new string[] { "anyMoved", "allMoved" })]
	public abstract ValueTuple<bool, bool> TryStackItem(int startIndex, ItemStack _itemStack);

	// Token: 0x06002D55 RID: 11605
	public abstract void PerformActionOnSlots(Action<ItemStack> _action);

	// Token: 0x06002D56 RID: 11606 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public InventoryBase()
	{
	}
}
