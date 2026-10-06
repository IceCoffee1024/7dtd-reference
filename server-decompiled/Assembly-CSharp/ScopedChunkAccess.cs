using System;

// Token: 0x02000CE6 RID: 3302
public static class ScopedChunkAccess
{
	// Token: 0x060064CC RID: 25804 RVA: 0x0027A7BC File Offset: 0x002789BC
	public static ScopedChunkReadAccess GetChunkReadAccess(ChunkCluster chunks, int x, int z)
	{
		return new ScopedChunkReadAccess(chunks.GetChunkSync(x, z));
	}

	// Token: 0x060064CD RID: 25805 RVA: 0x0027A7CB File Offset: 0x002789CB
	public static ScopedChunkWriteAccess GetChunkWriteAccess(ChunkCluster chunks, int x, int z)
	{
		return new ScopedChunkWriteAccess(chunks.GetChunkSync(x, z));
	}

	// Token: 0x060064CE RID: 25806 RVA: 0x0027A7DA File Offset: 0x002789DA
	public static ScopedChunkWriteAccess GetChunkWriteAccess(ChunkCluster chunks, long key)
	{
		return new ScopedChunkWriteAccess(chunks.GetChunkSync(key));
	}
}
