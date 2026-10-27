using System;

// Token: 0x02000CF1 RID: 3313
public static class ScopedChunkAccess
{
	// Token: 0x060064F1 RID: 25841 RVA: 0x0027BCE4 File Offset: 0x00279EE4
	public static ScopedChunkReadAccess GetChunkReadAccess(ChunkCluster chunks, int x, int z)
	{
		return new ScopedChunkReadAccess(chunks.GetChunkSync(x, z));
	}

	// Token: 0x060064F2 RID: 25842 RVA: 0x0027BCF3 File Offset: 0x00279EF3
	public static ScopedChunkWriteAccess GetChunkWriteAccess(ChunkCluster chunks, int x, int z)
	{
		return new ScopedChunkWriteAccess(chunks.GetChunkSync(x, z));
	}

	// Token: 0x060064F3 RID: 25843 RVA: 0x0027BD02 File Offset: 0x00279F02
	public static ScopedChunkWriteAccess GetChunkWriteAccess(ChunkCluster chunks, long key)
	{
		return new ScopedChunkWriteAccess(chunks.GetChunkSync(key));
	}
}
