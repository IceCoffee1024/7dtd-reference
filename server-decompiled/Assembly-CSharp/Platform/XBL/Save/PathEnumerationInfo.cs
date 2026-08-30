using System;
using System.Collections.Generic;
using System.Linq;

namespace Platform.XBL.Save
{
	// Token: 0x02001C3E RID: 7230
	public sealed class PathEnumerationInfo
	{
		// Token: 0x0600D660 RID: 54880 RVA: 0x004D233A File Offset: 0x004D053A
		public PathEnumerationInfo(string relativePath, bool isDirectory, bool isFile)
		{
			this.RelativePath = relativePath;
			this.IsDirectory = isDirectory;
			this.IsFile = isFile;
		}

		// Token: 0x0600D661 RID: 54881 RVA: 0x004D2358 File Offset: 0x004D0558
		public override string ToString()
		{
			return string.Format("{0}[{1}=\"{2}\", {3}={4}, {5}={6}]", new object[] { "PathEnumerationInfo", "RelativePath", this.RelativePath, "IsDirectory", this.IsDirectory, "IsFile", this.IsFile });
		}

		// Token: 0x0600D662 RID: 54882 RVA: 0x004D23BC File Offset: 0x004D05BC
		public void UsedOnlyForAOTCodeGeneration()
		{
			IEnumerable<SaveDataManagedPath> enumerable = from _ in Enumerable.Empty<PathEnumerationInfo>()
				select null;
			enumerable.Select<SaveDataManagedPath, SdFileSystemInfo>((SaveDataManagedPath _) => null);
			enumerable.Select<SaveDataManagedPath, SdDirectoryInfo>((SaveDataManagedPath _) => null);
			enumerable.Select<SaveDataManagedPath, SdFileInfo>((SaveDataManagedPath _) => null);
			throw new InvalidOperationException("This method is used for AOT code generation only. Do not call it at runtime.");
		}

		// Token: 0x0400A33F RID: 41791
		public readonly string RelativePath;

		// Token: 0x0400A340 RID: 41792
		public readonly bool IsDirectory;

		// Token: 0x0400A341 RID: 41793
		public readonly bool IsFile;
	}
}
