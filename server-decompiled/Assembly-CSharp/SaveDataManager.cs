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

// Token: 0x02000A1A RID: 2586
public class SaveDataManager : SaveDataManagerBase
{
	// Token: 0x17000848 RID: 2120
	// (get) Token: 0x06004DBB RID: 19899 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AppliesSaveSizeLimit
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004DBC RID: 19900 RVA: 0x001DBF30 File Offset: 0x001DA130
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

	// Token: 0x06004DBD RID: 19901 RVA: 0x001DC0D8 File Offset: 0x001DA2D8
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool IsPriorityFilePath(SaveDataManagedPath path)
	{
		return path.Type == SaveDataType.Saves && path.PathRelativeToSlot.AsSpan().IndexOf<char>("Region") == 0;
	}

	// Token: 0x06004DBE RID: 19902 RVA: 0x001DC110 File Offset: 0x001DA310
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

	// Token: 0x06004DBF RID: 19903 RVA: 0x001DC218 File Offset: 0x001DA418
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

	// Token: 0x06004DC0 RID: 19904 RVA: 0x001DC278 File Offset: 0x001DA478
	public override bool ShouldLimitSize()
	{
		return this.platformSaveGameProvider.ShouldLimitSize();
	}

	// Token: 0x06004DC1 RID: 19905 RVA: 0x001DC285 File Offset: 0x001DA485
	public override void UpdateSizes()
	{
		this.platformSaveGameProvider.UpdateSizes();
	}

	// Token: 0x06004DC2 RID: 19906 RVA: 0x001DC292 File Offset: 0x001DA492
	public override SaveDataSizes GetSizes()
	{
		return this.platformSaveGameProvider.GetSizes();
	}

	// Token: 0x06004DC3 RID: 19907 RVA: 0x001DC2A0 File Offset: 0x001DA4A0
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

	// Token: 0x06004DC4 RID: 19908 RVA: 0x001DC368 File Offset: 0x001DA568
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

	// Token: 0x06004DC5 RID: 19909 RVA: 0x001DC3CC File Offset: 0x001DA5CC
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

	// Token: 0x06004DC6 RID: 19910 RVA: 0x001DC430 File Offset: 0x001DA630
	public override SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
	{
		return this.platformSaveGameProvider.ManagedDirectoryCreateDirectory(path);
	}

	// Token: 0x06004DC7 RID: 19911 RVA: 0x001DC43E File Offset: 0x001DA63E
	public override DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return this.platformSaveGameProvider.ManagedDirectoryGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004DC8 RID: 19912 RVA: 0x001DC44C File Offset: 0x001DA64C
	public override bool ManagedDirectoryExists(SaveDataManagedPath path)
	{
		return this.platformSaveGameProvider.ManagedDirectoryExists(path);
	}

	// Token: 0x06004DC9 RID: 19913 RVA: 0x001DC45A File Offset: 0x001DA65A
	public override IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.platformSaveGameProvider.ManagedDirectoryEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004DCA RID: 19914 RVA: 0x001DC46A File Offset: 0x001DA66A
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

	// Token: 0x06004DCB RID: 19915 RVA: 0x001DC48F File Offset: 0x001DA68F
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

	// Token: 0x06004DCC RID: 19916 RVA: 0x001DC4B4 File Offset: 0x001DA6B4
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

	// Token: 0x06004DCD RID: 19917 RVA: 0x001DC65C File Offset: 0x001DA85C
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

	// Token: 0x06004DCE RID: 19918 RVA: 0x001DC6C8 File Offset: 0x001DA8C8
	public override IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.platformSaveGameProvider.ManagedDirectoryInfoEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004DCF RID: 19919 RVA: 0x001DC6D8 File Offset: 0x001DA8D8
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

	// Token: 0x06004DD0 RID: 19920 RVA: 0x001DC6FD File Offset: 0x001DA8FD
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

	// Token: 0x06004DD1 RID: 19921 RVA: 0x001DC724 File Offset: 0x001DA924
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

	// Token: 0x06004DD2 RID: 19922 RVA: 0x001DC780 File Offset: 0x001DA980
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

