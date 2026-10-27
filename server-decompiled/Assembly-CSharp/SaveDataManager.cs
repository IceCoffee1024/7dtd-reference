using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Platform;
using Unity.Profiling;

// Token: 0x02000A2E RID: 2606
public class SaveDataManager : SaveDataManagerBase
{
	// Token: 0x1700086E RID: 2158
	// (get) Token: 0x06004DAE RID: 19886 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AppliesSaveSizeLimit
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004DAF RID: 19887 RVA: 0x001DE2C8 File Offset: 0x001DC4C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Stream GetStream(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share)
	{
		if ((access == FileAccess.Read && (mode == FileMode.CreateNew || mode == FileMode.Create || mode == FileMode.Truncate || mode == FileMode.Append)) || (access == FileAccess.ReadWrite && mode == FileMode.Append))
		{
			throw new ArgumentException(string.Format("Combining {0}: {1} with {2}: {3} is invalid.", new object[] { "FileMode", mode, "FileAccess", access }));
		}
		if (this.cleanupComplete)
		{
			throw new InvalidOperationException("Attempting to get stream from Save Data Manager after Cleanup has been called.");
		}
		int num = path.PathRelativeToRoot.LastIndexOf('/');
		SaveDataManagedPath saveDataManagedPath = new SaveDataManagedPath((num >= 0) ? path.PathRelativeToRoot.Substring(0, num) : string.Empty);
		if (!this.ManagedDirectoryExists(saveDataManagedPath))
		{
			throw new IOException(SpanUtils.Concat("Parent of ", path.PathRelativeToRoot, " does not exist."));
		}
		object obj = this.mapLockObj;
		SaveDataManager.CachedStream cachedStream;
		lock (obj)
		{
			if (!this.pathToCacheMap.TryGetValue(path, out cachedStream))
			{
				cachedStream = new SaveDataManager.CachedStream(path, this.platformSaveGameProvider);
				this.pathToCacheMap[path] = cachedStream;
			}
			SaveDataManager.s_CachedStreamCount.Value = this.pathToCacheMap.Count;
		}
		Stream stream;
		cachedStream.EnterLock(mode, access, share, out stream);
		obj = this.mapLockObj;
		lock (obj)
		{
			this.streamToCacheMap[stream] = cachedStream;
			int value = SaveDataManager.s_StreamOperationCount.Value;
			ProfilerCounterValue<int> profilerCounterValue;
			profilerCounterValue.Value = value + 1;
		}
		return stream;
	}

	// Token: 0x06004DB0 RID: 19888 RVA: 0x001DE470 File Offset: 0x001DC670
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool IsPriorityFilePath(SaveDataManagedPath path)
	{
		return path.Type == SaveDataType.Saves && path.PathRelativeToSlot.AsSpan().IndexOf<char>("Region") == 0;
	}

	// Token: 0x06004DB1 RID: 19889 RVA: 0x001DE4A8 File Offset: 0x001DC6A8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ReturnStream(Stream stream)
	{
		if (this.cleanupComplete)
		{
			throw new InvalidOperationException("Attempting to return stream to Save Data Manager after Cleanup has been called.");
		}
		object obj = this.mapLockObj;
		SaveDataManager.CachedStream cachedStream;
		lock (obj)
		{
			cachedStream = this.streamToCacheMap[stream];
			cachedStream.ExitLock();
			this.streamToCacheMap.Remove(stream);
			long num;
			bool flag2;
			if (cachedStream.HasPendingChanges && this.TryGetTrackedSize(cachedStream.Path, out num, out flag2) && flag2 && num > cachedStream.PendingSize)
			{
				this.priorityCachedStreams.Add(cachedStream);
			}
			else
			{
				this.priorityCachedStreams.Remove(cachedStream);
			}
		}
		if (this.writeMode == SaveDataWriteMode.Immediate)
		{
			if (cachedStream.HasPendingChanges)
			{
				cachedStream.Commit();
			}
			obj = this.mapLockObj;
			lock (obj)
			{
				if (!cachedStream.IsLocked)
				{
					this.RemoveAndDispose(cachedStream);
				}
			}
		}
	}

	// Token: 0x06004DB2 RID: 19890 RVA: 0x001DE5B0 File Offset: 0x001DC7B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemoveAndDispose(SaveDataManager.CachedStream cachedStream)
	{
		object obj = this.mapLockObj;
		lock (obj)
		{
			this.pathToCacheMap.Remove(cachedStream.Path);
			this.priorityCachedStreams.Remove(cachedStream);
			cachedStream.Dispose();
		}
	}

	// Token: 0x06004DB3 RID: 19891 RVA: 0x001DE610 File Offset: 0x001DC810
	public override bool ShouldLimitSize()
	{
		return this.platformSaveGameProvider.ShouldLimitSize();
	}

	// Token: 0x06004DB4 RID: 19892 RVA: 0x001DE61D File Offset: 0x001DC81D
	public override void UpdateSizes()
	{
		this.platformSaveGameProvider.UpdateSizes();
	}

	// Token: 0x06004DB5 RID: 19893 RVA: 0x001DE62A File Offset: 0x001DC82A
	public override SaveDataSizes GetSizes()
	{
		return this.platformSaveGameProvider.GetSizes();
	}

	// Token: 0x06004DB6 RID: 19894 RVA: 0x001DE638 File Offset: 0x001DC838
	public override void ManagedFileDelete(SaveDataManagedPath path)
	{
		object obj = this.mapLockObj;
		lock (obj)
		{
			SaveDataManager.CachedStream cachedStream;
			bool flag2 = this.pathToCacheMap.TryGetValue(path, out cachedStream);
			if (flag2 && cachedStream.IsLocked)
			{
				throw new IOException(string.Format("Cached Stream for '{0}' can not be deleted because it is current locked (stream is loaned out and not returned yet).", path));
			}
			if (this.platformSaveGameProvider.ShouldBackup())
			{
				SaveDataManagedPath backupPath = SaveDataUtils.GetBackupPath(path);
				if (this.platformSaveGameProvider.ManagedFileExists(backupPath))
				{
					this.platformSaveGameProvider.ManagedFileDelete(backupPath);
				}
			}
			this.platformSaveGameProvider.ManagedFileDelete(path);
			if (flag2)
			{
				this.RemoveAndDispose(cachedStream);
			}
			SaveDataManager.SlotSizeData slotSizeData;
			if (this.fileSizesBySlot.TryGetValue(path.Slot, out slotSizeData))
			{
				slotSizeData.RemoveFileSize(path);
			}
		}
	}

	// Token: 0x06004DB7 RID: 19895 RVA: 0x001DE700 File Offset: 0x001DC900
	public override bool ManagedFileExists(SaveDataManagedPath path)
	{
		object obj = this.mapLockObj;
		lock (obj)
		{
			SaveDataManager.CachedStream cachedStream;
			if (this.pathToCacheMap.TryGetValue(path, out cachedStream) && cachedStream.ExistsVirtual)
			{
				return true;
			}
		}
		return this.platformSaveGameProvider.ManagedFileExists(path);
	}

