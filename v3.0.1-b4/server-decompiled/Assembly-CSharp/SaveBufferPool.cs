using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

// Token: 0x02000069 RID: 105
public sealed class SaveBufferPool
{
	// Token: 0x0600021B RID: 539 RVA: 0x00011814 File Offset: 0x0000FA14
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveBufferPool(int[] bufferLengths, int[] poolCapacities, int[] bufferPrefillCounts)
	{
		this.m_poolCapacities = poolCapacities;
		this.m_bufferLengths = bufferLengths;
		this.m_poolSizes = new int[this.m_bufferLengths.Length];
		this.m_pools = new List<byte[]>[this.m_bufferLengths.Length];
		for (int i = 0; i < this.m_bufferLengths.Length; i++)
		{
			this.m_pools[i] = new List<byte[]>(this.m_poolCapacities[i]);
		}
		if (bufferPrefillCounts != null)
		{
			for (int j = 0; j < this.m_bufferLengths.Length; j++)
			{
				for (int k = 0; k < bufferPrefillCounts[j]; k++)
				{
					this.m_pools[j].Add(new byte[this.m_bufferLengths[j]]);
					this.m_poolSizes[j]++;
				}
			}
		}
	}

	// Token: 0x0600021C RID: 540 RVA: 0x000118EC File Offset: 0x0000FAEC
	[PublicizedFrom(EAccessModifier.Private)]
	public int calcPoolIndex(int bufferSize)
	{
		for (int i = 0; i < this.m_bufferLengths.Length; i++)
		{
			if (bufferSize <= this.m_bufferLengths[i])
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x0600021D RID: 541 RVA: 0x0001191C File Offset: 0x0000FB1C
	public byte[] Alloc(int bufferSize)
	{
		int num = this.calcPoolIndex(bufferSize);
		if (num == -1)
		{
			Log.Warning(string.Format("SaveBufferPool was requested a buffer of length {0}, which is larger than any pool size.", bufferSize));
			return new byte[bufferSize];
		}
		object @lock = this.m_lock;
		byte[] array2;
		lock (@lock)
		{
			byte[] array;
			if (this.m_poolSizes[num] == 0)
			{
				array = new byte[this.m_bufferLengths[num]];
			}
			else
			{
				int[] poolSizes = this.m_poolSizes;
				int num2 = num;
				int num3 = poolSizes[num2] - 1;
				poolSizes[num2] = num3;
				int num4 = num3;
				array = this.m_pools[num][num4];
				this.m_pools[num][num4] = null;
			}
			array2 = array;
		}
		return array2;
	}

	// Token: 0x0600021E RID: 542 RVA: 0x000119D8 File Offset: 0x0000FBD8
	public void Free(byte[] buffer)
	{
		if (buffer == null)
		{
			return;
		}
		int num = this.calcPoolIndex(buffer.Length);
		if (num < 0 || this.m_bufferLengths[num] != buffer.Length)
		{
			Log.Error(string.Format("SaveBufferPool had a buffer returned that does not fit into any pool. buffer.Length == {0}", buffer.Length));
			return;
		}
		object @lock = this.m_lock;
		lock (@lock)
		{
			if (this.m_poolSizes[num] < this.m_poolCapacities[num])
			{
				if (this.m_poolSizes[num] >= this.m_pools[num].Count)
				{
					this.m_pools[num].Add(buffer);
					this.m_poolSizes[num]++;
				}
				else
				{
					List<byte[]> list = this.m_pools[num];
					int[] poolSizes = this.m_poolSizes;
					int num2 = num;
					int num3 = poolSizes[num2];
					poolSizes[num2] = num3 + 1;
					list[num3] = buffer;
				}
			}
		}
	}

	// Token: 0x0600021F RID: 543 RVA: 0x00011AB8 File Offset: 0x0000FCB8
	[Conditional("DEBUG_SAVE_BUFFER_POOL")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void DebugInit()
	{
		object debugLock = this.m_debugLock;
		lock (debugLock)
		{
			this.m_debugSampleCount = 0;
			this.m_debugLogPath = Path.Join(GameIO.GetApplicationTempPath(), "SaveBufferPool.csv");
			this.m_debugLogBuilder = new StringBuilder();
			this.m_debugConcurrentUsages = new int[this.m_bufferLengths.Length];
			this.m_debugAverageBufferOccupancies = new float[this.m_bufferLengths.Length];
			if (File.Exists(this.m_debugLogPath))
			{
				File.Delete(this.m_debugLogPath);
			}
			Log.Out("SaveBufferPool logging to: " + this.m_debugLogPath);
			this.m_debugLogBuilder.Append("Sample");
			foreach (int num in this.m_bufferLengths)
			{
				this.m_debugLogBuilder.Append(",").Append(num).Append(" capacity,")
					.Append(num)
					.Append(" concurrent usage,")
					.Append(num)
					.Append(" average occupancy");
			}
			this.m_debugLogBuilder.AppendLine();
		}
	}

	// Token: 0x06000220 RID: 544 RVA: 0x00011C00 File Offset: 0x0000FE00
	[Conditional("DEBUG_SAVE_BUFFER_POOL")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void DebugLogStats(bool output)
	{
		object debugLock = this.m_debugLock;
		lock (debugLock)
		{
			this.m_debugSampleCount++;
			this.m_debugLogBuilder.Append(string.Format("{0}", this.m_debugSampleCount));
			for (int i = 0; i < this.m_bufferLengths.Length; i++)
			{
				this.m_debugLogBuilder.Append(",").Append(this.m_poolCapacities[i]).Append(",")
					.Append(this.m_debugConcurrentUsages[i])
					.Append(",")
					.Append(this.m_debugAverageBufferOccupancies[i]);
			}
			this.m_debugLogBuilder.AppendLine();
			if (output)
			{
				File.AppendAllText(this.m_debugLogPath, this.m_debugLogBuilder.ToString());
				this.m_debugLogBuilder.Clear();
			}
		}
	}

	// Token: 0x06000221 RID: 545 RVA: 0x00011CFC File Offset: 0x0000FEFC
	[Conditional("DEBUG_SAVE_BUFFER_POOL")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void DebugAlloc(int bufferSize, int poolIndex)
	{
		this.m_debugConcurrentUsages[poolIndex]++;
		float num = (float)bufferSize / (float)this.m_bufferLengths[poolIndex];
		this.m_debugAverageBufferOccupancies[poolIndex] = (this.m_debugAverageBufferOccupancies[poolIndex] * (float)(this.m_debugConcurrentUsages[poolIndex] - 1) + num) / (float)this.m_debugConcurrentUsages[poolIndex];
	}

	// Token: 0x06000222 RID: 546 RVA: 0x00011D4F File Offset: 0x0000FF4F
	[Conditional("DEBUG_SAVE_BUFFER_POOL")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void DebugFree(int poolIndex)
	{
		this.m_debugConcurrentUsages[poolIndex]--;
	}

	// Token: 0x040002B4 RID: 692
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object m_lock = new object();

	// Token: 0x040002B5 RID: 693
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<byte[]>[] m_pools;

	// Token: 0x040002B6 RID: 694
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] m_bufferLengths;

	// Token: 0x040002B7 RID: 695
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] m_poolCapacities;

	// Token: 0x040002B8 RID: 696
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] m_poolSizes;

	// Token: 0x040002B9 RID: 697
	public static readonly SaveBufferPool Instance = new SaveBufferPool(new int[] { 64, 128, 1024, 16384, 65536, 393216, 2097152, 8388608, 16777216 }, new int[] { 4, 1, 2, 30, 6, 15, 2, 2, 0 }, new int[] { 4, 1, 2, 30, 6, 15, 2, 2, 0 });

	// Token: 0x040002BA RID: 698
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object m_debugLock = new object();

	// Token: 0x040002BB RID: 699
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_debugSampleCount;

	// Token: 0x040002BC RID: 700
	[PublicizedFrom(EAccessModifier.Private)]
	public string m_debugLogPath;

	// Token: 0x040002BD RID: 701
	[PublicizedFrom(EAccessModifier.Private)]
	public StringBuilder m_debugLogBuilder;

	// Token: 0x040002BE RID: 702
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] m_debugConcurrentUsages;

	// Token: 0x040002BF RID: 703
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] m_debugAverageBufferOccupancies;
}
