using System;

// Token: 0x02000CF3 RID: 3315
public struct ScopedChunkWriteAccess : IDisposable
{
	// Token: 0x17000AA4 RID: 2724
	// (get) Token: 0x060064F8 RID: 25848 RVA: 0x0027BD4C File Offset: 0x00279F4C
	// (set) Token: 0x060064F9 RID: 25849 RVA: 0x0027BD54 File Offset: 0x00279F54
	public Chunk Chunk
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060064FA RID: 25850 RVA: 0x0027BD5D File Offset: 0x00279F5D
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

	// Token: 0x060064FB RID: 25851 RVA: 0x0027BD76 File Offset: 0x00279F76
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
