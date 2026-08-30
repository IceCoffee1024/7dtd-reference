using System;

// Token: 0x02000BA5 RID: 2981
public class RegionFileFactoryRaw : IRegionFilePlatformFactory
{
	// Token: 0x06005A23 RID: 23075 RVA: 0x00229465 File Offset: 0x00227665
	public RegionFileAccessAbstract CreateRegionFileAccess()
	{
		return new RegionFileAccessRaw();
	}

	// Token: 0x06005A24 RID: 23076 RVA: 0x00229456 File Offset: 0x00227656
	public IRegionFileChunkSnapshotUtil CreateSnapshotUtil(RegionFileAccessAbstract regionFileAccess)
	{
		return new ChunkSnapshotUtil(regionFileAccess);
	}

	// Token: 0x06005A25 RID: 23077 RVA: 0x0022946C File Offset: 0x0022766C
	public IRegionFileDebugUtil CreateDebugUtil()
	{
		return new RegionFileDebugUtilRaw();
	}
}
