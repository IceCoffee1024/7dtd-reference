using System;
using System.Collections.Generic;

// Token: 0x02001461 RID: 5217
public class MemoryPooledArray<T> where T : new()
{
	// Token: 0x0600A38E RID: 41870 RVA: 0x003D31E0 File Offset: 0x003D13E0
	public MemoryPooledArray()
	{
		this.pools = new List<T[]>[MemoryPooledArraySizes.poolElements.Length];
		for (int i = 0; i < this.pools.Length; i++)
		{
			this.pools[i] = new List<T[]>();
		}
		this.poolSize = new int[MemoryPooledArraySizes.poolElements.Length];
	}

	// Token: 0x0600A38F RID: 41871 RVA: 0x003D3238 File Offset: 0x003D1438
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

	// Token: 0x0600A390 RID: 41872 RVA: 0x003D32D4 File Offset: 0x003D14D4
	public T[] Grow(T[] _array)
	{
		return this.Grow(_array, _array.Length + 1);
	}

	// Token: 0x0600A391 RID: 41873 RVA: 0x003D32E4 File Offset: 0x003D14E4
	public T[] Grow(T[] _array, int _minSize)
	{
		T[] array = this.Alloc(_minSize);
		Array.Copy(_array, array, _array.Length);
		this.Free(_array);
		return array;
	}

	// Token: 0x0600A392 RID: 41874 RVA: 0x003D330C File Offset: 0x003D150C
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

	// Token: 0x0600A393 RID: 41875 RVA: 0x003D33B0 File Offset: 0x003D15B0
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

	// Token: 0x0600A394 RID: 41876 RVA: 0x003D3414 File Offset: 0x003D1614
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

	// Token: 0x0600A395 RID: 41877 RVA: 0x003D3460 File Offset: 0x003D1660
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

	// Token: 0x0600A396 RID: 41878 RVA: 0x003D349D File Offset: 0x003D169D
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

	// Token: 0x0600A397 RID: 41879 RVA: 0x003D34C4 File Offset: 0x003D16C4
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

	// Token: 0x04007AA0 RID: 31392
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T[]>[] pools;

	// Token: 0x04007AA1 RID: 31393
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] poolSize;

	// Token: 0x04007AA2 RID: 31394
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxCapacity;
}
