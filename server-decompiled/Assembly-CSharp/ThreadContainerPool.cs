using System;
using System.Collections.Generic;

// Token: 0x02000C55 RID: 3157
public class ThreadContainerPool
{
	// Token: 0x170009DB RID: 2523
	// (get) Token: 0x06005F83 RID: 24451 RVA: 0x0025F438 File Offset: 0x0025D638
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

	// Token: 0x06005F84 RID: 24452 RVA: 0x0025F480 File Offset: 0x0025D680
	public ThreadContainerPool(int initialCapacity, int initialCount)
	{
		this._poolLock = new object();
		this.pool = new List<ThreadContainer>(initialCapacity);
		for (int i = 0; i < initialCount; i++)
		{
			this.pool.Add(new ThreadContainer());
		}
	}

	// Token: 0x06005F85 RID: 24453 RVA: 0x0025F4C8 File Offset: 0x0025D6C8
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

	// Token: 0x06005F86 RID: 24454 RVA: 0x0025F55C File Offset: 0x0025D75C
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

	// Token: 0x04004B1A RID: 19226
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ThreadContainer> pool;

	// Token: 0x04004B1B RID: 19227
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object _poolLock;
}
