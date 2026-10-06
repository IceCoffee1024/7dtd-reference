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

// Token: 0x02000A24 RID: 2596
public class SaveDataManager : SaveDataManagerBase
{
	// Token: 0x17000868 RID: 2152
	// (get) Token: 0x06004D8A RID: 19850 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AppliesSaveSizeLimit
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004D8B RID: 19851 RVA: 0x001DD1E0 File Offset: 0x001DB3E0
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

	// Token: 0x06004D8C RID: 19852 RVA: 0x001DD388 File Offset: 0x001DB588
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool IsPriorityFilePath(SaveDataManagedPath path)
	{
		return path.Type == SaveDataType.Saves && path.PathRelativeToSlot.AsSpan().IndexOf<char>("Region") == 0;
	}

	// Token: 0x06004D8D RID: 19853 RVA: 0x001DD3C0 File Offset: 0x001DB5C0
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

	// Token: 0x06004D8E RID: 19854 RVA: 0x001DD4C8 File Offset: 0x001DB6C8
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

	// Token: 0x06004D8F RID: 19855 RVA: 0x001DD528 File Offset: 0x001DB728
	public override bool ShouldLimitSize()
	{
		return this.platformSaveGameProvider.ShouldLimitSize();
	}

	// Token: 0x06004D90 RID: 19856 RVA: 0x001DD535 File Offset: 0x001DB735
	public override void UpdateSizes()
	{
		this.platformSaveGameProvider.UpdateSizes();
	}

	// Token: 0x06004D91 RID: 19857 RVA: 0x001DD542 File Offset: 0x001DB742
	public override SaveDataSizes GetSizes()
	{
		return this.platformSaveGameProvider.GetSizes();
	}

	// Token: 0x06004D92 RID: 19858 RVA: 0x001DD550 File Offset: 0x001DB750
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

	// Token: 0x06004D93 RID: 19859 RVA: 0x001DD618 File Offset: 0x001DB818
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

	// Token: 0x06004D94 RID: 19860 RVA: 0x001DD67C File Offset: 0x001DB87C
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

	// Token: 0x06004D95 RID: 19861 RVA: 0x001DD6E0 File Offset: 0x001DB8E0
	public override SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
	{
		return this.platformSaveGameProvider.ManagedDirectoryCreateDirectory(path);
	}

	// Token: 0x06004D96 RID: 19862 RVA: 0x001DD6EE File Offset: 0x001DB8EE
	public override DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return this.platformSaveGameProvider.ManagedDirectoryGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004D97 RID: 19863 RVA: 0x001DD6FC File Offset: 0x001DB8FC
	public override bool ManagedDirectoryExists(SaveDataManagedPath path)
	{
		return this.platformSaveGameProvider.ManagedDirectoryExists(path);
	}

	// Token: 0x06004D98 RID: 19864 RVA: 0x001DD70A File Offset: 0x001DB90A
	public override IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.platformSaveGameProvider.ManagedDirectoryEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004D99 RID: 19865 RVA: 0x001DD71A File Offset: 0x001DB91A
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

	// Token: 0x06004D9A RID: 19866 RVA: 0x001DD73F File Offset: 0x001DB93F
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

	// Token: 0x06004D9B RID: 19867 RVA: 0x001DD764 File Offset: 0x001DB964
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

	// Token: 0x06004D9C RID: 19868 RVA: 0x001DD90C File Offset: 0x001DBB0C
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

	// Token: 0x06004D9D RID: 19869 RVA: 0x001DD978 File Offset: 0x001DBB78
	public override IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return this.platformSaveGameProvider.ManagedDirectoryInfoEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004D9E RID: 19870 RVA: 0x001DD988 File Offset: 0x001DBB88
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

	// Token: 0x06004D9F RID: 19871 RVA: 0x001DD9AD File Offset: 0x001DBBAD
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

	// Token: 0x06004DA0 RID: 19872 RVA: 0x001DD9D4 File Offset: 0x001DBBD4
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

	// Token: 0x06004DA1 RID: 19873 RVA: 0x001DDA30 File Offset: 0x001DBC30
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

