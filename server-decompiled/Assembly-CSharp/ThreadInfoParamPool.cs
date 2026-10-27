using System;
using System.Collections.Generic;

// Token: 0x02000C51 RID: 3153
public class ThreadInfoParamPool
{
	// Token: 0x170009D8 RID: 2520
	// (get) Token: 0x06005F69 RID: 24425 RVA: 0x0025EBEC File Offset: 0x0025CDEC
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

	// Token: 0x170009D9 RID: 2521
	// (get) Token: 0x06005F6A RID: 24426 RVA: 0x0025EC34 File Offset: 0x0025CE34
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

	// Token: 0x06005F6B RID: 24427 RVA: 0x0025EC7C File Offset: 0x0025CE7C
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

	// Token: 0x06005F6C RID: 24428 RVA: 0x0025ECE0 File Offset: 0x0025CEE0
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

	// Token: 0x06005F6D RID: 24429 RVA: 0x0025ED70 File Offset: 0x0025CF70
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

	// Token: 0x06005F6E RID: 24430 RVA: 0x0025EE00 File Offset: 0x0025D000
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

	// Token: 0x04004B0B RID: 19211
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ThreadInfoParam> pool;

	// Token: 0x04004B0C RID: 19212
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ThreadInfoParam> poolBig;

	// Token: 0x04004B0D RID: 19213
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object _poolLock;
}
