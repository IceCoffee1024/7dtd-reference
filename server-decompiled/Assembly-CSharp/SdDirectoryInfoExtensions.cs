using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000A19 RID: 2585
public static class SdDirectoryInfoExtensions
{
	// Token: 0x06004CE0 RID: 19680 RVA: 0x001DBC60 File Offset: 0x001D9E60
	public static bool IsDirEmpty(this SdDirectoryInfo possiblyEmptyDir)
	{
		bool flag;
		using (IEnumerator<SdFileSystemInfo> enumerator = possiblyEmptyDir.EnumerateFileSystemInfos("*", SearchOption.AllDirectories).GetEnumerator())
		{
			flag = !enumerator.MoveNext();
		}
		return flag;
	}
}
