using System;
using ConcurrentCollections;

// Token: 0x02000359 RID: 857
public class ChunkQueue
{
	// Token: 0x060018D8 RID: 6360 RVA: 0x0008C340 File Offset: 0x0008A540
	public void Add(long item)
	{
		object @lock = this._lock;
		lock (@lock)
		{
			this.KeyQueue.Add(item);
		}
	}

	// Token: 0x060018D9 RID: 6361 RVA: 0x0008C388 File Offset: 0x0008A588
	public void Clear()
	{
		this.KeyQueue.Clear();
	}

	// Token: 0x060018DA RID: 6362 RVA: 0x0008C395 File Offset: 0x0008A595
	public bool Contains(long item)
	{
		return this.KeyQueue.Contains(item);
	}

	// Token: 0x060018DB RID: 6363 RVA: 0x0008C3A3 File Offset: 0x0008A5A3
	public void Remove(long item)
	{
		this.KeyQueue.TryRemove(item);
	}

	// Token: 0x04000FD3 RID: 4051
	[PublicizedFrom(EAccessModifier.Private)]
	public object _lock = new object();

	// Token: 0x04000FD4 RID: 4052
	[PublicizedFrom(EAccessModifier.Private)]
	public ConcurrentHashSet<long> KeyQueue = new ConcurrentHashSet<long>();
}
