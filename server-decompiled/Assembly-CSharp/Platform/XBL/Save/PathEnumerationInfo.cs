using System;
using System.Collections.Generic;
using System.Linq;

namespace Platform.XBL.Save
{
	// Token: 0x02001C17 RID: 7191
	public sealed class PathEnumerationInfo
	{
		// Token: 0x0600D4D0 RID: 54480 RVA: 0x004CBF16 File Offset: 0x004CA116
		public PathEnumerationInfo(string relativePath, bool isDirectory, bool isFile)
		{
			this.RelativePath = relativePath;
			this.IsDirectory = isDirectory;
			this.IsFile = isFile;
		}

		// Token: 0x0600D4D1 RID: 54481 RVA: 0x004CBF34 File Offset: 0x004CA134
		public override string ToString()
		{
			return string.Format("{0}[{1}=\"{2}\", {3}={4}, {5}={6}]", new object[] { "PathEnumerationInfo", "RelativePath", this.RelativePath, "IsDirectory", this.IsDirectory, "IsFile", this.IsFile });
		}

		// Token: 0x0600D4D2 RID: 54482 RVA: 0x004CBF98 File Offset: 0x004CA198
		public void UsedOnlyForAOTCodeGeneration()
		{
			IEnumerable<SaveDataManagedPath> enumerable = from _ in Enumerable.Empty<PathEnumerationInfo>()
				select null;
			enumerable.Select<SaveDataManagedPath, SdFileSystemInfo>((SaveDataManagedPath _) => null);
			enumerable.Select<SaveDataManagedPath, SdDirectoryInfo>((SaveDataManagedPath _) => null);
			enumerable.Select<SaveDataManagedPath, SdFileInfo>((SaveDataManagedPath _) => null);
			throw new InvalidOperationException("This method is used for AOT code generation only. Do not call it at runtime.");
		}

		// Token: 0x0400A20E RID: 41486
		public readonly string RelativePath;

		// Token: 0x0400A20F RID: 41487
		public readonly bool IsDirectory;

		// Token: 0x0400A210 RID: 41488
		public readonly bool IsFile;
	}
}
