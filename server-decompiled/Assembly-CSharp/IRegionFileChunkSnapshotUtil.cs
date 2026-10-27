using System;

// Token: 0x02000BB2 RID: 2994
public interface IRegionFileChunkSnapshotUtil
{
	// Token: 0x060059F6 RID: 23030
	IRegionFileChunkSnapshot TakeSnapshot(Chunk chunk, bool saveIfUnchanged);

	// Token: 0x060059F7 RID: 23031
	void WriteSnapshot(IRegionFileChunkSnapshot snapshot, string dir, int chunkX, int chunkZ);

	// Token: 0x060059F8 RID: 23032
	Chunk LoadChunk(string dir, long key);

	// Token: 0x060059F9 RID: 23033
	void Free(IRegionFileChunkSnapshot snapshot);

	// Token: 0x060059FA RID: 23034
	void Cleanup();
}
