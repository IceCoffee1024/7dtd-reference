using System;

// Token: 0x02000CD9 RID: 3289
public struct ScopedChunkReadAccess : IDisposable
{
	// Token: 0x17000A87 RID: 2695
	// (get) Token: 0x060064DD RID: 25821 RVA: 0x00277444 File Offset: 0x00275644
	// (set) Token: 0x060064DE RID: 25822 RVA: 0x0027744C File Offset: 0x0027564C
	public Chunk Chunk
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060064DF RID: 25823 RVA: 0x00277455 File Offset: 0x00275655
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

	// Token: 0x060064E0 RID: 25824 RVA: 0x0027746E File Offset: 0x0027566E
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