	// Token: 0x06004DA2 RID: 19874 RVA: 0x001DDA84 File Offset: 0x001DBC84
	public SaveDataManager(IPlatformSaveGameProvider platformSaveGameProvider)
	{
		this.platformSaveGameProvider = new SaveDataMergedPlatformSaveGameIOProvider(platformSaveGameProvider);
	}

	// Token: 0x06004DA3 RID: 19875 RVA: 0x001DDB06 File Offset: 0x001DBD06
	public override void Init()
	{
		this.SetWriteMode(SaveDataWriteMode.Deferred);
		this.InitRestoreBackups();
		this.InitFileSizeTracking();
	}

	// Token: 0x06004DA4 RID: 19876 RVA: 0x001DDB1C File Offset: 0x001DBD1C
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

	// Token: 0x06004DA5 RID: 19877 RVA: 0x001DDC04 File Offset: 0x001DBE04
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

	// Token: 0x06004DA6 RID: 19878 RVA: 0x001DDCF4 File Offset: 0x001DBEF4
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

	// Token: 0x06004DA7 RID: 19879 RVA: 0x001DDD2C File Offset: 0x001DBF2C
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

	// Token: 0x06004DA8 RID: 19880 RVA: 0x001DDDAC File Offset: 0x001DBFAC
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

	// Token: 0x06004DA9 RID: 19881 RVA: 0x001DE0F0 File Offset: 0x001DC2F0
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

	// Token: 0x06004DAA RID: 19882 RVA: 0x001DE17C File Offset: 0x001DC37C
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

	// Token: 0x06004DAB RID: 19883 RVA: 0x001DE29C File Offset: 0x001DC49C
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

	// Token: 0x06004DAC RID: 19884 RVA: 0x001DE2F0 File Offset: 0x001DC4F0
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

	// Token: 0x06004DAD RID: 19885 RVA: 0x001DE348 File Offset: 0x001DC548
	public override SaveDataWriteMode GetWriteMode()
	{
		return this.writeMode;
	}

	// Token: 0x06004DAE RID: 19886 RVA: 0x001DE350 File Offset: 0x001DC550
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

	// Token: 0x06004DAF RID: 19887 RVA: 0x001DE41C File Offset: 0x001DC61C
	public override void Cleanup()
	{
		this.SetWriteMode(SaveDataWriteMode.None);
		this.cleanupComplete = true;
	}

	// Token: 0x06004DB0 RID: 19888 RVA: 0x001DE42C File Offset: 0x001DC62C
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

	// Token: 0x06004DB1 RID: 19889 RVA: 0x001DE45A File Offset: 0x001DC65A
	public override bool IsCommitPending(int token)
	{
		return token > this.explicitCommitCount;
	}

	// Token: 0x06004DB2 RID: 19890 RVA: 0x001DE468 File Offset: 0x001DC668
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

	// Token: 0x06004DB3 RID: 19891 RVA: 0x001DE4A0 File Offset: 0x001DC6A0
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

	// Token: 0x06004DB4 RID: 19892 RVA: 0x001DE4B6 File Offset: 0x001DC6B6
	[Conditional("DEBUG_SAVE_DATA_MANAGER")]
	[PublicizedFrom(EAccessModifier.Private)]
	public new static void DebugLog(string text)
	{
		Log.Out("[SaveDataManager] " + text);
	}

	// Token: 0x06004DB6 RID: 19894 RVA: 0x001DE544 File Offset: 0x001DC744
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

	// Token: 0x04003C43 RID: 15427
	[PublicizedFrom(EAccessModifier.Private)]
	public const int writerThreadTerminationTimeout = 420;

	// Token: 0x04003C44 RID: 15428
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IPlatformSaveGameProvider platformSaveGameProvider;

	// Token: 0x04003C45 RID: 15429
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object mapLockObj = new object();

	// Token: 0x04003C46 RID: 15430
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object commitLockObj = new object();

	// Token: 0x04003C47 RID: 15431
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<SaveDataManagedPath, SaveDataManager.CachedStream> pathToCacheMap = new Dictionary<SaveDataManagedPath, SaveDataManager.CachedStream>();

	// Token: 0x04003C48 RID: 15432
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<SaveDataManager.CachedStream> priorityCachedStreams = new HashSet<SaveDataManager.CachedStream>();

