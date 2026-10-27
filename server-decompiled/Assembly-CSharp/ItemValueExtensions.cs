using System;

// Token: 0x0200058A RID: 1418
public static class ItemValueExtensions
{
	// Token: 0x06002E92 RID: 11922 RVA: 0x00127464 File Offset: 0x00125664
	public static ItemValue[] CloneItemValueArray(this ItemValue[] _items)
	{
		if (_items == null)
		{
			return null;
		}
		ItemValue[] array = new ItemValue[_items.Length];
		for (int i = 0; i < _items.Length; i++)
		{
			ItemValue[] array2 = array;
			int num = i;
			ItemValue itemValue = _items[i];
			array2[num] = ((itemValue != null) ? itemValue.Clone() : null);
		}
		return array;
	}

	// Token: 0x06002E93 RID: 11923 RVA: 0x001274A0 File Offset: 0x001256A0
	public static void ClearItemValueArray(this ItemValue[] _items)
	{
		if (_items == null)
		{
			return;
		}
		for (int i = 0; i < _items.Length; i++)
		{
			_items[i] = ItemValue.None;
		}
	}
}
