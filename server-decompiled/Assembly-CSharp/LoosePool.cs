using System;
using System.Collections.Generic;

// Token: 0x020003B2 RID: 946
public class LoosePool<T> where T : new()
{
	// Token: 0x06001C5C RID: 7260 RVA: 0x000A8090 File Offset: 0x000A6290
	public LoosePool()
	{
		this.pools = new List<T[]>[this.poolElements.Length];
		for (int i = 0; i < this.pools.Length; i++)
		{
			this.pools[i] = new List<T[]>();
		}
		this.poolSize = new int[this.poolElements.Length];
	}

	// Token: 0x06001C5D RID: 7261 RVA: 0x000A8120 File Offset: 0x000A6320
	public T[] Alloc(int _minSize = 0)
	{
		List<T[]>[] array = this.pools;
		T[] array4;
		lock (array)
		{
			int num = this.sizeToIdx(_minSize, this.AllowHigherPool);
			T[] array2;
			if (this.poolSize[num] == 0)
			{
				array2 = new T[this.poolElements[num]];
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

	// Token: 0x06001C5E RID: 7262 RVA: 0x000A81C0 File Offset: 0x000A63C0
	public void Free(T[] _array)
	{
		if (_array == null)
		{
			return;
		}
		List<T[]>[] array = this.pools;
		lock (array)
		{
			int num = this.sizeToIdx(_array.Length, false);
			int num2 = this.maxSize[num];
			if (_array.Length != this.poolElements[num])
			{
				Log.Out("removing item as it does not match the cache size");
			}
			else if (!this.EnforceMaxSize || num2 <= 0 || this.poolSize[num] < num2)
			{
				if (this.poolSize[num] >= this.pools[num].Count)
				{
					this.pools[num].Add(_array);
					this.poolSize[num]++;
				}
				else
				{
					List<T[]> list = this.pools[num];
					int[] array2 = this.poolSize;
					int num3 = num;
					int num4 = array2[num3];
					array2[num3] = num4 + 1;
					list[num4] = _array;
				}
			}
		}
	}

	// Token: 0x06001C5F RID: 7263 RVA: 0x000A82A4 File Offset: 0x000A64A4
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

	// Token: 0x06001C60 RID: 7264 RVA: 0x000A8308 File Offset: 0x000A6508
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeToIdx(int _size, bool allowHigher)
	{
		int num = -1;
		for (int i = 0; i < this.poolElements.Length; i++)
		{
			if (this.poolElements[i] >= _size)
			{
				if (num == -1)
				{
					num = i;
				}
				if (!allowHigher || this.poolSize[i] > 0)
				{
					return i;
				}
			}
		}
		if (num == -1)
		{
			throw new Exception("Array length in pool not supported " + _size.ToString());
		}
		return num;
	}

	// Token: 0x06001C61 RID: 7265 RVA: 0x000A8368 File Offset: 0x000A6568
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

	// Token: 0x06001C62 RID: 7266 RVA: 0x000A83A8 File Offset: 0x000A65A8
	public int GetTotalItems()
	{
		int num = 0;
		foreach (int num2 in this.poolSize)
		{
			num += num2;
		}
		return num;
	}

	// Token: 0x06001C63 RID: 7267 RVA: 0x000A83D8 File Offset: 0x000A65D8
	public string GetSize(out int totalBytes)
	{
		string text = "";
		totalBytes = 0;
		for (int i = 0; i < this.pools.Length; i++)
		{
			int num = this.poolSize[i];
			int num2 = this.poolElements[i];
			int num3 = 4 * num2 * num;
			totalBytes += num3;
			text += string.Format("{0} x {1} = {2}\n", num2, num, num3);
		}
		text = text + "Bytes: " + totalBytes.ToString() + "\n";
		text = text + "KBytes: " + (totalBytes / 1024).ToString() + "\n";
		return text + "MBytes: " + (totalBytes / 1024 / 1024).ToString();
	}

	// Token: 0x04001259 RID: 4697
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] poolElements = new int[]
	{
		64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768,
		65536, 131072, 262144, 524288, 1048576, 2097152, 4194304, 8388608, 16777216, 33554432,
		67108864, 134217728
	};

	// Token: 0x0400125A RID: 4698
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] maxSize = new int[]
	{
		0, 0, 0, 0, 0, 0, 0, 0, 0, 15,
		20, 20, 10, 10, 10, 10, 5, 5, 2, 2,
		2, 2
	};

	// Token: 0x0400125B RID: 4699
	public bool EnforceMaxSize;

	// Token: 0x0400125C RID: 4700
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T[]>[] pools;

	// Token: 0x0400125D RID: 4701
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] poolSize;

	// Token: 0x0400125E RID: 4702
	public bool AllowHigherPool = true;
}
