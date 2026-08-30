using System;
using System.Collections.Generic;

// Token: 0x02001426 RID: 5158
public class MemoryPooledArray<T> where T : new()
{
	// Token: 0x0600A1E5 RID: 41445 RVA: 0x003C928C File Offset: 0x003C748C
	public MemoryPooledArray()
	{
		this.pools = new List<T[]>[MemoryPooledArraySizes.poolElements.Length];
		for (int i = 0; i < this.pools.Length; i++)
		{
			this.pools[i] = new List<T[]>();
		}
		this.poolSize = new int[MemoryPooledArraySizes.poolElements.Length];
	}

	// Token: 0x0600A1E6 RID: 41446 RVA: 0x003C92E4 File Offset: 0x003C74E4
	public T[] Alloc(int _minSize = 0)
	{
		List<T[]>[] array = this.pools;
		T[] array4;
		lock (array)
		{
			int num = this.sizeToIdx(_minSize);
			T[] array2;
			if (this.poolSize[num] == 0)
			{
				array2 = new T[MemoryPooledArraySizes.poolElements[num]];
			}
			else
			{
				int[] array3 = this.poolSize;
				int num2 = num;
				int num3 = array3[num2] - 1;
				array3[num2] = num3;
				int num4 = num3;
				array2 = this.pools[num][num4];
				this.pools[num][num4] = null;
			}
			array4 = array2;
		}
		return array4;
	}

	// Token: 0x0600A1E7 RID: 41447 RVA: 0x003C9380 File Offset: 0x003C7580
	public T[] Grow(T[] _array)
	{
		return this.Grow(_array, _array.Length + 1);
	}

	// Token: 0x0600A1E8 RID: 41448 RVA: 0x003C9390 File Offset: 0x003C7590
	public T[] Grow(T[] _array, int _minSize)
	{
		T[] array = this.Alloc(_minSize);
		Array.Copy(_array, array, _array.Length);
		this.Free(_array);
		return array;
	}

	// Token: 0x0600A1E9 RID: 41449 RVA: 0x003C93B8 File Offset: 0x003C75B8
	public void Free(T[] _array)
	{
		if (_array == null)
		{
			return;
		}
		List<T[]>[] array = this.pools;
		lock (array)
		{
			int num = this.sizeToIdx(_array.Length);
			if (this.poolSize[num] >= this.pools[num].Count)
			{
				this.pools[num].Add(_array);
				this.poolSize[num]++;
			}
			else
			{
				List<T[]> list = this.pools[num];
				int[] array2 = this.poolSize;
				int num2 = num;
				int num3 = array2[num2];
				array2[num2] = num3 + 1;
				list[num3] = _array;
			}
		}
	}

	// Token: 0x0600A1EA RID: 41450 RVA: 0x003C945C File Offset: 0x003C765C
	public void FreeAll()
	{
		List<T[]>[] array = this.pools;
		lock (array)
		{
			for (int i = 0; i < this.pools.Length; i++)
			{
				this.pools[i].Clear();
				this.poolSize[i] = 0;
			}
		}
	}

	// Token: 0x0600A1EB RID: 41451 RVA: 0x003C94C0 File Offset: 0x003C76C0
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeToIdx(int _size)
	{
		int num = -1;
		for (int i = 0; i < MemoryPooledArraySizes.poolElements.Length; i++)
		{
			if (MemoryPooledArraySizes.poolElements[i] >= _size)
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			throw new Exception("Array length in pool not supported " + _size.ToString());
		}
		return num;
	}

	// Token: 0x0600A1EC RID: 41452 RVA: 0x003C950C File Offset: 0x003C770C
	public int GetCount()
	{
		int num = 0;
		for (int i = 0; i < this.pools.Length; i++)
		{
			if (this.pools[i] != null)
			{
				num += this.pools[i].Count;
			}
		}
		return num;
	}

	// Token: 0x0600A1ED RID: 41453 RVA: 0x003C9549 File Offset: 0x003C7749
	public int GetCount(int _poolIndex)
	{
		if (_poolIndex < 0 || _poolIndex >= this.pools.Length)
		{
			return 0;
		}
		List<T[]> list = this.pools[_poolIndex];
		if (list == null)
		{
			return 0;
		}
		return list.Count;
	}

	// Token: 0x0600A1EE RID: 41454 RVA: 0x003C9570 File Offset: 0x003C7770
	public long GetElementsCount()
	{
		int num = 0;
		for (int i = 0; i < this.pools.Length; i++)
		{
			if (this.pools[i] != null)
			{
				num += this.pools[i].Count * MemoryPooledArraySizes.poolElements[i];
			}
		}
		return (long)num;
	}

	// Token: 0x0400794C RID: 31052
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T[]>[] pools;

	// Token: 0x0400794D RID: 31053
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] poolSize;

	// Token: 0x0400794E RID: 31054
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxCapacity;
}
