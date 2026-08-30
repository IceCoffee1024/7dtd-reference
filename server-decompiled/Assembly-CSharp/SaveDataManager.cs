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

// Token: 0x02000A09 RID: 2569
public class SaveDataManager : SaveDataManagerBase
{
	// Token: 0x17000836 RID: 2102
	// (get) Token: 0x06004D35 RID: 19765 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AppliesSaveSizeLimit
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004D36 RID: 19766 RVA: 0x001D9498 File Offset: 0x001D7698
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

	// Token: 0x06004D37 RID: 19767 RVA: 0x001D9640 File Offset: 0x001D7840
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool IsPriorityFilePath(SaveDataManagedPath path)
	{
		return path.Type == SaveDataType.Saves && path.PathRelativeToSlot.AsSpan().IndexOf<char>("Region") == 0;
	}

	// Token: 0x06004D38 RID: 19768 RVA: 0x001D9678 File Offset: 0x001D7878
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

	// Token: 0x06004D39 RID: 19769 RVA: 0x001D9780 File Offset: 0x001D7980
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

	// Token: 0x06004D3A RID: 19770 RVA: 0x001D97E0 File Offset: 0x001D79E0
	public override bool ShouldLimitSize()
	{
		return this.platformSaveGameProvider.ShouldLimitSize();
	}

	// Token: 0x06004D3B RID: 19771 RVA: 0x001D97ED File Offset: 0x001D79ED
	public override void UpdateSizes()
	{
		this.platformSaveGameProvider.UpdateSizes();
	}

	// Token: 0x06004D3C RID: 19772 RVA: 0x001D97FA File Offset: 0x001D79FA
	public override SaveDataSizes GetSizes()
	{
		return this.platformSaveGameProvider.GetSizes();
	}

	// Token: 0x06004D3D RID: 19773 RVA: 0x001D9808 File Offset: 0x001D7A08
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

	// Token: 0x06004D3E RID: 19774 RVA: 0x001D98D0 File Offset: 0x001D7AD0
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

	// Token: 0x06004D3F RID: 19775 RVA: 0x001D9934 File Offset: 0x001D7B34
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

	// Token: 0x06004D40 RID: 19776 RVA: 0x001D9998 File Offset: 0x001D7B98
	public override SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
	{
		return this.platformSaveGameProvider.ManagedDirectoryCreateDirectory(path);
	}

	// Token: 0x06004D41 RID: 19777 RVA: 0x001D99A6 File Offset: 0x001D7BA6
	public override DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return this.platformSaveGameProvider.ManagedDirectoryGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004D42 RID: 19778 RVA: 0x001D99B4 File Offset: 0x001D7BB4
	public override bool ManagedDirectoryExists(SaveDataManagedPath path)
	{
		return this.platformSaveGameProvider.ManagedDirectoryExists(path);
	}

	// Token: 0x06004D43 RID: 19779 RVA: 0x001D99C2 File Offset: 0x001D7BC2
	public override IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.platformSaveGameProvider.ManagedDirectoryEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004D44 RID: 19780 RVA: 0x001D99D2 File Offset: 0x001D7BD2
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

	// Token: 0x06004D45 RID: 19781 RVA: 0x001D99F7 File Offset: 0x001D7BF7
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

	// Token: 0x06004D46 RID: 19782 RVA: 0x001D9A1C File Offset: 0x001D7C1C
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

	// Token: 0x06004D47 RID: 19783 RVA: 0x001D9BC4 File Offset: 0x001D7DC4
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

	// Token: 0x06004D48 RID: 19784 RVA: 0x001D9C30 File Offset: 0x001D7E30
	public override IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.platformSaveGameProvider.ManagedDirectoryInfoEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004D49 RID: 19785 RVA: 0x001D9C40 File Offset: 0x001D7E40
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

	// Token: 0x06004D4A RID: 19786 RVA: 0x001D9C65 File Offset: 0x001D7E65
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

	// Token: 0x06004D4B RID: 19787 RVA: 0x001D9C8C File Offset: 0x001D7E8C
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

	// Token: 0x06004D4C RID: 19788 RVA: 0x001D9CE8 File Offset: 0x001D7EE8
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

