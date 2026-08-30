using System;

// Token: 0x02000CD8 RID: 3288
public static class ScopedChunkAccess
{
	// Token: 0x060064DA RID: 25818 RVA: 0x00277418 File Offset: 0x00275618
	public static ScopedChunkReadAccess GetChunkReadAccess(ChunkCluster chunks, int x, int z)
	{
		return new ScopedChunkReadAccess(chunks.GetChunkSync(x, z));
	}

	// Token: 0x060064DB RID: 25819 RVA: 0x00277427 File Offset: 0x00275627
	public static ScopedChunkWriteAccess GetChunkWriteAccess(ChunkCluster chunks, int x, int z)
	{
		return new ScopedChunkWriteAccess(chunks.GetChunkSync(x, z));
	}

	// Token: 0x060064DC RID: 25820 RVA: 0x00277436 File Offset: 0x00275636
	public static ScopedChunkWriteAccess GetChunkWriteAccess(ChunkCluster chunks, long key)
	{
		return new ScopedChunkWriteAccess(chunks.GetChunkSync(key));
	}
}
