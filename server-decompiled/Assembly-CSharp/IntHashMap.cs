using System;

// Token: 0x02001441 RID: 5185
public class IntHashMap
{
	// Token: 0x0600A29E RID: 41630 RVA: 0x003D0640 File Offset: 0x003CE840
	public IntHashMap()
	{
		this.threshold = 12;
		this.slots = new IntHashMapEntry[16];
	}

	// Token: 0x0600A29F RID: 41631 RVA: 0x003D065D File Offset: 0x003CE85D
	[PublicizedFrom(EAccessModifier.Private)]
	public static int computeHash(int _v)
	{
		_v ^= (_v >> 20) ^ (_v >> 12);
		return _v ^ (_v >> 7) ^ (_v >> 4);
	}

	// Token: 0x0600A2A0 RID: 41632 RVA: 0x003D0675 File Offset: 0x003CE875
	[PublicizedFrom(EAccessModifier.Private)]
	public static int getSlotIndex(int _p1, int _p2)
	{
		return _p1 & (_p2 - 1);
	}

	// Token: 0x0600A2A1 RID: 41633 RVA: 0x003D067C File Offset: 0x003CE87C
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

	// Token: 0x0600A2A2 RID: 41634 RVA: 0x003D06C3 File Offset: 0x003CE8C3
	public bool containsItem(int _item)
	{
		return this.lookupEntry(_item) != null;
	}

	// Token: 0x0600A2A3 RID: 41635 RVA: 0x003D06D0 File Offset: 0x003CE8D0
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

	// Token: 0x0600A2A4 RID: 41636 RVA: 0x003D0714 File Offset: 0x003CE914
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

	// Token: 0x0600A2A5 RID: 41637 RVA: 0x003D0778 File Offset: 0x003CE978
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

	// Token: 0x0600A2A6 RID: 41638 RVA: 0x003D07C4 File Offset: 0x003CE9C4
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

	// Token: 0x0600A2A7 RID: 41639 RVA: 0x003D081C File Offset: 0x003CEA1C
	public object removeObject(int _key)
	{
		IntHashMapEntry intHashMapEntry = this.removeEntry(_key);
		if (intHashMapEntry == null)
		{
			return null;
		}
		return intHashMapEntry.valueEntry;
	}

	// Token: 0x0600A2A8 RID: 41640 RVA: 0x003D083C File Offset: 0x003CEA3C
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

	// Token: 0x0600A2A9 RID: 41641 RVA: 0x003D08BC File Offset: 0x003CEABC
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

	// Token: 0x0600A2AA RID: 41642 RVA: 0x003D08FC File Offset: 0x003CEAFC
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

	// Token: 0x0600A2AB RID: 41643 RVA: 0x003D0940 File Offset: 0x003CEB40
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

	// Token: 0x0600A2AC RID: 41644 RVA: 0x003D0992 File Offset: 0x003CEB92
	public static int getHash(int _v)
	{
		return IntHashMap.computeHash(_v);
	}

	// Token: 0x04007A2B RID: 31275
	[PublicizedFrom(EAccessModifier.Private)]
	public IntHashMapEntry[] slots;

	// Token: 0x04007A2C RID: 31276
	[PublicizedFrom(EAccessModifier.Private)]
	public int count;

	// Token: 0x04007A2D RID: 31277
	[PublicizedFrom(EAccessModifier.Private)]
	public int threshold;

	// Token: 0x04007A2E RID: 31278
	[PublicizedFrom(EAccessModifier.Private)]
	public const float growFactor = 0.75f;

	// Token: 0x04007A2F RID: 31279
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int versionStamp;
}