	// Token: 0x06004D4D RID: 19789 RVA: 0x001D9D3C File Offset: 0x001D7F3C
	public SaveDataManager(IPlatformSaveGameProvider platformSaveGameProvider)
	{
		this.platformSaveGameProvider = new SaveDataMergedPlatformSaveGameIOProvider(platformSaveGameProvider);
	}

	// Token: 0x06004D4E RID: 19790 RVA: 0x001D9DBE File Offset: 0x001D7FBE
	public override void Init()
	{
		this.SetWriteMode(SaveDataWriteMode.Deferred);
		this.InitRestoreBackups();
		this.InitFileSizeTracking();
	}

	// Token: 0x06004D4F RID: 19791 RVA: 0x001D9DD4 File Offset: 0x001D7FD4
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

	// Token: 0x06004D50 RID: 19792 RVA: 0x001D9EBC File Offset: 0x001D80BC
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

	// Token: 0x06004D51 RID: 19793 RVA: 0x001D9FAC File Offset: 0x001D81AC
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

	// Token: 0x06004D52 RID: 19794 RVA: 0x001D9FE4 File Offset: 0x001D81E4
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

	// Token: 0x06004D53 RID: 19795 RVA: 0x001DA064 File Offset: 0x001D8264
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

	// Token: 0x06004D54 RID: 19796 RVA: 0x001DA3A8 File Offset: 0x001D85A8
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

	// Token: 0x06004D55 RID: 19797 RVA: 0x001DA434 File Offset: 0x001D8634
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

	// Token: 0x06004D56 RID: 19798 RVA: 0x001DA554 File Offset: 0x001D8754
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

	// Token: 0x06004D57 RID: 19799 RVA: 0x001DA5A8 File Offset: 0x001D87A8
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

	// Token: 0x06004D58 RID: 19800 RVA: 0x001DA600 File Offset: 0x001D8800
	public override SaveDataWriteMode GetWriteMode()
	{
		return this.writeMode;
	}

	// Token: 0x06004D59 RID: 19801 RVA: 0x001DA608 File Offset: 0x001D8808
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

	// Token: 0x06004D5A RID: 19802 RVA: 0x001DA6D4 File Offset: 0x001D88D4
	public override void Cleanup()
	{
		this.SetWriteMode(SaveDataWriteMode.None);
		this.cleanupComplete = true;
	}

	// Token: 0x06004D5B RID: 19803 RVA: 0x001DA6E4 File Offset: 0x001D88E4
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

	// Token: 0x06004D5C RID: 19804 RVA: 0x001DA712 File Offset: 0x001D8912
	public override bool IsCommitPending(int token)
	{
		return token > this.explicitCommitCount;
	}

	// Token: 0x06004D5D RID: 19805 RVA: 0x001DA720 File Offset: 0x001D8920
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

	// Token: 0x06004D5E RID: 19806 RVA: 0x001DA758 File Offset: 0x001D8958
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

	// Token: 0x06004D5F RID: 19807 RVA: 0x001DA76E File Offset: 0x001D896E
	[Conditional("DEBUG_SAVE_DATA_MANAGER")]
	[PublicizedFrom(EAccessModifier.Private)]
	public new static void DebugLog(string text)
	{
		Log.Out("[SaveDataManager] " + text);
	}

	// Token: 0x06004D61 RID: 19809 RVA: 0x001DA7FC File Offset: 0x001D89FC
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

	// Token: 0x04003BA5 RID: 15269
	[PublicizedFrom(EAccessModifier.Private)]
	public const int writerThreadTerminationTimeout = 420;

	// Token: 0x04003BA6 RID: 15270
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IPlatformSaveGameProvider platformSaveGameProvider;

	// Token: 0x04003BA7 RID: 15271
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object mapLockObj = new object();

	// Token: 0x04003BA8 RID: 15272
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object commitLockObj = new object();

	// Token: 0x04003BA9 RID: 15273
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<SaveDataManagedPath, SaveDataManager.CachedStream> pathToCacheMap = new Dictionary<SaveDataManagedPath, SaveDataManager.CachedStream>();

	// Token: 0x04003BAA RID: 15274
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<SaveDataManager.CachedStream> priorityCachedStreams = new HashSet<SaveDataManager.CachedStream>();

