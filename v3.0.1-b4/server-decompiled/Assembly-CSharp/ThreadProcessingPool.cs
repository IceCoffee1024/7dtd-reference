using System;
using System.Collections.Generic;

// Token: 0x02000C24 RID: 3108
public class ThreadProcessingPool
{
	// Token: 0x1700099F RID: 2463
	// (get) Token: 0x06005EB1 RID: 24241 RVA: 0x00257B18 File Offset: 0x00255D18
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

	// Token: 0x06005EB2 RID: 24242 RVA: 0x00257B60 File Offset: 0x00255D60
	public ThreadProcessingPool(int initialCapacity, int initialCount)
	{
		this._poolLock = new object();
		this.pool = new List<ThreadProcessing>(initialCapacity);
		for (int i = 0; i < initialCount; i++)
		{
			this.pool.Add(new ThreadProcessing());
		}
	}

	// Token: 0x06005EB3 RID: 24243 RVA: 0x00257BA8 File Offset: 0x00255DA8
	public ThreadProcessing GetObject(List<ThreadInfoParam> _JobList)
	{
		object poolLock = this._poolLock;
		ThreadProcessing threadProcessing;
		lock (poolLock)
		{
			if (this.pool.Count == 0)
			{
				threadProcessing = new ThreadProcessing(_JobList);
			}
			else
			{
				ThreadProcessing threadProcessing2 = this.pool[this.pool.Count - 1];
				this.pool.RemoveAt(this.pool.Count - 1);
				threadProcessing2.Init(_JobList);
				threadProcessing = threadProcessing2;
			}
		}
		return threadProcessing;
	}

	// Token: 0x06005EB4 RID: 24244 RVA: 0x00257C34 File Offset: 0x00255E34
	public void ReturnObject(ThreadProcessing item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("ThreadProcessing is null");
		}
		object poolLock = this._poolLock;
		lock (poolLock)
		{
			if (this.pool.Contains(item))
			{
				throw new InvalidOperationException("ThreadProcessing already in pool");
			}
			this.pool.Add(item);
		}
	}

	// Token: 0x04004A12 RID: 18962
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ThreadProcessing> pool;

	// Token: 0x04004A13 RID: 18963
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object _poolLock;
}
