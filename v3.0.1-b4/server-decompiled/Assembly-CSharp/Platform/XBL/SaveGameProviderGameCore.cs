using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Platform.Shared;
using Platform.XBL.Save;
using Platform.XBL.Save.Storage;
using Unity.XGamingRuntime;

namespace Platform.XBL
{
	// Token: 0x02001BD5 RID: 7125
	public class SaveGameProviderGameCore : IPlatformSaveGameProvider, IPlatformSaveGameIOProvider
	{
		// Token: 0x0600D340 RID: 54080 RVA: 0x004C5BD4 File Offset: 0x004C3DD4
		public SaveGameProviderGameCore(long maxStorageSizeBytes = 1073741824L)
		{
			this.m_maxStorageSizeBytes = maxStorageSizeBytes;
		}

		// Token: 0x0600D341 RID: 54081 RVA: 0x004C5BEE File Offset: 0x004C3DEE
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnUserHandleReady(XUserHandle userHandle)
		{
			if (userHandle == null)
			{
				Log.Error("[XBL] SaveGameProviderGameCore.OnUserHandleReady: Attempting to retrieve XSTS token before acquiring XUserHandle");
				return;
			}
			if (userHandle.IsInvalid)
			{
				Log.Error("[XBL] SaveGameProviderGameCore.OnUserHandleReady: m_userHandle.IsInvalid is true");
			}
			if (userHandle.IsClosed)
			{
				Log.Error("[XBL] SaveGameProviderGameCore.OnUserHandleReady: m_userHandle.IsClosed is true");
			}
			this.InitializeSaveStorageProvider();
		}

		// Token: 0x0600D342 RID: 54082 RVA: 0x004C5C30 File Offset: 0x004C3E30
		[PublicizedFrom(EAccessModifier.Private)]
		public void InitializeSaveStorageProvider()
		{
			SaveStorageProvider value = LaunchPrefs.GameCoreSaveStorageProvider.Value;
			SaveGameProviderGameCore.LogInfo(string.Format("[GDK] Initializing Save Storage Provider '{0}'.", value));
			this.m_saveStorageProvider = value.Create();
			this.m_saveStorageProvider.InitializeAsync(this.m_owner, this.m_maxStorageSizeBytes, this.m_taskScheduler, new OnGameSaveProviderStatusChanged(this.OnGameSaveProviderStatusChanged));
		}

		// Token: 0x0600D343 RID: 54083 RVA: 0x004C5C94 File Offset: 0x004C3E94
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnGameSaveProviderStatusChanged(ESaveGameProviderStatus status)
		{
			if (status == ESaveGameProviderStatus.Ok && this.m_rootSaveContainer == null)
			{
				this.OnGameSaveProviderReady();
			}
			object initializedDelegateLock = this.m_initializedDelegateLock;
			lock (initializedDelegateLock)
			{
				bool flag2 = status == ESaveGameProviderStatus.Ok && this.Status != ESaveGameProviderStatus.Ok;
				this.Status = status;
				if (flag2)
				{
					Action initializedDelegate = this.m_initializedDelegate;
					if (initializedDelegate != null)
					{
						initializedDelegate();
					}
				}
			}
		}

		// Token: 0x0600D344 RID: 54084 RVA: 0x004C5D10 File Offset: 0x004C3F10
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnGameSaveProviderReady()
		{
			this.m_rootSaveContainer = new SaveContainer(this.m_saveStorageProvider.RootSaveStorageContainer, this.m_saveStorageProvider.SizeTracker);
			this.ResumeIO();
		}

		// Token: 0x0600D345 RID: 54085 RVA: 0x004C5D3C File Offset: 0x004C3F3C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnApplicationStateChanged(ApplicationState newstate)
		{
			bool flag = newstate == ApplicationState.Suspended;
			if (flag != this.m_suspended)
			{
				this.m_suspended = flag;
				if (flag)
				{
					this.OnSuspend();
					return;
				}
				this.OnResume();
			}
		}

		// Token: 0x0600D346 RID: 54086 RVA: 0x004C5D6E File Offset: 0x004C3F6E
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnSuspend()
		{
			this.PauseIO();
		}

