using System;
using System.Collections.Generic;
using UniLinq;

// Token: 0x0200148E RID: 5262
public static class StackSortUtil
{
	// Token: 0x0600A4E7 RID: 42215 RVA: 0x003D60A8 File Offset: 0x003D42A8
	public static ItemStack[] CombineAndSortStacks(ItemStack[] _stacks, int _ignoreSlots = 0, PackedBoolArray _ignoredSlots = null)
	{
		for (int i = _ignoreSlots; i < _stacks.Length - 1; i++)
		{
			if (_stacks[i].IsEmpty() && !StackSortUtil.IsIgnoredSlot(_ignoreSlots, _ignoredSlots, i))
			{
				for (int j = i + 1; j < _stacks.Length; j++)
				{
					if (!_stacks[j].IsEmpty() && !StackSortUtil.IsIgnoredSlot(_ignoreSlots, _ignoredSlots, j))
					{
						_stacks[i] = _stacks[j];
						_stacks[j] = ItemStack.Empty;
						break;
					}
				}
			}
			if (!_stacks[i].IsEmpty())
			{
				ItemClass itemClass = _stacks[i].itemValue.ItemClass;
				int num = itemClass.Stacknumber.Value - _stacks[i].count;
				if (!itemClass.HasQuality && num != 0)
				{
					for (int k = i + 1; k < _stacks.Length; k++)
					{
						if (!StackSortUtil.IsIgnoredSlot(_ignoreSlots, _ignoredSlots, k))
						{
							if (_stacks[i].itemValue.type == _stacks[k].itemValue.type)
							{
								int num2 = Utils.FastMin(_stacks[k].count, num);
								_stacks[i].count += num2;
								_stacks[k].count -= num2;
								num -= num2;
								if (_stacks[k].count == 0)
								{
									_stacks[k] = ItemStack.Empty;
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

	// Token: 0x0600A4E8 RID: 42216 RVA: 0x003D61E0 File Offset: 0x003D43E0
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

	// Token: 0x0600A4E9 RID: 42217 RVA: 0x003D62E3 File Offset: 0x003D44E3
	public static bool IsIgnoredSlot(int _ignoreSlots, PackedBoolArray _ignoredSlots, int _slot)
	{
		return _slot < _ignoreSlots || (_ignoredSlots != null && _slot < _ignoredSlots.Length && _ignoredSlots[_slot]);
	}

	// Token: 0x0600A4EA RID: 42218 RVA: 0x003D6300 File Offset: 0x003D4500
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

	// Token: 0x0600A4EB RID: 42219 RVA: 0x003D6340 File Offset: 0x003D4540
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
