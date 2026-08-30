using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Token: 0x02000A01 RID: 2561
public static class SdDirectory
{
	// Token: 0x06004C8E RID: 19598 RVA: 0x001D8EA8 File Offset: 0x001D70A8
	public static SdDirectoryInfo CreateDirectory(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdDirectory.ManagedCreateDirectory(saveDataManagedPath);
		}
		return new SdDirectoryInfo(Directory.CreateDirectory(path));
	}

	// Token: 0x06004C8F RID: 19599 RVA: 0x001D8ED1 File Offset: 0x001D70D1
	[PublicizedFrom(EAccessModifier.Internal)]
	public static SdDirectoryInfo ManagedCreateDirectory(SaveDataManagedPath path)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryCreateDirectory(path);
	}

	// Token: 0x06004C90 RID: 19600 RVA: 0x001D8EE0 File Offset: 0x001D70E0
	public static DateTime GetLastWriteTime(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdDirectory.ManagedGetLastWriteTime(saveDataManagedPath);
		}
		return Directory.GetLastWriteTime(path);
	}

	// Token: 0x06004C91 RID: 19601 RVA: 0x001D8F04 File Offset: 0x001D7104
	[PublicizedFrom(EAccessModifier.Private)]
	public static DateTime ManagedGetLastWriteTime(SaveDataManagedPath path)
	{
		return SdDirectory.ManagedGetLastWriteTimeUtc(path).ToLocalTime();
	}

	// Token: 0x06004C92 RID: 19602 RVA: 0x001D8F20 File Offset: 0x001D7120
	public static DateTime GetLastWriteTimeUtc(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdDirectory.ManagedGetLastWriteTimeUtc(saveDataManagedPath);
		}
		return Directory.GetLastWriteTimeUtc(path);
	}

	// Token: 0x06004C93 RID: 19603 RVA: 0x001D8F44 File Offset: 0x001D7144
	[PublicizedFrom(EAccessModifier.Internal)]
	public static DateTime ManagedGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004C94 RID: 19604 RVA: 0x001D8F54 File Offset: 0x001D7154
	public static bool Exists(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdDirectory.ManagedExists(saveDataManagedPath);
		}
		return Directory.Exists(path);
	}

	// Token: 0x06004C95 RID: 19605 RVA: 0x001D8F78 File Offset: 0x001D7178
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool ManagedExists(SaveDataManagedPath path)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryExists(path);
	}

	// Token: 0x06004C96 RID: 19606 RVA: 0x001D8F85 File Offset: 0x001D7185
	public static string[] GetFiles(string path)
	{
		return SdDirectory.EnumerateFiles(path).ToArray<string>();
	}

	// Token: 0x06004C97 RID: 19607 RVA: 0x001D8F92 File Offset: 0x001D7192
	public static string[] GetFiles(string path, string searchPattern)
	{
		return SdDirectory.EnumerateFiles(path, searchPattern).ToArray<string>();
	}

	// Token: 0x06004C98 RID: 19608 RVA: 0x001D8FA0 File Offset: 0x001D71A0
	public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
	{
		return SdDirectory.EnumerateFiles(path, searchPattern, searchOption).ToArray<string>();
	}

	// Token: 0x06004C99 RID: 19609 RVA: 0x001D8FAF File Offset: 0x001D71AF
	public static string[] GetDirectories(string path)
	{
		return SdDirectory.EnumerateDirectories(path).ToArray<string>();
	}

	// Token: 0x06004C9A RID: 19610 RVA: 0x001D8FBC File Offset: 0x001D71BC
	public static string[] GetDirectories(string path, string searchPattern)
	{
		return SdDirectory.EnumerateDirectories(path, searchPattern).ToArray<string>();
	}

	// Token: 0x06004C9B RID: 19611 RVA: 0x001D8FCA File Offset: 0x001D71CA
	public static string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
	{
		return SdDirectory.EnumerateDirectories(path, searchPattern, searchOption).ToArray<string>();
	}

	// Token: 0x06004C9C RID: 19612 RVA: 0x001D8FD9 File Offset: 0x001D71D9
	public static string[] GetFileSystemEntries(string path)
	{
		return SdDirectory.EnumerateFileSystemEntries(path).ToArray<string>();
	}

	// Token: 0x06004C9D RID: 19613 RVA: 0x001D8FE6 File Offset: 0x001D71E6
	public static string[] GetFileSystemEntries(string path, string searchPattern)
	{
		return SdDirectory.EnumerateFileSystemEntries(path, searchPattern).ToArray<string>();
	}

	// Token: 0x06004C9E RID: 19614 RVA: 0x001D8FF4 File Offset: 0x001D71F4
	public static string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
	{
		return SdDirectory.EnumerateFileSystemEntries(path, searchPattern, searchOption).ToArray<string>();
	}

	// Token: 0x06004C9F RID: 19615 RVA: 0x001D9004 File Offset: 0x001D7204
	public static IEnumerable<string> EnumerateDirectories(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return from x in SdDirectory.ManagedEnumerateDirectories(saveDataManagedPath, "*", SearchOption.TopDirectoryOnly)
				select x.GetOriginalPath();
		}
		return Directory.EnumerateDirectories(path);
	}

	// Token: 0x06004CA0 RID: 19616 RVA: 0x001D9054 File Offset: 0x001D7254
	public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return from x in SdDirectory.ManagedEnumerateDirectories(saveDataManagedPath, searchPattern, SearchOption.TopDirectoryOnly)
				select x.GetOriginalPath();
		}
		return Directory.EnumerateDirectories(path, searchPattern);
	}

	// Token: 0x06004CA1 RID: 19617 RVA: 0x001D90A0 File Offset: 0x001D72A0
	public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return from x in SdDirectory.ManagedEnumerateDirectories(saveDataManagedPath, searchPattern, searchOption)
				select x.GetOriginalPath();
		}
		return Directory.EnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004CA2 RID: 19618 RVA: 0x001D90EC File Offset: 0x001D72EC
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<SaveDataManagedPath> ManagedEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004CA3 RID: 19619 RVA: 0x001D90FC File Offset: 0x001D72FC
	public static IEnumerable<string> EnumerateFiles(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return from x in SdDirectory.ManagedEnumerateFiles(saveDataManagedPath, "*", SearchOption.TopDirectoryOnly)
				select x.GetOriginalPath();
		}
		return Directory.EnumerateFiles(path);
	}

	// Token: 0x06004CA4 RID: 19620 RVA: 0x001D914C File Offset: 0x001D734C
	public static IEnumerable<string> EnumerateFiles(string path, string searchPattern)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return from x in SdDirectory.ManagedEnumerateFiles(saveDataManagedPath, searchPattern, SearchOption.TopDirectoryOnly)
				select x.GetOriginalPath();
		}
		return Directory.EnumerateFiles(path, searchPattern);
	}

	// Token: 0x06004CA5 RID: 19621 RVA: 0x001D9198 File Offset: 0x001D7398
	public static IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return from x in SdDirectory.ManagedEnumerateFiles(saveDataManagedPath, searchPattern, searchOption)
				select x.GetOriginalPath();
		}
		return Directory.EnumerateFiles(path, searchPattern, searchOption);
	}

	// Token: 0x06004CA6 RID: 19622 RVA: 0x001D91E4 File Offset: 0x001D73E4
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<SaveDataManagedPath> ManagedEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryEnumerateFiles(path, searchPattern, searchOption);
	}

	// Token: 0x06004CA7 RID: 19623 RVA: 0x001D91F4 File Offset: 0x001D73F4
	public static IEnumerable<string> EnumerateFileSystemEntries(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return from x in SdDirectory.ManagedEnumerateFileSystemEntries(saveDataManagedPath, "*", SearchOption.TopDirectoryOnly)
				select x.GetOriginalPath();
		}
		return Directory.EnumerateFileSystemEntries(path);
	}

	// Token: 0x06004CA8 RID: 19624 RVA: 0x001D9244 File Offset: 0x001D7444
	public static IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return from x in SdDirectory.ManagedEnumerateFileSystemEntries(saveDataManagedPath, searchPattern, SearchOption.TopDirectoryOnly)
				select x.GetOriginalPath();
		}
		return Directory.EnumerateFileSystemEntries(path, searchPattern);
	}

	// Token: 0x06004CA9 RID: 19625 RVA: 0x001D9290 File Offset: 0x001D7490
	public static IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return from x in SdDirectory.ManagedEnumerateFileSystemEntries(saveDataManagedPath, searchPattern, searchOption)
				select x.GetOriginalPath();
		}
		return Directory.EnumerateFileSystemEntries(path, searchPattern, searchOption);
	}

	// Token: 0x06004CAA RID: 19626 RVA: 0x001D92DC File Offset: 0x001D74DC
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<SaveDataManagedPath> ManagedEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryEnumerateFileSystemEntries(path, searchPattern, searchOption);
	}

	// Token: 0x06004CAB RID: 19627 RVA: 0x001D92EB File Offset: 0x001D74EB
	public static string[] GetLogicalDrives()
	{
		return Directory.GetLogicalDrives();
	}

	// Token: 0x06004CAC RID: 19628 RVA: 0x001D92F2 File Offset: 0x001D74F2
	public static string GetDirectoryRoot(string path)
	{
		return Directory.GetDirectoryRoot(path);
	}

	// Token: 0x06004CAD RID: 19629 RVA: 0x001D92FA File Offset: 0x001D74FA
	public static string GetCurrentDirectory()
	{
		return Directory.GetCurrentDirectory();
	}

	// Token: 0x06004CAE RID: 19630 RVA: 0x001D9301 File Offset: 0x001D7501
	public static void SetCurrentDirectory(string path)
	{
		Directory.SetCurrentDirectory(path);
	}

	// Token: 0x06004CAF RID: 19631 RVA: 0x001D930C File Offset: 0x001D750C
	public static void Delete(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			SdDirectory.ManagedDelete(saveDataManagedPath, false);
			return;
		}
		Directory.Delete(path);
	}

	// Token: 0x06004CB0 RID: 19632 RVA: 0x001D9334 File Offset: 0x001D7534
	public static void Delete(string path, bool recursive)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			SdDirectory.ManagedDelete(saveDataManagedPath, recursive);
			return;
		}
		Directory.Delete(path, recursive);
	}

	// Token: 0x06004CB1 RID: 19633 RVA: 0x001D935A File Offset: 0x001D755A
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void ManagedDelete(SaveDataManagedPath path, bool recursive)
	{
		SaveDataUtils.SaveDataManager.ManagedDirectoryDelete(path, recursive);
	}
}