	// Token: 0x04003BAB RID: 15275
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Stream, SaveDataManager.CachedStream> streamToCacheMap = new Dictionary<Stream, SaveDataManager.CachedStream>();

	// Token: 0x04003BAC RID: 15276
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly TimeSpan commitInterval = TimeSpan.FromSeconds(30.0);

	// Token: 0x04003BAD RID: 15277
	[PublicizedFrom(EAccessModifier.Private)]
	public const int writeTaskDelay = 5;

	// Token: 0x04003BAE RID: 15278
	[PublicizedFrom(EAccessModifier.Private)]
	public const int memoryCopyTimeout = 30000;

	// Token: 0x04003BAF RID: 15279
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_CommitPendingChangesMarker = new ProfilerMarker("SaveDataManager.CommitPendingChanges");

	// Token: 0x04003BB0 RID: 15280
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_InitRestoreBackupsMarker = new ProfilerMarker("SaveDataManager.InitRestoreBackups");

	// Token: 0x04003BB1 RID: 15281
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_InitFileSizeTrackingMarker = new ProfilerMarker("SaveDataManager.InitFileSizeTracking");

	// Token: 0x04003BB2 RID: 15282
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounterValue<int> s_CachedStreamCount = new ProfilerCounterValue<int>(ProfilerCategory.Scripts, "Cached Streams", ProfilerMarkerDataUnit.Count, ProfilerCounterOptions.FlushOnEndOfFrame);

	// Token: 0x04003BB3 RID: 15283
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounterValue<int> s_StreamOperationCount = new ProfilerCounterValue<int>(ProfilerCategory.Scripts, "Stream Operations", ProfilerMarkerDataUnit.Count, ProfilerCounterOptions.FlushOnEndOfFrame | ProfilerCounterOptions.ResetToZeroOnFlush);

	// Token: 0x04003BB4 RID: 15284
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo writerThreadInfo;

	// Token: 0x04003BB5 RID: 15285
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime lastCommitTime;

	// Token: 0x04003BB6 RID: 15286
	[PublicizedFrom(EAccessModifier.Private)]
	public int explicitCommitNeeded;

	// Token: 0x04003BB7 RID: 15287
	[PublicizedFrom(EAccessModifier.Private)]
	public int explicitCommitCount;

	// Token: 0x04003BB8 RID: 15288
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SaveDataManager.CachedStream> commitList = new List<SaveDataManager.CachedStream>();

	// Token: 0x04003BB9 RID: 15289
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SaveDataManager.CachedStream> closeList = new List<SaveDataManager.CachedStream>();

	// Token: 0x04003BBA RID: 15290
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveDataWriteMode writeMode;

	// Token: 0x04003BBB RID: 15291
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cleanupComplete;

	// Token: 0x04003BBC RID: 15292
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<SaveDataSlot, SaveDataManager.SlotSizeData> fileSizesBySlot = new Dictionary<SaveDataSlot, SaveDataManager.SlotSizeData>();

	// Token: 0x04003BBD RID: 15293
	[PublicizedFrom(EAccessModifier.Private)]
	public object regionFileLock = new object();

	// Token: 0x04003BBE RID: 15294
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileManager activeRegionFileManager;

