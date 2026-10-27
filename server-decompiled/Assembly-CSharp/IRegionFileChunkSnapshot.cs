using System;

// Token: 0x02000BB1 RID: 2993
public interface IRegionFileChunkSnapshot
{
	// Token: 0x17000997 RID: 2455
	// (get) Token: 0x060059F2 RID: 23026
	long Size { get; }

	// Token: 0x060059F3 RID: 23027
	void Update(Chunk chunk, bool saveIfUnchanged);

	// Token: 0x060059F4 RID: 23028
	void Write(RegionFileChunkWriter writer, string dir, int chunkX, int chunkZ);

	// Token: 0x060059F5 RID: 23029
	void Reset();
}
