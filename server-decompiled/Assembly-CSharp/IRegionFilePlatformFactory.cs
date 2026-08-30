using System;

// Token: 0x02000BA2 RID: 2978
public interface IRegionFilePlatformFactory
{
	// Token: 0x06005A1B RID: 23067
	RegionFileAccessAbstract CreateRegionFileAccess();

	// Token: 0x06005A1C RID: 23068
	IRegionFileChunkSnapshotUtil CreateSnapshotUtil(RegionFileAccessAbstract regionFileAccess);

	// Token: 0x06005A1D RID: 23069
	IRegionFileDebugUtil CreateDebugUtil();
}
