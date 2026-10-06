using System;
using System.Collections.Generic;

// Token: 0x02000C48 RID: 3144
public class ThreadProcessingPool
{
	// Token: 0x170009D4 RID: 2516
	// (get) Token: 0x06005F54 RID: 24404 RVA: 0x0025DCC4 File Offset: 0x0025BEC4
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

	// Token: 0x06005F55 RID: 24405 RVA: 0x0025DD0C File Offset: 0x0025BF0C
	public ThreadProcessingPool(int initialCapacity, int initialCount)
	{
		this._poolLock = new object();
		this.pool = new List<ThreadProcessing>(initialCapacity);
		for (int i = 0; i < initialCount; i++)
		{
			this.pool.Add(new ThreadProcessing());
		}
	}

	// Token: 0x06005F56 RID: 24406 RVA: 0x0025DD54 File Offset: 0x0025BF54
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

	// Token: 0x06005F57 RID: 24407 RVA: 0x0025DDE0 File Offset: 0x0025BFE0
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

	// Token: 0x04004AEE RID: 19182
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ThreadProcessing> pool;

	// Token: 0x04004AEF RID: 19183
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object _poolLock;
}
