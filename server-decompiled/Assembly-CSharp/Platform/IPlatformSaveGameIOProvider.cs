using System;
using System.Collections.Generic;
using System.IO;

namespace Platform
{
	// Token: 0x02001B64 RID: 7012
	public interface IPlatformSaveGameIOProvider
	{
		// Token: 0x0600D153 RID: 53587
		void ManagedFileRead(SaveDataManagedPath path, Stream dest);

		// Token: 0x0600D154 RID: 53588
		void ManagedFileWrite(SaveDataManagedPath path, Stream src);

		// Token: 0x0600D155 RID: 53589
		void ManagedFileCopy(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName, bool overwrite = false);

		// Token: 0x0600D156 RID: 53590
		void ManagedFileDelete(SaveDataManagedPath path);

		// Token: 0x0600D157 RID: 53591
		bool ManagedFileExists(SaveDataManagedPath path);

		// Token: 0x0600D158 RID: 53592
		DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path);

		// Token: 0x0600D159 RID: 53593
		void ManagedFileMove(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName);

		// Token: 0x0600D15A RID: 53594
		SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path);

		// Token: 0x0600D15B RID: 53595
		DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path);

		// Token: 0x0600D15C RID: 53596
		bool ManagedDirectoryExists(SaveDataManagedPath path);

		// Token: 0x0600D15D RID: 53597
		IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D15E RID: 53598
		IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D15F RID: 53599
		IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D160 RID: 53600
		void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive);

		// Token: 0x0600D161 RID: 53601
		long ManagedFileInfoLength(SaveDataManagedPath path);

		// Token: 0x0600D162 RID: 53602
		IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D163 RID: 53603
		IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D164 RID: 53604
		IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);
	}
}