		// Token: 0x0600D347 RID: 54087 RVA: 0x004C5D78 File Offset: 0x004C3F78
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnResume()
		{
			if (!this.Status.IsTerminal())
			{
				SaveGameProviderGameCore.LogWarning("Status was not terminal on resume? Did sleep happen during loading?");
			}
			SaveContainer rootSaveContainer = this.m_rootSaveContainer;
			if (rootSaveContainer != null)
			{
				rootSaveContainer.Dispose();
			}
			this.m_rootSaveContainer = null;
			ISaveStorageProvider saveStorageProvider = this.m_saveStorageProvider;
			if (saveStorageProvider != null)
			{
				saveStorageProvider.Dispose();
			}
			this.m_saveStorageProvider = null;
			this.Status = ESaveGameProviderStatus.Uninitialized;
			this.InitializeSaveStorageProvider();
		}

		// Token: 0x0600D348 RID: 54088 RVA: 0x004C5DDC File Offset: 0x004C3FDC
		[PublicizedFrom(EAccessModifier.Private)]
		public void PauseIO()
		{
			if (this.m_paused)
			{
				SaveGameProviderGameCore.LogError("PauseIO should only be called when IO is not paused.");
				return;
			}
			this.m_paused = true;
			while (this.m_paused && this.m_operations > 0)
			{
				Thread.Sleep(5);
			}
			this.Flush(true);
			if (!this.m_paused)
			{
				SaveGameProviderGameCore.LogWarning("Was un-paused before we finished waiting on IO to pause.");
			}
		}

		// Token: 0x0600D349 RID: 54089 RVA: 0x004C5E35 File Offset: 0x004C4035
		[PublicizedFrom(EAccessModifier.Private)]
		public void ResumeIO()
		{
			if (!this.m_paused)
			{
				SaveGameProviderGameCore.LogError("ResumeIO should only be called when IO is paused.");
				return;
			}
			this.m_paused = false;
		}

		// Token: 0x0600D34A RID: 54090 RVA: 0x004C5E51 File Offset: 0x004C4051
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveGameProviderGameCore.OperationScope CreateOperationScope(string debugIdentifier)
		{
			if (this.m_paused)
			{
				while (this.m_paused)
				{
					Thread.Sleep(50);
				}
			}
			return new SaveGameProviderGameCore.OperationScope(this);
		}

		// Token: 0x0600D34B RID: 54091 RVA: 0x004C5E72 File Offset: 0x004C4072
		[PublicizedFrom(EAccessModifier.Private)]
		public void GetSaveContainerAndRelativePath(SaveDataManagedPath path, out SaveContainer saveContainer, out string relativePath)
		{
			saveContainer = this.m_rootSaveContainer;
			relativePath = path.PathRelativeToRoot;
		}

		// Token: 0x0600D34C RID: 54092 RVA: 0x004C5E84 File Offset: 0x004C4084
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string text)
		{
			Log.Out("[XBL: SaveGameProvider] " + text);
		}

		// Token: 0x0600D34D RID: 54093 RVA: 0x004C5E96 File Offset: 0x004C4096
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogWarning(string text)
		{
			Log.Warning("[XBL: SaveGameProvider] " + text);
		}

