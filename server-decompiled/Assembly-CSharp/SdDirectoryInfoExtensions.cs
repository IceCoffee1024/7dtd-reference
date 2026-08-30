using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x020009F4 RID: 2548
public static class SdDirectoryInfoExtensions
{
	// Token: 0x06004C67 RID: 19559 RVA: 0x001D70E0 File Offset: 0x001D52E0
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