	// Token: 0x04003C49 RID: 15433
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Stream, SaveDataManager.CachedStream> streamToCacheMap = new Dictionary<Stream, SaveDataManager.CachedStream>();

	// Token: 0x04003C4A RID: 15434
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly TimeSpan commitInterval = TimeSpan.FromSeconds(30.0);

	// Token: 0x04003C4B RID: 15435
	[PublicizedFrom(EAccessModifier.Private)]
	public const int writeTaskDelay = 5;

	// Token: 0x04003C4C RID: 15436
	[PublicizedFrom(EAccessModifier.Private)]
	public const int memoryCopyTimeout = 30000;

	// Token: 0x04003C4D RID: 15437
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_CommitPendingChangesMarker = new ProfilerMarker("SaveDataManager.CommitPendingChanges");

	// Token: 0x04003C4E RID: 15438
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_InitRestoreBackupsMarker = new ProfilerMarker("SaveDataManager.InitRestoreBackups");

	// Token: 0x04003C4F RID: 15439
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_InitFileSizeTrackingMarker = new ProfilerMarker("SaveDataManager.InitFileSizeTracking");

	// Token: 0x04003C50 RID: 15440
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounterValue<int> s_CachedStreamCount = new ProfilerCounterValue<int>(ProfilerCategory.Scripts, "Cached Streams", ProfilerMarkerDataUnit.Count, ProfilerCounterOptions.FlushOnEndOfFrame);

	// Token: 0x04003C51 RID: 15441
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounterValue<int> s_StreamOperationCount = new ProfilerCounterValue<int>(ProfilerCategory.Scripts, "Stream Operations", ProfilerMarkerDataUnit.Count, ProfilerCounterOptions.FlushOnEndOfFrame | ProfilerCounterOptions.ResetToZeroOnFlush);

	// Token: 0x04003C52 RID: 15442
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo writerThreadInfo;

	// Token: 0x04003C53 RID: 15443
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime lastCommitTime;

	// Token: 0x04003C54 RID: 15444
	[PublicizedFrom(EAccessModifier.Private)]
	public int explicitCommitNeeded;

	// Token: 0x04003C55 RID: 15445
	[PublicizedFrom(EAccessModifier.Private)]
	public int explicitCommitCount;

	// Token: 0x04003C56 RID: 15446
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SaveDataManager.CachedStream> commitList = new List<SaveDataManager.CachedStream>();

	// Token: 0x04003C57 RID: 15447
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SaveDataManager.CachedStream> closeList = new List<SaveDataManager.CachedStream>();

	// Token: 0x04003C58 RID: 15448
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveDataWriteMode writeMode;

	// Token: 0x04003C59 RID: 15449
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cleanupComplete;

	// Token: 0x04003C5A RID: 15450
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<SaveDataSlot, SaveDataManager.SlotSizeData> fileSizesBySlot = new Dictionary<SaveDataSlot, SaveDataManager.SlotSizeData>();

	// Token: 0x04003C5B RID: 15451
	[PublicizedFrom(EAccessModifier.Private)]
	public object regionFileLock = new object();

	// Token: 0x04003C5C RID: 15452
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileManager activeRegionFileManager;

