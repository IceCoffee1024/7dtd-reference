using System;

// Token: 0x02000BC2 RID: 3010
public class RegionFileFactoryRaw : IRegionFilePlatformFactory
{
	// Token: 0x06005A61 RID: 23137 RVA: 0x0022DCBD File Offset: 0x0022BEBD
	public RegionFileAccessAbstract CreateRegionFileAccess()
	{
		return new RegionFileAccessRaw();
	}

	// Token: 0x06005A62 RID: 23138 RVA: 0x0022DCAE File Offset: 0x0022BEAE
	public IRegionFileChunkSnapshotUtil CreateSnapshotUtil(RegionFileAccessAbstract regionFileAccess)
	{
		return new ChunkSnapshotUtil(regionFileAccess);
	}

	// Token: 0x06005A63 RID: 23139 RVA: 0x0022DCC4 File Offset: 0x0022BEC4
	public IRegionFileDebugUtil CreateDebugUtil()
	{
		return new RegionFileDebugUtilRaw();
	}
}
