using System;
using System.Collections.Generic;
using UniLinq;

// Token: 0x020014DB RID: 5339
public static class StackSortUtil
{
	// Token: 0x0600A6EA RID: 42730 RVA: 0x003E247C File Offset: 0x003E067C
	public static ItemStack[] CombineAndSortStacks(ItemStack[] _stacks, int _ignoreSlots = 0, PackedBoolArray _ignoredSlots = null)
	{
		ItemStack[] array = new ItemStack[_stacks.Length];
		for (int i = 0; i < _stacks.Length; i++)
		{
			array[i] = _stacks[i].Clone();
		}
		_stacks = array;
		for (int j = _ignoreSlots; j < _stacks.Length - 1; j++)
		{
			if (_stacks[j].IsEmpty() && !StackSortUtil.IsIgnoredSlot(_ignoreSlots, _ignoredSlots, j))
			{
				for (int k = j + 1; k < _stacks.Length; k++)
				{
					if (!_stacks[k].IsEmpty() && !StackSortUtil.IsIgnoredSlot(_ignoreSlots, _ignoredSlots, k))
					{
						_stacks[j] = _stacks[k];
						_stacks[k] = ItemStack.Empty;
						break;
					}
				}
			}
			if (!_stacks[j].IsEmpty())
			{
				ItemClass itemClass = _stacks[j].itemValue.ItemClass;
				int num = itemClass.MaxCount - _stacks[j].count;
				if (!itemClass.HasQuality && num != 0)
				{
					for (int l = j + 1; l < _stacks.Length; l++)
					{
						if (!StackSortUtil.IsIgnoredSlot(_ignoreSlots, _ignoredSlots, l))
						{
							if (_stacks[j].itemValue.type == _stacks[l].itemValue.type)
							{
								int num2 = Utils.FastMin(_stacks[l].count, num);
								_stacks[j].count += num2;
								_stacks[l].count -= num2;
								num -= num2;
								if (_stacks[l].count == 0)
								{
									_stacks[l] = ItemStack.Empty;
								}
							}
							if (num == 0)
							{
								break;
							}
						}
					}
				}
			}
		}
		_stacks = StackSortUtil.SortStacks(_stacks, _ignoreSlots, _ignoredSlots);
		return _stacks;
	}

	// Token: 0x0600A6EB RID: 42731 RVA: 0x003E25EC File Offset: 0x003E07EC
	public static ItemStack[] SortStacks(ItemStack[] _stacks, int _ignoreSlots = 0, PackedBoolArray _ignoredSlots = null)
	{
		ItemStack[] array = new ItemStack[_stacks.Length];
		for (int i = 0; i < _stacks.Length; i++)
		{
			if (StackSortUtil.IsIgnoredSlot(_ignoreSlots, _ignoredSlots, i))
			{
				array[i] = _stacks[i];
				_stacks[i] = ItemStack.Empty;
			}
		}
		IEnumerator<ItemStack> enumerator = _stacks.OrderBy<ItemStack, string>(new Func<ItemStack, string>(StackSortUtil.getGroup)).ThenBy<ItemStack, string>(new Func<ItemStack, string>(StackSortUtil.getName)).ThenBy<ItemStack, ushort>((ItemStack _stack) => _stack.itemValue.Quality)
			.ThenByDescending<ItemStack, float>((ItemStack _stack) => _stack.itemValue.UseTimes)
			.GetEnumerator();
		bool flag = enumerator.MoveNext();
		int j = 0;
		while (j < array.Length && flag)
		{
			if (!StackSortUtil.IsIgnoredSlot(_ignoreSlots, _ignoredSlots, j))
			{
				array[j] = enumerator.Current;
				flag = enumerator.MoveNext();
			}
			j++;
		}
		while (j < array.Length)
		{
			array[j] = ItemStack.Empty;
			j++;
		}
		enumerator.Dispose();
		return array;
	}

	// Token: 0x0600A6EC RID: 42732 RVA: 0x003E26EF File Offset: 0x003E08EF
	public static bool IsIgnoredSlot(int _ignoreSlots, PackedBoolArray _ignoredSlots, int _slot)
	{
		return _slot < _ignoreSlots || (_ignoredSlots != null && _slot < _ignoredSlots.Length && _ignoredSlots[_slot]);
	}

	// Token: 0x0600A6ED RID: 42733 RVA: 0x003E270C File Offset: 0x003E090C
	[PublicizedFrom(EAccessModifier.Private)]
	public static string getGroup(ItemStack _stack)
	{
		ItemClass forId = ItemClass.GetForId(_stack.itemValue.type);
		if (forId == null)
		{
			return "zzzzz";
		}
		if (forId.IsBlock())
		{
			return "aaaaa";
		}
		return forId.Groups[0];
	}

	// Token: 0x0600A6EE RID: 42734 RVA: 0x003E274C File Offset: 0x003E094C
	[PublicizedFrom(EAccessModifier.Private)]
	public static string getName(ItemStack _stack)
	{
		ItemClass forId = ItemClass.GetForId(_stack.itemValue.type);
		if (forId == null)
		{
			return "zzzzz";
		}
		return forId.GetLocalizedItemName();
	}
}
