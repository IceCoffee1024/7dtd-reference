using System;
using System.Collections.Generic;

// Token: 0x02000C36 RID: 3126
public class ThreadProcessingPool
{
	// Token: 0x170009B2 RID: 2482
	// (get) Token: 0x06005F3B RID: 24379 RVA: 0x0025A9C8 File Offset: 0x00258BC8
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

	// Token: 0x06005F3C RID: 24380 RVA: 0x0025AA10 File Offset: 0x00258C10
	public ThreadProcessingPool(int initialCapacity, int initialCount)
	{
		this._poolLock = new object();
		this.pool = new List<ThreadProcessing>(initialCapacity);
		for (int i = 0; i < initialCount; i++)
		{
			this.pool.Add(new ThreadProcessing());
		}
	}

	// Token: 0x06005F3D RID: 24381 RVA: 0x0025AA58 File Offset: 0x00258C58
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

	// Token: 0x06005F3E RID: 24382 RVA: 0x0025AAE4 File Offset: 0x00258CE4
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

	// Token: 0x04004AAF RID: 19119
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ThreadProcessing> pool;

	// Token: 0x04004AB0 RID: 19120
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object _poolLock;
}
