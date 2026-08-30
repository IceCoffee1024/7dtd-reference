using System;
using System.Collections.Generic;

// Token: 0x02000C34 RID: 3124
public class ThreadInfoParamPool
{
	// Token: 0x170009B0 RID: 2480
	// (get) Token: 0x06005F2B RID: 24363 RVA: 0x0025A3C8 File Offset: 0x002585C8
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

	// Token: 0x170009B1 RID: 2481
	// (get) Token: 0x06005F2C RID: 24364 RVA: 0x0025A410 File Offset: 0x00258610
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

	// Token: 0x06005F2D RID: 24365 RVA: 0x0025A458 File Offset: 0x00258658
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

	// Token: 0x06005F2E RID: 24366 RVA: 0x0025A4BC File Offset: 0x002586BC
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

	// Token: 0x06005F2F RID: 24367 RVA: 0x0025A54C File Offset: 0x0025874C
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

	// Token: 0x06005F30 RID: 24368 RVA: 0x0025A5DC File Offset: 0x002587DC
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

	// Token: 0x04004AA7 RID: 19111
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ThreadInfoParam> pool;

	// Token: 0x04004AA8 RID: 19112
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ThreadInfoParam> poolBig;

	// Token: 0x04004AA9 RID: 19113
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object _poolLock;
}
