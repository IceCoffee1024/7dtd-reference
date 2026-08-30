using System;
using System.Collections.Generic;

// Token: 0x02000C38 RID: 3128
public class ThreadContainerPool
{
	// Token: 0x170009B3 RID: 2483
	// (get) Token: 0x06005F45 RID: 24389 RVA: 0x0025AC14 File Offset: 0x00258E14
	public int Count
	{
		get
		{
			object poolLock = this._poolLock;
			int count;
			lock (poolLock)
			{
				count = this.pool.Count;
			}
			return count;
		}
	}

	// Token: 0x06005F46 RID: 24390 RVA: 0x0025AC5C File Offset: 0x00258E5C
	public ThreadContainerPool(int initialCapacity, int initialCount)
	{
		this._poolLock = new object();
		this.pool = new List<ThreadContainer>(initialCapacity);
		for (int i = 0; i < initialCount; i++)
		{
			this.pool.Add(new ThreadContainer());
		}
	}

	// Token: 0x06005F47 RID: 24391 RVA: 0x0025ACA4 File Offset: 0x00258EA4
	public ThreadContainer GetObject(DistantTerrain _TerExt, DistantChunk _DChunk, DistantChunkBasicMesh _BMesh, bool _WasReset)
	{
		object poolLock = this._poolLock;
		ThreadContainer threadContainer;
		lock (poolLock)
		{
			if (this.pool.Count == 0)
			{
				threadContainer = new ThreadContainer(_TerExt, _DChunk, _BMesh, _WasReset);
			}
			else
			{
				ThreadContainer threadContainer2 = this.pool[this.pool.Count - 1];
				this.pool.RemoveAt(this.pool.Count - 1);
				threadContainer2.Init(_TerExt, _DChunk, _BMesh, _WasReset);
				threadContainer = threadContainer2;
			}
		}
		return threadContainer;
	}

	// Token: 0x06005F48 RID: 24392 RVA: 0x0025AD38 File Offset: 0x00258F38
	public void ReturnObject(ThreadContainer item, bool IsClearItem)
	{
		if (item == null)
		{
			throw new ArgumentNullException("ThreadContainer is null");
		}
		object poolLock = this._poolLock;
		lock (poolLock)
		{
			if (this.pool.Contains(item))
			{
				throw new InvalidOperationException("ThreadContainer already in pool");
			}
			item.Clear(IsClearItem);
			this.pool.Add(item);
		}
	}

	// Token: 0x04004AB6 RID: 19126
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ThreadContainer> pool;

	// Token: 0x04004AB7 RID: 19127
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object _poolLock;
}
