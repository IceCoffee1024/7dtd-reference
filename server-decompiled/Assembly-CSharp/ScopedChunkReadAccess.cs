using System;

// Token: 0x02000CF2 RID: 3314
public struct ScopedChunkReadAccess : IDisposable
{
	// Token: 0x17000AA3 RID: 2723
	// (get) Token: 0x060064F4 RID: 25844 RVA: 0x0027BD10 File Offset: 0x00279F10
	// (set) Token: 0x060064F5 RID: 25845 RVA: 0x0027BD18 File Offset: 0x00279F18
	public Chunk Chunk
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060064F6 RID: 25846 RVA: 0x0027BD21 File Offset: 0x00279F21
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

	// Token: 0x060064F7 RID: 25847 RVA: 0x0027BD3A File Offset: 0x00279F3A
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
