using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Platform;

// Token: 0x02000A38 RID: 2616
public class SaveDataMergedPlatformSaveGameIOProvider : IPlatformSaveGameProvider, IPlatformSaveGameIOProvider
{
	// Token: 0x06004E93 RID: 20115 RVA: 0x001E0927 File Offset: 0x001DEB27
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogInfo(string text)
	{
		Log.Out("[SaveDataMergedPlatformSaveGameIOProvider] " + text);
	}

	// Token: 0x06004E94 RID: 20116 RVA: 0x001E093C File Offset: 0x001DEB3C
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

	// Token: 0x06004E95 RID: 20117 RVA: 0x001E0B90 File Offset: 0x001DED90
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

	// Token: 0x06004E96 RID: 20118 RVA: 0x001E0BD8 File Offset: 0x001DEDD8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsCached(SaveDataManagedPath path)
	{
		return this.m_cache != null && SaveDataMergedPlatformSaveGameIOProvider.s_cachedManagedPathRegex.IsMatch(path.PathRelativeToRoot);
	}

	// Token: 0x06004E97 RID: 20119 RVA: 0x001E0BF4 File Offset: 0x001DEDF4
	[PublicizedFrom(EAccessModifier.Private)]
	public IPlatformSaveGameIOProvider GetIOProvider(SaveDataManagedPath path)
	{
		if (!this.IsCached(path))
		{
			return this.m_main;
		}
		return this.m_cache;
	}

	// Token: 0x06004E98 RID: 20120 RVA: 0x001E0C1C File Offset: 0x001DEE1C
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

	// Token: 0x06004E99 RID: 20121 RVA: 0x001E0C94 File Offset: 0x001DEE94
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

	// Token: 0x06004E9A RID: 20122 RVA: 0x001E0CEC File Offset: 0x001DEEEC
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

	// Token: 0x06004E9B RID: 20123 RVA: 0x001E0EAC File Offset: 0x001DF0AC
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

	// Token: 0x06004E9C RID: 20124 RVA: 0x001E1028 File Offset: 0x001DF228
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

	// Token: 0x06004E9D RID: 20125 RVA: 0x001E10A4 File Offset: 0x001DF2A4
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

	// Token: 0x06004E9E RID: 20126 RVA: 0x001E113C File Offset: 0x001DF33C
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

	// Token: 0x1700088F RID: 2191
	// (get) Token: 0x06004E9F RID: 20127 RVA: 0x001E11FC File Offset: 0x001DF3FC
	public ESaveGameProviderStatus Status
	{
		get
		{
			return this.m_main.Status;
		}
	}

	// Token: 0x17000890 RID: 2192
	// (get) Token: 0x06004EA0 RID: 20128 RVA: 0x000200DA File Offset: 0x0001E2DA
	public IPlatformSaveGameIOProvider Cache
	{
		get
		{
			return null;
		}
	}

	// Token: 0x1400007E RID: 126
	// (add) Token: 0x06004EA1 RID: 20129 RVA: 0x001E1209 File Offset: 0x001DF409
	// (remove) Token: 0x06004EA2 RID: 20130 RVA: 0x001E1217 File Offset: 0x001DF417
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

	// Token: 0x06004EA3 RID: 20131 RVA: 0x001E1225 File Offset: 0x001DF425
	public void Init(IPlatform _owner)
	{
		this.m_main.Init(_owner);
	}

	// Token: 0x06004EA4 RID: 20132 RVA: 0x001E1233 File Offset: 0x001DF433
	public void Destroy()
	{
		this.m_main.Destroy();
	}

	// Token: 0x06004EA5 RID: 20133 RVA: 0x001E1240 File Offset: 0x001DF440
	public bool ShouldBackup()
	{
		return this.m_main.ShouldBackup();
	}

	// Token: 0x06004EA6 RID: 20134 RVA: 0x001E124D File Offset: 0x001DF44D
	public bool ShouldCommit()
	{
		return this.m_main.ShouldCommit();
	}

	// Token: 0x06004EA7 RID: 20135 RVA: 0x001E125A File Offset: 0x001DF45A
	public double GetCommitProgress()
	{
		return this.m_main.GetCommitProgress();
	}

	// Token: 0x06004EA8 RID: 20136 RVA: 0x001E1267 File Offset: 0x001DF467
	public void Flush(bool waitForFlush)
	{
		this.m_main.Flush(waitForFlush);
	}

	// Token: 0x06004EA9 RID: 20137 RVA: 0x001E1275 File Offset: 0x001DF475
	public bool ShouldLimitSize()
	{
		return this.m_main.ShouldLimitSize();
	}

	// Token: 0x06004EAA RID: 20138 RVA: 0x001E1282 File Offset: 0x001DF482
	public void UpdateSizes()
	{
		this.m_main.UpdateSizes();
	}

	// Token: 0x06004EAB RID: 20139 RVA: 0x001E128F File Offset: 0x001DF48F
	public SaveDataSizes GetSizes()
	{
		return this.m_main.GetSizes();
	}

