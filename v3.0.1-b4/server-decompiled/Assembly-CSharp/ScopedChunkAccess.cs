using System;

// Token: 0x02000CC6 RID: 3270
public static class ScopedChunkAccess
{
	// Token: 0x0600644E RID: 25678 RVA: 0x0027451C File Offset: 0x0027271C
	public static ScopedChunkReadAccess GetChunkReadAccess(ChunkCluster chunks, int x, int z)
	{
		return new ScopedChunkReadAccess(chunks.GetChunkSync(x, z));
	}

	// Token: 0x0600644F RID: 25679 RVA: 0x0027452B File Offset: 0x0027272B
	public static ScopedChunkWriteAccess GetChunkWriteAccess(ChunkCluster chunks, int x, int z)
	{
		return new ScopedChunkWriteAccess(chunks.GetChunkSync(x, z));
	}

	// Token: 0x06006450 RID: 25680 RVA: 0x0027453A File Offset: 0x0027273A
	public static ScopedChunkWriteAccess GetChunkWriteAccess(ChunkCluster chunks, long key)
	{
		return new ScopedChunkWriteAccess(chunks.GetChunkSync(key));
	}
}
