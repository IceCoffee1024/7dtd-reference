using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Platform;

// Token: 0x02000A2E RID: 2606
public class SaveDataMergedPlatformSaveGameIOProvider : IPlatformSaveGameProvider, IPlatformSaveGameIOProvider
{
	// Token: 0x06004EC4 RID: 20164 RVA: 0x001DF677 File Offset: 0x001DD877
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogInfo(string text)
	{
		Log.Out("[SaveDataMergedPlatformSaveGameIOProvider] " + text);
	}

	// Token: 0x06004EC5 RID: 20165 RVA: 0x001DF68C File Offset: 0x001DD88C
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

	// Token: 0x06004EC6 RID: 20166 RVA: 0x001DF8E0 File Offset: 0x001DDAE0
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

	// Token: 0x06004EC7 RID: 20167 RVA: 0x001DF928 File Offset: 0x001DDB28
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsCached(SaveDataManagedPath path)
	{
		return this.m_cache != null && SaveDataMergedPlatformSaveGameIOProvider.s_cachedManagedPathRegex.IsMatch(path.PathRelativeToRoot);
	}

	// Token: 0x06004EC8 RID: 20168 RVA: 0x001DF944 File Offset: 0x001DDB44
	[PublicizedFrom(EAccessModifier.Private)]
	public IPlatformSaveGameIOProvider GetIOProvider(SaveDataManagedPath path)
	{
		if (!this.IsCached(path))
		{
			return this.m_main;
		}
		return this.m_cache;
	}

	// Token: 0x06004EC9 RID: 20169 RVA: 0x001DF96C File Offset: 0x001DDB6C
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

	// Token: 0x06004ECA RID: 20170 RVA: 0x001DF9E4 File Offset: 0x001DDBE4
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

	// Token: 0x06004ECB RID: 20171 RVA: 0x001DFA3C File Offset: 0x001DDC3C
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

	// Token: 0x06004ECC RID: 20172 RVA: 0x001DFBFC File Offset: 0x001DDDFC
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

	// Token: 0x06004ECD RID: 20173 RVA: 0x001DFD78 File Offset: 0x001DDF78
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

	// Token: 0x06004ECE RID: 20174 RVA: 0x001DFDF4 File Offset: 0x001DDFF4
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

	// Token: 0x06004ECF RID: 20175 RVA: 0x001DFE8C File Offset: 0x001DE08C
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

	// Token: 0x1700086F RID: 2159
	// (get) Token: 0x06004ED0 RID: 20176 RVA: 0x001DFF4C File Offset: 0x001DE14C
	public ESaveGameProviderStatus Status
	{
		get
		{
			return this.m_main.Status;
		}
	}

	// Token: 0x17000870 RID: 2160
	// (get) Token: 0x06004ED1 RID: 20177 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public IPlatformSaveGameIOProvider Cache
	{
		get
		{
			return null;
		}
	}

	// Token: 0x1400007D RID: 125
	// (add) Token: 0x06004ED2 RID: 20178 RVA: 0x001DFF59 File Offset: 0x001DE159
	// (remove) Token: 0x06004ED3 RID: 20179 RVA: 0x001DFF67 File Offset: 0x001DE167
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

	// Token: 0x06004ED4 RID: 20180 RVA: 0x001DFF75 File Offset: 0x001DE175
	public void Init(IPlatform _owner)
	{
		this.m_main.Init(_owner);
	}

	// Token: 0x06004ED5 RID: 20181 RVA: 0x001DFF83 File Offset: 0x001DE183
	public void Destroy()
	{
		this.m_main.Destroy();
	}

	// Token: 0x06004ED6 RID: 20182 RVA: 0x001DFF90 File Offset: 0x001DE190
	public bool ShouldBackup()
	{
		return this.m_main.ShouldBackup();
	}

	// Token: 0x06004ED7 RID: 20183 RVA: 0x001DFF9D File Offset: 0x001DE19D
	public bool ShouldCommit()
	{
		return this.m_main.ShouldCommit();
	}

	// Token: 0x06004ED8 RID: 20184 RVA: 0x001DFFAA File Offset: 0x001DE1AA
	public double GetCommitProgress()
	{
		return this.m_main.GetCommitProgress();
	}

	// Token: 0x06004ED9 RID: 20185 RVA: 0x001DFFB7 File Offset: 0x001DE1B7
	public void Flush(bool waitForFlush)
	{
		this.m_main.Flush(waitForFlush);
	}

	// Token: 0x06004EDA RID: 20186 RVA: 0x001DFFC5 File Offset: 0x001DE1C5
	public bool ShouldLimitSize()
	{
		return this.m_main.ShouldLimitSize();
	}

	// Token: 0x06004EDB RID: 20187 RVA: 0x001DFFD2 File Offset: 0x001DE1D2
	public void UpdateSizes()
	{
		this.m_main.UpdateSizes();
	}

	// Token: 0x06004EDC RID: 20188 RVA: 0x001DFFDF File Offset: 0x001DE1DF
	public SaveDataSizes GetSizes()
	{
		return this.m_main.GetSizes();
	}

