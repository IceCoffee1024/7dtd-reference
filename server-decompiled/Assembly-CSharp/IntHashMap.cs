using System;

// Token: 0x02001453 RID: 5203
public class IntHashMap
{
	// Token: 0x0600A2F6 RID: 41718 RVA: 0x003D2A44 File Offset: 0x003D0C44
	public IntHashMap()
	{
		this.threshold = 12;
		this.slots = new IntHashMapEntry[16];
	}

	// Token: 0x0600A2F7 RID: 41719 RVA: 0x003D2A61 File Offset: 0x003D0C61
	[PublicizedFrom(EAccessModifier.Private)]
	public static int computeHash(int _v)
	{
		_v ^= (_v >> 20) ^ (_v >> 12);
		return _v ^ (_v >> 7) ^ (_v >> 4);
	}

	// Token: 0x0600A2F8 RID: 41720 RVA: 0x003D2A79 File Offset: 0x003D0C79
	[PublicizedFrom(EAccessModifier.Private)]
	public static int getSlotIndex(int _p1, int _p2)
	{
		return _p1 & (_p2 - 1);
	}

	// Token: 0x0600A2F9 RID: 41721 RVA: 0x003D2A80 File Offset: 0x003D0C80
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

	// Token: 0x0600A2FA RID: 41722 RVA: 0x003D2AC7 File Offset: 0x003D0CC7
	public bool containsItem(int _item)
	{
		return this.lookupEntry(_item) != null;
	}

	// Token: 0x0600A2FB RID: 41723 RVA: 0x003D2AD4 File Offset: 0x003D0CD4
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

	// Token: 0x0600A2FC RID: 41724 RVA: 0x003D2B18 File Offset: 0x003D0D18
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

	// Token: 0x0600A2FD RID: 41725 RVA: 0x003D2B7C File Offset: 0x003D0D7C
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

	// Token: 0x0600A2FE RID: 41726 RVA: 0x003D2BC8 File Offset: 0x003D0DC8
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

	// Token: 0x0600A2FF RID: 41727 RVA: 0x003D2C20 File Offset: 0x003D0E20
	public object removeObject(int _key)
	{
		IntHashMapEntry intHashMapEntry = this.removeEntry(_key);
		if (intHashMapEntry == null)
		{
			return null;
		}
		return intHashMapEntry.valueEntry;
	}

	// Token: 0x0600A300 RID: 41728 RVA: 0x003D2C40 File Offset: 0x003D0E40
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

	// Token: 0x0600A301 RID: 41729 RVA: 0x003D2CC0 File Offset: 0x003D0EC0
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

	// Token: 0x0600A302 RID: 41730 RVA: 0x003D2D00 File Offset: 0x003D0F00
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

	// Token: 0x0600A303 RID: 41731 RVA: 0x003D2D44 File Offset: 0x003D0F44
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

	// Token: 0x0600A304 RID: 41732 RVA: 0x003D2D96 File Offset: 0x003D0F96
	public static int getHash(int _v)
	{
		return IntHashMap.computeHash(_v);
	}

	// Token: 0x04007A6C RID: 31340
	[PublicizedFrom(EAccessModifier.Private)]
	public IntHashMapEntry[] slots;

	// Token: 0x04007A6D RID: 31341
	[PublicizedFrom(EAccessModifier.Private)]
	public int count;

	// Token: 0x04007A6E RID: 31342
	[PublicizedFrom(EAccessModifier.Private)]
	public int threshold;

	// Token: 0x04007A6F RID: 31343
	[PublicizedFrom(EAccessModifier.Private)]
	public const float growFactor = 0.75f;

	// Token: 0x04007A70 RID: 31344
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int versionStamp;
}