	// Token: 0x06004DD3 RID: 19923 RVA: 0x001DC7D4 File Offset: 0x001DA9D4
	public SaveDataManager(IPlatformSaveGameProvider platformSaveGameProvider)
	{
		this.platformSaveGameProvider = new SaveDataMergedPlatformSaveGameIOProvider(platformSaveGameProvider);
	}

	// Token: 0x06004DD4 RID: 19924 RVA: 0x001DC856 File Offset: 0x001DAA56
	public override void Init()
	{
		this.SetWriteMode(SaveDataWriteMode.Deferred);
		this.InitRestoreBackups();
		this.InitFileSizeTracking();
	}

	// Token: 0x06004DD5 RID: 19925 RVA: 0x001DC86C File Offset: 0x001DAA6C
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

	// Token: 0x06004DD6 RID: 19926 RVA: 0x001DC954 File Offset: 0x001DAB54
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

	// Token: 0x06004DD7 RID: 19927 RVA: 0x001DCA44 File Offset: 0x001DAC44
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

	// Token: 0x06004DD8 RID: 19928 RVA: 0x001DCA7C File Offset: 0x001DAC7C
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

	// Token: 0x06004DD9 RID: 19929 RVA: 0x001DCAFC File Offset: 0x001DACFC
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

	// Token: 0x06004DDA RID: 19930 RVA: 0x001DCE40 File Offset: 0x001DB040
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

	// Token: 0x06004DDB RID: 19931 RVA: 0x001DCECC File Offset: 0x001DB0CC
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

	// Token: 0x06004DDC RID: 19932 RVA: 0x001DCFEC File Offset: 0x001DB1EC
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

	// Token: 0x06004DDD RID: 19933 RVA: 0x001DD040 File Offset: 0x001DB240
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

	// Token: 0x06004DDE RID: 19934 RVA: 0x001DD098 File Offset: 0x001DB298
	public override SaveDataWriteMode GetWriteMode()
	{
		return this.writeMode;
	}

	// Token: 0x06004DDF RID: 19935 RVA: 0x001DD0A0 File Offset: 0x001DB2A0
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

	// Token: 0x06004DE0 RID: 19936 RVA: 0x001DD16C File Offset: 0x001DB36C
	public override void Cleanup()
	{
		this.SetWriteMode(SaveDataWriteMode.None);
		this.cleanupComplete = true;
	}

	// Token: 0x06004DE1 RID: 19937 RVA: 0x001DD17C File Offset: 0x001DB37C
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

	// Token: 0x06004DE2 RID: 19938 RVA: 0x001DD1AA File Offset: 0x001DB3AA
	public override bool IsCommitPending(int token)
	{
		return token > this.explicitCommitCount;
	}

	// Token: 0x06004DE3 RID: 19939 RVA: 0x001DD1B8 File Offset: 0x001DB3B8
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

	// Token: 0x06004DE4 RID: 19940 RVA: 0x001DD1F0 File Offset: 0x001DB3F0
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

	// Token: 0x06004DE5 RID: 19941 RVA: 0x001DD206 File Offset: 0x001DB406
	[Conditional("DEBUG_SAVE_DATA_MANAGER")]
	[PublicizedFrom(EAccessModifier.Private)]
	public new static void DebugLog(string text)
	{
		Log.Out("[SaveDataManager] " + text);
	}

	// Token: 0x06004DE7 RID: 19943 RVA: 0x001DD294 File Offset: 0x001DB494
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

	// Token: 0x04003C36 RID: 15414
	[PublicizedFrom(EAccessModifier.Private)]
	public const int writerThreadTerminationTimeout = 420;

	// Token: 0x04003C37 RID: 15415
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IPlatformSaveGameProvider platformSaveGameProvider;

	// Token: 0x04003C38 RID: 15416
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object mapLockObj = new object();

	// Token: 0x04003C39 RID: 15417
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object commitLockObj = new object();

	// Token: 0x04003C3A RID: 15418
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<SaveDataManagedPath, SaveDataManager.CachedStream> pathToCacheMap = new Dictionary<SaveDataManagedPath, SaveDataManager.CachedStream>();

	// Token: 0x04003C3B RID: 15419
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<SaveDataManager.CachedStream> priorityCachedStreams = new HashSet<SaveDataManager.CachedStream>();

