using System;
using ConcurrentCollections;

// Token: 0x02000362 RID: 866
public class ChunkQueue
{
	// Token: 0x060018F4 RID: 6388 RVA: 0x0008CF34 File Offset: 0x0008B134
	public void Add(long item)
	{
		object @lock = this._lock;
		lock (@lock)
		{
			this.KeyQueue.Add(item);
		}
	}

	// Token: 0x060018F5 RID: 6389 RVA: 0x0008CF7C File Offset: 0x0008B17C
	public void Clear()
	{
		this.KeyQueue.Clear();
	}

	// Token: 0x060018F6 RID: 6390 RVA: 0x0008CF89 File Offset: 0x0008B189
	public bool Contains(long item)
	{
		return this.KeyQueue.Contains(item);
	}

	// Token: 0x060018F7 RID: 6391 RVA: 0x0008CF97 File Offset: 0x0008B197
	public void Remove(long item)
	{
		this.KeyQueue.TryRemove(item);
	}

	// Token: 0x04000FF1 RID: 4081
	[PublicizedFrom(EAccessModifier.Private)]
	public object _lock = new object();

	// Token: 0x04000FF2 RID: 4082
	[PublicizedFrom(EAccessModifier.Private)]
	public ConcurrentHashSet<long> KeyQueue = new ConcurrentHashSet<long>();
}
