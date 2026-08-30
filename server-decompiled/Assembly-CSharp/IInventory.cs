using System;
using System.Runtime.CompilerServices;

// Token: 0x0200056B RID: 1387
public interface IInventory
{
	// Token: 0x06002D13 RID: 11539
	bool AddItem(ItemStack _itemStack);

	// Token: 0x06002D14 RID: 11540
	[return: TupleElementNames(new string[] { "anyMoved", "allMoved" })]
	ValueTuple<bool, bool> TryStackItem(int _startIndex, ItemStack _itemStack);

	// Token: 0x06002D15 RID: 11541
	bool HasItem(ItemValue _item);
}
