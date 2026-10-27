using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Token: 0x02000A3E RID: 2622
public class SaveDataManager_Placeholder : ISaveDataManager
{
	// Token: 0x06004E81 RID: 20097 RVA: 0x000027FC File Offset: 0x000009FC
	public void Init()
	{
	}

	// Token: 0x06004E82 RID: 20098 RVA: 0x00010E62 File Offset: 0x0000F062
	public SaveDataWriteMode GetWriteMode()
	{
		return SaveDataWriteMode.None;
	}

	// Token: 0x06004E83 RID: 20099 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetWriteMode(SaveDataWriteMode writeMode)
	{
	}

	// Token: 0x06004E84 RID: 20100 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x06004E85 RID: 20101 RVA: 0x000027FC File Offset: 0x000009FC
	public void RegisterRegionFileManager(RegionFileManager regionFileManager)
	{
	}

	// Token: 0x06004E86 RID: 20102 RVA: 0x000027FC File Offset: 0x000009FC
	public void DeregisterRegionFileManager(RegionFileManager regionFileManager)
	{
	}

	// Token: 0x17000890 RID: 2192
	// (get) Token: 0x06004E87 RID: 20103 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool AppliesSaveSizeLimit
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004E88 RID: 20104 RVA: 0x00010E62 File Offset: 0x0000F062
	public int CommitAsync()
	{
		return 0;
	}

	// Token: 0x06004E89 RID: 20105 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsCommitPending(int token)
	{
		return false;
	}

	// Token: 0x06004E8A RID: 20106 RVA: 0x000027FC File Offset: 0x000009FC
	public void CommitSync()
	{
	}

	// Token: 0x06004E8B RID: 20107 RVA: 0x001E16CD File Offset: 0x001DF8CD
	public IEnumerator CommitCoroutine(Action<double> progressFeedback = null)
	{
		yield break;
	}

	// Token: 0x1400007C RID: 124
	// (add) Token: 0x06004E8C RID: 20108 RVA: 0x000027FC File Offset: 0x000009FC
	// (remove) Token: 0x06004E8D RID: 20109 RVA: 0x000027FC File Offset: 0x000009FC
	public event Action CommitStarted
	{
		add
		{
		}
		remove
		{
		}
	}

	// Token: 0x1400007D RID: 125
	// (add) Token: 0x06004E8E RID: 20110 RVA: 0x000027FC File Offset: 0x000009FC
	// (remove) Token: 0x06004E8F RID: 20111 RVA: 0x000027FC File Offset: 0x000009FC
	public event Action CommitFinished
	{
		add
		{
		}
		remove
		{
		}
	}

	// Token: 0x06004E90 RID: 20112 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ShouldLimitSize()
	{
		return false;
	}

	// Token: 0x06004E91 RID: 20113 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UpdateSizes()
	{
	}

	// Token: 0x06004E92 RID: 20114 RVA: 0x001E16D8 File Offset: 0x001DF8D8
	public virtual SaveDataSizes GetSizes()
	{
		return default(SaveDataSizes);
	}

	// Token: 0x06004E93 RID: 20115 RVA: 0x001E16EE File Offset: 0x001DF8EE
	public virtual Stream ManagedFileOpen(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share)
	{
		return File.Open(path.GetOriginalPath(), mode, access, share);
	}

	// Token: 0x06004E94 RID: 20116 RVA: 0x001E16FF File Offset: 0x001DF8FF
	public virtual void ManagedFileDelete(SaveDataManagedPath path)
	{
		File.Delete(path.GetOriginalPath());
	}

	// Token: 0x06004E95 RID: 20117 RVA: 0x001E170C File Offset: 0x001DF90C
	public virtual bool ManagedFileExists(SaveDataManagedPath path)
	{
		return File.Exists(path.GetOriginalPath());
	}

