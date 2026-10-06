using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Token: 0x02000A34 RID: 2612
public class SaveDataManager_Placeholder : ISaveDataManager
{
	// Token: 0x06004E5D RID: 20061 RVA: 0x000027FC File Offset: 0x000009FC
	public void Init()
	{
	}

	// Token: 0x06004E5E RID: 20062 RVA: 0x00010E62 File Offset: 0x0000F062
	public SaveDataWriteMode GetWriteMode()
	{
		return SaveDataWriteMode.None;
	}

	// Token: 0x06004E5F RID: 20063 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetWriteMode(SaveDataWriteMode writeMode)
	{
	}

	// Token: 0x06004E60 RID: 20064 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x06004E61 RID: 20065 RVA: 0x000027FC File Offset: 0x000009FC
	public void RegisterRegionFileManager(RegionFileManager regionFileManager)
	{
	}

	// Token: 0x06004E62 RID: 20066 RVA: 0x000027FC File Offset: 0x000009FC
	public void DeregisterRegionFileManager(RegionFileManager regionFileManager)
	{
	}

	// Token: 0x1700088A RID: 2186
	// (get) Token: 0x06004E63 RID: 20067 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool AppliesSaveSizeLimit
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004E64 RID: 20068 RVA: 0x00010E62 File Offset: 0x0000F062
	public int CommitAsync()
	{
		return 0;
	}

	// Token: 0x06004E65 RID: 20069 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsCommitPending(int token)
	{
		return false;
	}

	// Token: 0x06004E66 RID: 20070 RVA: 0x000027FC File Offset: 0x000009FC
	public void CommitSync()
	{
	}

	// Token: 0x06004E67 RID: 20071 RVA: 0x001E05E5 File Offset: 0x001DE7E5
	public IEnumerator CommitCoroutine(Action<double> progressFeedback = null)
	{
		yield break;
	}

	// Token: 0x1400007C RID: 124
	// (add) Token: 0x06004E68 RID: 20072 RVA: 0x000027FC File Offset: 0x000009FC
	// (remove) Token: 0x06004E69 RID: 20073 RVA: 0x000027FC File Offset: 0x000009FC
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
	// (add) Token: 0x06004E6A RID: 20074 RVA: 0x000027FC File Offset: 0x000009FC
	// (remove) Token: 0x06004E6B RID: 20075 RVA: 0x000027FC File Offset: 0x000009FC
	public event Action CommitFinished
	{
		add
		{
		}
		remove
		{
		}
	}

	// Token: 0x06004E6C RID: 20076 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ShouldLimitSize()
	{
		return false;
	}

	// Token: 0x06004E6D RID: 20077 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UpdateSizes()
	{
	}

	// Token: 0x06004E6E RID: 20078 RVA: 0x001E05F0 File Offset: 0x001DE7F0
	public virtual SaveDataSizes GetSizes()
	{
		return default(SaveDataSizes);
	}

	// Token: 0x06004E6F RID: 20079 RVA: 0x001E0606 File Offset: 0x001DE806
	public virtual Stream ManagedFileOpen(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share)
	{
		return File.Open(path.GetOriginalPath(), mode, access, share);
	}

	// Token: 0x06004E70 RID: 20080 RVA: 0x001E0617 File Offset: 0x001DE817
	public virtual void ManagedFileDelete(SaveDataManagedPath path)
	{
		File.Delete(path.GetOriginalPath());
	}

	// Token: 0x06004E71 RID: 20081 RVA: 0x001E0624 File Offset: 0x001DE824
	public virtual bool ManagedFileExists(SaveDataManagedPath path)
	{
		return File.Exists(path.GetOriginalPath());
	}

	// Token: 0x06004E72 RID: 20082 RVA: 0x001E0631 File Offset: 0x001DE831
	public virtual DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return File.GetLastWriteTimeUtc(path.GetOriginalPath());
	}

	// Token: 0x06004E73 RID: 20083 RVA: 0x001E063E File Offset: 0x001DE83E
	public virtual SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
	{
		return new SdDirectoryInfo(Directory.CreateDirectory(path.GetOriginalPath()));
	}

	// Token: 0x06004E74 RID: 20084 RVA: 0x001E0650 File Offset: 0x001DE850
	public virtual DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return Directory.GetLastWriteTimeUtc(path.GetOriginalPath());
	}

	// Token: 0x06004E75 RID: 20085 RVA: 0x001E065D File Offset: 0x001DE85D
	public virtual bool ManagedDirectoryExists(SaveDataManagedPath path)
	{
		return Directory.Exists(path.GetOriginalPath());
	}

	// Token: 0x06004E76 RID: 20086 RVA: 0x001E066A File Offset: 0x001DE86A
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.ConvertPathsToManagedPaths(Directory.EnumerateDirectories(path.GetOriginalPath(), searchPattern, searchOption));
	}

	// Token: 0x06004E77 RID: 20087 RVA: 0x001E067E File Offset: 0x001DE87E
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.ConvertPathsToManagedPaths(Directory.EnumerateFiles(path.GetOriginalPath(), searchPattern, searchOption));
	}

	// Token: 0x06004E78 RID: 20088 RVA: 0x001E0692 File Offset: 0x001DE892
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.ConvertPathsToManagedPaths(Directory.EnumerateFileSystemEntries(path.GetOriginalPath(), searchPattern, searchOption));
	}

	// Token: 0x06004E79 RID: 20089 RVA: 0x001E06A6 File Offset: 0x001DE8A6
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

	// Token: 0x06004E7A RID: 20090 RVA: 0x001E06B6 File Offset: 0x001DE8B6
	public virtual void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive)
	{
		Directory.Delete(path.GetOriginalPath(), recursive);
	}

	// Token: 0x06004E7B RID: 20091 RVA: 0x001E06C4 File Offset: 0x001DE8C4
	public long ManagedFileInfoLength(SaveDataManagedPath path)
	{
		return new FileInfo(path.GetOriginalPath()).Length;
	}

	// Token: 0x06004E7C RID: 20092 RVA: 0x001E06D6 File Offset: 0x001DE8D6
	public virtual IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return from x in new DirectoryInfo(path.GetOriginalPath()).EnumerateDirectories(searchPattern, searchOption)
			select new SdDirectoryInfo(x);
	}

	// Token: 0x06004E7D RID: 20093 RVA: 0x001E070E File Offset: 0x001DE90E
	public virtual IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return from x in new DirectoryInfo(path.GetOriginalPath()).EnumerateFiles(searchPattern, searchOption)
			select new SdFileInfo(x);
	}

	// Token: 0x06004E7E RID: 20094 RVA: 0x001E0746 File Offset: 0x001DE946
	public virtual IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return new DirectoryInfo(path.GetOriginalPath()).EnumerateFileSystemInfos(searchPattern, searchOption).Select<FileSystemInfo, SdFileSystemInfo>(new Func<FileSystemInfo, SdFileSystemInfo>(SdDirectoryInfo.WrapFileSystemInfo));
	}

	// Token: 0x04003CC4 RID: 15556
	public static readonly SaveDataManager_Placeholder Instance = new SaveDataManager_Placeholder();
}
