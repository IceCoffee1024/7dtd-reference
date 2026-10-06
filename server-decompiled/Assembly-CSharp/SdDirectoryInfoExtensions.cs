using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000A0F RID: 2575
public static class SdDirectoryInfoExtensions
{
	// Token: 0x06004CBC RID: 19644 RVA: 0x001DAB78 File Offset: 0x001D8D78
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
