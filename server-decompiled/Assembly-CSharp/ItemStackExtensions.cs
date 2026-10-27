using System;

// Token: 0x02000584 RID: 1412
public static class ItemStackExtensions
{
	// Token: 0x06002DEF RID: 11759 RVA: 0x00124484 File Offset: 0x00122684
	public static int GetArrayHashCode(this ItemStack[] _items)
	{
		if (_items == null)
		{
			return 0;
		}
		int num = 17;
		for (int i = 0; i < _items.Length; i++)
		{
			int num2 = ((_items[i] != null) ? _items[i].GetHashCode() : 0);
			num = num * 31 + num2;
		}
		return num;
	}

	// Token: 0x06002DF0 RID: 11760 RVA: 0x001244C0 File Offset: 0x001226C0
	public static void ClearItemStackArray(this ItemStack[] _items)
	{
		if (_items == null)
		{
			return;
		}
		for (int i = 0; i < _items.Length; i++)
		{
			_items[i] = ItemStack.Empty;
		}
	}
}
