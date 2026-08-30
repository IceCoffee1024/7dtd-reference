using System;

// Token: 0x02001421 RID: 5153
public class IntHashMap
{
	// Token: 0x0600A1F3 RID: 41459 RVA: 0x003CBAA4 File Offset: 0x003C9CA4
	public IntHashMap()
	{
		this.threshold = 12;
		this.slots = new IntHashMapEntry[16];
	}

	// Token: 0x0600A1F4 RID: 41460 RVA: 0x003CBAC1 File Offset: 0x003C9CC1
	[PublicizedFrom(EAccessModifier.Private)]
	public static int computeHash(int _v)
	{
		_v ^= (_v >> 20) ^ (_v >> 12);
		return _v ^ (_v >> 7) ^ (_v >> 4);
	}

	// Token: 0x0600A1F5 RID: 41461 RVA: 0x003CBAD9 File Offset: 0x003C9CD9
	[PublicizedFrom(EAccessModifier.Private)]
	public static int getSlotIndex(int _p1, int _p2)
	{
		return _p1 & (_p2 - 1);
	}

	// Token: 0x0600A1F6 RID: 41462 RVA: 0x003CBAE0 File Offset: 0x003C9CE0
	public object lookup(int _p)
	{
		int num = IntHashMap.computeHash(_p);
		for (IntHashMapEntry intHashMapEntry = this.slots[IntHashMap.getSlotIndex(num, this.slots.Length)]; intHashMapEntry != null; intHashMapEntry = intHashMapEntry.nextEntry)
		{
			if (intHashMapEntry.hashEntry == _p)
			{
				return intHashMapEntry.valueEntry;
			}
		}
		return null;
	}

	// Token: 0x0600A1F7 RID: 41463 RVA: 0x003CBB27 File Offset: 0x003C9D27
	public bool containsItem(int _item)
	{
		return this.lookupEntry(_item) != null;
	}

	// Token: 0x0600A1F8 RID: 41464 RVA: 0x003CBB34 File Offset: 0x003C9D34
	[PublicizedFrom(EAccessModifier.Private)]
	public IntHashMapEntry lookupEntry(int _item)
	{
		int num = IntHashMap.computeHash(_item);
		for (IntHashMapEntry intHashMapEntry = this.slots[IntHashMap.getSlotIndex(num, this.slots.Length)]; intHashMapEntry != null; intHashMapEntry = intHashMapEntry.nextEntry)
		{
			if (intHashMapEntry.hashEntry == _item)
			{
				return intHashMapEntry;
			}
		}
		return null;
	}

	// Token: 0x0600A1F9 RID: 41465 RVA: 0x003CBB78 File Offset: 0x003C9D78
	public void addKey(int _key, object _value)
	{
		int num = IntHashMap.computeHash(_key);
		int slotIndex = IntHashMap.getSlotIndex(num, this.slots.Length);
		for (IntHashMapEntry intHashMapEntry = this.slots[slotIndex]; intHashMapEntry != null; intHashMapEntry = intHashMapEntry.nextEntry)
		{
			if (intHashMapEntry.hashEntry == _key)
			{
				intHashMapEntry.valueEntry = _value;
			}
		}
		this.versionStamp++;
		this.insert(num, _key, _value, slotIndex);
	}

	// Token: 0x0600A1FA RID: 41466 RVA: 0x003CBBDC File Offset: 0x003C9DDC
	[PublicizedFrom(EAccessModifier.Private)]
	public void grow(int _size)
	{
		if (this.slots.Length == 1073741824)
		{
			this.threshold = int.MaxValue;
			return;
		}
		IntHashMapEntry[] array = new IntHashMapEntry[_size];
		this.copyTo(array);
		this.slots = array;
		this.threshold = (int)((float)_size * 0.75f);
	}