	// Token: 0x02000A25 RID: 2597
	[PublicizedFrom(EAccessModifier.Private)]
	public class CachedStream : IDisposable
	{
		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06004DB7 RID: 19895 RVA: 0x001DE5F2 File Offset: 0x001DC7F2
		public SaveDataManagedPath Path { get; }

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06004DB8 RID: 19896 RVA: 0x001DE5FA File Offset: 0x001DC7FA
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

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06004DB9 RID: 19897 RVA: 0x001DE619 File Offset: 0x001DC819
		// (set) Token: 0x06004DBA RID: 19898 RVA: 0x001DE621 File Offset: 0x001DC821
		public bool HasPendingChanges
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06004DBB RID: 19899 RVA: 0x001DE62A File Offset: 0x001DC82A
		public bool IsLocked
		{
			get
			{
				return this.sl.IsHeld;
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06004DBC RID: 19900 RVA: 0x001DE637 File Offset: 0x001DC837
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

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06004DBD RID: 19901 RVA: 0x001DE64D File Offset: 0x001DC84D
		public DateTime LastWriteTime
		{
			get
			{
				return this.lastWriteTime;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06004DBE RID: 19902 RVA: 0x001DE658 File Offset: 0x001DC858
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

		// Token: 0x06004DBF RID: 19903 RVA: 0x001DE6A0 File Offset: 0x001DC8A0
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

		// Token: 0x06004DC0 RID: 19904 RVA: 0x001DE768 File Offset: 0x001DC968
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

		// Token: 0x06004DC1 RID: 19905 RVA: 0x001DE984 File Offset: 0x001DCB84
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

		// Token: 0x06004DC2 RID: 19906 RVA: 0x001DEA9C File Offset: 0x001DCC9C
		public void EnterCommitLock(ref bool lockTaken)
		{
			this.commitLock.Enter(ref lockTaken);
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x001DEAAA File Offset: 0x001DCCAA
		public void ExitCommitLock()
		{
			this.commitLock.Exit();
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x001DEAB8 File Offset: 0x001DCCB8
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

		// Token: 0x06004DC5 RID: 19909 RVA: 0x001DEBE0 File Offset: 0x001DCDE0
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

		// Token: 0x04003C5F RID: 15455
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastAccessTime;

		// Token: 0x04003C60 RID: 15456
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastWriteTime;

		// Token: 0x04003C61 RID: 15457
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IPlatformSaveGameProvider platformSaveGameProvider;

		// Token: 0x04003C62 RID: 15458
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SaveDataManagedPath backupPath;

		// Token: 0x04003C63 RID: 15459
		[PublicizedFrom(EAccessModifier.Private)]
		public SpinLock sl = new SpinLock(true);

		// Token: 0x04003C64 RID: 15460
		[PublicizedFrom(EAccessModifier.Private)]
		public SpinLock commitLock = new SpinLock(true);

		// Token: 0x04003C65 RID: 15461
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataManager.CachedStream.VirtualFileStream workingCopy;

		// Token: 0x04003C66 RID: 15462
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataManager.CachedStream.VirtualFileStream commitCopy;

		// Token: 0x04003C67 RID: 15463
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isReading;

		// Token: 0x04003C68 RID: 15464
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isWriting;

		// Token: 0x04003C69 RID: 15465
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object lockObj = new object();

		// Token: 0x02000A26 RID: 2598
		[PublicizedFrom(EAccessModifier.Private)]
		public class VirtualFileStream : MemoryStream
		{
			// Token: 0x17000870 RID: 2160
			// (get) Token: 0x06004DC6 RID: 19910 RVA: 0x001DEC54 File Offset: 0x001DCE54
			// (set) Token: 0x06004DC7 RID: 19911 RVA: 0x001DEC5C File Offset: 0x001DCE5C
			public bool ReadEnabled { get; set; }

			// Token: 0x17000871 RID: 2161
			// (get) Token: 0x06004DC8 RID: 19912 RVA: 0x001DEC65 File Offset: 0x001DCE65
			// (set) Token: 0x06004DC9 RID: 19913 RVA: 0x001DEC6D File Offset: 0x001DCE6D
			public bool WriteEnabled { get; set; }

			// Token: 0x17000872 RID: 2162
			// (get) Token: 0x06004DCA RID: 19914 RVA: 0x001DEC76 File Offset: 0x001DCE76
			public override bool CanRead
			{
				get
				{
					return this.ReadEnabled && base.CanRead;
				}
			}

			// Token: 0x17000873 RID: 2163
			// (get) Token: 0x06004DCB RID: 19915 RVA: 0x001DEC88 File Offset: 0x001DCE88
			public override bool CanWrite
			{
				get
				{
					return this.WriteEnabled && base.CanWrite;
				}
			}

			// Token: 0x06004DCC RID: 19916 RVA: 0x001DEC9A File Offset: 0x001DCE9A
			public VirtualFileStream()
			{
				this.ReadEnabled = true;
				this.WriteEnabled = true;
			}

			// Token: 0x06004DCD RID: 19917 RVA: 0x001DECB0 File Offset: 0x001DCEB0
			public VirtualFileStream(int capacity)
				: base(capacity)
			{
				this.ReadEnabled = true;
				this.WriteEnabled = true;
			}

			// Token: 0x06004DCE RID: 19918 RVA: 0x001DECC7 File Offset: 0x001DCEC7
			public VirtualFileStream(long capacity)
				: base((capacity < 2147483647L) ? ((int)capacity) : int.MaxValue)
			{
				this.ReadEnabled = true;
				this.WriteEnabled = true;
			}

			// Token: 0x06004DCF RID: 19919 RVA: 0x001DECEF File Offset: 0x001DCEEF
			public override int Read(byte[] buffer, int offset, int count)
			{
				if (!this.CanRead)
				{
					throw new NotSupportedException("Stream does not support reading.");
				}
				return base.Read(buffer, offset, count);
			}

			// Token: 0x06004DD0 RID: 19920 RVA: 0x001DED0D File Offset: 0x001DCF0D
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

	// Token: 0x02000A27 RID: 2599
	public class SlotSizeData
	{
		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06004DD1 RID: 19921 RVA: 0x001DED28 File Offset: 0x001DCF28
		// (set) Token: 0x06004DD2 RID: 19922 RVA: 0x001DED30 File Offset: 0x001DCF30
		public long TotalFileSize
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06004DD3 RID: 19923 RVA: 0x001DED39 File Offset: 0x001DCF39
		public long LargestPriorityFileSize
		{
			get
			{
				return this.largestPriorityFile.Item2;
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06004DD4 RID: 19924 RVA: 0x001DED46 File Offset: 0x001DCF46
		public int FileCount
		{
			get
			{
				return this.otherFileSizes.Count + this.priorityFileSizes.Count;
			}
		}

		// Token: 0x06004DD5 RID: 19925 RVA: 0x001DED5F File Offset: 0x001DCF5F
		public SlotSizeData(SaveDataSlot saveDataSlot)
		{
			this.saveDataSlot = saveDataSlot;
		}

		// Token: 0x06004DD6 RID: 19926 RVA: 0x001DED84 File Offset: 0x001DCF84
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

		// Token: 0x06004DD7 RID: 19927 RVA: 0x001DEDE4 File Offset: 0x001DCFE4
		public bool TryGetFileSize(SaveDataManagedPath managedPath, out long trackedSize)
		{
			bool flag;
			return this.TryGetFileSize(managedPath, out trackedSize, out flag);
		}

		// Token: 0x06004DD8 RID: 19928 RVA: 0x001DEDFB File Offset: 0x001DCFFB
		public bool TryGetFileSize(SaveDataManagedPath managedPath, out long trackedSize, out bool isPriorityFile)
		{
			isPriorityFile = this.priorityFileSizes.TryGetValue(managedPath, out trackedSize);
			return isPriorityFile || this.otherFileSizes.TryGetValue(managedPath, out trackedSize);
		}

		// Token: 0x06004DD9 RID: 19929 RVA: 0x001DEE20 File Offset: 0x001DD020
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

		// Token: 0x06004DDA RID: 19930 RVA: 0x001DEE74 File Offset: 0x001DD074
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

		// Token: 0x06004DDB RID: 19931 RVA: 0x001DEEC8 File Offset: 0x001DD0C8
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

		// Token: 0x06004DDC RID: 19932 RVA: 0x001DEF80 File Offset: 0x001DD180
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

		// Token: 0x04003C6C RID: 15468
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<SaveDataManagedPath, long> priorityFileSizes = new Dictionary<SaveDataManagedPath, long>();

		// Token: 0x04003C6D RID: 15469
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<SaveDataManagedPath, long> otherFileSizes = new Dictionary<SaveDataManagedPath, long>();

		// Token: 0x04003C6E RID: 15470
		[TupleElementNames(new string[] { "path", "size" })]
		[PublicizedFrom(EAccessModifier.Private)]
		public ValueTuple<SaveDataManagedPath, long> largestPriorityFile;

		// Token: 0x04003C70 RID: 15472
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SaveDataSlot saveDataSlot;
	}
}
