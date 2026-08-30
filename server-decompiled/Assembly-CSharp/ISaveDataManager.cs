using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// Token: 0x020009EC RID: 2540
public interface ISaveDataManager
{
	// Token: 0x06004BCF RID: 19407
	void Init();

	// Token: 0x06004BD0 RID: 19408
	void Cleanup();

	// Token: 0x06004BD1 RID: 19409
	SaveDataWriteMode GetWriteMode();

	// Token: 0x06004BD2 RID: 19410
	void SetWriteMode(SaveDataWriteMode writeMode);

	// Token: 0x06004BD3 RID: 19411
	void RegisterRegionFileManager(RegionFileManager regionFileManager);

	// Token: 0x06004BD4 RID: 19412
	void DeregisterRegionFileManager(RegionFileManager regionFileManager);

	// Token: 0x17000815 RID: 2069
	// (get) Token: 0x06004BD5 RID: 19413
	bool AppliesSaveSizeLimit { get; }

	// Token: 0x06004BD6 RID: 19414
	int CommitAsync();

	// Token: 0x06004BD7 RID: 19415
	bool IsCommitPending(int token);

	// Token: 0x06004BD8 RID: 19416
	void CommitSync();

	// Token: 0x06004BD9 RID: 19417
	IEnumerator CommitCoroutine(Action<double> progressFeedback = null);

	// Token: 0x14000077 RID: 119
	// (add) Token: 0x06004BDA RID: 19418
	// (remove) Token: 0x06004BDB RID: 19419
	event Action CommitStarted;

	// Token: 0x14000078 RID: 120
	// (add) Token: 0x06004BDC RID: 19420
	// (remove) Token: 0x06004BDD RID: 19421
	event Action CommitFinished;

	// Token: 0x06004BDE RID: 19422
	bool ShouldLimitSize();

	// Token: 0x06004BDF RID: 19423
	void UpdateSizes();

	// Token: 0x06004BE0 RID: 19424
	SaveDataSizes GetSizes();

	// Token: 0x06004BE1 RID: 19425
	Stream ManagedFileOpen(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share);

	// Token: 0x06004BE2 RID: 19426
	void ManagedFileDelete(SaveDataManagedPath path);

	// Token: 0x06004BE3 RID: 19427
	bool ManagedFileExists(SaveDataManagedPath path);

	// Token: 0x06004BE4 RID: 19428
	DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path);

	// Token: 0x06004BE5 RID: 19429
	SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path);

	// Token: 0x06004BE6 RID: 19430
	DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path);

	// Token: 0x06004BE7 RID: 19431
	bool ManagedDirectoryExists(SaveDataManagedPath path);

	// Token: 0x06004BE8 RID: 19432
	IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004BE9 RID: 19433
	IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004BEA RID: 19434
	IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004BEB RID: 19435
	void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive);

	// Token: 0x06004BEC RID: 19436
	long ManagedFileInfoLength(SaveDataManagedPath path);

	// Token: 0x06004BED RID: 19437
	IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004BEE RID: 19438
	IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004BEF RID: 19439
	IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);
}
