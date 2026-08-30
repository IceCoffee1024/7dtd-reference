using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Token: 0x02000A19 RID: 2585
public class SaveDataManager_Placeholder : ISaveDataManager
{
	// Token: 0x06004E08 RID: 19976 RVA: 0x000027FC File Offset: 0x000009FC
	public void Init()
	{
	}

	// Token: 0x06004E09 RID: 19977 RVA: 0x00010E62 File Offset: 0x0000F062
	public SaveDataWriteMode GetWriteMode()
	{
		return SaveDataWriteMode.None;
	}

	// Token: 0x06004E0A RID: 19978 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetWriteMode(SaveDataWriteMode writeMode)
	{
	}

	// Token: 0x06004E0B RID: 19979 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x06004E0C RID: 19980 RVA: 0x000027FC File Offset: 0x000009FC
	public void RegisterRegionFileManager(RegionFileManager regionFileManager)
	{
	}

	// Token: 0x06004E0D RID: 19981 RVA: 0x000027FC File Offset: 0x000009FC
	public void DeregisterRegionFileManager(RegionFileManager regionFileManager)
	{
	}

	// Token: 0x17000858 RID: 2136
	// (get) Token: 0x06004E0E RID: 19982 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool AppliesSaveSizeLimit
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004E0F RID: 19983 RVA: 0x00010E62 File Offset: 0x0000F062
	public int CommitAsync()
	{
		return 0;
	}

	// Token: 0x06004E10 RID: 19984 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsCommitPending(int token)
	{
		return false;
	}

	// Token: 0x06004E11 RID: 19985 RVA: 0x000027FC File Offset: 0x000009FC
	public void CommitSync()
	{
	}

	// Token: 0x06004E12 RID: 19986 RVA: 0x001DC89D File Offset: 0x001DAA9D
	public IEnumerator CommitCoroutine(Action<double> progressFeedback = null)
	{
		yield break;
	}

	// Token: 0x1400007B RID: 123
	// (add) Token: 0x06004E13 RID: 19987 RVA: 0x000027FC File Offset: 0x000009FC
	// (remove) Token: 0x06004E14 RID: 19988 RVA: 0x000027FC File Offset: 0x000009FC
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
	// (add) Token: 0x06004E15 RID: 19989 RVA: 0x000027FC File Offset: 0x000009FC
	// (remove) Token: 0x06004E16 RID: 19990 RVA: 0x000027FC File Offset: 0x000009FC
	public event Action CommitFinished
	{
		add
		{
		}
		remove
		{
		}
	}

	// Token: 0x06004E17 RID: 19991 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ShouldLimitSize()
	{
		return false;
	}

	// Token: 0x06004E18 RID: 19992 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UpdateSizes()
	{
	}

	// Token: 0x06004E19 RID: 19993 RVA: 0x001DC8A8 File Offset: 0x001DAAA8
	public virtual SaveDataSizes GetSizes()
	{
		return default(SaveDataSizes);
	}

	// Token: 0x06004E1A RID: 19994 RVA: 0x001DC8BE File Offset: 0x001DAABE
	public virtual Stream ManagedFileOpen(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share)
	{
		return File.Open(path.GetOriginalPath(), mode, access, share);
	}

	// Token: 0x06004E1B RID: 19995 RVA: 0x001DC8CF File Offset: 0x001DAACF
	public virtual void ManagedFileDelete(SaveDataManagedPath path)
	{
		File.Delete(path.GetOriginalPath());
	}

	// Token: 0x06004E1C RID: 19996 RVA: 0x001DC8DC File Offset: 0x001DAADC
	public virtual bool ManagedFileExists(SaveDataManagedPath path)
	{
		return File.Exists(path.GetOriginalPath());
	}

	// Token: 0x06004E1D RID: 19997 RVA: 0x001DC8E9 File Offset: 0x001DAAE9
	public virtual DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return File.GetLastWriteTimeUtc(path.GetOriginalPath());
	}

	// Token: 0x06004E1E RID: 19998 RVA: 0x001DC8F6 File Offset: 0x001DAAF6
	public virtual SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
	{
		return new SdDirectoryInfo(Directory.CreateDirectory(path.GetOriginalPath()));
	}

	// Token: 0x06004E1F RID: 19999 RVA: 0x001DC908 File Offset: 0x001DAB08
	public virtual DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return Directory.GetLastWriteTimeUtc(path.GetOriginalPath());
	}

	// Token: 0x06004E20 RID: 20000 RVA: 0x001DC915 File Offset: 0x001DAB15
	public virtual bool ManagedDirectoryExists(SaveDataManagedPath path)
	{
		return Directory.Exists(path.GetOriginalPath());
	}

	// Token: 0x06004E21 RID: 20001 RVA: 0x001DC922 File Offset: 0x001DAB22
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.ConvertPathsToManagedPaths(Directory.EnumerateDirectories(path.GetOriginalPath(), searchPattern, searchOption));
	}

	// Token: 0x06004E22 RID: 20002 RVA: 0x001DC936 File Offset: 0x001DAB36
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.ConvertPathsToManagedPaths(Directory.EnumerateFiles(path.GetOriginalPath(), searchPattern, searchOption));
	}

	// Token: 0x06004E23 RID: 20003 RVA: 0x001DC94A File Offset: 0x001DAB4A
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.ConvertPathsToManagedPaths(Directory.EnumerateFileSystemEntries(path.GetOriginalPath(), searchPattern, searchOption));
	}

	// Token: 0x06004E24 RID: 20004 RVA: 0x001DC95E File Offset: 0x001DAB5E
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

	// Token: 0x06004E25 RID: 20005 RVA: 0x001DC96E File Offset: 0x001DAB6E
	public virtual void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive)
	{
		Directory.Delete(path.GetOriginalPath(), recursive);
	}

	// Token: 0x06004E26 RID: 20006 RVA: 0x001DC97C File Offset: 0x001DAB7C
	public long ManagedFileInfoLength(SaveDataManagedPath path)
	{
		return new FileInfo(path.GetOriginalPath()).Length;
	}

	// Token: 0x06004E27 RID: 20007 RVA: 0x001DC98E File Offset: 0x001DAB8E
	public virtual IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return from x in new DirectoryInfo(path.GetOriginalPath()).EnumerateDirectories(searchPattern, searchOption)
			select new SdDirectoryInfo(x);
	}

	// Token: 0x06004E28 RID: 20008 RVA: 0x001DC9C6 File Offset: 0x001DABC6
	public virtual IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return from x in new DirectoryInfo(path.GetOriginalPath()).EnumerateFiles(searchPattern, searchOption)
			select new SdFileInfo(x);
	}

	// Token: 0x06004E29 RID: 20009 RVA: 0x001DC9FE File Offset: 0x001DABFE
	public virtual IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return new DirectoryInfo(path.GetOriginalPath()).EnumerateFileSystemInfos(searchPattern, searchOption).Select<FileSystemInfo, SdFileSystemInfo>(new Func<FileSystemInfo, SdFileSystemInfo>(SdDirectoryInfo.WrapFileSystemInfo));
	}

	// Token: 0x04003C26 RID: 15398
	public static readonly SaveDataManager_Placeholder Instance = new SaveDataManager_Placeholder();
}
