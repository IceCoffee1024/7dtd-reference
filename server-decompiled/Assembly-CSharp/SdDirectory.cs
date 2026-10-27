using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Token: 0x02000A15 RID: 2581
public static class SdDirectory
{
	// Token: 0x06004C81 RID: 19585 RVA: 0x001DB240 File Offset: 0x001D9440
	public static SdDirectoryInfo CreateDirectory(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdDirectory.ManagedCreateDirectory(saveDataManagedPath);
		}
		return new SdDirectoryInfo(Directory.CreateDirectory(path));
	}

	// Token: 0x06004C82 RID: 19586 RVA: 0x001DB269 File Offset: 0x001D9469
	[PublicizedFrom(EAccessModifier.Internal)]
	public static SdDirectoryInfo ManagedCreateDirectory(SaveDataManagedPath path)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryCreateDirectory(path);
	}

	// Token: 0x06004C83 RID: 19587 RVA: 0x001DB278 File Offset: 0x001D9478
	public static DateTime GetLastWriteTime(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdDirectory.ManagedGetLastWriteTime(saveDataManagedPath);
		}
		return Directory.GetLastWriteTime(path);
	}

	// Token: 0x06004C84 RID: 19588 RVA: 0x001DB29C File Offset: 0x001D949C
	[PublicizedFrom(EAccessModifier.Private)]
	public static DateTime ManagedGetLastWriteTime(SaveDataManagedPath path)
	{
		return SdDirectory.ManagedGetLastWriteTimeUtc(path).ToLocalTime();
	}

	// Token: 0x06004C85 RID: 19589 RVA: 0x001DB2B8 File Offset: 0x001D94B8
	public static DateTime GetLastWriteTimeUtc(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdDirectory.ManagedGetLastWriteTimeUtc(saveDataManagedPath);
		}
		return Directory.GetLastWriteTimeUtc(path);
	}

	// Token: 0x06004C86 RID: 19590 RVA: 0x001DB2DC File Offset: 0x001D94DC
	[PublicizedFrom(EAccessModifier.Internal)]
	public static DateTime ManagedGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004C87 RID: 19591 RVA: 0x001DB2EC File Offset: 0x001D94EC
	public static bool Exists(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdDirectory.ManagedExists(saveDataManagedPath);
		}
		return Directory.Exists(path);
	}

	// Token: 0x06004C88 RID: 19592 RVA: 0x001DB310 File Offset: 0x001D9510
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool ManagedExists(SaveDataManagedPath path)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryExists(path);
	}

	// Token: 0x06004C89 RID: 19593 RVA: 0x001DB31D File Offset: 0x001D951D
	public static string[] GetFiles(string path)
	{
		return SdDirectory.EnumerateFiles(path).ToArray<string>();
	}

	// Token: 0x06004C8A RID: 19594 RVA: 0x001DB32A File Offset: 0x001D952A
	public static string[] GetFiles(string path, string searchPattern)
	{
		return SdDirectory.EnumerateFiles(path, searchPattern).ToArray<string>();
	}

	// Token: 0x06004C8B RID: 19595 RVA: 0x001DB338 File Offset: 0x001D9538
	public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
	{
		return SdDirectory.EnumerateFiles(path, searchPattern, searchOption).ToArray<string>();
	}

	// Token: 0x06004C8C RID: 19596 RVA: 0x001DB347 File Offset: 0x001D9547
	public static string[] GetDirectories(string path)
	{
		return SdDirectory.EnumerateDirectories(path).ToArray<string>();
	}

	// Token: 0x06004C8D RID: 19597 RVA: 0x001DB354 File Offset: 0x001D9554
	public static string[] GetDirectories(string path, string searchPattern)
	{
		return SdDirectory.EnumerateDirectories(path, searchPattern).ToArray<string>();
	}

	// Token: 0x06004C8E RID: 19598 RVA: 0x001DB362 File Offset: 0x001D9562
	public static string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
	{
		return SdDirectory.EnumerateDirectories(path, searchPattern, searchOption).ToArray<string>();
	}

	// Token: 0x06004C8F RID: 19599 RVA: 0x001DB371 File Offset: 0x001D9571
	public static string[] GetFileSystemEntries(string path)
	{
		return SdDirectory.EnumerateFileSystemEntries(path).ToArray<string>();
	}

	// Token: 0x06004C90 RID: 19600 RVA: 0x001DB37E File Offset: 0x001D957E
	public static string[] GetFileSystemEntries(string path, string searchPattern)
	{
		return SdDirectory.EnumerateFileSystemEntries(path, searchPattern).ToArray<string>();
	}

	// Token: 0x06004C91 RID: 19601 RVA: 0x001DB38C File Offset: 0x001D958C
	public static string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
	{
		return SdDirectory.EnumerateFileSystemEntries(path, searchPattern, searchOption).ToArray<string>();
	}

	// Token: 0x06004C92 RID: 19602 RVA: 0x001DB39C File Offset: 0x001D959C
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

	// Token: 0x06004C93 RID: 19603 RVA: 0x001DB3EC File Offset: 0x001D95EC
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

	// Token: 0x06004C94 RID: 19604 RVA: 0x001DB438 File Offset: 0x001D9638
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

	// Token: 0x06004C95 RID: 19605 RVA: 0x001DB484 File Offset: 0x001D9684
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<SaveDataManagedPath> ManagedEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004C96 RID: 19606 RVA: 0x001DB494 File Offset: 0x001D9694
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

	// Token: 0x06004C97 RID: 19607 RVA: 0x001DB4E4 File Offset: 0x001D96E4
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

	// Token: 0x06004C98 RID: 19608 RVA: 0x001DB530 File Offset: 0x001D9730
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

	// Token: 0x06004C99 RID: 19609 RVA: 0x001DB57C File Offset: 0x001D977C
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<SaveDataManagedPath> ManagedEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryEnumerateFiles(path, searchPattern, searchOption);
	}

	// Token: 0x06004C9A RID: 19610 RVA: 0x001DB58C File Offset: 0x001D978C
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

	// Token: 0x06004C9B RID: 19611 RVA: 0x001DB5DC File Offset: 0x001D97DC
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

	// Token: 0x06004C9C RID: 19612 RVA: 0x001DB628 File Offset: 0x001D9828
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

	// Token: 0x06004C9D RID: 19613 RVA: 0x001DB674 File Offset: 0x001D9874
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<SaveDataManagedPath> ManagedEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryEnumerateFileSystemEntries(path, searchPattern, searchOption);
	}

	// Token: 0x06004C9E RID: 19614 RVA: 0x001DB683 File Offset: 0x001D9883
	public static string[] GetLogicalDrives()
	{
		return Directory.GetLogicalDrives();
	}

	// Token: 0x06004C9F RID: 19615 RVA: 0x001DB68A File Offset: 0x001D988A
	public static string GetDirectoryRoot(string path)
	{
		return Directory.GetDirectoryRoot(path);
	}

	// Token: 0x06004CA0 RID: 19616 RVA: 0x001DB692 File Offset: 0x001D9892
	public static string GetCurrentDirectory()
	{
		return Directory.GetCurrentDirectory();
	}

	// Token: 0x06004CA1 RID: 19617 RVA: 0x001DB699 File Offset: 0x001D9899
	public static void SetCurrentDirectory(string path)
	{
		Directory.SetCurrentDirectory(path);
	}

	// Token: 0x06004CA2 RID: 19618 RVA: 0x001DB6A4 File Offset: 0x001D98A4
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

	// Token: 0x06004CA3 RID: 19619 RVA: 0x001DB6CC File Offset: 0x001D98CC
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

	// Token: 0x06004CA4 RID: 19620 RVA: 0x001DB6F2 File Offset: 0x001D98F2
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void ManagedDelete(SaveDataManagedPath path, bool recursive)
	{
		SaveDataUtils.SaveDataManager.ManagedDirectoryDelete(path, recursive);
	}
}
