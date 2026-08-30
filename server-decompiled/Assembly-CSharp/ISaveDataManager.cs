using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// Token: 0x020009FD RID: 2557
public interface ISaveDataManager
{
	// Token: 0x06004C55 RID: 19541
	void Init();

	// Token: 0x06004C56 RID: 19542
	void Cleanup();

	// Token: 0x06004C57 RID: 19543
	SaveDataWriteMode GetWriteMode();

	// Token: 0x06004C58 RID: 19544
	void SetWriteMode(SaveDataWriteMode writeMode);

	// Token: 0x06004C59 RID: 19545
	void RegisterRegionFileManager(RegionFileManager regionFileManager);

	// Token: 0x06004C5A RID: 19546
	void DeregisterRegionFileManager(RegionFileManager regionFileManager);

	// Token: 0x17000827 RID: 2087
	// (get) Token: 0x06004C5B RID: 19547
	bool AppliesSaveSizeLimit { get; }

	// Token: 0x06004C5C RID: 19548
	int CommitAsync();

	// Token: 0x06004C5D RID: 19549
	bool IsCommitPending(int token);

	// Token: 0x06004C5E RID: 19550
	void CommitSync();

	// Token: 0x06004C5F RID: 19551
	IEnumerator CommitCoroutine(Action<double> progressFeedback = null);

	// Token: 0x14000077 RID: 119
	// (add) Token: 0x06004C60 RID: 19552
	// (remove) Token: 0x06004C61 RID: 19553
	event Action CommitStarted;

	// Token: 0x14000078 RID: 120
	// (add) Token: 0x06004C62 RID: 19554
	// (remove) Token: 0x06004C63 RID: 19555
	event Action CommitFinished;

	// Token: 0x06004C64 RID: 19556
	bool ShouldLimitSize();

	// Token: 0x06004C65 RID: 19557
	void UpdateSizes();

	// Token: 0x06004C66 RID: 19558
	SaveDataSizes GetSizes();

	// Token: 0x06004C67 RID: 19559
	Stream ManagedFileOpen(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share);

	// Token: 0x06004C68 RID: 19560
	void ManagedFileDelete(SaveDataManagedPath path);

	// Token: 0x06004C69 RID: 19561
	bool ManagedFileExists(SaveDataManagedPath path);

	// Token: 0x06004C6A RID: 19562
	DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path);

	// Token: 0x06004C6B RID: 19563
	SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path);

	// Token: 0x06004C6C RID: 19564
	DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path);

	// Token: 0x06004C6D RID: 19565
	bool ManagedDirectoryExists(SaveDataManagedPath path);

	// Token: 0x06004C6E RID: 19566
	IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004C6F RID: 19567
	IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004C70 RID: 19568
	IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004C71 RID: 19569
	void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive);

	// Token: 0x06004C72 RID: 19570
	long ManagedFileInfoLength(SaveDataManagedPath path);

	// Token: 0x06004C73 RID: 19571
	IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004C74 RID: 19572
	IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

	// Token: 0x06004C75 RID: 19573
	IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);
}