	// Token: 0x06004DB8 RID: 19896 RVA: 0x001DE764 File Offset: 0x001DC964
	public override DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		object obj = this.mapLockObj;
		lock (obj)
		{
			SaveDataManager.CachedStream cachedStream;
			if (this.pathToCacheMap.TryGetValue(path, out cachedStream))
			{
				return cachedStream.LastWriteTime;
			}
		}
		return this.platformSaveGameProvider.ManagedFileGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004DB9 RID: 19897 RVA: 0x001DE7C8 File Offset: 0x001DC9C8
	public override SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
	{
		return this.platformSaveGameProvider.ManagedDirectoryCreateDirectory(path);
	}

	// Token: 0x06004DBA RID: 19898 RVA: 0x001DE7D6 File Offset: 0x001DC9D6
	public override DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return this.platformSaveGameProvider.ManagedDirectoryGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004DBB RID: 19899 RVA: 0x001DE7E4 File Offset: 0x001DC9E4
	public override bool ManagedDirectoryExists(SaveDataManagedPath path)
	{
		return this.platformSaveGameProvider.ManagedDirectoryExists(path);
	}

	// Token: 0x06004DBC RID: 19900 RVA: 0x001DE7F2 File Offset: 0x001DC9F2
	public override IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.platformSaveGameProvider.ManagedDirectoryEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004DBD RID: 19901 RVA: 0x001DE802 File Offset: 0x001DCA02
	public override IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		HashSet<SaveDataManagedPath> cachedPaths = new HashSet<SaveDataManagedPath>();
		object obj = this.mapLockObj;
		lock (obj)
		{
			foreach (KeyValuePair<SaveDataManagedPath, SaveDataManager.CachedStream> keyValuePair in this.pathToCacheMap)
			{
				SaveDataManagedPath saveDataManagedPath;
				SaveDataManager.CachedStream cachedStream;
				keyValuePair.Deconstruct(out saveDataManagedPath, out cachedStream);
				SaveDataManagedPath saveDataManagedPath2 = saveDataManagedPath;
				if (cachedStream.ExistsVirtual && SaveDataManager.PathSearchMatches(path, saveDataManagedPath2, searchPattern, searchOption))
				{
					cachedPaths.Add(saveDataManagedPath2);
				}
			}
		}
		foreach (SaveDataManagedPath platformPath in this.platformSaveGameProvider.ManagedDirectoryEnumerateFiles(path, searchPattern, searchOption))
		{
			yield return platformPath;
			cachedPaths.Remove(platformPath);
			platformPath = null;
		}
		IEnumerator<SaveDataManagedPath> enumerator2 = null;
		foreach (SaveDataManagedPath saveDataManagedPath3 in cachedPaths)
		{
			yield return saveDataManagedPath3;
		}
		HashSet<SaveDataManagedPath>.Enumerator enumerator3 = default(HashSet<SaveDataManagedPath>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x06004DBE RID: 19902 RVA: 0x001DE827 File Offset: 0x001DCA27
	public override IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		HashSet<SaveDataManagedPath> cachedPaths = new HashSet<SaveDataManagedPath>();
		object obj = this.mapLockObj;
		lock (obj)
		{
			foreach (KeyValuePair<SaveDataManagedPath, SaveDataManager.CachedStream> keyValuePair in this.pathToCacheMap)
			{
				SaveDataManagedPath saveDataManagedPath;
				SaveDataManager.CachedStream cachedStream;
				keyValuePair.Deconstruct(out saveDataManagedPath, out cachedStream);
				SaveDataManagedPath saveDataManagedPath2 = saveDataManagedPath;
				if (cachedStream.ExistsVirtual && SaveDataManager.PathSearchMatches(path, saveDataManagedPath2, searchPattern, searchOption))
				{
					cachedPaths.Add(saveDataManagedPath2);
				}
			}
		}
		foreach (SaveDataManagedPath platformPath in this.platformSaveGameProvider.ManagedDirectoryEnumerateFileSystemEntries(path, searchPattern, searchOption))
		{
			yield return platformPath;
			cachedPaths.Remove(platformPath);
			platformPath = null;
		}
		IEnumerator<SaveDataManagedPath> enumerator2 = null;
		foreach (SaveDataManagedPath saveDataManagedPath3 in cachedPaths)
		{
			yield return saveDataManagedPath3;
		}
		HashSet<SaveDataManagedPath>.Enumerator enumerator3 = default(HashSet<SaveDataManagedPath>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x06004DBF RID: 19903 RVA: 0x001DE84C File Offset: 0x001DCA4C
	public override void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive)
	{
		object obj = this.mapLockObj;
		lock (obj)
		{
			List<KeyValuePair<SaveDataManagedPath, SaveDataManager.CachedStream>> list = this.pathToCacheMap.Where<KeyValuePair<SaveDataManagedPath, SaveDataManager.CachedStream>>((KeyValuePair<SaveDataManagedPath, SaveDataManager.CachedStream> kv) => path.IsParentOf(kv.Key)).ToList<KeyValuePair<SaveDataManagedPath, SaveDataManager.CachedStream>>();
			foreach (KeyValuePair<SaveDataManagedPath, SaveDataManager.CachedStream> keyValuePair in list)
			{
				SaveDataManagedPath saveDataManagedPath;
				SaveDataManager.CachedStream cachedStream;
				keyValuePair.Deconstruct(out saveDataManagedPath, out cachedStream);
				SaveDataManagedPath saveDataManagedPath2 = saveDataManagedPath;
				if (cachedStream.IsLocked)
				{
					throw new IOException(string.Format("Can not delete directory at '{0}' because Cached Stream for '{1}' can not be deleted because it is current locked (stream is loaned out and not returned yet).", path, saveDataManagedPath2));
				}
			}
			this.platformSaveGameProvider.ManagedDirectoryDelete(path, recursive);
			foreach (KeyValuePair<SaveDataManagedPath, SaveDataManager.CachedStream> keyValuePair in list)
			{
				SaveDataManagedPath saveDataManagedPath;
				SaveDataManager.CachedStream cachedStream;
				keyValuePair.Deconstruct(out saveDataManagedPath, out cachedStream);
				SaveDataManager.CachedStream cachedStream2 = cachedStream;
				this.RemoveAndDispose(cachedStream2);
			}
			foreach (SaveDataManager.SlotSizeData slotSizeData in this.fileSizesBySlot.Values)
			{
				slotSizeData.RemoveFileSizes(path);
			}
		}
	}

	// Token: 0x06004DC0 RID: 19904 RVA: 0x001DE9F4 File Offset: 0x001DCBF4
	public override long ManagedFileInfoLength(SaveDataManagedPath path)
	{
		object obj = this.mapLockObj;
		lock (obj)
		{
			SaveDataManager.CachedStream cachedStream;
			if (this.pathToCacheMap.TryGetValue(path, out cachedStream) && cachedStream.HasPendingChanges)
			{
				return cachedStream.PendingSize;
			}
		}
		return this.platformSaveGameProvider.ManagedFileInfoLength(path);
	}

	// Token: 0x06004DC1 RID: 19905 RVA: 0x001DEA60 File Offset: 0x001DCC60
	public override IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.platformSaveGameProvider.ManagedDirectoryInfoEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004DC2 RID: 19906 RVA: 0x001DEA70 File Offset: 0x001DCC70
	public override IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		HashSet<SaveDataManagedPath> cachedPaths = new HashSet<SaveDataManagedPath>();
		object obj = this.mapLockObj;
		lock (obj)
		{
			foreach (KeyValuePair<SaveDataManagedPath, SaveDataManager.CachedStream> keyValuePair in this.pathToCacheMap)
			{
				SaveDataManagedPath saveDataManagedPath;
				SaveDataManager.CachedStream cachedStream;
				keyValuePair.Deconstruct(out saveDataManagedPath, out cachedStream);
				SaveDataManagedPath saveDataManagedPath2 = saveDataManagedPath;
				if (cachedStream.ExistsVirtual && SaveDataManager.PathSearchMatches(path, saveDataManagedPath2, searchPattern, searchOption))
				{
					cachedPaths.Add(saveDataManagedPath2);
				}
			}
		}
		foreach (SdFileInfo fileInfo in this.platformSaveGameProvider.ManagedDirectoryInfoEnumerateFiles(path, searchPattern, searchOption))
		{
			yield return fileInfo;
			cachedPaths.Remove(fileInfo.ManagedPath);
			fileInfo = null;
		}
		IEnumerator<SdFileInfo> enumerator2 = null;
		foreach (SaveDataManagedPath saveDataManagedPath3 in cachedPaths)
		{
			yield return new SdFileInfo(saveDataManagedPath3);
		}
		HashSet<SaveDataManagedPath>.Enumerator enumerator3 = default(HashSet<SaveDataManagedPath>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x06004DC3 RID: 19907 RVA: 0x001DEA95 File Offset: 0x001DCC95
	public override IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		HashSet<SaveDataManagedPath> cachedPaths = new HashSet<SaveDataManagedPath>();
		object obj = this.mapLockObj;
		lock (obj)
		{
			foreach (KeyValuePair<SaveDataManagedPath, SaveDataManager.CachedStream> keyValuePair in this.pathToCacheMap)
			{
				SaveDataManagedPath saveDataManagedPath;
				SaveDataManager.CachedStream cachedStream;
				keyValuePair.Deconstruct(out saveDataManagedPath, out cachedStream);
				SaveDataManagedPath saveDataManagedPath2 = saveDataManagedPath;
				if (cachedStream.ExistsVirtual && SaveDataManager.PathSearchMatches(path, saveDataManagedPath2, searchPattern, searchOption))
				{
					cachedPaths.Add(saveDataManagedPath2);
				}
			}
		}
		foreach (SdFileSystemInfo fileInfo in this.platformSaveGameProvider.ManagedDirectoryInfoEnumerateFileSystemInfos(path, searchPattern, searchOption))
		{
			yield return fileInfo;
			cachedPaths.Remove(fileInfo.ManagedPath);
			fileInfo = null;
		}
		IEnumerator<SdFileSystemInfo> enumerator2 = null;
		foreach (SaveDataManagedPath saveDataManagedPath3 in cachedPaths)
		{
			yield return new SdFileInfo(saveDataManagedPath3);
		}
		HashSet<SaveDataManagedPath>.Enumerator enumerator3 = default(HashSet<SaveDataManagedPath>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x06004DC4 RID: 19908 RVA: 0x001DEABC File Offset: 0x001DCCBC
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool PathSearchMatches(SaveDataManagedPath parent, SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		if (!parent.IsParentOf(path))
		{
			return false;
		}
		if (searchOption == SearchOption.TopDirectoryOnly)
		{
			int num = ((parent.PathRelativeToRoot.Length > 0) ? (parent.PathRelativeToRoot.Length + 1) : 0);
			if (path.PathRelativeToRoot.IndexOf('/', num) >= 0)
			{
				return false;
			}
		}
		return SaveDataManager.MatchesSearchPattern(path, searchPattern);
	}

	// Token: 0x06004DC5 RID: 19909 RVA: 0x001DEB18 File Offset: 0x001DCD18
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool MatchesSearchPattern(SaveDataManagedPath path, string searchPattern)
	{
		if (searchPattern == "*")
		{
			return true;
		}
		StringSpan stringSpan = path.PathRelativeToRoot;
		int num = stringSpan.LastIndexOf('/');
		if (num >= 0 && num < stringSpan.Length - 1)
		{
			stringSpan = stringSpan.Slice(num + 1);
		}
		return SaveDataManager.<MatchesSearchPattern>g__MatchWildcard|32_0(stringSpan, searchPattern);
	}

	// Token: 0x06004DC6 RID: 19910 RVA: 0x001DEB6C File Offset: 0x001DCD6C
	public SaveDataManager(IPlatformSaveGameProvider platformSaveGameProvider)
	{
		this.platformSaveGameProvider = new SaveDataMergedPlatformSaveGameIOProvider(platformSaveGameProvider);
	}

	// Token: 0x06004DC7 RID: 19911 RVA: 0x001DEBEE File Offset: 0x001DCDEE
	public override void Init()
	{
		this.SetWriteMode(SaveDataWriteMode.Deferred);
		this.InitRestoreBackups();
		this.InitFileSizeTracking();
	}

	// Token: 0x06004DC8 RID: 19912 RVA: 0x001DEC04 File Offset: 0x001DCE04
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitRestoreBackups()
	{
		if (!this.platformSaveGameProvider.ShouldBackup())
		{
			return;
		}
		using (SaveDataManager.s_InitRestoreBackupsMarker.Auto())
		{
			SaveDataManagedPath path = SaveDataType.User.GetPath();
			if (this.ManagedDirectoryExists(path))
			{
				foreach (SdFileInfo sdFileInfo in this.ManagedDirectoryInfoEnumerateFiles(path, "*.bup", SearchOption.AllDirectories))
				{
					SaveDataManagedPath managedPath = sdFileInfo.ManagedPath;
					SaveDataManagedPath restorePath = SaveDataUtils.GetRestorePath(managedPath);
					if (this.platformSaveGameProvider.ManagedDirectoryExists(restorePath))
					{
						Log.Warning(string.Format("Could not restore to \"{0}\" using \"{1}\" because the former is a directory.", restorePath, managedPath));
					}
					else
					{
						this.platformSaveGameProvider.ManagedFileDelete(restorePath);
						this.platformSaveGameProvider.ManagedFileMove(managedPath, restorePath);
					}
				}
			}
		}
	}

	// Token: 0x06004DC9 RID: 19913 RVA: 0x001DECEC File Offset: 0x001DCEEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitFileSizeTracking()
	{
		using (SaveDataManager.s_InitFileSizeTrackingMarker.Auto())
		{
			this.fileSizesBySlot.Clear();
			SaveDataManagedPath path = SaveDataType.User.GetPath();
			if (this.ManagedDirectoryExists(path))
			{
				foreach (SdFileInfo sdFileInfo in this.ManagedDirectoryInfoEnumerateFiles(path, "*", SearchOption.AllDirectories))
				{
					SaveDataManagedPath managedPath = sdFileInfo.ManagedPath;
					SaveDataManager.SlotSizeData slotSizeData;
					if (!this.fileSizesBySlot.TryGetValue(managedPath.Slot, out slotSizeData))
					{
						SaveDataSlot simpleSlot = managedPath.Slot.GetSimpleSlot();
						slotSizeData = new SaveDataManager.SlotSizeData(simpleSlot);
						this.fileSizesBySlot[simpleSlot] = slotSizeData;
					}
					long length = sdFileInfo.Length;
					slotSizeData.SetFileSize(managedPath, length);
				}
			}
		}
	}

	// Token: 0x06004DCA RID: 19914 RVA: 0x001DEDDC File Offset: 0x001DCFDC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryGetTrackedSize(SaveDataManagedPath path, out long trackedSize, out bool isPriorityFile)
	{
		SaveDataManager.SlotSizeData slotSizeData;
		if (this.fileSizesBySlot.TryGetValue(path.Slot, out slotSizeData) && slotSizeData.TryGetFileSize(path, out trackedSize, out isPriorityFile))
		{
			return true;
		}
		trackedSize = 0L;
		isPriorityFile = false;
		return false;
	}

	// Token: 0x06004DCB RID: 19915 RVA: 0x001DEE14 File Offset: 0x001DD014
	[PublicizedFrom(EAccessModifier.Private)]
	public int WriteTask(ThreadManager.ThreadInfo _threadInfo)
	{
		try
		{
			if (_threadInfo.TerminationRequested())
			{
				return -1;
			}
			int num = this.explicitCommitNeeded;
			TimeSpan timeSpan = DateTime.Now - this.lastCommitTime;
			if (num != this.explicitCommitCount || timeSpan > SaveDataManager.commitInterval)
			{
				if (this.CommitPendingChanges(false))
				{
					this.explicitCommitCount = num;
				}
				this.lastCommitTime = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
		}
		return 5;
	}

	// Token: 0x06004DCC RID: 19916 RVA: 0x001DEE94 File Offset: 0x001DD094
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CommitPendingChanges(bool force = false)
	{
		if (!this.platformSaveGameProvider.ShouldCommit())
		{
			if (!force)
			{
				return false;
			}
			Log.Out("[SaveDataManager] Forcing CommitPendingChanges (platform currently suggests to not commit).");
		}
		bool flag = false;
		try
		{
			object obj = this.commitLockObj;
			lock (obj)
			{
				HashSet<SaveDataManager.CachedStream> hashSet = new HashSet<SaveDataManager.CachedStream>();
				SaveDataManager.CachedStream cachedStream;
				bool flag3;
				while (this.TryGetNextStreamToCommit(hashSet, out cachedStream, out flag3))
				{
					bool flag4 = true;
					try
					{
						if (!flag)
						{
							flag = true;
							base.OnCommitStarted();
						}
						long pendingSize = cachedStream.PendingSize;
						long num = pendingSize;
						SaveDataManager.SlotSizeData slotSizeData;
						long num2;
						if (!this.fileSizesBySlot.TryGetValue(cachedStream.Path.Slot, out slotSizeData))
						{
							SaveDataSlot simpleSlot = cachedStream.Path.Slot.GetSimpleSlot();
							slotSizeData = new SaveDataManager.SlotSizeData(simpleSlot);
							this.fileSizesBySlot[simpleSlot] = slotSizeData;
						}
						else if (slotSizeData.TryGetFileSize(cachedStream.Path, out num2))
						{
							num -= num2;
						}
						if (!flag3 && pendingSize > 0L)
						{
							object obj2 = this.regionFileLock;
							lock (obj2)
							{
								if (this.activeRegionFileManager != null && this.activeRegionFileManager.MaxBytes > 0L && cachedStream.Path.Slot == this.activeRegionFileManager.SaveDataSlot)
								{
									long num3 = slotSizeData.TotalFileSize + slotSizeData.LargestPriorityFileSize + pendingSize - this.activeRegionFileManager.MaxBytes;
									if (num3 > 0L)
									{
										cachedStream.ExitCommitLock();
										flag4 = false;
										if (this.activeRegionFileManager.MakeRoom(num3))
										{
											continue;
										}
										Log.Error(string.Format("[SaveDataManager] SaveDataLimit will be exceeded! Failed to clear enough space for file: {0}", cachedStream.Path));
										cachedStream.EnterCommitLock(ref flag4);
										if (!flag4)
										{
											Log.Error(string.Format("[SaveDataManager] Failed to take commit lock on cached stream for path: {0}", cachedStream.Path));
											continue;
										}
									}
								}
							}
						}
						slotSizeData.SetFileSize(cachedStream.Path, pendingSize);
						cachedStream.Commit();
						continue;
					}
					catch (Exception ex)
					{
						hashSet.Add(cachedStream);
						Log.Error(string.Format("[SaveDataManager] Failed to commit cached stream '{0}'.", cachedStream.Path));
						Log.Exception(ex);
						continue;
					}
					finally
					{
						if (flag4)
						{
							cachedStream.ExitCommitLock();
						}
						object obj2 = this.mapLockObj;
						lock (obj2)
						{
							if (!cachedStream.HasPendingChanges)
							{
								this.priorityCachedStreams.Remove(cachedStream);
							}
						}
					}
					break;
				}
				this.CloseDormantStreams();
			}
		}
		finally
		{
			try
			{
				IPlatformSaveGameProvider platformSaveGameProvider = this.platformSaveGameProvider;
				if (platformSaveGameProvider != null)
				{
					platformSaveGameProvider.Flush(true);
				}
			}
			catch (Exception ex2)
			{
				Log.Error(string.Format("Error while flushing platform save game provider: {0}", ex2));
			}
			if (flag)
			{
				base.OnCommitFinished();
			}
		}
		return true;
	}

	// Token: 0x06004DCD RID: 19917 RVA: 0x001DF1D8 File Offset: 0x001DD3D8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryGetNextStreamToCommit(HashSet<SaveDataManager.CachedStream> ignored, out SaveDataManager.CachedStream commitTarget, out bool isPriorityStream)
	{
		SaveDataManager.<>c__DisplayClass58_0 CS$<>8__locals1 = new SaveDataManager.<>c__DisplayClass58_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.ignored = ignored;
		object obj = this.mapLockObj;
		bool flag3;
		lock (obj)
		{
			if (CS$<>8__locals1.<TryGetNextStreamToCommit>g__TryGetNextStreamInternal|0(out commitTarget, out isPriorityStream))
			{
				bool flag2 = false;
				commitTarget.EnterCommitLock(ref flag2);
				if (flag2)
				{
					return true;
				}
				commitTarget = null;
				isPriorityStream = false;
				Log.Error(string.Format("[SaveDataManager] Failed to take commit lock on cached stream for path: {0}", commitTarget.Path));
			}
			flag3 = false;
		}
		return flag3;
	}

	// Token: 0x06004DCE RID: 19918 RVA: 0x001DF264 File Offset: 0x001DD464
	[PublicizedFrom(EAccessModifier.Private)]
	public void CloseDormantStreams()
	{
		object obj = this.mapLockObj;
		lock (obj)
		{
			this.closeList.Clear();
			DateTime now = DateTime.Now;
			foreach (SaveDataManager.CachedStream cachedStream in this.pathToCacheMap.Values)
			{
				if (!cachedStream.HasPendingChanges && (now - cachedStream.LastAccessTime).TotalMilliseconds > 30000.0)
				{
					this.closeList.Add(cachedStream);
				}
			}
			foreach (SaveDataManager.CachedStream cachedStream2 in this.closeList)
			{
				this.RemoveAndDispose(cachedStream2);
			}
			SaveDataManager.s_CachedStreamCount.Value = this.pathToCacheMap.Count;
		}
	}

	// Token: 0x06004DCF RID: 19919 RVA: 0x001DF384 File Offset: 0x001DD584
	public override void RegisterRegionFileManager(RegionFileManager regionFileManager)
	{
		object obj = this.regionFileLock;
		lock (obj)
		{
			if (this.activeRegionFileManager != null)
			{
				Log.Warning("[SaveDataManager] Attempting to register a RegionFileManager when one is already registered. The existing reference will be overridden.");
			}
			this.activeRegionFileManager = regionFileManager;
		}
	}

	// Token: 0x06004DD0 RID: 19920 RVA: 0x001DF3D8 File Offset: 0x001DD5D8
	public override void DeregisterRegionFileManager(RegionFileManager regionFileManager)
	{
		object obj = this.regionFileLock;
		lock (obj)
		{
			if (this.activeRegionFileManager != regionFileManager)
			{
				Log.Error("[SaveDataManager] Attempting to deregister a RegionFileManager which is not currently registered.");
			}
			else
			{
				this.activeRegionFileManager = null;
			}
		}
	}

	// Token: 0x06004DD1 RID: 19921 RVA: 0x001DF430 File Offset: 0x001DD630
	public override SaveDataWriteMode GetWriteMode()
	{
		return this.writeMode;
	}

	// Token: 0x06004DD2 RID: 19922 RVA: 0x001DF438 File Offset: 0x001DD638
	public override void SetWriteMode(SaveDataWriteMode writeMode)
	{
		if (this.writeMode == writeMode)
		{
			return;
		}
		object obj = this.commitLockObj;
		lock (obj)
		{
			object obj2 = this.mapLockObj;
			lock (obj2)
			{
				if (writeMode == SaveDataWriteMode.Deferred)
				{
					this.lastCommitTime = DateTime.Now;
					this.writerThreadInfo = ThreadManager.StartThread("SaveDataManager_WriterThread", null, new ThreadManager.ThreadFunctionLoopDelegate(this.WriteTask), null, null, null, false, false);
				}
				else
				{
					ThreadManager.ThreadInfo threadInfo = this.writerThreadInfo;
					if (threadInfo != null)
					{
						threadInfo.WaitForEnd(420);
					}
					this.writerThreadInfo = null;
					this.CommitPendingChanges(true);
				}
				this.writeMode = writeMode;
			}
		}
	}

	// Token: 0x06004DD3 RID: 19923 RVA: 0x001DF504 File Offset: 0x001DD704
	public override void Cleanup()
	{
		this.SetWriteMode(SaveDataWriteMode.None);
		this.cleanupComplete = true;
	}

	// Token: 0x06004DD4 RID: 19924 RVA: 0x001DF514 File Offset: 0x001DD714
	public override int CommitAsync()
	{
		if (this.writeMode != SaveDataWriteMode.Deferred)
		{
			return this.explicitCommitCount;
		}
		int num = this.explicitCommitNeeded + 1;
		this.explicitCommitNeeded = num;
		return num;
	}

	// Token: 0x06004DD5 RID: 19925 RVA: 0x001DF542 File Offset: 0x001DD742
	public override bool IsCommitPending(int token)
	{
		return token > this.explicitCommitCount;
	}

	// Token: 0x06004DD6 RID: 19926 RVA: 0x001DF550 File Offset: 0x001DD750
	public override void CommitSync()
	{
		if (this.writeMode != SaveDataWriteMode.Deferred)
		{
			return;
		}
		int num = this.CommitAsync();
		while (this.IsCommitPending(num) && this.writeMode == SaveDataWriteMode.Deferred)
		{
			Thread.Sleep(5);
		}
	}

	// Token: 0x06004DD7 RID: 19927 RVA: 0x001DF588 File Offset: 0x001DD788
	public override IEnumerator CommitCoroutine(Action<double> progressFeedback = null)
	{
		if (this.writeMode != SaveDataWriteMode.Deferred)
		{
			yield break;
		}
		int token = this.CommitAsync();
		while (this.IsCommitPending(token) && this.writeMode == SaveDataWriteMode.Deferred)
		{
			if (progressFeedback != null)
			{
				progressFeedback(this.platformSaveGameProvider.GetCommitProgress());
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06004DD8 RID: 19928 RVA: 0x001DF59E File Offset: 0x001DD79E
	[Conditional("DEBUG_SAVE_DATA_MANAGER")]
	[PublicizedFrom(EAccessModifier.Private)]
	public new static void DebugLog(string text)
	{
		Log.Out("[SaveDataManager] " + text);
	}

	// Token: 0x06004DDA RID: 19930 RVA: 0x001DF62C File Offset: 0x001DD82C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public unsafe static bool <MatchesSearchPattern>g__MatchWildcard|32_0(StringSpan text, string pattern)
	{
		int i = 0;
		int num = 0;
		int num2 = -1;
		int num3 = -1;
		while (i < text.Length)
		{
			if (num < pattern.Length && (pattern[num] == '?' || char.ToLowerInvariant(pattern[num]) == char.ToLowerInvariant((char)(*text[i]))))
			{
				i++;
				num++;
			}
			else if (num < pattern.Length && pattern[num] == '*')
			{
				num3 = num++;
				num2 = i;
			}
			else
			{
				if (num3 < 0)
				{
					return false;
				}
				num = num3 + 1;
				num2 = (i = num2 + 1);
			}
		}
		while (num < pattern.Length && pattern[num] == '*')
		{
			num++;
		}
		return num == pattern.Length;
	}

	// Token: 0x04003C64 RID: 15460
	[PublicizedFrom(EAccessModifier.Private)]
	public const int writerThreadTerminationTimeout = 420;

	// Token: 0x04003C65 RID: 15461
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IPlatformSaveGameProvider platformSaveGameProvider;

	// Token: 0x04003C66 RID: 15462
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object mapLockObj = new object();

	// Token: 0x04003C67 RID: 15463
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object commitLockObj = new object();

	// Token: 0x04003C68 RID: 15464
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<SaveDataManagedPath, SaveDataManager.CachedStream> pathToCacheMap = new Dictionary<SaveDataManagedPath, SaveDataManager.CachedStream>();

	// Token: 0x04003C69 RID: 15465
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<SaveDataManager.CachedStream> priorityCachedStreams = new HashSet<SaveDataManager.CachedStream>();

	// Token: 0x04003C6A RID: 15466
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Stream, SaveDataManager.CachedStream> streamToCacheMap = new Dictionary<Stream, SaveDataManager.CachedStream>();

	// Token: 0x04003C6B RID: 15467
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly TimeSpan commitInterval = TimeSpan.FromSeconds(30.0);

	// Token: 0x04003C6C RID: 15468
	[PublicizedFrom(EAccessModifier.Private)]
	public const int writeTaskDelay = 5;

	// Token: 0x04003C6D RID: 15469
	[PublicizedFrom(EAccessModifier.Private)]
	public const int memoryCopyTimeout = 30000;

	// Token: 0x04003C6E RID: 15470
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_CommitPendingChangesMarker = new ProfilerMarker("SaveDataManager.CommitPendingChanges");

	// Token: 0x04003C6F RID: 15471
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_InitRestoreBackupsMarker = new ProfilerMarker("SaveDataManager.InitRestoreBackups");

	// Token: 0x04003C70 RID: 15472
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_InitFileSizeTrackingMarker = new ProfilerMarker("SaveDataManager.InitFileSizeTracking");

	// Token: 0x04003C71 RID: 15473
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounterValue<int> s_CachedStreamCount = new ProfilerCounterValue<int>(ProfilerCategory.Scripts, "Cached Streams", ProfilerMarkerDataUnit.Count, ProfilerCounterOptions.FlushOnEndOfFrame);

	// Token: 0x04003C72 RID: 15474
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounterValue<int> s_StreamOperationCount = new ProfilerCounterValue<int>(ProfilerCategory.Scripts, "Stream Operations", ProfilerMarkerDataUnit.Count, ProfilerCounterOptions.FlushOnEndOfFrame | ProfilerCounterOptions.ResetToZeroOnFlush);

	// Token: 0x04003C73 RID: 15475
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo writerThreadInfo;

	// Token: 0x04003C74 RID: 15476
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime lastCommitTime;

	// Token: 0x04003C75 RID: 15477
	[PublicizedFrom(EAccessModifier.Private)]
	public int explicitCommitNeeded;

	// Token: 0x04003C76 RID: 15478
	[PublicizedFrom(EAccessModifier.Private)]
	public int explicitCommitCount;

	// Token: 0x04003C77 RID: 15479
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SaveDataManager.CachedStream> commitList = new List<SaveDataManager.CachedStream>();

	// Token: 0x04003C78 RID: 15480
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SaveDataManager.CachedStream> closeList = new List<SaveDataManager.CachedStream>();

	// Token: 0x04003C79 RID: 15481
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveDataWriteMode writeMode;

	// Token: 0x04003C7A RID: 15482
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cleanupComplete;

	// Token: 0x04003C7B RID: 15483
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<SaveDataSlot, SaveDataManager.SlotSizeData> fileSizesBySlot = new Dictionary<SaveDataSlot, SaveDataManager.SlotSizeData>();

	// Token: 0x04003C7C RID: 15484
	[PublicizedFrom(EAccessModifier.Private)]
	public object regionFileLock = new object();

	// Token: 0x04003C7D RID: 15485
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileManager activeRegionFileManager;

	// Token: 0x02000A2F RID: 2607
	[PublicizedFrom(EAccessModifier.Private)]
	public class CachedStream : IDisposable
	{
		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06004DDB RID: 19931 RVA: 0x001DF6DA File Offset: 0x001DD8DA
		public SaveDataManagedPath Path { get; }

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06004DDC RID: 19932 RVA: 0x001DF6E2 File Offset: 0x001DD8E2
		public long PendingSize
		{
			get
			{
				if (!this.HasPendingChanges)
				{
					return 0L;
				}
				SaveDataManager.CachedStream.VirtualFileStream virtualFileStream = this.commitCopy;
				if (virtualFileStream == null)
				{
					return 0L;
				}
				return virtualFileStream.Length;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06004DDD RID: 19933 RVA: 0x001DF701 File Offset: 0x001DD901
		// (set) Token: 0x06004DDE RID: 19934 RVA: 0x001DF709 File Offset: 0x001DD909
		public bool HasPendingChanges
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06004DDF RID: 19935 RVA: 0x001DF712 File Offset: 0x001DD912
		public bool IsLocked
		{
			get
			{
				return this.sl.IsHeld;
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06004DE0 RID: 19936 RVA: 0x001DF71F File Offset: 0x001DD91F
		public DateTime LastAccessTime
		{
			get
			{
				if (!this.IsLocked)
				{
					return this.lastAccessTime;
				}
				return DateTime.Now;
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06004DE1 RID: 19937 RVA: 0x001DF735 File Offset: 0x001DD935
		public DateTime LastWriteTime
		{
			get
			{
				return this.lastWriteTime;
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06004DE2 RID: 19938 RVA: 0x001DF740 File Offset: 0x001DD940
		public bool ExistsVirtual
		{
			get
			{
				object obj = this.lockObj;
				bool flag2;
				lock (obj)
				{
					flag2 = this.workingCopy != null;
				}
				return flag2;
			}
		}

		// Token: 0x06004DE3 RID: 19939 RVA: 0x001DF788 File Offset: 0x001DD988
		public CachedStream(SaveDataManagedPath path, IPlatformSaveGameProvider platformSaveGameProvider)
		{
			this.Path = path;
			this.backupPath = SaveDataUtils.GetBackupPath(path);
			this.platformSaveGameProvider = platformSaveGameProvider;
			this.lastAccessTime = DateTime.MinValue;
			this.lastWriteTime = platformSaveGameProvider.ManagedFileGetLastWriteTimeUtc(path);
			if (platformSaveGameProvider.ShouldBackup() && platformSaveGameProvider.ManagedFileExists(this.backupPath))
			{
				if (platformSaveGameProvider.ManagedDirectoryExists(this.Path))
				{
					Log.Warning(string.Format("Could not restore to \"{0}\" using \"{1}\" because the former is a directory.", this.Path, this.backupPath));
					return;
				}
				platformSaveGameProvider.ManagedFileDelete(this.Path);
				platformSaveGameProvider.ManagedFileMove(this.backupPath, this.Path);
			}
		}

		// Token: 0x06004DE4 RID: 19940 RVA: 0x001DF850 File Offset: 0x001DDA50
		public void EnterLock(FileMode mode, FileAccess access, FileShare share, out Stream stream)
		{
			bool flag = false;
			stream = null;
			try
			{
				this.sl.Enter(ref flag);
				if (!flag)
				{
					Log.Error("[SaveDataManager] Spin lock should have blocked till it was acquired or thrown an exception.");
				}
				else
				{
					object obj = this.lockObj;
					lock (obj)
					{
						bool flag3 = this.workingCopy != null;
						bool flag4 = this.platformSaveGameProvider.ManagedFileExists(this.Path);
						bool flag5 = flag3 || flag4;
						if (mode == FileMode.Create)
						{
							mode = (flag5 ? FileMode.Truncate : FileMode.CreateNew);
						}
						this.isReading = (access & FileAccess.Read) > (FileAccess)0;
						this.isWriting = (access & FileAccess.Write) > (FileAccess)0;
						if (mode == FileMode.Open || mode == FileMode.OpenOrCreate || mode == FileMode.Append)
						{
							if (!flag3)
							{
								SaveDataManager.CachedStream.VirtualFileStream virtualFileStream;
								if (mode != FileMode.Open && !flag4)
								{
									virtualFileStream = new SaveDataManager.CachedStream.VirtualFileStream();
									this.lastWriteTime = DateTime.UtcNow;
								}
								else
								{
									virtualFileStream = new SaveDataManager.CachedStream.VirtualFileStream();
									this.platformSaveGameProvider.ManagedFileRead(this.Path, virtualFileStream);
								}
								this.workingCopy = virtualFileStream;
							}
							SeekOrigin seekOrigin = ((mode == FileMode.Append) ? SeekOrigin.End : SeekOrigin.Begin);
							this.workingCopy.Seek(0L, seekOrigin);
						}
						else if (mode == FileMode.Truncate)
						{
							if (flag3)
							{
								this.workingCopy.SetLength(0L);
								this.lastWriteTime = DateTime.UtcNow;
							}
							else
							{
								if (!flag4)
								{
									throw new IOException(string.Format("File does not exist at {0}", this.Path));
								}
								this.workingCopy = new SaveDataManager.CachedStream.VirtualFileStream();
								this.lastWriteTime = DateTime.UtcNow;
							}
						}
						else
						{
							if (mode != FileMode.CreateNew)
							{
								throw new NotImplementedException(string.Format("Unhandled mode: {0}", mode));
							}
							if (flag5)
							{
								throw new IOException(string.Format("File already exists at {0}", this.Path));
							}
							this.workingCopy = new SaveDataManager.CachedStream.VirtualFileStream();
							this.lastWriteTime = DateTime.UtcNow;
						}
						this.workingCopy.ReadEnabled = this.isReading;
						this.workingCopy.WriteEnabled = this.isWriting;
						stream = this.workingCopy;
					}
				}
			}
			finally
			{
				if (stream == null && flag)
				{
					this.sl.Exit();
				}
			}
		}

		// Token: 0x06004DE5 RID: 19941 RVA: 0x001DFA6C File Offset: 0x001DDC6C
		public void ExitLock()
		{
			bool flag = false;
			this.EnterCommitLock(ref flag);
			if (flag)
			{
				object obj = this.lockObj;
				lock (obj)
				{
					this.workingCopy.ReadEnabled = true;
					this.workingCopy.WriteEnabled = true;
					if (this.isWriting)
					{
						if (this.commitCopy == null)
						{
							this.commitCopy = new SaveDataManager.CachedStream.VirtualFileStream(this.workingCopy.Length);
						}
						else
						{
							this.commitCopy.Seek(0L, SeekOrigin.Begin);
							this.commitCopy.SetLength(0L);
						}
						this.workingCopy.Position = 0L;
						StreamUtils.StreamCopy(this.workingCopy, this.commitCopy, null, true);
						this.HasPendingChanges = true;
						this.lastWriteTime = DateTime.UtcNow;
					}
					this.isReading = false;
					this.isWriting = false;
					this.lastAccessTime = DateTime.Now;
					this.sl.Exit();
					goto IL_00F4;
				}
			}
			Log.Error(string.Format("[SaveDataManager] Failed to take commit lock on cached stream for path: {0}", this.Path));
			IL_00F4:
			this.ExitCommitLock();
		}

		// Token: 0x06004DE6 RID: 19942 RVA: 0x001DFB84 File Offset: 0x001DDD84
		public void EnterCommitLock(ref bool lockTaken)
		{
			this.commitLock.Enter(ref lockTaken);
		}

		// Token: 0x06004DE7 RID: 19943 RVA: 0x001DFB92 File Offset: 0x001DDD92
		public void ExitCommitLock()
		{
			this.commitLock.Exit();
		}

		// Token: 0x06004DE8 RID: 19944 RVA: 0x001DFBA0 File Offset: 0x001DDDA0
		public void Commit()
		{
			object obj = this.lockObj;
			lock (obj)
			{
				if (this.commitCopy == null)
				{
					Log.Error("CachedStream commit failed: commitCopy is null.");
				}
				else
				{
					if (this.platformSaveGameProvider.ShouldBackup() && this.platformSaveGameProvider.ManagedFileExists(this.Path))
					{
						if (this.platformSaveGameProvider.ManagedDirectoryExists(this.backupPath))
						{
							Log.Warning(string.Format("Could not create backup of \"{0}\" to \"{1}\" because the latter is a directory.", this.Path, this.backupPath));
						}
						else
						{
							this.platformSaveGameProvider.ManagedFileDelete(this.backupPath);
							this.platformSaveGameProvider.ManagedFileMove(this.Path, this.backupPath);
						}
					}
					this.commitCopy.Position = 0L;
					this.platformSaveGameProvider.ManagedFileWrite(this.Path, this.commitCopy);
					if (this.platformSaveGameProvider.ShouldBackup() && this.platformSaveGameProvider.ManagedFileExists(this.backupPath))
					{
						this.platformSaveGameProvider.ManagedFileDelete(this.backupPath);
					}
					this.HasPendingChanges = false;
				}
			}
		}

		// Token: 0x06004DE9 RID: 19945 RVA: 0x001DFCC8 File Offset: 0x001DDEC8
		public void Dispose()
		{
			object obj = this.lockObj;
			lock (obj)
			{
				SaveDataManager.CachedStream.VirtualFileStream virtualFileStream = this.workingCopy;
				if (virtualFileStream != null)
				{
					virtualFileStream.Dispose();
				}
				SaveDataManager.CachedStream.VirtualFileStream virtualFileStream2 = this.commitCopy;
				if (virtualFileStream2 != null)
				{
					virtualFileStream2.Dispose();
				}
				this.workingCopy = null;
				this.commitCopy = null;
				this.HasPendingChanges = false;
			}
		}

		// Token: 0x04003C80 RID: 15488
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastAccessTime;

		// Token: 0x04003C81 RID: 15489
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastWriteTime;

		// Token: 0x04003C82 RID: 15490
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IPlatformSaveGameProvider platformSaveGameProvider;

		// Token: 0x04003C83 RID: 15491
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SaveDataManagedPath backupPath;

		// Token: 0x04003C84 RID: 15492
		[PublicizedFrom(EAccessModifier.Private)]
		public SpinLock sl = new SpinLock(true);

		// Token: 0x04003C85 RID: 15493
		[PublicizedFrom(EAccessModifier.Private)]
		public SpinLock commitLock = new SpinLock(true);

		// Token: 0x04003C86 RID: 15494
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataManager.CachedStream.VirtualFileStream workingCopy;

		// Token: 0x04003C87 RID: 15495
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataManager.CachedStream.VirtualFileStream commitCopy;

		// Token: 0x04003C88 RID: 15496
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isReading;

		// Token: 0x04003C89 RID: 15497
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isWriting;

		// Token: 0x04003C8A RID: 15498
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object lockObj = new object();

		// Token: 0x02000A30 RID: 2608
		[PublicizedFrom(EAccessModifier.Private)]
		public class VirtualFileStream : MemoryStream
		{
			// Token: 0x17000876 RID: 2166
			// (get) Token: 0x06004DEA RID: 19946 RVA: 0x001DFD3C File Offset: 0x001DDF3C
			// (set) Token: 0x06004DEB RID: 19947 RVA: 0x001DFD44 File Offset: 0x001DDF44
			public bool ReadEnabled { get; set; }

			// Token: 0x17000877 RID: 2167
			// (get) Token: 0x06004DEC RID: 19948 RVA: 0x001DFD4D File Offset: 0x001DDF4D
			// (set) Token: 0x06004DED RID: 19949 RVA: 0x001DFD55 File Offset: 0x001DDF55
			public bool WriteEnabled { get; set; }

			// Token: 0x17000878 RID: 2168
			// (get) Token: 0x06004DEE RID: 19950 RVA: 0x001DFD5E File Offset: 0x001DDF5E
			public override bool CanRead
			{
				get
				{
					return this.ReadEnabled && base.CanRead;
				}
			}

			// Token: 0x17000879 RID: 2169
			// (get) Token: 0x06004DEF RID: 19951 RVA: 0x001DFD70 File Offset: 0x001DDF70
			public override bool CanWrite
			{
				get
				{
					return this.WriteEnabled && base.CanWrite;
				}
			}

			// Token: 0x06004DF0 RID: 19952 RVA: 0x001DFD82 File Offset: 0x001DDF82
			public VirtualFileStream()
			{
				this.ReadEnabled = true;
				this.WriteEnabled = true;
			}

			// Token: 0x06004DF1 RID: 19953 RVA: 0x001DFD98 File Offset: 0x001DDF98
			public VirtualFileStream(int capacity)
				: base(capacity)
			{
				this.ReadEnabled = true;
				this.WriteEnabled = true;
			}

			// Token: 0x06004DF2 RID: 19954 RVA: 0x001DFDAF File Offset: 0x001DDFAF
			public VirtualFileStream(long capacity)
				: base((capacity < 2147483647L) ? ((int)capacity) : int.MaxValue)
			{
				this.ReadEnabled = true;
				this.WriteEnabled = true;
			}

			// Token: 0x06004DF3 RID: 19955 RVA: 0x001DFDD7 File Offset: 0x001DDFD7
			public override int Read(byte[] buffer, int offset, int count)
			{
				if (!this.CanRead)
				{
					throw new NotSupportedException("Stream does not support reading.");
				}
				return base.Read(buffer, offset, count);
			}

			// Token: 0x06004DF4 RID: 19956 RVA: 0x001DFDF5 File Offset: 0x001DDFF5
			public override int ReadByte()
			{
				if (!this.CanRead)
				{
					throw new NotSupportedException("Stream does not support reading.");
				}
				return base.ReadByte();
			}
		}
	}

	// Token: 0x02000A31 RID: 2609
	public class SlotSizeData
	{
		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06004DF5 RID: 19957 RVA: 0x001DFE10 File Offset: 0x001DE010
		// (set) Token: 0x06004DF6 RID: 19958 RVA: 0x001DFE18 File Offset: 0x001DE018
		public long TotalFileSize
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06004DF7 RID: 19959 RVA: 0x001DFE21 File Offset: 0x001DE021
		public long LargestPriorityFileSize
		{
			get
			{
				return this.largestPriorityFile.Item2;
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06004DF8 RID: 19960 RVA: 0x001DFE2E File Offset: 0x001DE02E
		public int FileCount
		{
			get
			{
				return this.otherFileSizes.Count + this.priorityFileSizes.Count;
			}
		}

		// Token: 0x06004DF9 RID: 19961 RVA: 0x001DFE47 File Offset: 0x001DE047
		public SlotSizeData(SaveDataSlot saveDataSlot)
		{
			this.saveDataSlot = saveDataSlot;
		}

		// Token: 0x06004DFA RID: 19962 RVA: 0x001DFE6C File Offset: 0x001DE06C
		public void SetFileSize(SaveDataManagedPath managedPath, long pendingSize)
		{
			long num = pendingSize;
			long num2;
			bool flag;
			if (this.TryGetFileSize(managedPath, out num2, out flag))
			{
				num -= num2;
			}
			else
			{
				flag = SaveDataManager.IsPriorityFilePath(managedPath);
			}
			this.TotalFileSize += num;
			if (flag)
			{
				this.priorityFileSizes[managedPath] = pendingSize;
				this.UpdateLargestPriorityFileSize(managedPath, pendingSize, num);
				return;
			}
			this.otherFileSizes[managedPath] = pendingSize;
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x001DFECC File Offset: 0x001DE0CC
		public bool TryGetFileSize(SaveDataManagedPath managedPath, out long trackedSize)
		{
			bool flag;
			return this.TryGetFileSize(managedPath, out trackedSize, out flag);
		}

		// Token: 0x06004DFC RID: 19964 RVA: 0x001DFEE3 File Offset: 0x001DE0E3
		public bool TryGetFileSize(SaveDataManagedPath managedPath, out long trackedSize, out bool isPriorityFile)
		{
			isPriorityFile = this.priorityFileSizes.TryGetValue(managedPath, out trackedSize);
			return isPriorityFile || this.otherFileSizes.TryGetValue(managedPath, out trackedSize);
		}

		// Token: 0x06004DFD RID: 19965 RVA: 0x001DFF08 File Offset: 0x001DE108
		public void RemoveFileSize(SaveDataManagedPath managedPath)
		{
			long num;
			bool flag;
			if (this.TryGetFileSize(managedPath, out num, out flag))
			{
				this.TotalFileSize -= num;
				if (flag)
				{
					this.priorityFileSizes.Remove(managedPath);
					this.UpdateLargestPriorityFileSize(managedPath, 0L, -num);
					return;
				}
				this.otherFileSizes.Remove(managedPath);
			}
		}

		// Token: 0x06004DFE RID: 19966 RVA: 0x001DFF5C File Offset: 0x001DE15C
		public void RemoveFileSizes(SaveDataManagedPath parentManagedPath)
		{
			SaveDataManager.SlotSizeData.<>c__DisplayClass17_0 CS$<>8__locals1;
			CS$<>8__locals1.parentManagedPath = parentManagedPath;
			CS$<>8__locals1.<>4__this = this;
			List<SaveDataManagedPath> list = this.priorityFileSizes.Keys.ToList<SaveDataManagedPath>();
			this.<RemoveFileSizes>g__RemoveFileSizesInternal|17_0(list, ref CS$<>8__locals1);
			List<SaveDataManagedPath> list2 = this.otherFileSizes.Keys.ToList<SaveDataManagedPath>();
			this.<RemoveFileSizes>g__RemoveFileSizesInternal|17_0(list2, ref CS$<>8__locals1);
		}

		// Token: 0x06004DFF RID: 19967 RVA: 0x001DFFB0 File Offset: 0x001DE1B0
		[PublicizedFrom(EAccessModifier.Private)]
		public void UpdateLargestPriorityFileSize(SaveDataManagedPath managedPath, long pendingSize, long delta)
		{
			if (delta == 0L)
			{
				return;
			}
			if (pendingSize > this.largestPriorityFile.Item2)
			{
				this.largestPriorityFile = new ValueTuple<SaveDataManagedPath, long>(managedPath, pendingSize);
				return;
			}
			if (delta < 0L && this.largestPriorityFile.Item1 == managedPath)
			{
				this.largestPriorityFile = default(ValueTuple<SaveDataManagedPath, long>);
				foreach (KeyValuePair<SaveDataManagedPath, long> keyValuePair in this.priorityFileSizes)
				{
					if (keyValuePair.Value > this.largestPriorityFile.Item2)
					{
						this.largestPriorityFile = new ValueTuple<SaveDataManagedPath, long>(keyValuePair.Key, pendingSize);
					}
				}
			}
		}

		// Token: 0x06004E00 RID: 19968 RVA: 0x001E0068 File Offset: 0x001DE268
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <RemoveFileSizes>g__RemoveFileSizesInternal|17_0(List<SaveDataManagedPath> managedPaths, ref SaveDataManager.SlotSizeData.<>c__DisplayClass17_0 A_2)
		{
			foreach (SaveDataManagedPath saveDataManagedPath in managedPaths)
			{
				if (A_2.parentManagedPath.IsParentOf(saveDataManagedPath))
				{
					this.RemoveFileSize(saveDataManagedPath);
				}
			}
		}

		// Token: 0x04003C8D RID: 15501
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<SaveDataManagedPath, long> priorityFileSizes = new Dictionary<SaveDataManagedPath, long>();

		// Token: 0x04003C8E RID: 15502
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<SaveDataManagedPath, long> otherFileSizes = new Dictionary<SaveDataManagedPath, long>();

		// Token: 0x04003C8F RID: 15503
		[TupleElementNames(new string[] { "path", "size" })]
		[PublicizedFrom(EAccessModifier.Private)]
		public ValueTuple<SaveDataManagedPath, long> largestPriorityFile;

		// Token: 0x04003C91 RID: 15505
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SaveDataSlot saveDataSlot;
	}
}
