using System;
using System.Collections.Generic;
using System.Linq;

namespace Platform.XBL.Save
{
	// Token: 0x02001C70 RID: 7280
	public sealed class PathEnumerationInfo
	{
		// Token: 0x0600D7D9 RID: 55257 RVA: 0x004D6E76 File Offset: 0x004D5076
		public PathEnumerationInfo(string relativePath, bool isDirectory, bool isFile)
		{
			this.RelativePath = relativePath;
			this.IsDirectory = isDirectory;
			this.IsFile = isFile;
		}

		// Token: 0x0600D7DA RID: 55258 RVA: 0x004D6E94 File Offset: 0x004D5094
		public override string ToString()
		{
			return string.Format("{0}[{1}=\"{2}\", {3}={4}, {5}={6}]", new object[] { "PathEnumerationInfo", "RelativePath", this.RelativePath, "IsDirectory", this.IsDirectory, "IsFile", this.IsFile });
		}

		// Token: 0x0600D7DB RID: 55259 RVA: 0x004D6EF8 File Offset: 0x004D50F8
		public void UsedOnlyForAOTCodeGeneration()
		{
			IEnumerable<SaveDataManagedPath> enumerable = from _ in Enumerable.Empty<PathEnumerationInfo>()
				select null;
			enumerable.Select<SaveDataManagedPath, SdFileSystemInfo>((SaveDataManagedPath _) => null);
			enumerable.Select<SaveDataManagedPath, SdDirectoryInfo>((SaveDataManagedPath _) => null);
			enumerable.Select<SaveDataManagedPath, SdFileInfo>((SaveDataManagedPath _) => null);
			throw new InvalidOperationException("This method is used for AOT code generation only. Do not call it at runtime.");
		}

		// Token: 0x0400A414 RID: 42004
		public readonly string RelativePath;

		// Token: 0x0400A415 RID: 42005
		public readonly bool IsDirectory;

		// Token: 0x0400A416 RID: 42006
		public readonly bool IsFile;
	}
}
