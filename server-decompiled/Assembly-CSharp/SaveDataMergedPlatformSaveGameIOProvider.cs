using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Platform;

// Token: 0x02000A42 RID: 2626
public class SaveDataMergedPlatformSaveGameIOProvider : IPlatformSaveGameProvider, IPlatformSaveGameIOProvider
{
	// Token: 0x06004EB7 RID: 20151 RVA: 0x001E1A0F File Offset: 0x001DFC0F
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogInfo(string text)
	{
		Log.Out("[SaveDataMergedPlatformSaveGameIOProvider] " + text);
	}

	// Token: 0x06004EB8 RID: 20152 RVA: 0x001E1A24 File Offset: 0x001DFC24
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

	// Token: 0x06004EB9 RID: 20153 RVA: 0x001E1C78 File Offset: 0x001DFE78
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

	// Token: 0x06004EBA RID: 20154 RVA: 0x001E1CC0 File Offset: 0x001DFEC0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsCached(SaveDataManagedPath path)
	{
		return this.m_cache != null && SaveDataMergedPlatformSaveGameIOProvider.s_cachedManagedPathRegex.IsMatch(path.PathRelativeToRoot);
	}

	// Token: 0x06004EBB RID: 20155 RVA: 0x001E1CDC File Offset: 0x001DFEDC
	[PublicizedFrom(EAccessModifier.Private)]
	public IPlatformSaveGameIOProvider GetIOProvider(SaveDataManagedPath path)
	{
		if (!this.IsCached(path))
		{
			return this.m_main;
		}
		return this.m_cache;
	}

	// Token: 0x06004EBC RID: 20156 RVA: 0x001E1D04 File Offset: 0x001DFF04
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

	// Token: 0x06004EBD RID: 20157 RVA: 0x001E1D7C File Offset: 0x001DFF7C
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

	// Token: 0x06004EBE RID: 20158 RVA: 0x001E1DD4 File Offset: 0x001DFFD4
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

	// Token: 0x06004EBF RID: 20159 RVA: 0x001E1F94 File Offset: 0x001E0194
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

	// Token: 0x06004EC0 RID: 20160 RVA: 0x001E2110 File Offset: 0x001E0310
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

	// Token: 0x06004EC1 RID: 20161 RVA: 0x001E218C File Offset: 0x001E038C
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

	// Token: 0x06004EC2 RID: 20162 RVA: 0x001E2224 File Offset: 0x001E0424
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

	// Token: 0x17000895 RID: 2197
	// (get) Token: 0x06004EC3 RID: 20163 RVA: 0x001E22E4 File Offset: 0x001E04E4
	public ESaveGameProviderStatus Status
	{
		get
		{
			return this.m_main.Status;
		}
	}

	// Token: 0x17000896 RID: 2198
	// (get) Token: 0x06004EC4 RID: 20164 RVA: 0x000200DA File Offset: 0x0001E2DA
	public IPlatformSaveGameIOProvider Cache
	{
		get
		{
			return null;
		}
	}

	// Token: 0x1400007E RID: 126
	// (add) Token: 0x06004EC5 RID: 20165 RVA: 0x001E22F1 File Offset: 0x001E04F1
	// (remove) Token: 0x06004EC6 RID: 20166 RVA: 0x001E22FF File Offset: 0x001E04FF
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

	// Token: 0x06004EC7 RID: 20167 RVA: 0x001E230D File Offset: 0x001E050D
	public void Init(IPlatform _owner)
	{
		this.m_main.Init(_owner);
	}

	// Token: 0x06004EC8 RID: 20168 RVA: 0x001E231B File Offset: 0x001E051B
	public void Destroy()
	{
		this.m_main.Destroy();
	}

	// Token: 0x06004EC9 RID: 20169 RVA: 0x001E2328 File Offset: 0x001E0528
	public bool ShouldBackup()
	{
		return this.m_main.ShouldBackup();
	}

	// Token: 0x06004ECA RID: 20170 RVA: 0x001E2335 File Offset: 0x001E0535
	public bool ShouldCommit()
	{
		return this.m_main.ShouldCommit();
	}

	// Token: 0x06004ECB RID: 20171 RVA: 0x001E2342 File Offset: 0x001E0542
	public double GetCommitProgress()
	{
		return this.m_main.GetCommitProgress();
	}

	// Token: 0x06004ECC RID: 20172 RVA: 0x001E234F File Offset: 0x001E054F
	public void Flush(bool waitForFlush)
	{
		this.m_main.Flush(waitForFlush);
	}

	// Token: 0x06004ECD RID: 20173 RVA: 0x001E235D File Offset: 0x001E055D
	public bool ShouldLimitSize()
	{
		return this.m_main.ShouldLimitSize();
	}

	// Token: 0x06004ECE RID: 20174 RVA: 0x001E236A File Offset: 0x001E056A
	public void UpdateSizes()
	{
		this.m_main.UpdateSizes();
	}

	// Token: 0x06004ECF RID: 20175 RVA: 0x001E2377 File Offset: 0x001E0577
	public SaveDataSizes GetSizes()
	{
		return this.m_main.GetSizes();
	}