	// Token: 0x04003C3C RID: 15420
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Stream, SaveDataManager.CachedStream> streamToCacheMap = new Dictionary<Stream, SaveDataManager.CachedStream>();

	// Token: 0x04003C3D RID: 15421
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly TimeSpan commitInterval = TimeSpan.FromSeconds(30.0);

	// Token: 0x04003C3E RID: 15422
	[PublicizedFrom(EAccessModifier.Private)]
	public const int writeTaskDelay = 5;

	// Token: 0x04003C3F RID: 15423
	[PublicizedFrom(EAccessModifier.Private)]
	public const int memoryCopyTimeout = 30000;

	// Token: 0x04003C40 RID: 15424
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_CommitPendingChangesMarker = new ProfilerMarker("SaveDataManager.CommitPendingChanges");

	// Token: 0x04003C41 RID: 15425
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_InitRestoreBackupsMarker = new ProfilerMarker("SaveDataManager.InitRestoreBackups");

	// Token: 0x04003C42 RID: 15426
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_InitFileSizeTrackingMarker = new ProfilerMarker("SaveDataManager.InitFileSizeTracking");

	// Token: 0x04003C43 RID: 15427
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounterValue<int> s_CachedStreamCount = new ProfilerCounterValue<int>(ProfilerCategory.Scripts, "Cached Streams", ProfilerMarkerDataUnit.Count, ProfilerCounterOptions.FlushOnEndOfFrame);

	// Token: 0x04003C44 RID: 15428
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounterValue<int> s_StreamOperationCount = new ProfilerCounterValue<int>(ProfilerCategory.Scripts, "Stream Operations", ProfilerMarkerDataUnit.Count, ProfilerCounterOptions.FlushOnEndOfFrame | ProfilerCounterOptions.ResetToZeroOnFlush);

	// Token: 0x04003C45 RID: 15429
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo writerThreadInfo;

	// Token: 0x04003C46 RID: 15430
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime lastCommitTime;

	// Token: 0x04003C47 RID: 15431
	[PublicizedFrom(EAccessModifier.Private)]
	public int explicitCommitNeeded;

	// Token: 0x04003C48 RID: 15432
	[PublicizedFrom(EAccessModifier.Private)]
	public int explicitCommitCount;

	// Token: 0x04003C49 RID: 15433
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SaveDataManager.CachedStream> commitList = new List<SaveDataManager.CachedStream>();

	// Token: 0x04003C4A RID: 15434
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SaveDataManager.CachedStream> closeList = new List<SaveDataManager.CachedStream>();

	// Token: 0x04003C4B RID: 15435
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveDataWriteMode writeMode;

	// Token: 0x04003C4C RID: 15436
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cleanupComplete;

	// Token: 0x04003C4D RID: 15437
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<SaveDataSlot, SaveDataManager.SlotSizeData> fileSizesBySlot = new Dictionary<SaveDataSlot, SaveDataManager.SlotSizeData>();

	// Token: 0x04003C4E RID: 15438
	[PublicizedFrom(EAccessModifier.Private)]
	public object regionFileLock = new object();

	// Token: 0x04003C4F RID: 15439
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileManager activeRegionFileManager;

