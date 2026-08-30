using System;
using System.Collections.Generic;

// Token: 0x02001441 RID: 5185
public class MemoryPooledArray<T> where T : new()
{
	// Token: 0x0600A2E2 RID: 41698 RVA: 0x003CE5D0 File Offset: 0x003CC7D0
	public MemoryPooledArray()
	{
		this.pools = new List<T[]>[MemoryPooledArraySizes.poolElements.Length];
		for (int i = 0; i < this.pools.Length; i++)
		{
			this.pools[i] = new List<T[]>();
		}
		this.poolSize = new int[MemoryPooledArraySizes.poolElements.Length];
	}

	// Token: 0x0600A2E3 RID: 41699 RVA: 0x003CE628 File Offset: 0x003CC828
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

	// Token: 0x0600A2E4 RID: 41700 RVA: 0x003CE6C4 File Offset: 0x003CC8C4
	public T[] Grow(T[] _array)
	{
		return this.Grow(_array, _array.Length + 1);
	}

	// Token: 0x0600A2E5 RID: 41701 RVA: 0x003CE6D4 File Offset: 0x003CC8D4
	public T[] Grow(T[] _array, int _minSize)
	{
		T[] array = this.Alloc(_minSize);
		Array.Copy(_array, array, _array.Length);
		this.Free(_array);
		return array;
	}

	// Token: 0x0600A2E6 RID: 41702 RVA: 0x003CE6FC File Offset: 0x003CC8FC
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

	// Token: 0x0600A2E7 RID: 41703 RVA: 0x003CE7A0 File Offset: 0x003CC9A0
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

	// Token: 0x0600A2E8 RID: 41704 RVA: 0x003CE804 File Offset: 0x003CCA04
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

	// Token: 0x0600A2E9 RID: 41705 RVA: 0x003CE850 File Offset: 0x003CCA50
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

	// Token: 0x0600A2EA RID: 41706 RVA: 0x003CE88D File Offset: 0x003CCA8D
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

	// Token: 0x0600A2EB RID: 41707 RVA: 0x003CE8B4 File Offset: 0x003CCAB4
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

	// Token: 0x04007A25 RID: 31269
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T[]>[] pools;

	// Token: 0x04007A26 RID: 31270
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] poolSize;

	// Token: 0x04007A27 RID: 31271
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxCapacity;
}