	// Token: 0x0600A1FB RID: 41467 RVA: 0x003CBC28 File Offset: 0x003C9E28
	[PublicizedFrom(EAccessModifier.Private)]
	public void copyTo(IntHashMapEntry[] _other)
	{
		IntHashMapEntry[] array = this.slots;
		int num = _other.Length;
		for (int i = 0; i < array.Length; i++)
		{
			IntHashMapEntry intHashMapEntry = array[i];
			if (intHashMapEntry != null)
			{
				array[i] = null;
				do
				{
					IntHashMapEntry nextEntry = intHashMapEntry.nextEntry;
					int slotIndex = IntHashMap.getSlotIndex(intHashMapEntry.slotHash, num);
					intHashMapEntry.nextEntry = _other[slotIndex];
					_other[slotIndex] = intHashMapEntry;
					intHashMapEntry = nextEntry;
				}
				while (intHashMapEntry != null);
			}
		}
	}

	// Token: 0x0600A1FC RID: 41468 RVA: 0x003CBC80 File Offset: 0x003C9E80
	public object removeObject(int _key)
	{
		IntHashMapEntry intHashMapEntry = this.removeEntry(_key);
		if (intHashMapEntry == null)
		{
			return null;
		}
		return intHashMapEntry.valueEntry;
	}

	// Token: 0x0600A1FD RID: 41469 RVA: 0x003CBCA0 File Offset: 0x003C9EA0
	[PublicizedFrom(EAccessModifier.Private)]
	public IntHashMapEntry removeEntry(int _key)
	{
		int slotIndex = IntHashMap.getSlotIndex(IntHashMap.computeHash(_key), this.slots.Length);
		IntHashMapEntry intHashMapEntry = this.slots[slotIndex];
		IntHashMapEntry intHashMapEntry2;
		IntHashMapEntry nextEntry;
		for (intHashMapEntry2 = intHashMapEntry; intHashMapEntry2 != null; intHashMapEntry2 = nextEntry)
		{
			nextEntry = intHashMapEntry2.nextEntry;
			if (intHashMapEntry2.hashEntry == _key)
			{
				this.versionStamp++;
				this.count--;
				if (intHashMapEntry == intHashMapEntry2)
				{
					this.slots[slotIndex] = nextEntry;
				}
				else
				{
					intHashMapEntry.nextEntry = nextEntry;
				}
				return intHashMapEntry2;
			}
			intHashMapEntry = intHashMapEntry2;
		}
		return intHashMapEntry2;
	}

	// Token: 0x0600A1FE RID: 41470 RVA: 0x003CBD20 File Offset: 0x003C9F20
	public void clearMap()
	{
		this.versionStamp++;
		IntHashMapEntry[] array = this.slots;
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = null;
		}
		this.count = 0;
	}

	// Token: 0x0600A1FF RID: 41471 RVA: 0x003CBD60 File Offset: 0x003C9F60
	public void map(Action<object> callback)
	{
		for (int i = 0; i < this.slots.Length; i++)
		{
			for (IntHashMapEntry intHashMapEntry = this.slots[i]; intHashMapEntry != null; intHashMapEntry = intHashMapEntry.nextEntry)
			{
				callback(intHashMapEntry.valueEntry);
			}
		}
	}

	// Token: 0x0600A200 RID: 41472 RVA: 0x003CBDA4 File Offset: 0x003C9FA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void insert(int _i1, int _i2, object _object, int _key)
	{
		IntHashMapEntry intHashMapEntry = this.slots[_key];
		this.slots[_key] = new IntHashMapEntry(_i1, _i2, _object, intHashMapEntry);
		int num = this.count;
		this.count = num + 1;
		if (num >= this.threshold)
		{
			this.grow(2 * this.slots.Length);
		}
	}

	// Token: 0x0600A201 RID: 41473 RVA: 0x003CBDF6 File Offset: 0x003C9FF6
	public static int getHash(int _v)
	{
		return IntHashMap.computeHash(_v);
	}

	// Token: 0x040079B0 RID: 31152
	[PublicizedFrom(EAccessModifier.Private)]
	public IntHashMapEntry[] slots;

	// Token: 0x040079B1 RID: 31153
	[PublicizedFrom(EAccessModifier.Private)]
	public int count;

	// Token: 0x040079B2 RID: 31154
	[PublicizedFrom(EAccessModifier.Private)]
	public int threshold;

	// Token: 0x040079B3 RID: 31155
	[PublicizedFrom(EAccessModifier.Private)]
	public const float growFactor = 0.75f;

	// Token: 0x040079B4 RID: 31156
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int versionStamp;
}
