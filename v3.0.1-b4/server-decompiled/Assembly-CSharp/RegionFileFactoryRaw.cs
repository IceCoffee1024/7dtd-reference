using System;

// Token: 0x02000B93 RID: 2963
public class RegionFileFactoryRaw : IRegionFilePlatformFactory
{
	// Token: 0x0600599B RID: 22939 RVA: 0x00226749 File Offset: 0x00224949
	public RegionFileAccessAbstract CreateRegionFileAccess()
	{
		return new RegionFileAccessRaw();
	}

	// Token: 0x0600599C RID: 22940 RVA: 0x0022673A File Offset: 0x0022493A
	public IRegionFileChunkSnapshotUtil CreateSnapshotUtil(RegionFileAccessAbstract regionFileAccess)
	{
		return new ChunkSnapshotUtil(regionFileAccess);
	}

	// Token: 0x0600599D RID: 22941 RVA: 0x00226750 File Offset: 0x00224950
	public IRegionFileDebugUtil CreateDebugUtil()
	{
		return new RegionFileDebugUtilRaw();
	}
}
