using System;
using System.Collections.Generic;

// Token: 0x02000C26 RID: 3110
public class ThreadContainerPool
{
	// Token: 0x170009A0 RID: 2464
	// (get) Token: 0x06005EBB RID: 24251 RVA: 0x00257D64 File Offset: 0x00255F64
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

	// Token: 0x06005EBC RID: 24252 RVA: 0x00257DAC File Offset: 0x00255FAC
	public ThreadContainerPool(int initialCapacity, int initialCount)
	{
		this._poolLock = new object();
		this.pool = new List<ThreadContainer>(initialCapacity);
		for (int i = 0; i < initialCount; i++)
		{
			this.pool.Add(new ThreadContainer());
		}
	}

	// Token: 0x06005EBD RID: 24253 RVA: 0x00257DF4 File Offset: 0x00255FF4
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

	// Token: 0x06005EBE RID: 24254 RVA: 0x00257E88 File Offset: 0x00256088
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

	// Token: 0x04004A19 RID: 18969
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ThreadContainer> pool;

	// Token: 0x04004A1A RID: 18970
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object _poolLock;
}
