using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Platform;

// Token: 0x02000A1D RID: 2589
public class SaveDataMergedPlatformSaveGameIOProvider : IPlatformSaveGameProvider, IPlatformSaveGameIOProvider
{
	// Token: 0x06004E3E RID: 20030 RVA: 0x001DCBDF File Offset: 0x001DADDF
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogInfo(string text)
	{
		Log.Out("[SaveDataMergedPlatformSaveGameIOProvider] " + text);
	}

	// Token: 0x06004E3F RID: 20031 RVA: 0x001DCBF4 File Offset: 0x001DADF4
	[PublicizedFrom(EAccessModifier.Private)]
	public static Regex CreateCachedManagedPathRegex()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("^(?:");
		stringBuilder.Append("(?:");
		stringBuilder.Append("(?:");
		stringBuilder.Append("(?:");
		stringBuilder.Append("Saves");
		stringBuilder.Append("[/]");
		stringBuilder.Append("[^/]+");
		stringBuilder.Append("[/]");
		stringBuilder.Append("[^/]+");
		stringBuilder.Append(')');
		stringBuilder.Append('|');
		stringBuilder.Append("(?:");
		stringBuilder.Append("SavesLocal");
		stringBuilder.Append("[/]");
		stringBuilder.Append("[^/]+");
		stringBuilder.Append(')');
		stringBuilder.Append(')');
		stringBuilder.Append("[/]");
		stringBuilder.Append("DynamicMeshes");
		stringBuilder.Append(')');
		stringBuilder.Append('|');
		stringBuilder.Append(Regex.Escape("CacheOwner.txt"));
		stringBuilder.Append('|');
		stringBuilder.Append(Regex.Escape("GeneratedWorlds"));
		stringBuilder.Append('/');
		stringBuilder.Append("[^/]+?");
		stringBuilder.Append('/');
		stringBuilder.Append("(?:");
		stringBuilder.Append(Regex.Escape("checksums.txt"));
		stringBuilder.Append('|');
		stringBuilder.Append("[^/]+?(?:");
		stringBuilder.Append(Regex.Escape("_processed"));
		stringBuilder.Append('|');
		stringBuilder.Append(Regex.Escape("_half"));
		stringBuilder.Append(")\\.(?:");
		stringBuilder.Append(Regex.Escape("raw"));
		stringBuilder.Append('|');
		stringBuilder.Append(Regex.Escape("png"));
		stringBuilder.Append('|');
		stringBuilder.Append(Regex.Escape("tga"));
		stringBuilder.Append(')');
		stringBuilder.Append(')');
		stringBuilder.Append(')');
		stringBuilder.Append("(?:");
		stringBuilder.Append(Regex.Escape(".bup"));
		stringBuilder.Append(")*");
		stringBuilder.Append("(?:/|$)");
		return new Regex(stringBuilder.ToString(), RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);
	}

	// Token: 0x06004E40 RID: 20032 RVA: 0x001DCE48 File Offset: 0x001DB048
	public SaveDataMergedPlatformSaveGameIOProvider(IPlatformSaveGameProvider main)
	{
		this.m_main = main;
		this.m_cache = main.Cache;
		try
		{
			this.InitialSaveCacheSync();
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
		}
	}

	// Token: 0x06004E41 RID: 20033 RVA: 0x001DCE90 File Offset: 0x001DB090
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsCached(SaveDataManagedPath path)
	{
		return this.m_cache != null && SaveDataMergedPlatformSaveGameIOProvider.s_cachedManagedPathRegex.IsMatch(path.PathRelativeToRoot);
	}

	// Token: 0x06004E42 RID: 20034 RVA: 0x001DCEAC File Offset: 0x001DB0AC
	[PublicizedFrom(EAccessModifier.Private)]
	public IPlatformSaveGameIOProvider GetIOProvider(SaveDataManagedPath path)
	{
		if (!this.IsCached(path))
		{
			return this.m_main;
		}
		return this.m_cache;
	}

	// Token: 0x06004E43 RID: 20035 RVA: 0x001DCED4 File Offset: 0x001DB0D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SyncParentDirectories(SaveDataManagedPath path)
	{
		if (!this.IsCached(path))
		{
			return;
		}
		if (this.m_cache.ManagedFileExists(path) || this.m_cache.ManagedDirectoryExists(path))
		{
			return;
		}
		SaveDataManagedPath saveDataManagedPath;
		if (!path.TryGetParentPath(out saveDataManagedPath) || saveDataManagedPath.PathRelativeToRoot.Length <= 0)
		{
			return;
		}
		if (this.m_cache.ManagedDirectoryExists(saveDataManagedPath))
		{
			return;
		}
		if (!this.m_main.ManagedDirectoryExists(saveDataManagedPath))
		{
			return;
		}
		this.m_cache.ManagedDirectoryCreateDirectory(saveDataManagedPath);
	}

	// Token: 0x06004E44 RID: 20036 RVA: 0x001DCF4C File Offset: 0x001DB14C
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitialSaveCacheSync()
	{
		if (this.m_cache == null)
		{
			return;
		}
		using (MemoryStream memoryStream = new MemoryStream())
		{
			this.InitEnsureSameCacheUser(memoryStream);
			this.InitMoveToCache(memoryStream);
			this.InitRemoveInvalidCachedFiles();
			this.InitRemoveCachedGeneratedWorlds();
			this.InitRemoveEmptyCacheDirectories();
		}
	}

	// Token: 0x06004E45 RID: 20037 RVA: 0x001DCFA4 File Offset: 0x001DB1A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitEnsureSameCacheUser(MemoryStream buffer)
	{
		SaveDataMergedPlatformSaveGameIOProvider.<>c__DisplayClass12_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.buffer = buffer;
		CS$<>8__locals1.cacheOwnerPath = new SaveDataManagedPath("CacheOwner.txt");
		CS$<>8__locals1.saveOwnerPath = new SaveDataManagedPath("SaveOwner.txt");
		if (!this.m_main.ManagedFileExists(CS$<>8__locals1.saveOwnerPath))
		{
			SaveDataMergedPlatformSaveGameIOProvider.LogInfo("Cache needs a reset because the save owner is missing.");
			this.<InitEnsureSameCacheUser>g__ResetCache|12_0(ref CS$<>8__locals1);
			return;
		}
		if (!this.m_cache.ManagedFileExists(CS$<>8__locals1.cacheOwnerPath))
		{
			SaveDataMergedPlatformSaveGameIOProvider.LogInfo("Cache needs a reset because the cache owner is missing.");
			this.<InitEnsureSameCacheUser>g__ResetCache|12_0(ref CS$<>8__locals1);
			return;
		}
		CS$<>8__locals1.buffer.Position = 0L;
		CS$<>8__locals1.buffer.SetLength(0L);
		this.m_main.ManagedFileRead(CS$<>8__locals1.saveOwnerPath, CS$<>8__locals1.buffer);
		CS$<>8__locals1.buffer.Position = 0L;
		string text;
		using (StreamReader streamReader = new StreamReader(CS$<>8__locals1.buffer, Encoding.UTF8, true, 1024, true))
		{
			text = streamReader.ReadToEnd();
		}
		CS$<>8__locals1.buffer.Position = 0L;
		CS$<>8__locals1.buffer.SetLength(0L);
		this.m_cache.ManagedFileRead(CS$<>8__locals1.cacheOwnerPath, CS$<>8__locals1.buffer);
		CS$<>8__locals1.buffer.Position = 0L;
		string text2;
		using (StreamReader streamReader2 = new StreamReader(CS$<>8__locals1.buffer, Encoding.UTF8, true, 1024, true))
		{
			text2 = streamReader2.ReadToEnd();
		}
		if (text == text2)
		{
			return;
		}
		SaveDataMergedPlatformSaveGameIOProvider.LogInfo(string.Concat(new string[] { "Cache needs a reset because the save owner '", text, "' does not match the cache owner '", text2, "'." }));
		this.<InitEnsureSameCacheUser>g__ResetCache|12_0(ref CS$<>8__locals1);
	}

	// Token: 0x06004E46 RID: 20038 RVA: 0x001DD164 File Offset: 0x001DB364
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitMoveToCache(MemoryStream buffer)
	{
		foreach (SaveDataManagedPath saveDataManagedPath in this.m_main.ManagedDirectoryEnumerateFiles(SaveDataManagedPath.RootPath, "*", SearchOption.AllDirectories))
		{
			if (this.IsCached(saveDataManagedPath))
			{
				SaveDataManagedPath saveDataManagedPath2;
				if (saveDataManagedPath.TryGetParentPath(out saveDataManagedPath2))
				{
					this.m_cache.ManagedDirectoryCreateDirectory(saveDataManagedPath2);
				}
				buffer.Position = 0L;
				buffer.SetLength(0L);
				this.m_main.ManagedFileRead(saveDataManagedPath, buffer);
				buffer.Position = 0L;
				this.m_cache.ManagedFileWrite(saveDataManagedPath, buffer);
				this.m_main.ManagedFileDelete(saveDataManagedPath);
				SaveDataMergedPlatformSaveGameIOProvider.LogInfo(string.Format("Moved from save data to cache: {0}", saveDataManagedPath));
			}
		}
		foreach (SaveDataManagedPath saveDataManagedPath3 in from p in this.m_main.ManagedDirectoryEnumerateDirectories(SaveDataManagedPath.RootPath, "*", SearchOption.AllDirectories)
			orderby p.PathRelativeToRoot.Count<char>((char c) => c == '/') descending
			select p)
		{
			if (this.IsCached(saveDataManagedPath3) && this.m_main.ManagedDirectoryExists(saveDataManagedPath3) && !this.m_main.ManagedDirectoryEnumerateFileSystemEntries(saveDataManagedPath3, "*", SearchOption.TopDirectoryOnly).Any<SaveDataManagedPath>())
			{
				this.m_main.ManagedDirectoryDelete(saveDataManagedPath3, false);
				SaveDataMergedPlatformSaveGameIOProvider.LogInfo(string.Format("Deleted cache directory remaining in main: {0}", saveDataManagedPath3));
			}
		}
	}

	// Token: 0x06004E47 RID: 20039 RVA: 0x001DD2E0 File Offset: 0x001DB4E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitRemoveInvalidCachedFiles()
	{
		foreach (SaveDataManagedPath saveDataManagedPath in this.m_cache.ManagedDirectoryEnumerateFiles(SaveDataManagedPath.RootPath, "*", SearchOption.AllDirectories))
		{
			if (!this.IsCached(saveDataManagedPath))
			{
				this.m_cache.ManagedFileDelete(saveDataManagedPath);
				SaveDataMergedPlatformSaveGameIOProvider.LogInfo(string.Format("Deleted invalid cached file: {0}", saveDataManagedPath));
			}
		}
	}

	// Token: 0x06004E48 RID: 20040 RVA: 0x001DD35C File Offset: 0x001DB55C
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitRemoveCachedGeneratedWorlds()
	{
		SaveDataManagedPath saveDataManagedPath = new SaveDataManagedPath("GeneratedWorlds");
		if (!this.m_cache.ManagedDirectoryExists(saveDataManagedPath))
		{
			return;
		}
		foreach (SaveDataManagedPath saveDataManagedPath2 in this.m_cache.ManagedDirectoryEnumerateDirectories(saveDataManagedPath, "*", SearchOption.TopDirectoryOnly))
		{
			if (!this.m_main.ManagedDirectoryExists(saveDataManagedPath2))
			{
				this.m_cache.ManagedDirectoryDelete(saveDataManagedPath2, true);
				SaveDataMergedPlatformSaveGameIOProvider.LogInfo(string.Format("Delete cache for non-existent generated world: {0}", saveDataManagedPath2));
			}
		}
	}

	// Token: 0x06004E49 RID: 20041 RVA: 0x001DD3F4 File Offset: 0x001DB5F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitRemoveEmptyCacheDirectories()
	{
		foreach (SaveDataManagedPath saveDataManagedPath in from p in this.m_cache.ManagedDirectoryEnumerateDirectories(SaveDataManagedPath.RootPath, "*", SearchOption.AllDirectories)
			orderby p.PathRelativeToRoot.Count<char>((char c) => c == '/') descending
			select p)
		{
			if (this.m_cache.ManagedDirectoryExists(saveDataManagedPath) && !this.m_cache.ManagedDirectoryEnumerateFileSystemEntries(saveDataManagedPath, "*", SearchOption.TopDirectoryOnly).Any<SaveDataManagedPath>())
			{
				this.m_cache.ManagedDirectoryDelete(saveDataManagedPath, false);
				SaveDataMergedPlatformSaveGameIOProvider.LogInfo(string.Format("Deleted empty cache directory: {0}", saveDataManagedPath));
			}
		}
	}

	// Token: 0x1700085D RID: 2141
	// (get) Token: 0x06004E4A RID: 20042 RVA: 0x001DD4B4 File Offset: 0x001DB6B4
	public ESaveGameProviderStatus Status
	{
		get
		{
			return this.m_main.Status;
		}
	}

	// Token: 0x1700085E RID: 2142
	// (get) Token: 0x06004E4B RID: 20043 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public IPlatformSaveGameIOProvider Cache
	{
		get
		{
			return null;
		}
	}

	// Token: 0x1400007D RID: 125
	// (add) Token: 0x06004E4C RID: 20044 RVA: 0x001DD4C1 File Offset: 0x001DB6C1
	// (remove) Token: 0x06004E4D RID: 20045 RVA: 0x001DD4CF File Offset: 0x001DB6CF
	public event Action Initialized
	{
		add
		{
			this.m_main.Initialized += value;
		}
		remove
		{
			this.m_main.Initialized -= value;
		}
	}

	// Token: 0x06004E4E RID: 20046 RVA: 0x001DD4DD File Offset: 0x001DB6DD
	public void Init(IPlatform _owner)
	{
		this.m_main.Init(_owner);
	}

	// Token: 0x06004E4F RID: 20047 RVA: 0x001DD4EB File Offset: 0x001DB6EB
	public void Destroy()
	{
		this.m_main.Destroy();
	}

	// Token: 0x06004E50 RID: 20048 RVA: 0x001DD4F8 File Offset: 0x001DB6F8
	public bool ShouldBackup()
	{
		return this.m_main.ShouldBackup();
	}

	// Token: 0x06004E51 RID: 20049 RVA: 0x001DD505 File Offset: 0x001DB705
	public bool ShouldCommit()
	{
		return this.m_main.ShouldCommit();
	}

	// Token: 0x06004E52 RID: 20050 RVA: 0x001DD512 File Offset: 0x001DB712
	public double GetCommitProgress()
	{
		return this.m_main.GetCommitProgress();
	}

	// Token: 0x06004E53 RID: 20051 RVA: 0x001DD51F File Offset: 0x001DB71F
	public void Flush(bool waitForFlush)
	{
		this.m_main.Flush(waitForFlush);
	}

	// Token: 0x06004E54 RID: 20052 RVA: 0x001DD52D File Offset: 0x001DB72D
	public bool ShouldLimitSize()
	{
		return this.m_main.ShouldLimitSize();
	}

	// Token: 0x06004E55 RID: 20053 RVA: 0x001DD53A File Offset: 0x001DB73A
	public void UpdateSizes()
	{
		this.m_main.UpdateSizes();
	}

	// Token: 0x06004E56 RID: 20054 RVA: 0x001DD547 File Offset: 0x001DB747
	public SaveDataSizes GetSizes()
	{
		return this.m_main.GetSizes();
	}

	// Token: 0x06004E57 RID: 20055 RVA: 0x001DD554 File Offset: 0x001DB754
	public void ManagedFileRead(SaveDataManagedPath path, Stream dest)
	{
		this.GetIOProvider(path).ManagedFileRead(path, dest);
	}

	// Token: 0x06004E58 RID: 20056 RVA: 0x001DD564 File Offset: 0x001DB764
	public void ManagedFileWrite(SaveDataManagedPath path, Stream src)
	{
		this.SyncParentDirectories(path);
		this.GetIOProvider(path).ManagedFileWrite(path, src);
	}

	// Token: 0x06004E59 RID: 20057 RVA: 0x001DD57C File Offset: 0x001DB77C
	public void ManagedFileCopy(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName, bool overwrite = false)
	{
		IPlatformSaveGameIOProvider ioprovider = this.GetIOProvider(sourceFileName);
		IPlatformSaveGameIOProvider ioprovider2 = this.GetIOProvider(destFileName);
		if (ioprovider == ioprovider2)
		{
			ioprovider.ManagedFileCopy(sourceFileName, destFileName, overwrite);
			return;
		}
		if (!overwrite && ioprovider2.ManagedFileExists(destFileName))
		{
			throw new IOException(string.Format("File already exists at '{0}'.", destFileName));
		}
		this.SyncParentDirectories(destFileName);
		using (MemoryStream memoryStream = new MemoryStream())
		{
			ioprovider.ManagedFileRead(sourceFileName, memoryStream);
			memoryStream.Position = 0L;
			ioprovider2.ManagedFileWrite(destFileName, memoryStream);
		}
	}

	// Token: 0x06004E5A RID: 20058 RVA: 0x001DD608 File Offset: 0x001DB808
	public void ManagedFileDelete(SaveDataManagedPath path)
	{
		this.GetIOProvider(path).ManagedFileDelete(path);
	}

	// Token: 0x06004E5B RID: 20059 RVA: 0x001DD617 File Offset: 0x001DB817
	public bool ManagedFileExists(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedFileExists(path);
	}

	// Token: 0x06004E5C RID: 20060 RVA: 0x001DD626 File Offset: 0x001DB826
	public DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedFileGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004E5D RID: 20061 RVA: 0x001DD638 File Offset: 0x001DB838
	public void ManagedFileMove(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName)
	{
		IPlatformSaveGameIOProvider ioprovider = this.GetIOProvider(sourceFileName);
		IPlatformSaveGameIOProvider ioprovider2 = this.GetIOProvider(destFileName);
		if (ioprovider == ioprovider2)
		{
			ioprovider.ManagedFileMove(sourceFileName, destFileName);
			return;
		}
		if (ioprovider2.ManagedFileExists(destFileName))
		{
			throw new IOException(string.Format("File already exists at '{0}'.", destFileName));
		}
		this.SyncParentDirectories(destFileName);
		using (MemoryStream memoryStream = new MemoryStream())
		{
			ioprovider.ManagedFileRead(sourceFileName, memoryStream);
			memoryStream.Position = 0L;
			ioprovider2.ManagedFileWrite(destFileName, memoryStream);
		}
		ioprovider.ManagedFileDelete(sourceFileName);
	}

	// Token: 0x06004E5E RID: 20062 RVA: 0x001DD6C4 File Offset: 0x001DB8C4
	public SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedDirectoryCreateDirectory(path);
	}

	// Token: 0x06004E5F RID: 20063 RVA: 0x001DD6D3 File Offset: 0x001DB8D3
	public DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedDirectoryGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004E60 RID: 20064 RVA: 0x001DD6E2 File Offset: 0x001DB8E2
	public bool ManagedDirectoryExists(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedDirectoryExists(path);
	}

	// Token: 0x06004E61 RID: 20065 RVA: 0x001DD6F4 File Offset: 0x001DB8F4
	public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryEnumerateInternal(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryEnumerateDirectories(path, searchPattern, searchOption));
	}

	// Token: 0x06004E62 RID: 20066 RVA: 0x001DD734 File Offset: 0x001DB934
	public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryEnumerateInternal(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryEnumerateFiles(path, searchPattern, searchOption));
	}

	// Token: 0x06004E63 RID: 20067 RVA: 0x001DD774 File Offset: 0x001DB974
	public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryEnumerateInternal(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryEnumerateFileSystemEntries(path, searchPattern, searchOption));
	}

	// Token: 0x06004E64 RID: 20068 RVA: 0x001DD7B4 File Offset: 0x001DB9B4
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateInternal(SaveDataManagedPath path, Func<IPlatformSaveGameIOProvider, IEnumerable<SaveDataManagedPath>> getEnumerable)
	{
		SaveDataMergedPlatformSaveGameIOProvider.<>c__DisplayClass46_0 CS$<>8__locals1 = new SaveDataMergedPlatformSaveGameIOProvider.<>c__DisplayClass46_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.getEnumerable = getEnumerable;
		if (this.m_cache == null)
		{
			return CS$<>8__locals1.getEnumerable(this.m_main);
		}
		if (this.IsCached(path))
		{
			return CS$<>8__locals1.getEnumerable(this.m_cache).Where<SaveDataManagedPath>(new Func<SaveDataManagedPath, bool>(this.IsCached));
		}
		if (!this.m_cache.ManagedDirectoryExists(path))
		{
			return from p in CS$<>8__locals1.getEnumerable(this.m_main)
				where !CS$<>8__locals1.<>4__this.IsCached(p)
				select p;
		}
		if (!this.m_main.ManagedDirectoryExists(path))
		{
			throw new IOException(string.Format("Directory does not exist at '{0}'.", path));
		}
		return CS$<>8__locals1.<ManagedDirectoryEnumerateInternal>g__EnumerateBoth|1();
	}

	// Token: 0x06004E65 RID: 20069 RVA: 0x001DD874 File Offset: 0x001DBA74
	public void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive)
	{
		if (this.IsCached(path))
		{
			this.m_cache.ManagedDirectoryDelete(path, recursive);
			return;
		}
		this.m_main.ManagedDirectoryDelete(path, recursive);
		if (this.m_cache != null && this.m_cache.ManagedDirectoryExists(path))
		{
			this.m_cache.ManagedDirectoryDelete(path, recursive);
		}
	}

	// Token: 0x06004E66 RID: 20070 RVA: 0x001DD8C8 File Offset: 0x001DBAC8
	public long ManagedFileInfoLength(SaveDataManagedPath path)
	{
		if (this.IsCached(path))
		{
			return 0L;
		}
		return this.m_main.ManagedFileInfoLength(path);
	}

	// Token: 0x06004E67 RID: 20071 RVA: 0x001DD8E4 File Offset: 0x001DBAE4
	public IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryInfoEnumerateInternal<SdDirectoryInfo>(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryInfoEnumerateDirectories(path, searchPattern, searchOption));
	}

	// Token: 0x06004E68 RID: 20072 RVA: 0x001DD924 File Offset: 0x001DBB24
	public IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryInfoEnumerateInternal<SdFileInfo>(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryInfoEnumerateFiles(path, searchPattern, searchOption));
	}

	// Token: 0x06004E69 RID: 20073 RVA: 0x001DD964 File Offset: 0x001DBB64
	public IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryInfoEnumerateInternal<SdFileSystemInfo>(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryInfoEnumerateFileSystemInfos(path, searchPattern, searchOption));
	}

	// Token: 0x06004E6A RID: 20074 RVA: 0x001DD9A4 File Offset: 0x001DBBA4
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerable<T> ManagedDirectoryInfoEnumerateInternal<T>(SaveDataManagedPath path, Func<IPlatformSaveGameIOProvider, IEnumerable<T>> getEnumerable) where T : SdFileSystemInfo
	{
		SaveDataMergedPlatformSaveGameIOProvider.<>c__DisplayClass52_0<T> CS$<>8__locals1 = new SaveDataMergedPlatformSaveGameIOProvider.<>c__DisplayClass52_0<T>();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.getEnumerable = getEnumerable;
		if (this.m_cache == null)
		{
			return CS$<>8__locals1.getEnumerable(this.m_main);
		}
		if (this.IsCached(path))
		{
			return from info in CS$<>8__locals1.getEnumerable(this.m_cache)
				where CS$<>8__locals1.<>4__this.IsCached(info.ManagedPath)
				select info;
		}
		if (!this.m_cache.ManagedDirectoryExists(path))
		{
			return from info in CS$<>8__locals1.getEnumerable(this.m_main)
				where !CS$<>8__locals1.<>4__this.IsCached(info.ManagedPath)
				select info;
		}
		if (!this.m_main.ManagedDirectoryExists(path))
		{
			throw new IOException(string.Format("Directory does not exist at '{0}'.", path));
		}
		return CS$<>8__locals1.<ManagedDirectoryInfoEnumerateInternal>g__EnumerateBoth|2();
	}

	// Token: 0x06004E6C RID: 20076 RVA: 0x001DDA70 File Offset: 0x001DBC70
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <InitEnsureSameCacheUser>g__ResetCache|12_0(ref SaveDataMergedPlatformSaveGameIOProvider.<>c__DisplayClass12_0 A_1)
	{
		foreach (SaveDataManagedPath saveDataManagedPath in this.m_cache.ManagedDirectoryEnumerateDirectories(SaveDataManagedPath.RootPath, "*", SearchOption.TopDirectoryOnly))
		{
			this.m_cache.ManagedDirectoryDelete(saveDataManagedPath, true);
		}
		foreach (SaveDataManagedPath saveDataManagedPath2 in this.m_cache.ManagedDirectoryEnumerateFiles(SaveDataManagedPath.RootPath, "*", SearchOption.TopDirectoryOnly))
		{
			this.m_cache.ManagedFileDelete(saveDataManagedPath2);
		}
		A_1.buffer.Position = 0L;
		A_1.buffer.SetLength(0L);
		if (!this.m_main.ManagedFileExists(A_1.saveOwnerPath))
		{
			using (StreamWriter streamWriter = new StreamWriter(A_1.buffer, Encoding.UTF8, 1024, true))
			{
				streamWriter.Write(Guid.NewGuid());
			}
			A_1.buffer.Position = 0L;
			this.m_main.ManagedFileWrite(A_1.saveOwnerPath, A_1.buffer);
		}
		else
		{
			this.m_main.ManagedFileRead(A_1.saveOwnerPath, A_1.buffer);
		}
		A_1.buffer.Position = 0L;
		this.m_cache.ManagedFileWrite(A_1.cacheOwnerPath, A_1.buffer);
	}

	// Token: 0x04003C32 RID: 15410
	[PublicizedFrom(EAccessModifier.Private)]
	public const string CACHE_OWNER_FILENAME = "CacheOwner.txt";

	// Token: 0x04003C33 RID: 15411
	[PublicizedFrom(EAccessModifier.Private)]
	public const string SAVE_OWNER_FILENAME = "SaveOwner.txt";

	// Token: 0x04003C34 RID: 15412
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex s_cachedManagedPathRegex = SaveDataMergedPlatformSaveGameIOProvider.CreateCachedManagedPathRegex();

	// Token: 0x04003C35 RID: 15413
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IPlatformSaveGameProvider m_main;

	// Token: 0x04003C36 RID: 15414
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IPlatformSaveGameIOProvider m_cache;
}
