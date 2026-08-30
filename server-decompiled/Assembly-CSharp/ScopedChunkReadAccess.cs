using System;

// Token: 0x02000CC7 RID: 3271
public struct ScopedChunkReadAccess : IDisposable
{
	// Token: 0x17000A74 RID: 2676
	// (get) Token: 0x06006451 RID: 25681 RVA: 0x00274548 File Offset: 0x00272748
	// (set) Token: 0x06006452 RID: 25682 RVA: 0x00274550 File Offset: 0x00272750
	public Chunk Chunk
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06006453 RID: 25683 RVA: 0x00274559 File Offset: 0x00272759
	public ScopedChunkReadAccess(Chunk chunk)
	{
		this.Chunk = chunk;
		Chunk chunk2 = this.Chunk;
		if (chunk2 == null)
		{
			return;
		}
		chunk2.EnterReadLock();
	}

	// Token: 0x06006454 RID: 25684 RVA: 0x00274572 File Offset: 0x00272772
	public void Dispose()
	{
		Chunk chunk = this.Chunk;
		if (chunk == null)
		{
			return;
		}
		chunk.ExitReadLock();
	}
}
