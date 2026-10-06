using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000A07 RID: 2567
public interface ISaveDataManager
{
	// Token: 0x06004C24 RID: 19492
	void Init();

	// Token: 0x06004C25 RID: 19493
	void Cleanup();

	// Token: 0x06004C26 RID: 19494
	SaveDataWriteMode GetWriteMode();

	// Token: 0x06004C27 RID: 19495
	void SetWriteMode(SaveDataWriteMode writeMode);

	// Token: 0x06004C28 RID: 19496
	void RegisterRegionFileManager(RegionFileManager regionFileManager);

	// Token: 0x06004C29 RID: 19497
	void DeregisterRegionFileManager(RegionFileManager regionFileManager);

	// Token: 0x17000847 RID: 2119
	// (get) Token: 0x06004C2A RID: 19498
	bool AppliesSaveSizeLimit { get; }

	// Token: 0x06004C2B RID: 19499
	int CommitAsync();

	// Token: 0x06004C2C RID: 19500
	bool IsCommitPending(int token);

	// Token: 0x06004C2D RID: 19501
	void CommitSync();

	// Token: 0x06004C2E RID: 19502
	IEnumerator CommitCoroutine(Action<double> progressFeedback = null);

	// Token: 0x14000078 RID: 120
	// (add) Token: 0x06004C2F RID: 19503
	// (remove) Token: 0x06004C30 RID: 19504
	event Action CommitStarted;

	// Token: 0x14000079 RID: 121
	// (add) Token: 0x06004C31 RID: 19505
	// (remove) Token: 0x06004C32 RID: 19506
	event Action CommitFinished;

	// Token: 0x06004C33 RID: 19507
	bool ShouldLimitSize();

	// Token: 0x06004C34 RID: 19508
	void UpdateSizes();

	// Token: 0x06004C35 RID: 19509
	SaveDataSizes GetSizes();

	// Token: 0x06004C36 RID: 19510
	Stream ManagedFileOpen(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share);

	// Token: 0x06004C37 RID: 19511
	void ManagedFileDelete(SaveDataManagedPath path);

	// Token: 0x06004C38 RID: 19512
	bool ManagedFileExists(SaveDataManagedPath path);

	// Token: 0x06004C39 RID: 19513
	DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path);

	// Token: 0x06004C3A RID: 19514
	SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path);

	// Token: 0x06004C3B RID: 19515
	DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path);

	// Token: 0x06004C3C RID: 19516
	bool ManagedDirectoryExists(SaveDataManagedPath path);

	// Token: 0x06004C3D RID: 19517
	IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004C3E RID: 19518
	IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004C3F RID: 19519
	IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004C40 RID: 19520
	void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive);

	// Token: 0x06004C41 RID: 19521
	long ManagedFileInfoLength(SaveDataManagedPath path);

	// Token: 0x06004C42 RID: 19522
	IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004C43 RID: 19523
	IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004C44 RID: 19524
	IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);
}