	// Token: 0x06004ED0 RID: 20176 RVA: 0x001E2384 File Offset: 0x001E0584
	public void ManagedFileRead(SaveDataManagedPath path, Stream dest)
	{
		this.GetIOProvider(path).ManagedFileRead(path, dest);
	}

	// Token: 0x06004ED1 RID: 20177 RVA: 0x001E2394 File Offset: 0x001E0594
	public void ManagedFileWrite(SaveDataManagedPath path, Stream src)
	{
		this.SyncParentDirectories(path);
		this.GetIOProvider(path).ManagedFileWrite(path, src);
	}

	// Token: 0x06004ED2 RID: 20178 RVA: 0x001E23AC File Offset: 0x001E05AC
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

	// Token: 0x06004ED3 RID: 20179 RVA: 0x001E2438 File Offset: 0x001E0638
	public void ManagedFileDelete(SaveDataManagedPath path)
	{
		this.GetIOProvider(path).ManagedFileDelete(path);
	}

	// Token: 0x06004ED4 RID: 20180 RVA: 0x001E2447 File Offset: 0x001E0647
	public bool ManagedFileExists(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedFileExists(path);
	}

	// Token: 0x06004ED5 RID: 20181 RVA: 0x001E2456 File Offset: 0x001E0656
	public DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedFileGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004ED6 RID: 20182 RVA: 0x001E2468 File Offset: 0x001E0668
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

	// Token: 0x06004ED7 RID: 20183 RVA: 0x001E24F4 File Offset: 0x001E06F4
	public SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedDirectoryCreateDirectory(path);
	}

	// Token: 0x06004ED8 RID: 20184 RVA: 0x001E2503 File Offset: 0x001E0703
	public DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedDirectoryGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004ED9 RID: 20185 RVA: 0x001E2512 File Offset: 0x001E0712
	public bool ManagedDirectoryExists(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedDirectoryExists(path);
	}

	// Token: 0x06004EDA RID: 20186 RVA: 0x001E2524 File Offset: 0x001E0724
	public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryEnumerateInternal(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryEnumerateDirectories(path, searchPattern, searchOption));
	}

	// Token: 0x06004EDB RID: 20187 RVA: 0x001E2564 File Offset: 0x001E0764
	public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryEnumerateInternal(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryEnumerateFiles(path, searchPattern, searchOption));
	}

	// Token: 0x06004EDC RID: 20188 RVA: 0x001E25A4 File Offset: 0x001E07A4
	public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryEnumerateInternal(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryEnumerateFileSystemEntries(path, searchPattern, searchOption));
	}

	// Token: 0x06004EDD RID: 20189 RVA: 0x001E25E4 File Offset: 0x001E07E4
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

	// Token: 0x06004EDE RID: 20190 RVA: 0x001E26A4 File Offset: 0x001E08A4
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

	// Token: 0x06004EDF RID: 20191 RVA: 0x001E26F8 File Offset: 0x001E08F8
	public long ManagedFileInfoLength(SaveDataManagedPath path)
	{
		if (this.IsCached(path))
		{
			return 0L;
		}
		return this.m_main.ManagedFileInfoLength(path);
	}

	// Token: 0x06004EE0 RID: 20192 RVA: 0x001E2714 File Offset: 0x001E0914
	public IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryInfoEnumerateInternal<SdDirectoryInfo>(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryInfoEnumerateDirectories(path, searchPattern, searchOption));
	}

	// Token: 0x06004EE1 RID: 20193 RVA: 0x001E2754 File Offset: 0x001E0954
	public IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryInfoEnumerateInternal<SdFileInfo>(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryInfoEnumerateFiles(path, searchPattern, searchOption));
	}

	// Token: 0x06004EE2 RID: 20194 RVA: 0x001E2794 File Offset: 0x001E0994
	public IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryInfoEnumerateInternal<SdFileSystemInfo>(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryInfoEnumerateFileSystemInfos(path, searchPattern, searchOption));
	}

	// Token: 0x06004EE3 RID: 20195 RVA: 0x001E27D4 File Offset: 0x001E09D4
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

	// Token: 0x06004EE5 RID: 20197 RVA: 0x001E28A0 File Offset: 0x001E0AA0
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

	// Token: 0x04003CF1 RID: 15601
	[PublicizedFrom(EAccessModifier.Private)]
	public const string CACHE_OWNER_FILENAME = "CacheOwner.txt";

	// Token: 0x04003CF2 RID: 15602
	[PublicizedFrom(EAccessModifier.Private)]
	public const string SAVE_OWNER_FILENAME = "SaveOwner.txt";

	// Token: 0x04003CF3 RID: 15603
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex s_cachedManagedPathRegex = SaveDataMergedPlatformSaveGameIOProvider.CreateCachedManagedPathRegex();

	// Token: 0x04003CF4 RID: 15604
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IPlatformSaveGameProvider m_main;

	// Token: 0x04003CF5 RID: 15605
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IPlatformSaveGameIOProvider m_cache;
}
