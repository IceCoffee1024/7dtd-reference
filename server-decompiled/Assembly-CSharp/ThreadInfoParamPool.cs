using System;
using System.Collections.Generic;

// Token: 0x02000C22 RID: 3106
public class ThreadInfoParamPool
{
	// Token: 0x1700099D RID: 2461
	// (get) Token: 0x06005EA1 RID: 24225 RVA: 0x00257518 File Offset: 0x00255718
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

	// Token: 0x1700099E RID: 2462
	// (get) Token: 0x06005EA2 RID: 24226 RVA: 0x00257560 File Offset: 0x00255760
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

	// Token: 0x06005EA3 RID: 24227 RVA: 0x002575A8 File Offset: 0x002557A8
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

	// Token: 0x06005EA4 RID: 24228 RVA: 0x0025760C File Offset: 0x0025580C
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

	// Token: 0x06005EA5 RID: 24229 RVA: 0x0025769C File Offset: 0x0025589C
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

	// Token: 0x06005EA6 RID: 24230 RVA: 0x0025772C File Offset: 0x0025592C
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

	// Token: 0x04004A0A RID: 18954
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ThreadInfoParam> pool;

	// Token: 0x04004A0B RID: 18955
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ThreadInfoParam> poolBig;

	// Token: 0x04004A0C RID: 18956
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object _poolLock;
}
