using System;

// Token: 0x02000CC8 RID: 3272
public struct ScopedChunkWriteAccess : IDisposable
{
	// Token: 0x17000A75 RID: 2677
	// (get) Token: 0x06006455 RID: 25685 RVA: 0x00274584 File Offset: 0x00272784
	// (set) Token: 0x06006456 RID: 25686 RVA: 0x0027458C File Offset: 0x0027278C
	public Chunk Chunk
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06006457 RID: 25687 RVA: 0x00274595 File Offset: 0x00272795
	public ScopedChunkWriteAccess(Chunk chunk)
	{
		this.Chunk = chunk;
		Chunk chunk2 = this.Chunk;
		if (chunk2 == null)
		{
			return;
		}
		chunk2.EnterWriteLock();
	}

	// Token: 0x06006458 RID: 25688 RVA: 0x002745AE File Offset: 0x002727AE
	public void Dispose()
	{
		Chunk chunk = this.Chunk;
		if (chunk == null)
		{
			return;
		}
		chunk.ExitWriteLock();
	}
}
