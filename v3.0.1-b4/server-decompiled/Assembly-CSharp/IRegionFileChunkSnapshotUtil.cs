using System;

// Token: 0x02000B83 RID: 2947
public interface IRegionFileChunkSnapshotUtil
{
	// Token: 0x0600592F RID: 22831
	IRegionFileChunkSnapshot TakeSnapshot(Chunk chunk, bool saveIfUnchanged);

	// Token: 0x06005930 RID: 22832
	void WriteSnapshot(IRegionFileChunkSnapshot snapshot, string dir, int chunkX, int chunkZ);

	// Token: 0x06005931 RID: 22833
	Chunk LoadChunk(string dir, long key);

	// Token: 0x06005932 RID: 22834
	void Free(IRegionFileChunkSnapshot snapshot);

	// Token: 0x06005933 RID: 22835
	void Cleanup();
}
