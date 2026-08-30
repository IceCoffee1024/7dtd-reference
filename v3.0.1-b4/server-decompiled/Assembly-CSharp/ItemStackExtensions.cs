using System;

// Token: 0x02000578 RID: 1400
public static class ItemStackExtensions
{
	// Token: 0x06002DB4 RID: 11700 RVA: 0x0012260C File Offset: 0x0012080C
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
}
