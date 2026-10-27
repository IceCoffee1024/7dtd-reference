using System;

// Token: 0x02000BBF RID: 3007
public interface IRegionFilePlatformFactory
{
	// Token: 0x06005A59 RID: 23129
	RegionFileAccessAbstract CreateRegionFileAccess();

	// Token: 0x06005A5A RID: 23130
	IRegionFileChunkSnapshotUtil CreateSnapshotUtil(RegionFileAccessAbstract regionFileAccess);

	// Token: 0x06005A5B RID: 23131
	IRegionFileDebugUtil CreateDebugUtil();
}
