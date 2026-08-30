using System;

// Token: 0x02000B95 RID: 2965
public interface IRegionFileChunkSnapshotUtil
{
	// Token: 0x060059B7 RID: 22967
	IRegionFileChunkSnapshot TakeSnapshot(Chunk chunk, bool saveIfUnchanged);

	// Token: 0x060059B8 RID: 22968
	void WriteSnapshot(IRegionFileChunkSnapshot snapshot, string dir, int chunkX, int chunkZ);

	// Token: 0x060059B9 RID: 22969
	Chunk LoadChunk(string dir, long key);

	// Token: 0x060059BA RID: 22970
	void Free(IRegionFileChunkSnapshot snapshot);

	// Token: 0x060059BB RID: 22971
	void Cleanup();
}
