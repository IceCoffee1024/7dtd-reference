using System;

// Token: 0x02000B82 RID: 2946
public interface IRegionFileChunkSnapshot
{
	// Token: 0x1700095D RID: 2397
	// (get) Token: 0x0600592B RID: 22827
	long Size { get; }

	// Token: 0x0600592C RID: 22828
	void Update(Chunk chunk, bool saveIfUnchanged);

	// Token: 0x0600592D RID: 22829
	void Write(RegionFileChunkWriter writer, string dir, int chunkX, int chunkZ);

	// Token: 0x0600592E RID: 22830
	void Reset();
}
