using System;
using System.Runtime.CompilerServices;

// Token: 0x02000566 RID: 1382
public interface IInventory
{
	// Token: 0x06002CE9 RID: 11497
	bool AddItem(ItemStack _itemStack);

	// Token: 0x06002CEA RID: 11498
	[return: TupleElementNames(new string[] { "anyMoved", "allMoved" })]
	ValueTuple<bool, bool> TryStackItem(int _startIndex, ItemStack _itemStack);

	// Token: 0x06002CEB RID: 11499
	bool HasItem(ItemValue _item);
}
