using System;
using System.Collections.Generic;

// Token: 0x02001473 RID: 5235
public class MemoryPooledArray<T> where T : new()
{
	// Token: 0x0600A3E6 RID: 41958 RVA: 0x003D55E4 File Offset: 0x003D37E4
	public MemoryPooledArray()
	{
		this.pools = new List<T[]>[MemoryPooledArraySizes.poolElements.Length];
		for (int i = 0; i < this.pools.Length; i++)
		{
			this.pools[i] = new List<T[]>();
		}
		this.poolSize = new int[MemoryPooledArraySizes.poolElements.Length];
	}

	// Token: 0x0600A3E7 RID: 41959 RVA: 0x003D563C File Offset: 0x003D383C
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

	// Token: 0x0600A3E8 RID: 41960 RVA: 0x003D56D8 File Offset: 0x003D38D8
	public T[] Grow(T[] _array)
	{
		return this.Grow(_array, _array.Length + 1);
	}

	// Token: 0x0600A3E9 RID: 41961 RVA: 0x003D56E8 File Offset: 0x003D38E8
	public T[] Grow(T[] _array, int _minSize)
	{
		T[] array = this.Alloc(_minSize);
		Array.Copy(_array, array, _array.Length);
		this.Free(_array);
		return array;
	}

	// Token: 0x0600A3EA RID: 41962 RVA: 0x003D5710 File Offset: 0x003D3910
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

	// Token: 0x0600A3EB RID: 41963 RVA: 0x003D57B4 File Offset: 0x003D39B4
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

	// Token: 0x0600A3EC RID: 41964 RVA: 0x003D5818 File Offset: 0x003D3A18
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

	// Token: 0x0600A3ED RID: 41965 RVA: 0x003D5864 File Offset: 0x003D3A64
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

	// Token: 0x0600A3EE RID: 41966 RVA: 0x003D58A1 File Offset: 0x003D3AA1
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

	// Token: 0x0600A3EF RID: 41967 RVA: 0x003D58C8 File Offset: 0x003D3AC8
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

	// Token: 0x04007AE1 RID: 31457
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T[]>[] pools;

	// Token: 0x04007AE2 RID: 31458
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] poolSize;

	// Token: 0x04007AE3 RID: 31459
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxCapacity;
}
