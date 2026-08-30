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
	// Token: 0x02001BFC RID: 7164
	public class SaveGameProviderGameCore : IPlatformSaveGameProvider, IPlatformSaveGameIOProvider
	{
		// Token: 0x0600D4CE RID: 54478 RVA: 0x004CBFC8 File Offset: 0x004CA1C8
		public SaveGameProviderGameCore(long maxStorageSizeBytes = 1073741824L)
		{
			this.m_maxStorageSizeBytes = maxStorageSizeBytes;
		}

		// Token: 0x0600D4CF RID: 54479 RVA: 0x004CBFE2 File Offset: 0x004CA1E2
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

		// Token: 0x0600D4D0 RID: 54480 RVA: 0x004CC024 File Offset: 0x004CA224
		[PublicizedFrom(EAccessModifier.Private)]
		public void InitializeSaveStorageProvider()
		{
			SaveStorageProvider value = LaunchPrefs.GameCoreSaveStorageProvider.Value;
			SaveGameProviderGameCore.LogInfo(string.Format("[GDK] Initializing Save Storage Provider '{0}'.", value));
			this.m_saveStorageProvider = value.Create();
			this.m_saveStorageProvider.InitializeAsync(this.m_owner, this.m_maxStorageSizeBytes, this.m_taskScheduler, new OnGameSaveProviderStatusChanged(this.OnGameSaveProviderStatusChanged));
		}

		// Token: 0x0600D4D1 RID: 54481 RVA: 0x004CC088 File Offset: 0x004CA288
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

		// Token: 0x0600D4D2 RID: 54482 RVA: 0x004CC104 File Offset: 0x004CA304
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnGameSaveProviderReady()
		{
			this.m_rootSaveContainer = new SaveContainer(this.m_saveStorageProvider.RootSaveStorageContainer, this.m_saveStorageProvider.SizeTracker);
			this.ResumeIO();
		}

		// Token: 0x0600D4D3 RID: 54483 RVA: 0x004CC130 File Offset: 0x004CA330
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

		// Token: 0x0600D4D4 RID: 54484 RVA: 0x004CC162 File Offset: 0x004CA362
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnSuspend()
		{
			this.PauseIO();
		}

		// Token: 0x0600D4D5 RID: 54485 RVA: 0x004CC16C File Offset: 0x004CA36C
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

		// Token: 0x0600D4D6 RID: 54486 RVA: 0x004CC1D0 File Offset: 0x004CA3D0
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

		// Token: 0x0600D4D7 RID: 54487 RVA: 0x004CC229 File Offset: 0x004CA429
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

		// Token: 0x0600D4D8 RID: 54488 RVA: 0x004CC245 File Offset: 0x004CA445
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

		// Token: 0x0600D4D9 RID: 54489 RVA: 0x004CC266 File Offset: 0x004CA466
		[PublicizedFrom(EAccessModifier.Private)]
		public void GetSaveContainerAndRelativePath(SaveDataManagedPath path, out SaveContainer saveContainer, out string relativePath)
		{
			saveContainer = this.m_rootSaveContainer;
			relativePath = path.PathRelativeToRoot;
		}

		// Token: 0x0600D4DA RID: 54490 RVA: 0x004CC278 File Offset: 0x004CA478
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string text)
		{
			Log.Out("[XBL: SaveGameProvider] " + text);
		}

		// Token: 0x0600D4DB RID: 54491 RVA: 0x004CC28A File Offset: 0x004CA48A
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogWarning(string text)
		{
			Log.Warning("[XBL: SaveGameProvider] " + text);
		}

		// Token: 0x0600D4DC RID: 54492 RVA: 0x004CC29C File Offset: 0x004CA49C
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string text)
		{
			Log.Error("[XBL: SaveGameProvider] " + text);
		}

		// Token: 0x0600D4DD RID: 54493 RVA: 0x004CC278 File Offset: 0x004CA478
		[Conditional("DEBUG_SAVE_DATA_MANAGER")]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogTrace(string text)
		{
			Log.Out("[XBL: SaveGameProvider] " + text);
		}

		// Token: 0x17001A68 RID: 6760
		// (get) Token: 0x0600D4DE RID: 54494 RVA: 0x004CC2AE File Offset: 0x004CA4AE
		// (set) Token: 0x0600D4DF RID: 54495 RVA: 0x004CC2B6 File Offset: 0x004CA4B6
		public ESaveGameProviderStatus Status
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A69 RID: 6761
		// (get) Token: 0x0600D4E0 RID: 54496 RVA: 0x004CC2BF File Offset: 0x004CA4BF
		// (set) Token: 0x0600D4E1 RID: 54497 RVA: 0x004CC2C7 File Offset: 0x004CA4C7
		public IPlatformSaveGameIOProvider Cache
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1400012B RID: 299
		// (add) Token: 0x0600D4E2 RID: 54498 RVA: 0x004CC2D0 File Offset: 0x004CA4D0
		// (remove) Token: 0x0600D4E3 RID: 54499 RVA: 0x004CC330 File Offset: 0x004CA530
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

		// Token: 0x0600D4E4 RID: 54500 RVA: 0x004CC384 File Offset: 0x004CA584
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

		// Token: 0x0600D4E5 RID: 54501 RVA: 0x004CC424 File Offset: 0x004CA624
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

		// Token: 0x0600D4E6 RID: 54502 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool ShouldBackup()
		{
			return false;
		}

		// Token: 0x0600D4E7 RID: 54503 RVA: 0x004CC4CE File Offset: 0x004CA6CE
		public bool ShouldCommit()
		{
			return !this.m_paused;
		}

		// Token: 0x0600D4E8 RID: 54504 RVA: 0x004CC4D9 File Offset: 0x004CA6D9
		public double GetCommitProgress()
		{
			return 1.0;
		}

		// Token: 0x0600D4E9 RID: 54505 RVA: 0x004CC4E4 File Offset: 0x004CA6E4
		public void Flush(bool waitForFlush)
		{
			ISaveStorageProvider saveStorageProvider = this.m_saveStorageProvider;
			if (saveStorageProvider == null)
			{
				return;
			}
			saveStorageProvider.Flush(waitForFlush);
		}

		// Token: 0x0600D4EA RID: 54506 RVA: 0x00020021 File Offset: 0x0001E221
		public bool ShouldLimitSize()
		{
			return true;
		}

		// Token: 0x0600D4EB RID: 54507 RVA: 0x004CC4F8 File Offset: 0x004CA6F8
		public void UpdateSizes()
		{
			using (this.CreateOperationScope("UpdateSizes"))
			{
				this.m_saveStorageProvider.SizeTracker.RefreshSync();
			}
		}

		// Token: 0x0600D4EC RID: 54508 RVA: 0x004CC544 File Offset: 0x004CA744
		public SaveDataSizes GetSizes()
		{
			SaveDataSizes sizes;
			using (this.CreateOperationScope("GetSizes"))
			{
				sizes = this.m_saveStorageProvider.SizeTracker.Sizes;
			}
			return sizes;
		}

		// Token: 0x0600D4ED RID: 54509 RVA: 0x004CC590 File Offset: 0x004CA790
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

		// Token: 0x0600D4EE RID: 54510 RVA: 0x004CC5E4 File Offset: 0x004CA7E4
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

		// Token: 0x0600D4EF RID: 54511 RVA: 0x004CC638 File Offset: 0x004CA838
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

		// Token: 0x0600D4F0 RID: 54512 RVA: 0x004CC6F0 File Offset: 0x004CA8F0
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

		// Token: 0x0600D4F1 RID: 54513 RVA: 0x004CC740 File Offset: 0x004CA940
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

		// Token: 0x0600D4F2 RID: 54514 RVA: 0x004CC794 File Offset: 0x004CA994
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

		// Token: 0x0600D4F3 RID: 54515 RVA: 0x004CC7E8 File Offset: 0x004CA9E8
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

		// Token: 0x0600D4F4 RID: 54516 RVA: 0x004CC880 File Offset: 0x004CAA80
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

		// Token: 0x0600D4F5 RID: 54517 RVA: 0x004CC8E0 File Offset: 0x004CAAE0
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

		// Token: 0x0600D4F6 RID: 54518 RVA: 0x004CC934 File Offset: 0x004CAB34
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

		// Token: 0x0600D4F7 RID: 54519 RVA: 0x004CC988 File Offset: 0x004CAB88
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			IEnumerable<SaveDataManagedPath> enumerable;
			using (this.CreateOperationScope("ManagedDirectoryEnumerateDirectories"))
			{
				enumerable = this.ManagedDirectoryEnumerateInternal(path, searchPattern, searchOption, true, false).ToArray<SaveDataManagedPath>();
			}
			return enumerable;
		}

		// Token: 0x0600D4F8 RID: 54520 RVA: 0x004CC9D4 File Offset: 0x004CABD4
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			IEnumerable<SaveDataManagedPath> enumerable;
			using (this.CreateOperationScope("ManagedDirectoryEnumerateFiles"))
			{
				enumerable = this.ManagedDirectoryEnumerateInternal(path, searchPattern, searchOption, false, true).ToArray<SaveDataManagedPath>();
			}
			return enumerable;
		}

		// Token: 0x0600D4F9 RID: 54521 RVA: 0x004CCA20 File Offset: 0x004CAC20
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			IEnumerable<SaveDataManagedPath> enumerable;
			using (this.CreateOperationScope("ManagedDirectoryEnumerateFileSystemEntries"))
			{
				enumerable = this.ManagedDirectoryEnumerateInternal(path, searchPattern, searchOption, true, true).ToArray<SaveDataManagedPath>();
			}
			return enumerable;
		}

		// Token: 0x0600D4FA RID: 54522 RVA: 0x004CCA6C File Offset: 0x004CAC6C
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateInternal(SaveDataManagedPath path, string searchPattern, SearchOption searchOption, bool includeDirectories, bool includeFiles)
		{
			SaveContainer saveContainer;
			string text;
			this.GetSaveContainerAndRelativePath(path, out saveContainer, out text);
			IEnumerable<string> enumerable = saveContainer.DirectoryEnumerate(text, searchPattern, searchOption == SearchOption.AllDirectories, includeDirectories, includeFiles);
			return SaveGameProviderHelper.GetManagedPathsFromBaseAndSubPaths(path, text, enumerable);
		}

		// Token: 0x0600D4FB RID: 54523 RVA: 0x004CCAA0 File Offset: 0x004CACA0
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

		// Token: 0x0600D4FC RID: 54524 RVA: 0x004CCAF4 File Offset: 0x004CACF4
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

		// Token: 0x0600D4FD RID: 54525 RVA: 0x004CCB48 File Offset: 0x004CAD48
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

		// Token: 0x0600D4FE RID: 54526 RVA: 0x004CCBB8 File Offset: 0x004CADB8
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

		// Token: 0x0600D4FF RID: 54527 RVA: 0x004CCC28 File Offset: 0x004CAE28
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

		// Token: 0x0400A1EB RID: 41451
		[PublicizedFrom(EAccessModifier.Private)]
		public SingleThreadTaskScheduler m_taskScheduler;

		// Token: 0x0400A1EC RID: 41452
		[PublicizedFrom(EAccessModifier.Private)]
		public Action m_initializedDelegate;

		// Token: 0x0400A1ED RID: 41453
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_initializedDelegateLock = new object();

		// Token: 0x0400A1EE RID: 41454
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform m_owner;

		// Token: 0x0400A1EF RID: 41455
		[PublicizedFrom(EAccessModifier.Private)]
		public IApplicationStateController m_appState;

		// Token: 0x0400A1F0 RID: 41456
		[PublicizedFrom(EAccessModifier.Private)]
		public User m_userClient;

		// Token: 0x0400A1F1 RID: 41457
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_maxStorageSizeBytes;

		// Token: 0x0400A1F2 RID: 41458
		[PublicizedFrom(EAccessModifier.Private)]
		public ISaveStorageProvider m_saveStorageProvider;

		// Token: 0x0400A1F3 RID: 41459
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_suspended;

		// Token: 0x0400A1F4 RID: 41460
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_paused;

		// Token: 0x0400A1F5 RID: 41461
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_operations;

		// Token: 0x0400A1F6 RID: 41462
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveContainer m_rootSaveContainer;

		// Token: 0x02001BFD RID: 7165
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly struct OperationScope : IDisposable
		{
			// Token: 0x0600D500 RID: 54528 RVA: 0x004CCCAC File Offset: 0x004CAEAC
			public OperationScope(SaveGameProviderGameCore provider)
			{
				this.m_provider = provider;
				Interlocked.Increment(ref this.m_provider.m_operations);
			}

			// Token: 0x0600D501 RID: 54529 RVA: 0x004CCCC6 File Offset: 0x004CAEC6
			public void Dispose()
			{
				Interlocked.Decrement(ref this.m_provider.m_operations);
			}

			// Token: 0x0400A1F9 RID: 41465
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly SaveGameProviderGameCore m_provider;
		}
	}
}
