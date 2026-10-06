using System;
using System.Collections.Generic;

// Token: 0x02000C46 RID: 3142
public class ThreadInfoParamPool
{
	// Token: 0x170009D2 RID: 2514
	// (get) Token: 0x06005F44 RID: 24388 RVA: 0x0025D6C4 File Offset: 0x0025B8C4
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

	// Token: 0x170009D3 RID: 2515
	// (get) Token: 0x06005F45 RID: 24389 RVA: 0x0025D70C File Offset: 0x0025B90C
	public int CountBig
	{
		get
		{
			object poolLock = this._poolLock;
			int count;
			lock (poolLock)
			{
				count = this.poolBig.Count;
			}
			return count;
		}
	}

	// Token: 0x06005F46 RID: 24390 RVA: 0x0025D754 File Offset: 0x0025B954
	public ThreadInfoParamPool(int initialCapacity, int initialCapacityBig, int initialCount)
	{
		this._poolLock = new object();
		this.pool = new List<ThreadInfoParam>(initialCapacity);
		this.poolBig = new List<ThreadInfoParam>(initialCapacityBig);
		for (int i = 0; i < initialCount; i++)
		{
			this.pool.Add(new ThreadInfoParam());
			this.poolBig.Add(new ThreadInfoParam());
		}
	}

	// Token: 0x06005F47 RID: 24391 RVA: 0x0025D7B8 File Offset: 0x0025B9B8
	public ThreadInfoParam GetObject(DistantChunkMap _CMap, int _ResLevel, int _OutId)
	{
		object poolLock = this._poolLock;
		ThreadInfoParam threadInfoParam;
		lock (poolLock)
		{
			if (this.pool.Count == 0)
			{
				threadInfoParam = new ThreadInfoParam(_CMap, _ResLevel, _OutId, false);
			}
			else
			{
				ThreadInfoParam threadInfoParam2 = this.pool[this.pool.Count - 1];
				this.pool.RemoveAt(this.pool.Count - 1);
				threadInfoParam2.Init(_CMap, _ResLevel, _OutId, false);
				threadInfoParam = threadInfoParam2;
			}
		}
		return threadInfoParam;
	}

	// Token: 0x06005F48 RID: 24392 RVA: 0x0025D848 File Offset: 0x0025BA48
	public ThreadInfoParam GetObjectBig(DistantChunkMap _CMap, int _ResLevel, int _OutId)
	{
		object poolLock = this._poolLock;
		ThreadInfoParam threadInfoParam;
		lock (poolLock)
		{
			if (this.poolBig.Count == 0)
			{
				threadInfoParam = new ThreadInfoParam(_CMap, _ResLevel, _OutId, true);
			}
			else
			{
				ThreadInfoParam threadInfoParam2 = this.poolBig[this.poolBig.Count - 1];
				this.poolBig.RemoveAt(this.poolBig.Count - 1);
				threadInfoParam2.Init(_CMap, _ResLevel, _OutId, true);
				threadInfoParam = threadInfoParam2;
			}
		}
		return threadInfoParam;
	}

	// Token: 0x06005F49 RID: 24393 RVA: 0x0025D8D8 File Offset: 0x0025BAD8
	public void ReturnObject(ThreadInfoParam item, ThreadContainerPool TmpThContPool = null)
	{
		if (item == null)
		{
			throw new ArgumentNullException("ThreadInfoParam is null");
		}
		item.ClearAll(TmpThContPool);
		object poolLock = this._poolLock;
		lock (poolLock)
		{
			if (item.IsBigCapacity)
			{
				if (this.poolBig.Contains(item))
				{
					throw new InvalidOperationException("ThreadInfoParam Big already in pool");
				}
				this.poolBig.Add(item);
			}
			else
			{
				if (this.pool.Contains(item))
				{
					throw new InvalidOperationException("ThreadInfoParam already in pool");
				}
				this.pool.Add(item);
			}
		}
	}

	// Token: 0x04004AE6 RID: 19174
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ThreadInfoParam> pool;

	// Token: 0x04004AE7 RID: 19175
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ThreadInfoParam> poolBig;

	// Token: 0x04004AE8 RID: 19176
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object _poolLock;
}
