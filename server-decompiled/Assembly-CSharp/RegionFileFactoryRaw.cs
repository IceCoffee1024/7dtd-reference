using System;

// Token: 0x02000BB7 RID: 2999
public class RegionFileFactoryRaw : IRegionFilePlatformFactory
{
	// Token: 0x06005A3C RID: 23100 RVA: 0x0022C761 File Offset: 0x0022A961
	public RegionFileAccessAbstract CreateRegionFileAccess()
	{
		return new RegionFileAccessRaw();
	}

	// Token: 0x06005A3D RID: 23101 RVA: 0x0022C752 File Offset: 0x0022A952
	public IRegionFileChunkSnapshotUtil CreateSnapshotUtil(RegionFileAccessAbstract regionFileAccess)
	{
		return new ChunkSnapshotUtil(regionFileAccess);
	}

	// Token: 0x06005A3E RID: 23102 RVA: 0x0022C768 File Offset: 0x0022A968
	public IRegionFileDebugUtil CreateDebugUtil()
	{
		return new RegionFileDebugUtilRaw();
	}
}
