using System;

// Token: 0x02001406 RID: 5126
public class IntHashMap
{
	// Token: 0x0600A0F6 RID: 41206 RVA: 0x003C6760 File Offset: 0x003C4960
	public IntHashMap()
	{
		this.threshold = 12;
		this.slots = new IntHashMapEntry[16];
	}

	// Token: 0x0600A0F7 RID: 41207 RVA: 0x003C677D File Offset: 0x003C497D
	[PublicizedFrom(EAccessModifier.Private)]
	public static int computeHash(int _v)
	{
		_v ^= (_v >> 20) ^ (_v >> 12);
		return _v ^ (_v >> 7) ^ (_v >> 4);
	}

	// Token: 0x0600A0F8 RID: 41208 RVA: 0x003C6795 File Offset: 0x003C4995
	[PublicizedFrom(EAccessModifier.Private)]
	public static int getSlotIndex(int _p1, int _p2)
	{
		return _p1 & (_p2 - 1);
	}

	// Token: 0x0600A0F9 RID: 41209 RVA: 0x003C679C File Offset: 0x003C499C
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

	// Token: 0x0600A0FA RID: 41210 RVA: 0x003C67E3 File Offset: 0x003C49E3
	public bool containsItem(int _item)
	{
		return this.lookupEntry(_item) != null;
	}

	// Token: 0x0600A0FB RID: 41211 RVA: 0x003C67F0 File Offset: 0x003C49F0
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

	// Token: 0x0600A0FC RID: 41212 RVA: 0x003C6834 File Offset: 0x003C4A34
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

	// Token: 0x0600A0FD RID: 41213 RVA: 0x003C6898 File Offset: 0x003C4A98
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

	// Token: 0x0600A0FE RID: 41214 RVA: 0x003C68E4 File Offset: 0x003C4AE4
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

	// Token: 0x0600A0FF RID: 41215 RVA: 0x003C693C File Offset: 0x003C4B3C
	public object removeObject(int _key)
	{
		IntHashMapEntry intHashMapEntry = this.removeEntry(_key);
		if (intHashMapEntry == null)
		{
			return null;
		}
		return intHashMapEntry.valueEntry;
	}

	// Token: 0x0600A100 RID: 41216 RVA: 0x003C695C File Offset: 0x003C4B5C
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

	// Token: 0x0600A101 RID: 41217 RVA: 0x003C69DC File Offset: 0x003C4BDC
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

	// Token: 0x0600A102 RID: 41218 RVA: 0x003C6A1C File Offset: 0x003C4C1C
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

	// Token: 0x0600A103 RID: 41219 RVA: 0x003C6A60 File Offset: 0x003C4C60
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

	// Token: 0x0600A104 RID: 41220 RVA: 0x003C6AB2 File Offset: 0x003C4CB2
	public static int getHash(int _v)
	{
		return IntHashMap.computeHash(_v);
	}

	// Token: 0x040078D7 RID: 30935
	[PublicizedFrom(EAccessModifier.Private)]
	public IntHashMapEntry[] slots;

	// Token: 0x040078D8 RID: 30936
	[PublicizedFrom(EAccessModifier.Private)]
	public int count;

	// Token: 0x040078D9 RID: 30937
	[PublicizedFrom(EAccessModifier.Private)]
	public int threshold;

	// Token: 0x040078DA RID: 30938
	[PublicizedFrom(EAccessModifier.Private)]
	public const float growFactor = 0.75f;

	// Token: 0x040078DB RID: 30939
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int versionStamp;
}
