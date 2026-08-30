using System;

// Token: 0x02000CDA RID: 3290
public struct ScopedChunkWriteAccess : IDisposable
{
	// Token: 0x17000A88 RID: 2696
	// (get) Token: 0x060064E1 RID: 25825 RVA: 0x00277480 File Offset: 0x00275680
	// (set) Token: 0x060064E2 RID: 25826 RVA: 0x00277488 File Offset: 0x00275688
	public Chunk Chunk
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060064E3 RID: 25827 RVA: 0x00277491 File Offset: 0x00275691
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

	// Token: 0x060064E4 RID: 25828 RVA: 0x002774AA File Offset: 0x002756AA
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
