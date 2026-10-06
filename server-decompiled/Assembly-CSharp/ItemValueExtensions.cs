using System;

// Token: 0x02000583 RID: 1411
public static class ItemValueExtensions
{
	// Token: 0x06002E84 RID: 11908 RVA: 0x00126870 File Offset: 0x00124A70
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

	// Token: 0x06002E85 RID: 11909 RVA: 0x001268AC File Offset: 0x00124AAC
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
