using System;

// Token: 0x02000BB4 RID: 2996
public interface IRegionFilePlatformFactory
{
	// Token: 0x06005A34 RID: 23092
	RegionFileAccessAbstract CreateRegionFileAccess();

	// Token: 0x06005A35 RID: 23093
	IRegionFileChunkSnapshotUtil CreateSnapshotUtil(RegionFileAccessAbstract regionFileAccess);

	// Token: 0x06005A36 RID: 23094
	IRegionFileDebugUtil CreateDebugUtil();
}
