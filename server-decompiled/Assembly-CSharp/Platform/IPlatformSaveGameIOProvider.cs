using System;
using System.Collections.Generic;
using System.IO;

namespace Platform
{
	// Token: 0x02001B8B RID: 7051
	public interface IPlatformSaveGameIOProvider
	{
		// Token: 0x0600D2DE RID: 53982
		void ManagedFileRead(SaveDataManagedPath path, Stream dest);

		// Token: 0x0600D2DF RID: 53983
		void ManagedFileWrite(SaveDataManagedPath path, Stream src);

		// Token: 0x0600D2E0 RID: 53984
		void ManagedFileCopy(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName, bool overwrite = false);

		// Token: 0x0600D2E1 RID: 53985
		void ManagedFileDelete(SaveDataManagedPath path);

		// Token: 0x0600D2E2 RID: 53986
		bool ManagedFileExists(SaveDataManagedPath path);

		// Token: 0x0600D2E3 RID: 53987
		DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path);

		// Token: 0x0600D2E4 RID: 53988
		void ManagedFileMove(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName);

		// Token: 0x0600D2E5 RID: 53989
		SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path);

		// Token: 0x0600D2E6 RID: 53990
		DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path);

		// Token: 0x0600D2E7 RID: 53991
		bool ManagedDirectoryExists(SaveDataManagedPath path);

		// Token: 0x0600D2E8 RID: 53992
		IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D2E9 RID: 53993
		IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D2EA RID: 53994
		IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D2EB RID: 53995
		void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive);

		// Token: 0x0600D2EC RID: 53996
		long ManagedFileInfoLength(SaveDataManagedPath path);

		// Token: 0x0600D2ED RID: 53997
		IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D2EE RID: 53998
		IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);

		// Token: 0x0600D2EF RID: 53999
		IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption);
	}
}
