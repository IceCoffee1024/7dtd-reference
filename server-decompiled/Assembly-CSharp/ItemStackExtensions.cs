using System;

// Token: 0x0200057D RID: 1405
public static class ItemStackExtensions
{
	// Token: 0x06002DDF RID: 11743 RVA: 0x001236BC File Offset: 0x001218BC
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