	// Token: 0x06004EAC RID: 20140 RVA: 0x001E129C File Offset: 0x001DF49C
	public void ManagedFileRead(SaveDataManagedPath path, Stream dest)
	{
		this.GetIOProvider(path).ManagedFileRead(path, dest);
	}

	// Token: 0x06004EAD RID: 20141 RVA: 0x001E12AC File Offset: 0x001DF4AC
	public void ManagedFileWrite(SaveDataManagedPath path, Stream src)
	{
		this.SyncParentDirectories(path);
		this.GetIOProvider(path).ManagedFileWrite(path, src);
	}

	// Token: 0x06004EAE RID: 20142 RVA: 0x001E12C4 File Offset: 0x001DF4C4
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

	// Token: 0x06004EAF RID: 20143 RVA: 0x001E1350 File Offset: 0x001DF550
	public void ManagedFileDelete(SaveDataManagedPath path)
	{
		this.GetIOProvider(path).ManagedFileDelete(path);
	}

	// Token: 0x06004EB0 RID: 20144 RVA: 0x001E135F File Offset: 0x001DF55F
	public bool ManagedFileExists(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedFileExists(path);
	}

	// Token: 0x06004EB1 RID: 20145 RVA: 0x001E136E File Offset: 0x001DF56E
	public DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedFileGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004EB2 RID: 20146 RVA: 0x001E1380 File Offset: 0x001DF580
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

	// Token: 0x06004EB3 RID: 20147 RVA: 0x001E140C File Offset: 0x001DF60C
	public SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedDirectoryCreateDirectory(path);
	}

	// Token: 0x06004EB4 RID: 20148 RVA: 0x001E141B File Offset: 0x001DF61B
	public DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedDirectoryGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004EB5 RID: 20149 RVA: 0x001E142A File Offset: 0x001DF62A
	public bool ManagedDirectoryExists(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedDirectoryExists(path);
	}

	// Token: 0x06004EB6 RID: 20150 RVA: 0x001E143C File Offset: 0x001DF63C
	public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryEnumerateInternal(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryEnumerateDirectories(path, searchPattern, searchOption));
	}

	// Token: 0x06004EB7 RID: 20151 RVA: 0x001E147C File Offset: 0x001DF67C
	public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryEnumerateInternal(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryEnumerateFiles(path, searchPattern, searchOption));
	}

	// Token: 0x06004EB8 RID: 20152 RVA: 0x001E14BC File Offset: 0x001DF6BC
	public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryEnumerateInternal(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryEnumerateFileSystemEntries(path, searchPattern, searchOption));
	}

	// Token: 0x06004EB9 RID: 20153 RVA: 0x001E14FC File Offset: 0x001DF6FC
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

	// Token: 0x06004EBA RID: 20154 RVA: 0x001E15BC File Offset: 0x001DF7BC
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

	// Token: 0x06004EBB RID: 20155 RVA: 0x001E1610 File Offset: 0x001DF810
	public long ManagedFileInfoLength(SaveDataManagedPath path)
	{
		if (this.IsCached(path))
		{
			return 0L;
		}
		return this.m_main.ManagedFileInfoLength(path);
	}

	// Token: 0x06004EBC RID: 20156 RVA: 0x001E162C File Offset: 0x001DF82C
	public IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryInfoEnumerateInternal<SdDirectoryInfo>(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryInfoEnumerateDirectories(path, searchPattern, searchOption));
	}

	// Token: 0x06004EBD RID: 20157 RVA: 0x001E166C File Offset: 0x001DF86C
	public IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryInfoEnumerateInternal<SdFileInfo>(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryInfoEnumerateFiles(path, searchPattern, searchOption));
	}

	// Token: 0x06004EBE RID: 20158 RVA: 0x001E16AC File Offset: 0x001DF8AC
	public IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryInfoEnumerateInternal<SdFileSystemInfo>(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryInfoEnumerateFileSystemInfos(path, searchPattern, searchOption));
	}

	// Token: 0x06004EBF RID: 20159 RVA: 0x001E16EC File Offset: 0x001DF8EC
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

	// Token: 0x06004EC1 RID: 20161 RVA: 0x001E17B8 File Offset: 0x001DF9B8
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

	// Token: 0x04003CD0 RID: 15568
	[PublicizedFrom(EAccessModifier.Private)]
	public const string CACHE_OWNER_FILENAME = "CacheOwner.txt";

	// Token: 0x04003CD1 RID: 15569
	[PublicizedFrom(EAccessModifier.Private)]
	public const string SAVE_OWNER_FILENAME = "SaveOwner.txt";

	// Token: 0x04003CD2 RID: 15570
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex s_cachedManagedPathRegex = SaveDataMergedPlatformSaveGameIOProvider.CreateCachedManagedPathRegex();

	// Token: 0x04003CD3 RID: 15571
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IPlatformSaveGameProvider m_main;

	// Token: 0x04003CD4 RID: 15572
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IPlatformSaveGameIOProvider m_cache;
}