	// Token: 0x02000A0A RID: 2570
	[PublicizedFrom(EAccessModifier.Private)]
	public class CachedStream : IDisposable
	{
		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06004D62 RID: 19810 RVA: 0x001DA8AA File Offset: 0x001D8AAA
		public SaveDataManagedPath Path { get; }

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06004D63 RID: 19811 RVA: 0x001DA8B2 File Offset: 0x001D8AB2
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

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06004D64 RID: 19812 RVA: 0x001DA8D1 File Offset: 0x001D8AD1
		// (set) Token: 0x06004D65 RID: 19813 RVA: 0x001DA8D9 File Offset: 0x001D8AD9
		public bool HasPendingChanges
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06004D66 RID: 19814 RVA: 0x001DA8E2 File Offset: 0x001D8AE2
		public bool IsLocked
		{
			get
			{
				return this.sl.IsHeld;
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06004D67 RID: 19815 RVA: 0x001DA8EF File Offset: 0x001D8AEF
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

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06004D68 RID: 19816 RVA: 0x001DA905 File Offset: 0x001D8B05
		public DateTime LastWriteTime
		{
			get
			{
				return this.lastWriteTime;
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06004D69 RID: 19817 RVA: 0x001DA910 File Offset: 0x001D8B10
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

		// Token: 0x06004D6A RID: 19818 RVA: 0x001DA958 File Offset: 0x001D8B58
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

		// Token: 0x06004D6B RID: 19819 RVA: 0x001DAA20 File Offset: 0x001D8C20
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

		// Token: 0x06004D6C RID: 19820 RVA: 0x001DAC3C File Offset: 0x001D8E3C
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

		// Token: 0x06004D6D RID: 19821 RVA: 0x001DAD54 File Offset: 0x001D8F54
		public void EnterCommitLock(ref bool lockTaken)
		{
			this.commitLock.Enter(ref lockTaken);
		}

		// Token: 0x06004D6E RID: 19822 RVA: 0x001DAD62 File Offset: 0x001D8F62
		public void ExitCommitLock()
		{
			this.commitLock.Exit();
		}

		// Token: 0x06004D6F RID: 19823 RVA: 0x001DAD70 File Offset: 0x001D8F70
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

		// Token: 0x06004D70 RID: 19824 RVA: 0x001DAE98 File Offset: 0x001D9098
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

		// Token: 0x04003BC1 RID: 15297
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastAccessTime;

		// Token: 0x04003BC2 RID: 15298
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastWriteTime;

		// Token: 0x04003BC3 RID: 15299
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IPlatformSaveGameProvider platformSaveGameProvider;

		// Token: 0x04003BC4 RID: 15300
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SaveDataManagedPath backupPath;

		// Token: 0x04003BC5 RID: 15301
		[PublicizedFrom(EAccessModifier.Private)]
		public SpinLock sl = new SpinLock(true);

		// Token: 0x04003BC6 RID: 15302
		[PublicizedFrom(EAccessModifier.Private)]
		public SpinLock commitLock = new SpinLock(true);

		// Token: 0x04003BC7 RID: 15303
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataManager.CachedStream.VirtualFileStream workingCopy;

		// Token: 0x04003BC8 RID: 15304
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataManager.CachedStream.VirtualFileStream commitCopy;

		// Token: 0x04003BC9 RID: 15305
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isReading;

		// Token: 0x04003BCA RID: 15306
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isWriting;

		// Token: 0x04003BCB RID: 15307
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object lockObj = new object();

		// Token: 0x02000A0B RID: 2571
		[PublicizedFrom(EAccessModifier.Private)]
		public class VirtualFileStream : MemoryStream
		{
			// Token: 0x1700083E RID: 2110
			// (get) Token: 0x06004D71 RID: 19825 RVA: 0x001DAF0C File Offset: 0x001D910C
			// (set) Token: 0x06004D72 RID: 19826 RVA: 0x001DAF14 File Offset: 0x001D9114
			public bool ReadEnabled { get; set; }

			// Token: 0x1700083F RID: 2111
			// (get) Token: 0x06004D73 RID: 19827 RVA: 0x001DAF1D File Offset: 0x001D911D
			// (set) Token: 0x06004D74 RID: 19828 RVA: 0x001DAF25 File Offset: 0x001D9125
			public bool WriteEnabled { get; set; }

			// Token: 0x17000840 RID: 2112
			// (get) Token: 0x06004D75 RID: 19829 RVA: 0x001DAF2E File Offset: 0x001D912E
			public override bool CanRead
			{
				get
				{
					return this.ReadEnabled && base.CanRead;
				}
			}

			// Token: 0x17000841 RID: 2113
			// (get) Token: 0x06004D76 RID: 19830 RVA: 0x001DAF40 File Offset: 0x001D9140
			public override bool CanWrite
			{
				get
				{
					return this.WriteEnabled && base.CanWrite;
				}
			}

			// Token: 0x06004D77 RID: 19831 RVA: 0x001DAF52 File Offset: 0x001D9152
			public VirtualFileStream()
			{
				this.ReadEnabled = true;
				this.WriteEnabled = true;
			}

			// Token: 0x06004D78 RID: 19832 RVA: 0x001DAF68 File Offset: 0x001D9168
			public VirtualFileStream(int capacity)
				: base(capacity)
			{
				this.ReadEnabled = true;
				this.WriteEnabled = true;
			}

			// Token: 0x06004D79 RID: 19833 RVA: 0x001DAF7F File Offset: 0x001D917F
			public VirtualFileStream(long capacity)
				: base((capacity < 2147483647L) ? ((int)capacity) : int.MaxValue)
			{
				this.ReadEnabled = true;
				this.WriteEnabled = true;
			}

			// Token: 0x06004D7A RID: 19834 RVA: 0x001DAFA7 File Offset: 0x001D91A7
			public override int Read(byte[] buffer, int offset, int count)
			{
				if (!this.CanRead)
				{
					throw new NotSupportedException("Stream does not support reading.");
				}
				return base.Read(buffer, offset, count);
			}

			// Token: 0x06004D7B RID: 19835 RVA: 0x001DAFC5 File Offset: 0x001D91C5
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

	// Token: 0x02000A0C RID: 2572
	public class SlotSizeData
	{
		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06004D7C RID: 19836 RVA: 0x001DAFE0 File Offset: 0x001D91E0
		// (set) Token: 0x06004D7D RID: 19837 RVA: 0x001DAFE8 File Offset: 0x001D91E8
		public long TotalFileSize
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06004D7E RID: 19838 RVA: 0x001DAFF1 File Offset: 0x001D91F1
		public long LargestPriorityFileSize
		{
			get
			{
				return this.largestPriorityFile.Item2;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06004D7F RID: 19839 RVA: 0x001DAFFE File Offset: 0x001D91FE
		public int FileCount
		{
			get
			{
				return this.otherFileSizes.Count + this.priorityFileSizes.Count;
			}
		}

		// Token: 0x06004D80 RID: 19840 RVA: 0x001DB017 File Offset: 0x001D9217
		public SlotSizeData(SaveDataSlot saveDataSlot)
		{
			this.saveDataSlot = saveDataSlot;
		}

		// Token: 0x06004D81 RID: 19841 RVA: 0x001DB03C File Offset: 0x001D923C
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

		// Token: 0x06004D82 RID: 19842 RVA: 0x001DB09C File Offset: 0x001D929C
		public bool TryGetFileSize(SaveDataManagedPath managedPath, out long trackedSize)
		{
			bool flag;
			return this.TryGetFileSize(managedPath, out trackedSize, out flag);
		}

		// Token: 0x06004D83 RID: 19843 RVA: 0x001DB0B3 File Offset: 0x001D92B3
		public bool TryGetFileSize(SaveDataManagedPath managedPath, out long trackedSize, out bool isPriorityFile)
		{
			isPriorityFile = this.priorityFileSizes.TryGetValue(managedPath, out trackedSize);
			return isPriorityFile || this.otherFileSizes.TryGetValue(managedPath, out trackedSize);
		}

		// Token: 0x06004D84 RID: 19844 RVA: 0x001DB0D8 File Offset: 0x001D92D8
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

		// Token: 0x06004D85 RID: 19845 RVA: 0x001DB12C File Offset: 0x001D932C
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

		// Token: 0x06004D86 RID: 19846 RVA: 0x001DB180 File Offset: 0x001D9380
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

		// Token: 0x06004D87 RID: 19847 RVA: 0x001DB238 File Offset: 0x001D9438
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

		// Token: 0x04003BCE RID: 15310
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<SaveDataManagedPath, long> priorityFileSizes = new Dictionary<SaveDataManagedPath, long>();

		// Token: 0x04003BCF RID: 15311
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<SaveDataManagedPath, long> otherFileSizes = new Dictionary<SaveDataManagedPath, long>();

		// Token: 0x04003BD0 RID: 15312
		[TupleElementNames(new string[] { "path", "size" })]
		[PublicizedFrom(EAccessModifier.Private)]
		public ValueTuple<SaveDataManagedPath, long> largestPriorityFile;

		// Token: 0x04003BD2 RID: 15314
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SaveDataSlot saveDataSlot;
	}
}
