using System;

// Token: 0x02000BA7 RID: 2983
public interface IRegionFileChunkSnapshotUtil
{
	// Token: 0x060059D1 RID: 22993
	IRegionFileChunkSnapshot TakeSnapshot(Chunk chunk, bool saveIfUnchanged);

	// Token: 0x060059D2 RID: 22994
	void WriteSnapshot(IRegionFileChunkSnapshot snapshot, string dir, int chunkX, int chunkZ);

	// Token: 0x060059D3 RID: 22995
	Chunk LoadChunk(string dir, long key);

	// Token: 0x060059D4 RID: 22996
	void Free(IRegionFileChunkSnapshot snapshot);

	// Token: 0x060059D5 RID: 22997
	void Cleanup();
}
