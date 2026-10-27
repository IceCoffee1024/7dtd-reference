using System;
using System.Collections.Generic;
using System.Linq;

namespace Platform.XBL.Save
{
	// Token: 0x02001CA1 RID: 7329
	public sealed class PathEnumerationInfo
	{
		// Token: 0x0600D8ED RID: 55533 RVA: 0x004DA9C6 File Offset: 0x004D8BC6
		public PathEnumerationInfo(string relativePath, bool isDirectory, bool isFile)
		{
			this.RelativePath = relativePath;
			this.IsDirectory = isDirectory;
			this.IsFile = isFile;
		}

		// Token: 0x0600D8EE RID: 55534 RVA: 0x004DA9E4 File Offset: 0x004D8BE4
		public override string ToString()
		{
			return string.Format("{0}[{1}=\"{2}\", {3}={4}, {5}={6}]", new object[] { "PathEnumerationInfo", "RelativePath", this.RelativePath, "IsDirectory", this.IsDirectory, "IsFile", this.IsFile });
		}

		// Token: 0x0600D8EF RID: 55535 RVA: 0x004DAA48 File Offset: 0x004D8C48
		public void UsedOnlyForAOTCodeGeneration()
		{
			IEnumerable<SaveDataManagedPath> enumerable = from _ in Enumerable.Empty<PathEnumerationInfo>()
				select null;
			enumerable.Select<SaveDataManagedPath, SdFileSystemInfo>((SaveDataManagedPath _) => null);
			enumerable.Select<SaveDataManagedPath, SdDirectoryInfo>((SaveDataManagedPath _) => null);
			enumerable.Select<SaveDataManagedPath, SdFileInfo>((SaveDataManagedPath _) => null);
			throw new InvalidOperationException("This method is used for AOT code generation only. Do not call it at runtime.");
		}

		// Token: 0x0400A4FD RID: 42237
		public readonly string RelativePath;

		// Token: 0x0400A4FE RID: 42238
		public readonly bool IsDirectory;

		// Token: 0x0400A4FF RID: 42239
		public readonly bool IsFile;
	}
}
