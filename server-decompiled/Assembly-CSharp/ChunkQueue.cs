using System;
using ConcurrentCollections;

// Token: 0x02000360 RID: 864
public class ChunkQueue
{
	// Token: 0x060018F0 RID: 6384 RVA: 0x0008CD00 File Offset: 0x0008AF00
	public void Add(long item)
	{
		object @lock = this._lock;
		lock (@lock)
		{
			this.KeyQueue.Add(item);
		}
	}

	// Token: 0x060018F1 RID: 6385 RVA: 0x0008CD48 File Offset: 0x0008AF48
	public void Clear()
	{
		this.KeyQueue.Clear();
	}

	// Token: 0x060018F2 RID: 6386 RVA: 0x0008CD55 File Offset: 0x0008AF55
	public bool Contains(long item)
	{
		return this.KeyQueue.Contains(item);
	}

	// Token: 0x060018F3 RID: 6387 RVA: 0x0008CD63 File Offset: 0x0008AF63
	public void Remove(long item)
	{
		this.KeyQueue.TryRemove(item);
	}

	// Token: 0x04000FE1 RID: 4065
	[PublicizedFrom(EAccessModifier.Private)]
	public object _lock = new object();

	// Token: 0x04000FE2 RID: 4066
	[PublicizedFrom(EAccessModifier.Private)]
	public ConcurrentHashSet<long> KeyQueue = new ConcurrentHashSet<long>();
}
