using System;
using System.Collections.Generic;

// Token: 0x02000C4A RID: 3146
public class ThreadContainerPool
{
	// Token: 0x170009D5 RID: 2517
	// (get) Token: 0x06005F5E RID: 24414 RVA: 0x0025DF10 File Offset: 0x0025C110
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

	// Token: 0x06005F5F RID: 24415 RVA: 0x0025DF58 File Offset: 0x0025C158
	public ThreadContainerPool(int initialCapacity, int initialCount)
	{
		this._poolLock = new object();
		this.pool = new List<ThreadContainer>(initialCapacity);
		for (int i = 0; i < initialCount; i++)
		{
			this.pool.Add(new ThreadContainer());
		}
	}

	// Token: 0x06005F60 RID: 24416 RVA: 0x0025DFA0 File Offset: 0x0025C1A0
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

	// Token: 0x06005F61 RID: 24417 RVA: 0x0025E034 File Offset: 0x0025C234
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

	// Token: 0x04004AF5 RID: 19189
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ThreadContainer> pool;

	// Token: 0x04004AF6 RID: 19190
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object _poolLock;
}