	// Token: 0x06004E96 RID: 20118 RVA: 0x001E1719 File Offset: 0x001DF919
	public virtual DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return File.GetLastWriteTimeUtc(path.GetOriginalPath());
	}

	// Token: 0x06004E97 RID: 20119 RVA: 0x001E1726 File Offset: 0x001DF926
	public virtual SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
	{
		return new SdDirectoryInfo(Directory.CreateDirectory(path.GetOriginalPath()));
	}

	// Token: 0x06004E98 RID: 20120 RVA: 0x001E1738 File Offset: 0x001DF938
	public virtual DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return Directory.GetLastWriteTimeUtc(path.GetOriginalPath());
	}

	// Token: 0x06004E99 RID: 20121 RVA: 0x001E1745 File Offset: 0x001DF945
	public virtual bool ManagedDirectoryExists(SaveDataManagedPath path)
	{
		return Directory.Exists(path.GetOriginalPath());
	}

	// Token: 0x06004E9A RID: 20122 RVA: 0x001E1752 File Offset: 0x001DF952
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.ConvertPathsToManagedPaths(Directory.EnumerateDirectories(path.GetOriginalPath(), searchPattern, searchOption));
	}

	// Token: 0x06004E9B RID: 20123 RVA: 0x001E1766 File Offset: 0x001DF966
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.ConvertPathsToManagedPaths(Directory.EnumerateFiles(path.GetOriginalPath(), searchPattern, searchOption));
	}

	// Token: 0x06004E9C RID: 20124 RVA: 0x001E177A File Offset: 0x001DF97A
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.ConvertPathsToManagedPaths(Directory.EnumerateFileSystemEntries(path.GetOriginalPath(), searchPattern, searchOption));
	}

	// Token: 0x06004E9D RID: 20125 RVA: 0x001E178E File Offset: 0x001DF98E
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<SaveDataManagedPath> ConvertPathsToManagedPaths(IEnumerable<string> paths)
	{
		using (IEnumerator<string> enumerator = paths.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SaveDataManagedPath saveDataManagedPath;
				if (SaveDataUtils.TryGetManagedPath(enumerator.Current, out saveDataManagedPath))
				{
					yield return saveDataManagedPath;
				}
			}
		}
		IEnumerator<string> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x06004E9E RID: 20126 RVA: 0x001E179E File Offset: 0x001DF99E
	public virtual void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive)
	{
		Directory.Delete(path.GetOriginalPath(), recursive);
	}

	// Token: 0x06004E9F RID: 20127 RVA: 0x001E17AC File Offset: 0x001DF9AC
	public long ManagedFileInfoLength(SaveDataManagedPath path)
	{
		return new FileInfo(path.GetOriginalPath()).Length;
	}

	// Token: 0x06004EA0 RID: 20128 RVA: 0x001E17BE File Offset: 0x001DF9BE
	public virtual IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return from x in new DirectoryInfo(path.GetOriginalPath()).EnumerateDirectories(searchPattern, searchOption)
			select new SdDirectoryInfo(x);
	}

	// Token: 0x06004EA1 RID: 20129 RVA: 0x001E17F6 File Offset: 0x001DF9F6
	public virtual IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return from x in new DirectoryInfo(path.GetOriginalPath()).EnumerateFiles(searchPattern, searchOption)
			select new SdFileInfo(x);
	}

	// Token: 0x06004EA2 RID: 20130 RVA: 0x001E182E File Offset: 0x001DFA2E
	public virtual IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return new DirectoryInfo(path.GetOriginalPath()).EnumerateFileSystemInfos(searchPattern, searchOption).Select<FileSystemInfo, SdFileSystemInfo>(new Func<FileSystemInfo, SdFileSystemInfo>(SdDirectoryInfo.WrapFileSystemInfo));
	}

	// Token: 0x04003CE5 RID: 15589
	public static readonly SaveDataManager_Placeholder Instance = new SaveDataManager_Placeholder();
}
