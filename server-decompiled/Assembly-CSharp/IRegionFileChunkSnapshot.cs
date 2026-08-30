using System;

// Token: 0x02000B94 RID: 2964
public interface IRegionFileChunkSnapshot
{
	// Token: 0x1700096F RID: 2415
	// (get) Token: 0x060059B3 RID: 22963
	long Size { get; }

	// Token: 0x060059B4 RID: 22964
	void Update(Chunk chunk, bool saveIfUnchanged);

	// Token: 0x060059B5 RID: 22965
	void Write(RegionFileChunkWriter writer, string dir, int chunkX, int chunkZ);

	// Token: 0x060059B6 RID: 22966
	void Reset();
}
