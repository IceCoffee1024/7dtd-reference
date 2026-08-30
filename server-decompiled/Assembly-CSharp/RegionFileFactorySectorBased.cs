using System;

// Token: 0x02000B92 RID: 2962
public class RegionFileFactorySectorBased : IRegionFilePlatformFactory
{
	// Token: 0x06005997 RID: 22935 RVA: 0x00226733 File Offset: 0x00224933
	public RegionFileAccessAbstract CreateRegionFileAccess()
	{
		return new RegionFileAccessSectorBased();
	}

	// Token: 0x06005998 RID: 22936 RVA: 0x0022673A File Offset: 0x0022493A
	public IRegionFileChunkSnapshotUtil CreateSnapshotUtil(RegionFileAccessAbstract regionFileAccess)
	{
		return new ChunkSnapshotUtil(regionFileAccess);
	}

	// Token: 0x06005999 RID: 22937 RVA: 0x00226742 File Offset: 0x00224942
	public IRegionFileDebugUtil CreateDebugUtil()
	{
		return new RegionFileDebugUtilSectorBased();
	}
}