	// Token: 0x06004EDD RID: 20189 RVA: 0x001DFFEC File Offset: 0x001DE1EC
	public void ManagedFileRead(SaveDataManagedPath path, Stream dest)
	{
		this.GetIOProvider(path).ManagedFileRead(path, dest);
	}

	// Token: 0x06004EDE RID: 20190 RVA: 0x001DFFFC File Offset: 0x001DE1FC
	public void ManagedFileWrite(SaveDataManagedPath path, Stream src)
	{
		this.SyncParentDirectories(path);
		this.GetIOProvider(path).ManagedFileWrite(path, src);
	}

	// Token: 0x06004EDF RID: 20191 RVA: 0x001E0014 File Offset: 0x001DE214
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

	// Token: 0x06004EE0 RID: 20192 RVA: 0x001E00A0 File Offset: 0x001DE2A0
	public void ManagedFileDelete(SaveDataManagedPath path)
	{
		this.GetIOProvider(path).ManagedFileDelete(path);
	}

	// Token: 0x06004EE1 RID: 20193 RVA: 0x001E00AF File Offset: 0x001DE2AF
	public bool ManagedFileExists(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedFileExists(path);
	}

	// Token: 0x06004EE2 RID: 20194 RVA: 0x001E00BE File Offset: 0x001DE2BE
	public DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedFileGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004EE3 RID: 20195 RVA: 0x001E00D0 File Offset: 0x001DE2D0
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

	// Token: 0x06004EE4 RID: 20196 RVA: 0x001E015C File Offset: 0x001DE35C
	public SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedDirectoryCreateDirectory(path);
	}

	// Token: 0x06004EE5 RID: 20197 RVA: 0x001E016B File Offset: 0x001DE36B
	public DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedDirectoryGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004EE6 RID: 20198 RVA: 0x001E017A File Offset: 0x001DE37A
	public bool ManagedDirectoryExists(SaveDataManagedPath path)
	{
		return this.GetIOProvider(path).ManagedDirectoryExists(path);
	}

	// Token: 0x06004EE7 RID: 20199 RVA: 0x001E018C File Offset: 0x001DE38C
	public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryEnumerateInternal(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryEnumerateDirectories(path, searchPattern, searchOption));
	}

	// Token: 0x06004EE8 RID: 20200 RVA: 0x001E01CC File Offset: 0x001DE3CC
	public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryEnumerateInternal(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryEnumerateFiles(path, searchPattern, searchOption));
	}

	// Token: 0x06004EE9 RID: 20201 RVA: 0x001E020C File Offset: 0x001DE40C
	public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryEnumerateInternal(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryEnumerateFileSystemEntries(path, searchPattern, searchOption));
	}

	// Token: 0x06004EEA RID: 20202 RVA: 0x001E024C File Offset: 0x001DE44C
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

	// Token: 0x06004EEB RID: 20203 RVA: 0x001E030C File Offset: 0x001DE50C
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

	// Token: 0x06004EEC RID: 20204 RVA: 0x001E0360 File Offset: 0x001DE560
	public long ManagedFileInfoLength(SaveDataManagedPath path)
	{
		if (this.IsCached(path))
		{
			return 0L;
		}
		return this.m_main.ManagedFileInfoLength(path);
	}

	// Token: 0x06004EED RID: 20205 RVA: 0x001E037C File Offset: 0x001DE57C
	public IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryInfoEnumerateInternal<SdDirectoryInfo>(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryInfoEnumerateDirectories(path, searchPattern, searchOption));
	}

	// Token: 0x06004EEE RID: 20206 RVA: 0x001E03BC File Offset: 0x001DE5BC
	public IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryInfoEnumerateInternal<SdFileInfo>(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryInfoEnumerateFiles(path, searchPattern, searchOption));
	}

	// Token: 0x06004EEF RID: 20207 RVA: 0x001E03FC File Offset: 0x001DE5FC
	public IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.ManagedDirectoryInfoEnumerateInternal<SdFileSystemInfo>(path, (IPlatformSaveGameIOProvider provider) => provider.ManagedDirectoryInfoEnumerateFileSystemInfos(path, searchPattern, searchOption));
	}

	// Token: 0x06004EF0 RID: 20208 RVA: 0x001E043C File Offset: 0x001DE63C
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

	// Token: 0x06004EF2 RID: 20210 RVA: 0x001E0508 File Offset: 0x001DE708
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

	// Token: 0x04003CC3 RID: 15555
	[PublicizedFrom(EAccessModifier.Private)]
	public const string CACHE_OWNER_FILENAME = "CacheOwner.txt";

	// Token: 0x04003CC4 RID: 15556
	[PublicizedFrom(EAccessModifier.Private)]
	public const string SAVE_OWNER_FILENAME = "SaveOwner.txt";

	// Token: 0x04003CC5 RID: 15557
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex s_cachedManagedPathRegex = SaveDataMergedPlatformSaveGameIOProvider.CreateCachedManagedPathRegex();

	// Token: 0x04003CC6 RID: 15558
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IPlatformSaveGameProvider m_main;

	// Token: 0x04003CC7 RID: 15559
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IPlatformSaveGameIOProvider m_cache;
}
