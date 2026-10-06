using System;

// Token: 0x0200057D RID: 1405
public static class ItemStackExtensions
{
	// Token: 0x06002DE3 RID: 11747 RVA: 0x00123908 File Offset: 0x00121B08
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

	// Token: 0x06002DE4 RID: 11748 RVA: 0x00123944 File Offset: 0x00121B44
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
