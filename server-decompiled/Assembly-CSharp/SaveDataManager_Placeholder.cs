using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Token: 0x02000A2A RID: 2602
public class SaveDataManager_Placeholder : ISaveDataManager
{
	// Token: 0x06004E8E RID: 20110 RVA: 0x000027FC File Offset: 0x000009FC
	public void Init()
	{
	}

	// Token: 0x06004E8F RID: 20111 RVA: 0x00010E62 File Offset: 0x0000F062
	public SaveDataWriteMode GetWriteMode()
	{
		return SaveDataWriteMode.None;
	}

	// Token: 0x06004E90 RID: 20112 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetWriteMode(SaveDataWriteMode writeMode)
	{
	}

	// Token: 0x06004E91 RID: 20113 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x06004E92 RID: 20114 RVA: 0x000027FC File Offset: 0x000009FC
	public void RegisterRegionFileManager(RegionFileManager regionFileManager)
	{
	}

	// Token: 0x06004E93 RID: 20115 RVA: 0x000027FC File Offset: 0x000009FC
	public void DeregisterRegionFileManager(RegionFileManager regionFileManager)
	{
	}

	// Token: 0x1700086A RID: 2154
	// (get) Token: 0x06004E94 RID: 20116 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool AppliesSaveSizeLimit
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004E95 RID: 20117 RVA: 0x00010E62 File Offset: 0x0000F062
	public int CommitAsync()
	{
		return 0;
	}

	// Token: 0x06004E96 RID: 20118 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsCommitPending(int token)
	{
		return false;
	}

	// Token: 0x06004E97 RID: 20119 RVA: 0x000027FC File Offset: 0x000009FC
	public void CommitSync()
	{
	}

	// Token: 0x06004E98 RID: 20120 RVA: 0x001DF335 File Offset: 0x001DD535
	public IEnumerator CommitCoroutine(Action<double> progressFeedback = null)
	{
		yield break;
	}

	// Token: 0x1400007B RID: 123
	// (add) Token: 0x06004E99 RID: 20121 RVA: 0x000027FC File Offset: 0x000009FC
	// (remove) Token: 0x06004E9A RID: 20122 RVA: 0x000027FC File Offset: 0x000009FC
	public event Action CommitStarted
	{
		add
		{
		}
		remove
		{
		}
	}

	// Token: 0x1400007C RID: 124
	// (add) Token: 0x06004E9B RID: 20123 RVA: 0x000027FC File Offset: 0x000009FC
	// (remove) Token: 0x06004E9C RID: 20124 RVA: 0x000027FC File Offset: 0x000009FC
	public event Action CommitFinished
	{
		add
		{
		}
		remove
		{
		}
	}

	// Token: 0x06004E9D RID: 20125 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ShouldLimitSize()
	{
		return false;
	}

	// Token: 0x06004E9E RID: 20126 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UpdateSizes()
	{
	}

	// Token: 0x06004E9F RID: 20127 RVA: 0x001DF340 File Offset: 0x001DD540
	public virtual SaveDataSizes GetSizes()
	{
		return default(SaveDataSizes);
	}

	// Token: 0x06004EA0 RID: 20128 RVA: 0x001DF356 File Offset: 0x001DD556
	public virtual Stream ManagedFileOpen(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share)
	{
		return File.Open(path.GetOriginalPath(), mode, access, share);
	}

	// Token: 0x06004EA1 RID: 20129 RVA: 0x001DF367 File Offset: 0x001DD567
	public virtual void ManagedFileDelete(SaveDataManagedPath path)
	{
		File.Delete(path.GetOriginalPath());
	}

	// Token: 0x06004EA2 RID: 20130 RVA: 0x001DF374 File Offset: 0x001DD574
	public virtual bool ManagedFileExists(SaveDataManagedPath path)
	{
		return File.Exists(path.GetOriginalPath());
	}

	// Token: 0x06004EA3 RID: 20131 RVA: 0x001DF381 File Offset: 0x001DD581
	public virtual DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return File.GetLastWriteTimeUtc(path.GetOriginalPath());
	}

	// Token: 0x06004EA4 RID: 20132 RVA: 0x001DF38E File Offset: 0x001DD58E
	public virtual SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
	{
		return new SdDirectoryInfo(Directory.CreateDirectory(path.GetOriginalPath()));
	}

	// Token: 0x06004EA5 RID: 20133 RVA: 0x001DF3A0 File Offset: 0x001DD5A0
	public virtual DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return Directory.GetLastWriteTimeUtc(path.GetOriginalPath());
	}

	// Token: 0x06004EA6 RID: 20134 RVA: 0x001DF3AD File Offset: 0x001DD5AD
	public virtual bool ManagedDirectoryExists(SaveDataManagedPath path)
	{
		return Directory.Exists(path.GetOriginalPath());
	}

	// Token: 0x06004EA7 RID: 20135 RVA: 0x001DF3BA File Offset: 0x001DD5BA
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.ConvertPathsToManagedPaths(Directory.EnumerateDirectories(path.GetOriginalPath(), searchPattern, searchOption));
	}

	// Token: 0x06004EA8 RID: 20136 RVA: 0x001DF3CE File Offset: 0x001DD5CE
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.ConvertPathsToManagedPaths(Directory.EnumerateFiles(path.GetOriginalPath(), searchPattern, searchOption));
	}

	// Token: 0x06004EA9 RID: 20137 RVA: 0x001DF3E2 File Offset: 0x001DD5E2
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.ConvertPathsToManagedPaths(Directory.EnumerateFileSystemEntries(path.GetOriginalPath(), searchPattern, searchOption));
	}

	// Token: 0x06004EAA RID: 20138 RVA: 0x001DF3F6 File Offset: 0x001DD5F6
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

	// Token: 0x06004EAB RID: 20139 RVA: 0x001DF406 File Offset: 0x001DD606
	public virtual void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive)
	{
		Directory.Delete(path.GetOriginalPath(), recursive);
	}

	// Token: 0x06004EAC RID: 20140 RVA: 0x001DF414 File Offset: 0x001DD614
	public long ManagedFileInfoLength(SaveDataManagedPath path)
	{
		return new FileInfo(path.GetOriginalPath()).Length;
	}

	// Token: 0x06004EAD RID: 20141 RVA: 0x001DF426 File Offset: 0x001DD626
	public virtual IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return from x in new DirectoryInfo(path.GetOriginalPath()).EnumerateDirectories(searchPattern, searchOption)
			select new SdDirectoryInfo(x);
	}

	// Token: 0x06004EAE RID: 20142 RVA: 0x001DF45E File Offset: 0x001DD65E
	public virtual IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return from x in new DirectoryInfo(path.GetOriginalPath()).EnumerateFiles(searchPattern, searchOption)
			select new SdFileInfo(x);
	}

	// Token: 0x06004EAF RID: 20143 RVA: 0x001DF496 File Offset: 0x001DD696
	public virtual IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return new DirectoryInfo(path.GetOriginalPath()).EnumerateFileSystemInfos(searchPattern, searchOption).Select<FileSystemInfo, SdFileSystemInfo>(new Func<FileSystemInfo, SdFileSystemInfo>(SdDirectoryInfo.WrapFileSystemInfo));
	}

	// Token: 0x04003CB7 RID: 15543
	public static readonly SaveDataManager_Placeholder Instance = new SaveDataManager_Placeholder();
}
