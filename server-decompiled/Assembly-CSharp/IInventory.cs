using System;
using System.Runtime.CompilerServices;

// Token: 0x0200057D RID: 1405
public interface IInventory
{
	// Token: 0x06002D4F RID: 11599
	bool AddItem(ItemStack _itemStack);

	// Token: 0x06002D50 RID: 11600
	[return: TupleElementNames(new string[] { "anyMoved", "allMoved" })]
	ValueTuple<bool, bool> TryStackItem(int _startIndex, ItemStack _itemStack);

	// Token: 0x06002D51 RID: 11601
	bool HasItem(ItemValue _item);
}