		// Token: 0x0600D34E RID: 54094 RVA: 0x004C5EA8 File Offset: 0x004C40A8
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string text)
		{
			Log.Error("[XBL: SaveGameProvider] " + text);
		}

		// Token: 0x0600D34F RID: 54095 RVA: 0x004C5E84 File Offset: 0x004C4084
		[Conditional("DEBUG_SAVE_DATA_MANAGER")]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogTrace(string text)
		{
			Log.Out("[XBL: SaveGameProvider] " + text);
		}

		// Token: 0x170019F6 RID: 6646
		// (get) Token: 0x0600D350 RID: 54096 RVA: 0x004C5EBA File Offset: 0x004C40BA
		// (set) Token: 0x0600D351 RID: 54097 RVA: 0x004C5EC2 File Offset: 0x004C40C2
		public ESaveGameProviderStatus Status
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170019F7 RID: 6647
		// (get) Token: 0x0600D352 RID: 54098 RVA: 0x004C5ECB File Offset: 0x004C40CB
		// (set) Token: 0x0600D353 RID: 54099 RVA: 0x004C5ED3 File Offset: 0x004C40D3
		public IPlatformSaveGameIOProvider Cache
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1400012A RID: 298
		// (add) Token: 0x0600D354 RID: 54100 RVA: 0x004C5EDC File Offset: 0x004C40DC
		// (remove) Token: 0x0600D355 RID: 54101 RVA: 0x004C5F3C File Offset: 0x004C413C
		public event Action Initialized
		{
			add
			{
				object initializedDelegateLock = this.m_initializedDelegateLock;
				lock (initializedDelegateLock)
				{
					this.m_initializedDelegate = (Action)Delegate.Combine(this.m_initializedDelegate, value);
					if (this.Status == ESaveGameProviderStatus.Ok)
					{
						value();
					}
				}
			}
			remove
			{
				object initializedDelegateLock = this.m_initializedDelegateLock;
				lock (initializedDelegateLock)
				{
					this.m_initializedDelegate = (Action)Delegate.Remove(this.m_initializedDelegate, value);
				}
			}
		}

		// Token: 0x0600D356 RID: 54102 RVA: 0x004C5F90 File Offset: 0x004C4190
		public void Init(IPlatform _owner)
		{
			this.PauseIO();
			this.m_taskScheduler = new SingleThreadTaskScheduler("GameCore", "Game Core Save Tasks");
			this.m_owner = _owner;
			this.m_appState = _owner.ApplicationState;
			this.m_appState.OnApplicationStateChanged += this.OnApplicationStateChanged;
			this.m_userClient = (User)this.m_owner.User;
			this.m_userClient.UserHandleReady += this.OnUserHandleReady;
			this.Cache = new SaveGameIOProviderFixedRoot(GameIO.GetNormalizedPath(GameIO.GetDeviceLocalUserGameDataDir()) + "Cache");
		}

		// Token: 0x0600D357 RID: 54103 RVA: 0x004C6030 File Offset: 0x004C4230
		public void Destroy()
		{
			if (this.m_rootSaveContainer != null)
			{
				this.m_rootSaveContainer.Dispose();
				this.m_rootSaveContainer = null;
			}
			ISaveStorageProvider saveStorageProvider = this.m_saveStorageProvider;
			if (saveStorageProvider != null)
			{
				saveStorageProvider.Dispose();
			}
			this.m_saveStorageProvider = null;
			if (this.m_userClient != null)
			{
				this.m_userClient.UserHandleReady -= this.OnUserHandleReady;
				this.m_userClient = null;
			}
			if (this.m_appState != null)
			{
				this.m_appState.OnApplicationStateChanged -= this.OnApplicationStateChanged;
				this.m_appState = null;
			}
			this.m_owner = null;
			SingleThreadTaskScheduler taskScheduler = this.m_taskScheduler;
			if (taskScheduler != null)
			{
				taskScheduler.Dispose();
			}
			this.m_taskScheduler = null;
		}

		// Token: 0x0600D358 RID: 54104 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool ShouldBackup()
		{
			return false;
		}

		// Token: 0x0600D359 RID: 54105 RVA: 0x004C60DA File Offset: 0x004C42DA
		public bool ShouldCommit()
		{
			return !this.m_paused;
		}

		// Token: 0x0600D35A RID: 54106 RVA: 0x004C60E5 File Offset: 0x004C42E5
		public double GetCommitProgress()
		{
			return 1.0;
		}

		// Token: 0x0600D35B RID: 54107 RVA: 0x004C60F0 File Offset: 0x004C42F0
		public void Flush(bool waitForFlush)
		{
			ISaveStorageProvider saveStorageProvider = this.m_saveStorageProvider;
			if (saveStorageProvider == null)
			{
				return;
			}
			saveStorageProvider.Flush(waitForFlush);
		}

		// Token: 0x0600D35C RID: 54108 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public bool ShouldLimitSize()
		{
			return true;
		}

		// Token: 0x0600D35D RID: 54109 RVA: 0x004C6104 File Offset: 0x004C4304
		public void UpdateSizes()
		{
			using (this.CreateOperationScope("UpdateSizes"))
			{
				this.m_saveStorageProvider.SizeTracker.RefreshSync();
			}
		}

		// Token: 0x0600D35E RID: 54110 RVA: 0x004C6150 File Offset: 0x004C4350
		public SaveDataSizes GetSizes()
		{
			SaveDataSizes sizes;
			using (this.CreateOperationScope("GetSizes"))
			{
				sizes = this.m_saveStorageProvider.SizeTracker.Sizes;
			}
			return sizes;
		}

		// Token: 0x0600D35F RID: 54111 RVA: 0x004C619C File Offset: 0x004C439C
		public void ManagedFileRead(SaveDataManagedPath path, Stream dest)
		{
			using (this.CreateOperationScope("ManagedFileRead"))
			{
				SaveContainer saveContainer;
				string text;
				this.GetSaveContainerAndRelativePath(path, out saveContainer, out text);
				saveContainer.FileRead(text, dest);
			}
		}

		// Token: 0x0600D360 RID: 54112 RVA: 0x004C61F0 File Offset: 0x004C43F0
		public void ManagedFileWrite(SaveDataManagedPath path, Stream src)
		{
			using (this.CreateOperationScope("ManagedFileWrite"))
			{
				SaveContainer saveContainer;
				string text;
				this.GetSaveContainerAndRelativePath(path, out saveContainer, out text);
				saveContainer.FileWrite(text, src);
			}
		}

		// Token: 0x0600D361 RID: 54113 RVA: 0x004C6244 File Offset: 0x004C4444
		public void ManagedFileCopy(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName, bool overwrite = false)
		{
			using (this.CreateOperationScope("ManagedFileCopy"))
			{
				SaveContainer saveContainer;
				string text;
				this.GetSaveContainerAndRelativePath(destFileName, out saveContainer, out text);
				if (!overwrite && saveContainer.FileExists(text))
				{
					throw new IOException(string.Format("Destination '{0}' already exists.", destFileName));
				}
				SaveContainer saveContainer2;
				string text2;
				this.GetSaveContainerAndRelativePath(sourceFileName, out saveContainer2, out text2);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					saveContainer2.FileRead(text2, memoryStream);
					memoryStream.Position = 0L;
					saveContainer.FileWrite(text, memoryStream);
				}
			}
		}

		// Token: 0x0600D362 RID: 54114 RVA: 0x004C62FC File Offset: 0x004C44FC
		public void ManagedFileDelete(SaveDataManagedPath path)
		{
			using (this.CreateOperationScope("ManagedFileDelete"))
			{
				SaveContainer saveContainer;
				string text;
				this.GetSaveContainerAndRelativePath(path, out saveContainer, out text);
				saveContainer.FileDelete(text);
			}
		}

		// Token: 0x0600D363 RID: 54115 RVA: 0x004C634C File Offset: 0x004C454C
		public bool ManagedFileExists(SaveDataManagedPath path)
		{
			bool flag;
			using (this.CreateOperationScope("ManagedFileExists"))
			{
				SaveContainer saveContainer;
				string text;
				this.GetSaveContainerAndRelativePath(path, out saveContainer, out text);
				flag = saveContainer.FileExists(text);
			}
			return flag;
		}

		// Token: 0x0600D364 RID: 54116 RVA: 0x004C63A0 File Offset: 0x004C45A0
		public DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path)
		{
			DateTime lastWriteTimeUtc;
			using (this.CreateOperationScope("ManagedFileGetLastWriteTimeUtc"))
			{
				SaveContainer saveContainer;
				string text;
				this.GetSaveContainerAndRelativePath(path, out saveContainer, out text);
				lastWriteTimeUtc = saveContainer.GetLastWriteTimeUtc(text);
			}
			return lastWriteTimeUtc;
		}

		// Token: 0x0600D365 RID: 54117 RVA: 0x004C63F4 File Offset: 0x004C45F4
		public void ManagedFileMove(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName)
		{
			using (this.CreateOperationScope("ManagedFileMove"))
			{
				SaveContainer saveContainer;
				string text;
				this.GetSaveContainerAndRelativePath(destFileName, out saveContainer, out text);
				if (saveContainer.FileExists(text))
				{
					throw new IOException(string.Format("Destination '{0}' already exists.", destFileName));
				}
				SaveContainer saveContainer2;
				string text2;
				this.GetSaveContainerAndRelativePath(sourceFileName, out saveContainer2, out text2);
				if (saveContainer2 != saveContainer)
				{
					this.ManagedFileCopy(sourceFileName, destFileName, false);
					this.ManagedFileDelete(sourceFileName);
				}
				else
				{
					saveContainer2.FileMove(text2, text);
				}
			}
		}

		// Token: 0x0600D366 RID: 54118 RVA: 0x004C648C File Offset: 0x004C468C
		public SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
		{
			SdDirectoryInfo sdDirectoryInfo;
			using (this.CreateOperationScope("ManagedDirectoryCreateDirectory"))
			{
				SaveContainer saveContainer;
				string text;
				this.GetSaveContainerAndRelativePath(path, out saveContainer, out text);
				saveContainer.DirectoryCreate(text);
				sdDirectoryInfo = new SdDirectoryInfo(path.GetOriginalPath());
			}
			return sdDirectoryInfo;
		}

		// Token: 0x0600D367 RID: 54119 RVA: 0x004C64EC File Offset: 0x004C46EC
		public DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
		{
			DateTime lastWriteTimeUtc;
			using (this.CreateOperationScope("ManagedDirectoryGetLastWriteTimeUtc"))
			{
				SaveContainer saveContainer;
				string text;
				this.GetSaveContainerAndRelativePath(path, out saveContainer, out text);
				lastWriteTimeUtc = saveContainer.GetLastWriteTimeUtc(text);
			}
			return lastWriteTimeUtc;
		}

		// Token: 0x0600D368 RID: 54120 RVA: 0x004C6540 File Offset: 0x004C4740
		public bool ManagedDirectoryExists(SaveDataManagedPath path)
		{
			bool flag;
			using (this.CreateOperationScope("ManagedDirectoryExists"))
			{
				SaveContainer saveContainer;
				string text;
				this.GetSaveContainerAndRelativePath(path, out saveContainer, out text);
				flag = saveContainer.DirectoryExists(text);
			}
			return flag;
		}

		// Token: 0x0600D369 RID: 54121 RVA: 0x004C6594 File Offset: 0x004C4794
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			IEnumerable<SaveDataManagedPath> enumerable;
			using (this.CreateOperationScope("ManagedDirectoryEnumerateDirectories"))
			{
				enumerable = this.ManagedDirectoryEnumerateInternal(path, searchPattern, searchOption, true, false).ToArray<SaveDataManagedPath>();
			}
			return enumerable;
		}

		// Token: 0x0600D36A RID: 54122 RVA: 0x004C65E0 File Offset: 0x004C47E0
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			IEnumerable<SaveDataManagedPath> enumerable;
			using (this.CreateOperationScope("ManagedDirectoryEnumerateFiles"))
			{
				enumerable = this.ManagedDirectoryEnumerateInternal(path, searchPattern, searchOption, false, true).ToArray<SaveDataManagedPath>();
			}
			return enumerable;
		}

		// Token: 0x0600D36B RID: 54123 RVA: 0x004C662C File Offset: 0x004C482C
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			IEnumerable<SaveDataManagedPath> enumerable;
			using (this.CreateOperationScope("ManagedDirectoryEnumerateFileSystemEntries"))
			{
				enumerable = this.ManagedDirectoryEnumerateInternal(path, searchPattern, searchOption, true, true).ToArray<SaveDataManagedPath>();
			}
			return enumerable;
		}

		// Token: 0x0600D36C RID: 54124 RVA: 0x004C6678 File Offset: 0x004C4878
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateInternal(SaveDataManagedPath path, string searchPattern, SearchOption searchOption, bool includeDirectories, bool includeFiles)
		{
			SaveContainer saveContainer;
			string text;
			this.GetSaveContainerAndRelativePath(path, out saveContainer, out text);
			IEnumerable<string> enumerable = saveContainer.DirectoryEnumerate(text, searchPattern, searchOption == SearchOption.AllDirectories, includeDirectories, includeFiles);
			return SaveGameProviderHelper.GetManagedPathsFromBaseAndSubPaths(path, text, enumerable);
		}

		// Token: 0x0600D36D RID: 54125 RVA: 0x004C66AC File Offset: 0x004C48AC
		public void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive)
		{
			using (this.CreateOperationScope("ManagedDirectoryDelete"))
			{
				SaveContainer saveContainer;
				string text;
				this.GetSaveContainerAndRelativePath(path, out saveContainer, out text);
				saveContainer.DirectoryDelete(text, recursive);
			}
		}

		// Token: 0x0600D36E RID: 54126 RVA: 0x004C6700 File Offset: 0x004C4900
		public long ManagedFileInfoLength(SaveDataManagedPath path)
		{
			long num;
			using (this.CreateOperationScope("ManagedFileInfoLength"))
			{
				SaveContainer saveContainer;
				string text;
				this.GetSaveContainerAndRelativePath(path, out saveContainer, out text);
				num = saveContainer.FileLength(text);
			}
			return num;
		}

		// Token: 0x0600D36F RID: 54127 RVA: 0x004C6754 File Offset: 0x004C4954
		public IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			IEnumerable<SdDirectoryInfo> enumerable;
			using (this.CreateOperationScope("ManagedDirectoryInfoEnumerateDirectories"))
			{
				enumerable = (from managedPath in this.ManagedDirectoryEnumerateInternal(path, searchPattern, searchOption, true, false)
					select new SdDirectoryInfo(managedPath)).ToArray<SdDirectoryInfo>();
			}
			return enumerable;
		}

		// Token: 0x0600D370 RID: 54128 RVA: 0x004C67C4 File Offset: 0x004C49C4
		public IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			IEnumerable<SdFileInfo> enumerable;
			using (this.CreateOperationScope("ManagedDirectoryInfoEnumerateFiles"))
			{
				enumerable = (from managedPath in this.ManagedDirectoryEnumerateInternal(path, searchPattern, searchOption, false, true)
					select new SdFileInfo(managedPath)).ToArray<SdFileInfo>();
			}
			return enumerable;
		}

		// Token: 0x0600D371 RID: 54129 RVA: 0x004C6834 File Offset: 0x004C4A34
		public IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			IEnumerable<SdFileSystemInfo> enumerable;
			using (this.CreateOperationScope("ManagedDirectoryInfoEnumerateFileSystemInfos"))
			{
				string relativePath;
				SaveContainer saveContainer;
				this.GetSaveContainerAndRelativePath(path, out saveContainer, out relativePath);
				enumerable = saveContainer.DirectoryEnumerate(relativePath, searchPattern, searchOption == SearchOption.AllDirectories).Select<PathEnumerationInfo, SdFileSystemInfo>(delegate(PathEnumerationInfo x)
				{
					SaveDataManagedPath managedPathFromBaseAndSubPath = SaveGameProviderHelper.GetManagedPathFromBaseAndSubPath(path, relativePath, x.RelativePath);
					if (!x.IsDirectory)
					{
						return new SdFileInfo(managedPathFromBaseAndSubPath);
					}
					return new SdDirectoryInfo(managedPathFromBaseAndSubPath);
				}).ToArray<SdFileSystemInfo>();
			}
			return enumerable;
		}

		// Token: 0x0400A0BA RID: 41146
		[PublicizedFrom(EAccessModifier.Private)]
		public SingleThreadTaskScheduler m_taskScheduler;

		// Token: 0x0400A0BB RID: 41147
		[PublicizedFrom(EAccessModifier.Private)]
		public Action m_initializedDelegate;

		// Token: 0x0400A0BC RID: 41148
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_initializedDelegateLock = new object();

		// Token: 0x0400A0BD RID: 41149
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform m_owner;

		// Token: 0x0400A0BE RID: 41150
		[PublicizedFrom(EAccessModifier.Private)]
		public IApplicationStateController m_appState;

		// Token: 0x0400A0BF RID: 41151
		[PublicizedFrom(EAccessModifier.Private)]
		public User m_userClient;

		// Token: 0x0400A0C0 RID: 41152
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_maxStorageSizeBytes;

		// Token: 0x0400A0C1 RID: 41153
		[PublicizedFrom(EAccessModifier.Private)]
		public ISaveStorageProvider m_saveStorageProvider;

		// Token: 0x0400A0C2 RID: 41154
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_suspended;

		// Token: 0x0400A0C3 RID: 41155
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_paused;

		// Token: 0x0400A0C4 RID: 41156
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_operations;

		// Token: 0x0400A0C5 RID: 41157
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveContainer m_rootSaveContainer;

		// Token: 0x02001BD6 RID: 7126
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly struct OperationScope : IDisposable
		{
			// Token: 0x0600D372 RID: 54130 RVA: 0x004C68B8 File Offset: 0x004C4AB8
			public OperationScope(SaveGameProviderGameCore provider)
			{
				this.m_provider = provider;
				Interlocked.Increment(ref this.m_provider.m_operations);
			}

			// Token: 0x0600D373 RID: 54131 RVA: 0x004C68D2 File Offset: 0x004C4AD2
			public void Dispose()
			{
				Interlocked.Decrement(ref this.m_provider.m_operations);
			}

			// Token: 0x0400A0C8 RID: 41160
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly SaveGameProviderGameCore m_provider;
		}
	}
}
