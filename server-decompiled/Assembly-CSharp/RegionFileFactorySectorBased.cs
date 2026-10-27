using System;

// Token: 0x02000BC1 RID: 3009
public class RegionFileFactorySectorBased : IRegionFilePlatformFactory
{
	// Token: 0x06005A5D RID: 23133 RVA: 0x0022DCA7 File Offset: 0x0022BEA7
	public RegionFileAccessAbstract CreateRegionFileAccess()
	{
		return new RegionFileAccessSectorBased();
	}

	// Token: 0x06005A5E RID: 23134 RVA: 0x0022DCAE File Offset: 0x0022BEAE
	public IRegionFileChunkSnapshotUtil CreateSnapshotUtil(RegionFileAccessAbstract regionFileAccess)
	{
		return new ChunkSnapshotUtil(regionFileAccess);
	}

	// Token: 0x06005A5F RID: 23135 RVA: 0x0022DCB6 File Offset: 0x0022BEB6
	public IRegionFileDebugUtil CreateDebugUtil()
	{
		return new RegionFileDebugUtilSectorBased();
	}
}
