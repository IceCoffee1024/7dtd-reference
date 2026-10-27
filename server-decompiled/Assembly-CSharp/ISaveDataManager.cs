using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000A11 RID: 2577
public interface ISaveDataManager
{
	// Token: 0x06004C48 RID: 19528
	void Init();

	// Token: 0x06004C49 RID: 19529
	void Cleanup();

	// Token: 0x06004C4A RID: 19530
	SaveDataWriteMode GetWriteMode();

	// Token: 0x06004C4B RID: 19531
	void SetWriteMode(SaveDataWriteMode writeMode);

	// Token: 0x06004C4C RID: 19532
	void RegisterRegionFileManager(RegionFileManager regionFileManager);

	// Token: 0x06004C4D RID: 19533
	void DeregisterRegionFileManager(RegionFileManager regionFileManager);

	// Token: 0x1700084D RID: 2125
	// (get) Token: 0x06004C4E RID: 19534
	bool AppliesSaveSizeLimit { get; }

	// Token: 0x06004C4F RID: 19535
	int CommitAsync();

	// Token: 0x06004C50 RID: 19536
	bool IsCommitPending(int token);

	// Token: 0x06004C51 RID: 19537
	void CommitSync();

	// Token: 0x06004C52 RID: 19538
	IEnumerator CommitCoroutine(Action<double> progressFeedback = null);

	// Token: 0x14000078 RID: 120
	// (add) Token: 0x06004C53 RID: 19539
	// (remove) Token: 0x06004C54 RID: 19540
	event Action CommitStarted;

	// Token: 0x14000079 RID: 121
	// (add) Token: 0x06004C55 RID: 19541
	// (remove) Token: 0x06004C56 RID: 19542
	event Action CommitFinished;

	// Token: 0x06004C57 RID: 19543
	bool ShouldLimitSize();

	// Token: 0x06004C58 RID: 19544
	void UpdateSizes();

	// Token: 0x06004C59 RID: 19545
	SaveDataSizes GetSizes();

	// Token: 0x06004C5A RID: 19546
	Stream ManagedFileOpen(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share);

	// Token: 0x06004C5B RID: 19547
	void ManagedFileDelete(SaveDataManagedPath path);

	// Token: 0x06004C5C RID: 19548
	bool ManagedFileExists(SaveDataManagedPath path);

	// Token: 0x06004C5D RID: 19549
	DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path);

	// Token: 0x06004C5E RID: 19550
	SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path);

	// Token: 0x06004C5F RID: 19551
	DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path);

	// Token: 0x06004C60 RID: 19552
	bool ManagedDirectoryExists(SaveDataManagedPath path);

	// Token: 0x06004C61 RID: 19553
	IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004C62 RID: 19554
	IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004C63 RID: 19555
	IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004C64 RID: 19556
	void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive);

	// Token: 0x06004C65 RID: 19557
	long ManagedFileInfoLength(SaveDataManagedPath path);

	// Token: 0x06004C66 RID: 19558
	IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004C67 RID: 19559
	IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004C68 RID: 19560
	IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);
}
