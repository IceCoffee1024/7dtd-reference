using System;

// Token: 0x02000B90 RID: 2960
public interface IRegionFilePlatformFactory
{
	// Token: 0x06005993 RID: 22931
	RegionFileAccessAbstract CreateRegionFileAccess();

	// Token: 0x06005994 RID: 22932
	IRegionFileChunkSnapshotUtil CreateSnapshotUtil(RegionFileAccessAbstract regionFileAccess);

	// Token: 0x06005995 RID: 22933
	IRegionFileDebugUtil CreateDebugUtil();
}
