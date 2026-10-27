using System;
using System.Collections.Generic;
using System.IO;

namespace Platform
{
	// Token: 0x02001BEE RID: 7150
	public interface IPlatformSaveGameIOProvider
	{
		// Token: 0x0600D56A RID: 54634
		void ManagedFileRead(SaveDataManagedPath path, Stream dest);

		// Token: 0x0600D56B RID: 54635
		void ManagedFileWrite(SaveDataManagedPath path, Stream src);

		// Token: 0x0600D56C RID: 54636
		void ManagedFileCopy(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName, bool overwrite = false);

		// Token: 0x0600D56D RID: 54637
		void ManagedFileDelete(SaveDataManagedPath path);

		// Token: 0x0600D56E RID: 54638
		bool ManagedFileExists(SaveDataManagedPath path);

		// Token: 0x0600D56F RID: 54639
		DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path);

		// Token: 0x0600D570 RID: 54640
		void ManagedFileMove(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName);

		// Token: 0x0600D571 RID: 54641
		SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path);

		// Token: 0x0600D572 RID: 54642
		DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path);

		// Token: 0x0600D573 RID: 54643
		bool ManagedDirectoryExists(SaveDataManagedPath path);

		// Token: 0x0600D574 RID: 54644
		IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D575 RID: 54645
		IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D576 RID: 54646
		IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D577 RID: 54647
		void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive);

		// Token: 0x0600D578 RID: 54648
		long ManagedFileInfoLength(SaveDataManagedPath path);

		// Token: 0x0600D579 RID: 54649
		IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D57A RID: 54650
		IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D57B RID: 54651
		IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);
	}
}
