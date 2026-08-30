using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000A05 RID: 2565
public static class SdDirectoryInfoExtensions
{
	// Token: 0x06004CED RID: 19693 RVA: 0x001D98C8 File Offset: 0x001D7AC8
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
