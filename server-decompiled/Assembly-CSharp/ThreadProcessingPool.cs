using System;
using System.Collections.Generic;

// Token: 0x02000C53 RID: 3155
public class ThreadProcessingPool
{
	// Token: 0x170009DA RID: 2522
	// (get) Token: 0x06005F79 RID: 24441 RVA: 0x0025F1EC File Offset: 0x0025D3EC
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

	// Token: 0x06005F7A RID: 24442 RVA: 0x0025F234 File Offset: 0x0025D434
	public ThreadProcessingPool(int initialCapacity, int initialCount)
	{
		this._poolLock = new object();
		this.pool = new List<ThreadProcessing>(initialCapacity);
		for (int i = 0; i < initialCount; i++)
		{
			this.pool.Add(new ThreadProcessing());
		}
	}

	// Token: 0x06005F7B RID: 24443 RVA: 0x0025F27C File Offset: 0x0025D47C
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

	// Token: 0x06005F7C RID: 24444 RVA: 0x0025F308 File Offset: 0x0025D508
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

	// Token: 0x04004B13 RID: 19219
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ThreadProcessing> pool;

	// Token: 0x04004B14 RID: 19220
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object _poolLock;
}
