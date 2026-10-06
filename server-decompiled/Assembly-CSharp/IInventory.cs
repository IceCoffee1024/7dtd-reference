using System;
using System.Runtime.CompilerServices;

// Token: 0x02000576 RID: 1398
public interface IInventory
{
	// Token: 0x06002D44 RID: 11588
	bool AddItem(ItemStack _itemStack);

	// Token: 0x06002D45 RID: 11589
	[return: TupleElementNames(new string[] { "anyMoved", "allMoved" })]
	ValueTuple<bool, bool> TryStackItem(int _startIndex, ItemStack _itemStack);

	// Token: 0x06002D46 RID: 11590
	bool HasItem(ItemValue _item);
}
