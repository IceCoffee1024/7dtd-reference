using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Token: 0x02000A0B RID: 2571
public static class SdDirectory
{
	// Token: 0x06004C5D RID: 19549 RVA: 0x001DA158 File Offset: 0x001D8358
	public static SdDirectoryInfo CreateDirectory(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdDirectory.ManagedCreateDirectory(saveDataManagedPath);
		}
		return new SdDirectoryInfo(Directory.CreateDirectory(path));
	}

	// Token: 0x06004C5E RID: 19550 RVA: 0x001DA181 File Offset: 0x001D8381
	[PublicizedFrom(EAccessModifier.Internal)]
	public static SdDirectoryInfo ManagedCreateDirectory(SaveDataManagedPath path)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryCreateDirectory(path);
	}

	// Token: 0x06004C5F RID: 19551 RVA: 0x001DA190 File Offset: 0x001D8390
	public static DateTime GetLastWriteTime(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdDirectory.ManagedGetLastWriteTime(saveDataManagedPath);
		}
		return Directory.GetLastWriteTime(path);
	}

	// Token: 0x06004C60 RID: 19552 RVA: 0x001DA1B4 File Offset: 0x001D83B4
	[PublicizedFrom(EAccessModifier.Private)]
	public static DateTime ManagedGetLastWriteTime(SaveDataManagedPath path)
	{
		return SdDirectory.ManagedGetLastWriteTimeUtc(path).ToLocalTime();
	}

	// Token: 0x06004C61 RID: 19553 RVA: 0x001DA1D0 File Offset: 0x001D83D0
	public static DateTime GetLastWriteTimeUtc(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdDirectory.ManagedGetLastWriteTimeUtc(saveDataManagedPath);
		}
		return Directory.GetLastWriteTimeUtc(path);
	}

	// Token: 0x06004C62 RID: 19554 RVA: 0x001DA1F4 File Offset: 0x001D83F4
	[PublicizedFrom(EAccessModifier.Internal)]
	public static DateTime ManagedGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004C63 RID: 19555 RVA: 0x001DA204 File Offset: 0x001D8404
	public static bool Exists(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdDirectory.ManagedExists(saveDataManagedPath);
		}
		return Directory.Exists(path);
	}

	// Token: 0x06004C64 RID: 19556 RVA: 0x001DA228 File Offset: 0x001D8428
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool ManagedExists(SaveDataManagedPath path)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryExists(path);
	}

	// Token: 0x06004C65 RID: 19557 RVA: 0x001DA235 File Offset: 0x001D8435
	public static string[] GetFiles(string path)
	{
		return SdDirectory.EnumerateFiles(path).ToArray<string>();
	}

	// Token: 0x06004C66 RID: 19558 RVA: 0x001DA242 File Offset: 0x001D8442
	public static string[] GetFiles(string path, string searchPattern)
	{
		return SdDirectory.EnumerateFiles(path, searchPattern).ToArray<string>();
	}

	// Token: 0x06004C67 RID: 19559 RVA: 0x001DA250 File Offset: 0x001D8450
	public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
	{
		return SdDirectory.EnumerateFiles(path, searchPattern, searchOption).ToArray<string>();
	}

	// Token: 0x06004C68 RID: 19560 RVA: 0x001DA25F File Offset: 0x001D845F
	public static string[] GetDirectories(string path)
	{
		return SdDirectory.EnumerateDirectories(path).ToArray<string>();
	}

	// Token: 0x06004C69 RID: 19561 RVA: 0x001DA26C File Offset: 0x001D846C
	public static string[] GetDirectories(string path, string searchPattern)
	{
		return SdDirectory.EnumerateDirectories(path, searchPattern).ToArray<string>();
	}

	// Token: 0x06004C6A RID: 19562 RVA: 0x001DA27A File Offset: 0x001D847A
	public static string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
	{
		return SdDirectory.EnumerateDirectories(path, searchPattern, searchOption).ToArray<string>();
	}

	// Token: 0x06004C6B RID: 19563 RVA: 0x001DA289 File Offset: 0x001D8489
	public static string[] GetFileSystemEntries(string path)
	{
		return SdDirectory.EnumerateFileSystemEntries(path).ToArray<string>();
	}

	// Token: 0x06004C6C RID: 19564 RVA: 0x001DA296 File Offset: 0x001D8496
	public static string[] GetFileSystemEntries(string path, string searchPattern)
	{
		return SdDirectory.EnumerateFileSystemEntries(path, searchPattern).ToArray<string>();
	}

	// Token: 0x06004C6D RID: 19565 RVA: 0x001DA2A4 File Offset: 0x001D84A4
	public static string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
	{
		return SdDirectory.EnumerateFileSystemEntries(path, searchPattern, searchOption).ToArray<string>();
	}

	// Token: 0x06004C6E RID: 19566 RVA: 0x001DA2B4 File Offset: 0x001D84B4
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

	// Token: 0x06004C6F RID: 19567 RVA: 0x001DA304 File Offset: 0x001D8504
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

	// Token: 0x06004C70 RID: 19568 RVA: 0x001DA350 File Offset: 0x001D8550
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

	// Token: 0x06004C71 RID: 19569 RVA: 0x001DA39C File Offset: 0x001D859C
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<SaveDataManagedPath> ManagedEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004C72 RID: 19570 RVA: 0x001DA3AC File Offset: 0x001D85AC
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

	// Token: 0x06004C73 RID: 19571 RVA: 0x001DA3FC File Offset: 0x001D85FC
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

	// Token: 0x06004C74 RID: 19572 RVA: 0x001DA448 File Offset: 0x001D8648
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

	// Token: 0x06004C75 RID: 19573 RVA: 0x001DA494 File Offset: 0x001D8694
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<SaveDataManagedPath> ManagedEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryEnumerateFiles(path, searchPattern, searchOption);
	}

	// Token: 0x06004C76 RID: 19574 RVA: 0x001DA4A4 File Offset: 0x001D86A4
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

	// Token: 0x06004C77 RID: 19575 RVA: 0x001DA4F4 File Offset: 0x001D86F4
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

	// Token: 0x06004C78 RID: 19576 RVA: 0x001DA540 File Offset: 0x001D8740
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

	// Token: 0x06004C79 RID: 19577 RVA: 0x001DA58C File Offset: 0x001D878C
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<SaveDataManagedPath> ManagedEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryEnumerateFileSystemEntries(path, searchPattern, searchOption);
	}

	// Token: 0x06004C7A RID: 19578 RVA: 0x001DA59B File Offset: 0x001D879B
	public static string[] GetLogicalDrives()
	{
		return Directory.GetLogicalDrives();
	}

	// Token: 0x06004C7B RID: 19579 RVA: 0x001DA5A2 File Offset: 0x001D87A2
	public static string GetDirectoryRoot(string path)
	{
		return Directory.GetDirectoryRoot(path);
	}

	// Token: 0x06004C7C RID: 19580 RVA: 0x001DA5AA File Offset: 0x001D87AA
	public static string GetCurrentDirectory()
	{
		return Directory.GetCurrentDirectory();
	}

	// Token: 0x06004C7D RID: 19581 RVA: 0x001DA5B1 File Offset: 0x001D87B1
	public static void SetCurrentDirectory(string path)
	{
		Directory.SetCurrentDirectory(path);
	}

	// Token: 0x06004C7E RID: 19582 RVA: 0x001DA5BC File Offset: 0x001D87BC
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

	// Token: 0x06004C7F RID: 19583 RVA: 0x001DA5E4 File Offset: 0x001D87E4
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

	// Token: 0x06004C80 RID: 19584 RVA: 0x001DA60A File Offset: 0x001D880A
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void ManagedDelete(SaveDataManagedPath path, bool recursive)
	{
		SaveDataUtils.SaveDataManager.ManagedDirectoryDelete(path, recursive);
	}
}
