using System;
using System.Collections.Generic;
using System.IO;

namespace Platform
{
	// Token: 0x02001BBD RID: 7101
	public interface IPlatformSaveGameIOProvider
	{
		// Token: 0x0600D456 RID: 54358
		void ManagedFileRead(SaveDataManagedPath path, Stream dest);

		// Token: 0x0600D457 RID: 54359
		void ManagedFileWrite(SaveDataManagedPath path, Stream src);

		// Token: 0x0600D458 RID: 54360
		void ManagedFileCopy(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName, bool overwrite = false);

		// Token: 0x0600D459 RID: 54361
		void ManagedFileDelete(SaveDataManagedPath path);

		// Token: 0x0600D45A RID: 54362
		bool ManagedFileExists(SaveDataManagedPath path);

		// Token: 0x0600D45B RID: 54363
		DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path);

		// Token: 0x0600D45C RID: 54364
		void ManagedFileMove(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName);

		// Token: 0x0600D45D RID: 54365
		SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path);

		// Token: 0x0600D45E RID: 54366
		DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path);

		// Token: 0x0600D45F RID: 54367
		bool ManagedDirectoryExists(SaveDataManagedPath path);

		// Token: 0x0600D460 RID: 54368
		IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D461 RID: 54369
		IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D462 RID: 54370
		IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D463 RID: 54371
		void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive);

		// Token: 0x0600D464 RID: 54372
		long ManagedFileInfoLength(SaveDataManagedPath path);

		// Token: 0x0600D465 RID: 54373
		IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D466 RID: 54374
		IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D467 RID: 54375
		IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);
	}
}
