using System;

// Token: 0x02000BA6 RID: 2982
public interface IRegionFileChunkSnapshot
{
	// Token: 0x17000991 RID: 2449
	// (get) Token: 0x060059CD RID: 22989
	long Size { get; }

	// Token: 0x060059CE RID: 22990
	void Update(Chunk chunk, bool saveIfUnchanged);

	// Token: 0x060059CF RID: 22991
	void Write(RegionFileChunkWriter writer, string dir, int chunkX, int chunkZ);

	// Token: 0x060059D0 RID: 22992
	void Reset();
}