	// Token: 0x02000A1B RID: 2587
	[PublicizedFrom(EAccessModifier.Private)]
	public class CachedStream : IDisposable
	{
		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06004DE8 RID: 19944 RVA: 0x001DD342 File Offset: 0x001DB542
		public SaveDataManagedPath Path { get; }

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06004DE9 RID: 19945 RVA: 0x001DD34A File Offset: 0x001DB54A
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

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06004DEA RID: 19946 RVA: 0x001DD369 File Offset: 0x001DB569
		// (set) Token: 0x06004DEB RID: 19947 RVA: 0x001DD371 File Offset: 0x001DB571
		public bool HasPendingChanges
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06004DEC RID: 19948 RVA: 0x001DD37A File Offset: 0x001DB57A
		public bool IsLocked
		{
			get
			{
				return this.sl.IsHeld;
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06004DED RID: 19949 RVA: 0x001DD387 File Offset: 0x001DB587
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

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06004DEE RID: 19950 RVA: 0x001DD39D File Offset: 0x001DB59D
		public DateTime LastWriteTime
		{
			get
			{
				return this.lastWriteTime;
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06004DEF RID: 19951 RVA: 0x001DD3A8 File Offset: 0x001DB5A8
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

		// Token: 0x06004DF0 RID: 19952 RVA: 0x001DD3F0 File Offset: 0x001DB5F0
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

		// Token: 0x06004DF1 RID: 19953 RVA: 0x001DD4B8 File Offset: 0x001DB6B8
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

		// Token: 0x06004DF2 RID: 19954 RVA: 0x001DD6D4 File Offset: 0x001DB8D4
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

		// Token: 0x06004DF3 RID: 19955 RVA: 0x001DD7EC File Offset: 0x001DB9EC
		public void EnterCommitLock(ref bool lockTaken)
		{
			this.commitLock.Enter(ref lockTaken);
		}

		// Token: 0x06004DF4 RID: 19956 RVA: 0x001DD7FA File Offset: 0x001DB9FA
		public void ExitCommitLock()
		{
			this.commitLock.Exit();
		}

		// Token: 0x06004DF5 RID: 19957 RVA: 0x001DD808 File Offset: 0x001DBA08
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

		// Token: 0x06004DF6 RID: 19958 RVA: 0x001DD930 File Offset: 0x001DBB30
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

		// Token: 0x04003C52 RID: 15442
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastAccessTime;

		// Token: 0x04003C53 RID: 15443
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastWriteTime;

		// Token: 0x04003C54 RID: 15444
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IPlatformSaveGameProvider platformSaveGameProvider;

		// Token: 0x04003C55 RID: 15445
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SaveDataManagedPath backupPath;

		// Token: 0x04003C56 RID: 15446
		[PublicizedFrom(EAccessModifier.Private)]
		public SpinLock sl = new SpinLock(true);

		// Token: 0x04003C57 RID: 15447
		[PublicizedFrom(EAccessModifier.Private)]
		public SpinLock commitLock = new SpinLock(true);

		// Token: 0x04003C58 RID: 15448
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataManager.CachedStream.VirtualFileStream workingCopy;

		// Token: 0x04003C59 RID: 15449
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataManager.CachedStream.VirtualFileStream commitCopy;

		// Token: 0x04003C5A RID: 15450
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isReading;

		// Token: 0x04003C5B RID: 15451
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isWriting;

		// Token: 0x04003C5C RID: 15452
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object lockObj = new object();

		// Token: 0x02000A1C RID: 2588
		[PublicizedFrom(EAccessModifier.Private)]
		public class VirtualFileStream : MemoryStream
		{
			// Token: 0x17000850 RID: 2128
			// (get) Token: 0x06004DF7 RID: 19959 RVA: 0x001DD9A4 File Offset: 0x001DBBA4
			// (set) Token: 0x06004DF8 RID: 19960 RVA: 0x001DD9AC File Offset: 0x001DBBAC
			public bool ReadEnabled { get; set; }

			// Token: 0x17000851 RID: 2129
			// (get) Token: 0x06004DF9 RID: 19961 RVA: 0x001DD9B5 File Offset: 0x001DBBB5
			// (set) Token: 0x06004DFA RID: 19962 RVA: 0x001DD9BD File Offset: 0x001DBBBD
			public bool WriteEnabled { get; set; }

			// Token: 0x17000852 RID: 2130
			// (get) Token: 0x06004DFB RID: 19963 RVA: 0x001DD9C6 File Offset: 0x001DBBC6
			public override bool CanRead
			{
				get
				{
					return this.ReadEnabled && base.CanRead;
				}
			}

			// Token: 0x17000853 RID: 2131
			// (get) Token: 0x06004DFC RID: 19964 RVA: 0x001DD9D8 File Offset: 0x001DBBD8
			public override bool CanWrite
			{
				get
				{
					return this.WriteEnabled && base.CanWrite;
				}
			}

			// Token: 0x06004DFD RID: 19965 RVA: 0x001DD9EA File Offset: 0x001DBBEA
			public VirtualFileStream()
			{
				this.ReadEnabled = true;
				this.WriteEnabled = true;
			}

			// Token: 0x06004DFE RID: 19966 RVA: 0x001DDA00 File Offset: 0x001DBC00
			public VirtualFileStream(int capacity)
				: base(capacity)
			{
				this.ReadEnabled = true;
				this.WriteEnabled = true;
			}

			// Token: 0x06004DFF RID: 19967 RVA: 0x001DDA17 File Offset: 0x001DBC17
			public VirtualFileStream(long capacity)
				: base((capacity < 2147483647L) ? ((int)capacity) : int.MaxValue)
			{
				this.ReadEnabled = true;
				this.WriteEnabled = true;
			}

			// Token: 0x06004E00 RID: 19968 RVA: 0x001DDA3F File Offset: 0x001DBC3F
			public override int Read(byte[] buffer, int offset, int count)
			{
				if (!this.CanRead)
				{
					throw new NotSupportedException("Stream does not support reading.");
				}
				return base.Read(buffer, offset, count);
			}

			// Token: 0x06004E01 RID: 19969 RVA: 0x001DDA5D File Offset: 0x001DBC5D
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

	// Token: 0x02000A1D RID: 2589
	public class SlotSizeData
	{
		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06004E02 RID: 19970 RVA: 0x001DDA78 File Offset: 0x001DBC78
		// (set) Token: 0x06004E03 RID: 19971 RVA: 0x001DDA80 File Offset: 0x001DBC80
		public long TotalFileSize
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06004E04 RID: 19972 RVA: 0x001DDA89 File Offset: 0x001DBC89
		public long LargestPriorityFileSize
		{
			get
			{
				return this.largestPriorityFile.Item2;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06004E05 RID: 19973 RVA: 0x001DDA96 File Offset: 0x001DBC96
		public int FileCount
		{
			get
			{
				return this.otherFileSizes.Count + this.priorityFileSizes.Count;
			}
		}

		// Token: 0x06004E06 RID: 19974 RVA: 0x001DDAAF File Offset: 0x001DBCAF
		public SlotSizeData(SaveDataSlot saveDataSlot)
		{
			this.saveDataSlot = saveDataSlot;
		}

		// Token: 0x06004E07 RID: 19975 RVA: 0x001DDAD4 File Offset: 0x001DBCD4
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

		// Token: 0x06004E08 RID: 19976 RVA: 0x001DDB34 File Offset: 0x001DBD34
		public bool TryGetFileSize(SaveDataManagedPath managedPath, out long trackedSize)
		{
			bool flag;
			return this.TryGetFileSize(managedPath, out trackedSize, out flag);
		}

		// Token: 0x06004E09 RID: 19977 RVA: 0x001DDB4B File Offset: 0x001DBD4B
		public bool TryGetFileSize(SaveDataManagedPath managedPath, out long trackedSize, out bool isPriorityFile)
		{
			isPriorityFile = this.priorityFileSizes.TryGetValue(managedPath, out trackedSize);
			return isPriorityFile || this.otherFileSizes.TryGetValue(managedPath, out trackedSize);
		}

		// Token: 0x06004E0A RID: 19978 RVA: 0x001DDB70 File Offset: 0x001DBD70
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

		// Token: 0x06004E0B RID: 19979 RVA: 0x001DDBC4 File Offset: 0x001DBDC4
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

		// Token: 0x06004E0C RID: 19980 RVA: 0x001DDC18 File Offset: 0x001DBE18
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

		// Token: 0x06004E0D RID: 19981 RVA: 0x001DDCD0 File Offset: 0x001DBED0
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

		// Token: 0x04003C5F RID: 15455
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<SaveDataManagedPath, long> priorityFileSizes = new Dictionary<SaveDataManagedPath, long>();

		// Token: 0x04003C60 RID: 15456
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<SaveDataManagedPath, long> otherFileSizes = new Dictionary<SaveDataManagedPath, long>();

		// Token: 0x04003C61 RID: 15457
		[TupleElementNames(new string[] { "path", "size" })]
		[PublicizedFrom(EAccessModifier.Private)]
		public ValueTuple<SaveDataManagedPath, long> largestPriorityFile;

		// Token: 0x04003C63 RID: 15459
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SaveDataSlot saveDataSlot;
	}
}
