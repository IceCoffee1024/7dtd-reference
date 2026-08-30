using System;

// Token: 0x02000BA4 RID: 2980
public class RegionFileFactorySectorBased : IRegionFilePlatformFactory
{
	// Token: 0x06005A1F RID: 23071 RVA: 0x0022944F File Offset: 0x0022764F
	public RegionFileAccessAbstract CreateRegionFileAccess()
	{
		return new RegionFileAccessSectorBased();
	}

	// Token: 0x06005A20 RID: 23072 RVA: 0x00229456 File Offset: 0x00227656
	public IRegionFileChunkSnapshotUtil CreateSnapshotUtil(RegionFileAccessAbstract regionFileAccess)
	{
		return new ChunkSnapshotUtil(regionFileAccess);
	}

	// Token: 0x06005A21 RID: 23073 RVA: 0x0022945E File Offset: 0x0022765E
	public IRegionFileDebugUtil CreateDebugUtil()
	{
		return new RegionFileDebugUtilSectorBased();
	}
}
