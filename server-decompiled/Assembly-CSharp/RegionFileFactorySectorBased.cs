using System;

// Token: 0x02000BB6 RID: 2998
public class RegionFileFactorySectorBased : IRegionFilePlatformFactory
{
	// Token: 0x06005A38 RID: 23096 RVA: 0x0022C74B File Offset: 0x0022A94B
	public RegionFileAccessAbstract CreateRegionFileAccess()
	{
		return new RegionFileAccessSectorBased();
	}

	// Token: 0x06005A39 RID: 23097 RVA: 0x0022C752 File Offset: 0x0022A952
	public IRegionFileChunkSnapshotUtil CreateSnapshotUtil(RegionFileAccessAbstract regionFileAccess)
	{
		return new ChunkSnapshotUtil(regionFileAccess);
	}

	// Token: 0x06005A3A RID: 23098 RVA: 0x0022C75A File Offset: 0x0022A95A
	public IRegionFileDebugUtil CreateDebugUtil()
	{
		return new RegionFileDebugUtilSectorBased();
	}
}
