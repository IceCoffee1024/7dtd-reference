using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Token: 0x020009F0 RID: 2544
public static class SdDirectory
{
	// Token: 0x06004C08 RID: 19464 RVA: 0x001D66C0 File Offset: 0x001D48C0
	public static SdDirectoryInfo CreateDirectory(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdDirectory.ManagedCreateDirectory(saveDataManagedPath);
		}
		return new SdDirectoryInfo(Directory.CreateDirectory(path));
	}

	// Token: 0x06004C09 RID: 19465 RVA: 0x001D66E9 File Offset: 0x001D48E9
	[PublicizedFrom(EAccessModifier.Internal)]
	public static SdDirectoryInfo ManagedCreateDirectory(SaveDataManagedPath path)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryCreateDirectory(path);
	}

	// Token: 0x06004C0A RID: 19466 RVA: 0x001D66F8 File Offset: 0x001D48F8
	public static DateTime GetLastWriteTime(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdDirectory.ManagedGetLastWriteTime(saveDataManagedPath);
		}
		return Directory.GetLastWriteTime(path);
	}

	// Token: 0x06004C0B RID: 19467 RVA: 0x001D671C File Offset: 0x001D491C
	[PublicizedFrom(EAccessModifier.Private)]
	public static DateTime ManagedGetLastWriteTime(SaveDataManagedPath path)
	{
		return SdDirectory.ManagedGetLastWriteTimeUtc(path).ToLocalTime();
	}

	// Token: 0x06004C0C RID: 19468 RVA: 0x001D6738 File Offset: 0x001D4938
	public static DateTime GetLastWriteTimeUtc(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdDirectory.ManagedGetLastWriteTimeUtc(saveDataManagedPath);
		}
		return Directory.GetLastWriteTimeUtc(path);
	}

	// Token: 0x06004C0D RID: 19469 RVA: 0x001D675C File Offset: 0x001D495C
	[PublicizedFrom(EAccessModifier.Internal)]
	public static DateTime ManagedGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004C0E RID: 19470 RVA: 0x001D676C File Offset: 0x001D496C
	public static bool Exists(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdDirectory.ManagedExists(saveDataManagedPath);
		}
		return Directory.Exists(path);
	}

	// Token: 0x06004C0F RID: 19471 RVA: 0x001D6790 File Offset: 0x001D4990
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool ManagedExists(SaveDataManagedPath path)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryExists(path);
	}

	// Token: 0x06004C10 RID: 19472 RVA: 0x001D679D File Offset: 0x001D499D
	public static string[] GetFiles(string path)
	{
		return SdDirectory.EnumerateFiles(path).ToArray<string>();
	}

	// Token: 0x06004C11 RID: 19473 RVA: 0x001D67AA File Offset: 0x001D49AA
	public static string[] GetFiles(string path, string searchPattern)
	{
		return SdDirectory.EnumerateFiles(path, searchPattern).ToArray<string>();
	}

	// Token: 0x06004C12 RID: 19474 RVA: 0x001D67B8 File Offset: 0x001D49B8
	public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
	{
		return SdDirectory.EnumerateFiles(path, searchPattern, searchOption).ToArray<string>();
	}

	// Token: 0x06004C13 RID: 19475 RVA: 0x001D67C7 File Offset: 0x001D49C7
	public static string[] GetDirectories(string path)
	{
		return SdDirectory.EnumerateDirectories(path).ToArray<string>();
	}

	// Token: 0x06004C14 RID: 19476 RVA: 0x001D67D4 File Offset: 0x001D49D4
	public static string[] GetDirectories(string path, string searchPattern)
	{
		return SdDirectory.EnumerateDirectories(path, searchPattern).ToArray<string>();
	}

	// Token: 0x06004C15 RID: 19477 RVA: 0x001D67E2 File Offset: 0x001D49E2
	public static string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
	{
		return SdDirectory.EnumerateDirectories(path, searchPattern, searchOption).ToArray<string>();
	}

	// Token: 0x06004C16 RID: 19478 RVA: 0x001D67F1 File Offset: 0x001D49F1
	public static string[] GetFileSystemEntries(string path)
	{
		return SdDirectory.EnumerateFileSystemEntries(path).ToArray<string>();
	}

	// Token: 0x06004C17 RID: 19479 RVA: 0x001D67FE File Offset: 0x001D49FE
	public static string[] GetFileSystemEntries(string path, string searchPattern)
	{
		return SdDirectory.EnumerateFileSystemEntries(path, searchPattern).ToArray<string>();
	}

	// Token: 0x06004C18 RID: 19480 RVA: 0x001D680C File Offset: 0x001D4A0C
	public static string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
	{
		return SdDirectory.EnumerateFileSystemEntries(path, searchPattern, searchOption).ToArray<string>();
	}

	// Token: 0x06004C19 RID: 19481 RVA: 0x001D681C File Offset: 0x001D4A1C
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

	// Token: 0x06004C1A RID: 19482 RVA: 0x001D686C File Offset: 0x001D4A6C
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

	// Token: 0x06004C1B RID: 19483 RVA: 0x001D68B8 File Offset: 0x001D4AB8
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

	// Token: 0x06004C1C RID: 19484 RVA: 0x001D6904 File Offset: 0x001D4B04
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<SaveDataManagedPath> ManagedEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004C1D RID: 19485 RVA: 0x001D6914 File Offset: 0x001D4B14
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

	// Token: 0x06004C1E RID: 19486 RVA: 0x001D6964 File Offset: 0x001D4B64
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

	// Token: 0x06004C1F RID: 19487 RVA: 0x001D69B0 File Offset: 0x001D4BB0
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

	// Token: 0x06004C20 RID: 19488 RVA: 0x001D69FC File Offset: 0x001D4BFC
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<SaveDataManagedPath> ManagedEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryEnumerateFiles(path, searchPattern, searchOption);
	}

	// Token: 0x06004C21 RID: 19489 RVA: 0x001D6A0C File Offset: 0x001D4C0C
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

	// Token: 0x06004C22 RID: 19490 RVA: 0x001D6A5C File Offset: 0x001D4C5C
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

	// Token: 0x06004C23 RID: 19491 RVA: 0x001D6AA8 File Offset: 0x001D4CA8
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

	// Token: 0x06004C24 RID: 19492 RVA: 0x001D6AF4 File Offset: 0x001D4CF4
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<SaveDataManagedPath> ManagedEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryEnumerateFileSystemEntries(path, searchPattern, searchOption);
	}

	// Token: 0x06004C25 RID: 19493 RVA: 0x001D6B03 File Offset: 0x001D4D03
	public static string[] GetLogicalDrives()
	{
		return Directory.GetLogicalDrives();
	}

	// Token: 0x06004C26 RID: 19494 RVA: 0x001D6B0A File Offset: 0x001D4D0A
	public static string GetDirectoryRoot(string path)
	{
		return Directory.GetDirectoryRoot(path);
	}

	// Token: 0x06004C27 RID: 19495 RVA: 0x001D6B12 File Offset: 0x001D4D12
	public static string GetCurrentDirectory()
	{
		return Directory.GetCurrentDirectory();
	}

	// Token: 0x06004C28 RID: 19496 RVA: 0x001D6B19 File Offset: 0x001D4D19
	public static void SetCurrentDirectory(string path)
	{
		Directory.SetCurrentDirectory(path);
	}

	// Token: 0x06004C29 RID: 19497 RVA: 0x001D6B24 File Offset: 0x001D4D24
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

	// Token: 0x06004C2A RID: 19498 RVA: 0x001D6B4C File Offset: 0x001D4D4C
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

	// Token: 0x06004C2B RID: 19499 RVA: 0x001D6B72 File Offset: 0x001D4D72
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void ManagedDelete(SaveDataManagedPath path, bool recursive)
	{
		SaveDataUtils.SaveDataManager.ManagedDirectoryDelete(path, recursive);
	}
}
