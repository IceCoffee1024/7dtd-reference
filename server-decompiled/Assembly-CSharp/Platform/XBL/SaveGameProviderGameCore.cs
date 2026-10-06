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
	// Token: 0x02001C2E RID: 7214
	public class SaveGameProviderGameCore : IPlatformSaveGameProvider, IPlatformSaveGameIOProvider
	{
		// Token: 0x0600D646 RID: 54854 RVA: 0x004D0AB0 File Offset: 0x004CECB0
		public SaveGameProviderGameCore(long maxStorageSizeBytes = 1073741824L)
		{
			this.m_maxStorageSizeBytes = maxStorageSizeBytes;
		}

		// Token: 0x0600D647 RID: 54855 RVA: 0x004D0ACA File Offset: 0x004CECCA
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

		// Token: 0x0600D648 RID: 54856 RVA: 0x004D0B0C File Offset: 0x004CED0C
		[PublicizedFrom(EAccessModifier.Private)]
		public void InitializeSaveStorageProvider()
		{
			SaveStorageProvider value = LaunchPrefs.GameCoreSaveStorageProvider.Value;
			SaveGameProviderGameCore.LogInfo(string.Format("[GDK] Initializing Save Storage Provider '{0}'.", value));
			this.m_saveStorageProvider = value.Create();
			this.m_saveStorageProvider.InitializeAsync(this.m_owner, this.m_maxStorageSizeBytes, this.m_taskScheduler, new OnGameSaveProviderStatusChanged(this.OnGameSaveProviderStatusChanged));
		}

		// Token: 0x0600D649 RID: 54857 RVA: 0x004D0B70 File Offset: 0x004CED70
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

		// Token: 0x0600D64A RID: 54858 RVA: 0x004D0BEC File Offset: 0x004CEDEC
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnGameSaveProviderReady()
		{
			this.m_rootSaveContainer = new SaveContainer(this.m_saveStorageProvider.RootSaveStorageContainer, this.m_saveStorageProvider.SizeTracker);
			this.ResumeIO();
		}

		// Token: 0x0600D64B RID: 54859 RVA: 0x004D0C18 File Offset: 0x004CEE18
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

		// Token: 0x0600D64C RID: 54860 RVA: 0x004D0C4A File Offset: 0x004CEE4A
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnSuspend()
		{
			this.PauseIO();
		}

		// Token: 0x0600D64D RID: 54861 RVA: 0x004D0C54 File Offset: 0x004CEE54
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

		// Token: 0x0600D64E RID: 54862 RVA: 0x004D0CB8 File Offset: 0x004CEEB8
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

		// Token: 0x0600D64F RID: 54863 RVA: 0x004D0D11 File Offset: 0x004CEF11
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

		// Token: 0x0600D650 RID: 54864 RVA: 0x004D0D2D File Offset: 0x004CEF2D
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

		// Token: 0x0600D651 RID: 54865 RVA: 0x004D0D4E File Offset: 0x004CEF4E
		[PublicizedFrom(EAccessModifier.Private)]
		public void GetSaveContainerAndRelativePath(SaveDataManagedPath path, out SaveContainer saveContainer, out string relativePath)
		{
			saveContainer = this.m_rootSaveContainer;
			relativePath = path.PathRelativeToRoot;
		}

		// Token: 0x0600D652 RID: 54866 RVA: 0x004D0D60 File Offset: 0x004CEF60
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string text)
		{
			Log.Out("[XBL: SaveGameProvider] " + text);
		}

		// Token: 0x0600D653 RID: 54867 RVA: 0x004D0D72 File Offset: 0x004CEF72
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogWarning(string text)
		{
			Log.Warning("[XBL: SaveGameProvider] " + text);
		}

		// Token: 0x0600D654 RID: 54868 RVA: 0x004D0D84 File Offset: 0x004CEF84
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string text)
		{
			Log.Error("[XBL: SaveGameProvider] " + text);
		}

		// Token: 0x0600D655 RID: 54869 RVA: 0x004D0D60 File Offset: 0x004CEF60
		[Conditional("DEBUG_SAVE_DATA_MANAGER")]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogTrace(string text)
		{
			Log.Out("[XBL: SaveGameProvider] " + text);
		}

		// Token: 0x17001B0E RID: 6926
		// (get) Token: 0x0600D656 RID: 54870 RVA: 0x004D0D96 File Offset: 0x004CEF96
		// (set) Token: 0x0600D657 RID: 54871 RVA: 0x004D0D9E File Offset: 0x004CEF9E
		public ESaveGameProviderStatus Status
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B0F RID: 6927
		// (get) Token: 0x0600D658 RID: 54872 RVA: 0x004D0DA7 File Offset: 0x004CEFA7
		// (set) Token: 0x0600D659 RID: 54873 RVA: 0x004D0DAF File Offset: 0x004CEFAF
		public IPlatformSaveGameIOProvider Cache
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1400012A RID: 298
		// (add) Token: 0x0600D65A RID: 54874 RVA: 0x004D0DB8 File Offset: 0x004CEFB8
		// (remove) Token: 0x0600D65B RID: 54875 RVA: 0x004D0E18 File Offset: 0x004CF018
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

		// Token: 0x0600D65C RID: 54876 RVA: 0x004D0E6C File Offset: 0x004CF06C
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

		// Token: 0x0600D65D RID: 54877 RVA: 0x004D0F0C File Offset: 0x004CF10C
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

		// Token: 0x0600D65E RID: 54878 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool ShouldBackup()
		{
			return false;
		}

		// Token: 0x0600D65F RID: 54879 RVA: 0x004D0FB6 File Offset: 0x004CF1B6
		public bool ShouldCommit()
		{
			return !this.m_paused;
		}

		// Token: 0x0600D660 RID: 54880 RVA: 0x004D0FC1 File Offset: 0x004CF1C1
		public double GetCommitProgress()
		{
			return 1.0;
		}

		// Token: 0x0600D661 RID: 54881 RVA: 0x004D0FCC File Offset: 0x004CF1CC
		public void Flush(bool waitForFlush)
		{
			ISaveStorageProvider saveStorageProvider = this.m_saveStorageProvider;
			if (saveStorageProvider == null)
			{
				return;
			}
			saveStorageProvider.Flush(waitForFlush);
		}

		// Token: 0x0600D662 RID: 54882 RVA: 0x00020119 File Offset: 0x0001E319
		public bool ShouldLimitSize()
		{
			return true;
		}

		// Token: 0x0600D663 RID: 54883 RVA: 0x004D0FE0 File Offset: 0x004CF1E0
		public void UpdateSizes()
		{
			using (this.CreateOperationScope("UpdateSizes"))
			{
				this.m_saveStorageProvider.SizeTracker.RefreshSync();
			}
		}

		// Token: 0x0600D664 RID: 54884 RVA: 0x004D102C File Offset: 0x004CF22C
		public SaveDataSizes GetSizes()
		{
			SaveDataSizes sizes;
			using (this.CreateOperationScope("GetSizes"))
			{
				sizes = this.m_saveStorageProvider.SizeTracker.Sizes;
			}
			return sizes;
		}

		// Token: 0x0600D665 RID: 54885 RVA: 0x004D1078 File Offset: 0x004CF278
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

		// Token: 0x0600D666 RID: 54886 RVA: 0x004D10CC File Offset: 0x004CF2CC
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

		// Token: 0x0600D667 RID: 54887 RVA: 0x004D1120 File Offset: 0x004CF320
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

		// Token: 0x0600D668 RID: 54888 RVA: 0x004D11D8 File Offset: 0x004CF3D8
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

		// Token: 0x0600D669 RID: 54889 RVA: 0x004D1228 File Offset: 0x004CF428
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

		// Token: 0x0600D66A RID: 54890 RVA: 0x004D127C File Offset: 0x004CF47C
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

		// Token: 0x0600D66B RID: 54891 RVA: 0x004D12D0 File Offset: 0x004CF4D0
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

		// Token: 0x0600D66C RID: 54892 RVA: 0x004D1368 File Offset: 0x004CF568
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

		// Token: 0x0600D66D RID: 54893 RVA: 0x004D13C8 File Offset: 0x004CF5C8
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

		// Token: 0x0600D66E RID: 54894 RVA: 0x004D141C File Offset: 0x004CF61C
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

		// Token: 0x0600D66F RID: 54895 RVA: 0x004D1470 File Offset: 0x004CF670
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			IEnumerable<SaveDataManagedPath> enumerable;
			using (this.CreateOperationScope("ManagedDirectoryEnumerateDirectories"))
			{
				enumerable = this.ManagedDirectoryEnumerateInternal(path, searchPattern, searchOption, true, false).ToArray<SaveDataManagedPath>();
			}
			return enumerable;
		}

		// Token: 0x0600D670 RID: 54896 RVA: 0x004D14BC File Offset: 0x004CF6BC
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			IEnumerable<SaveDataManagedPath> enumerable;
			using (this.CreateOperationScope("ManagedDirectoryEnumerateFiles"))
			{
				enumerable = this.ManagedDirectoryEnumerateInternal(path, searchPattern, searchOption, false, true).ToArray<SaveDataManagedPath>();
			}
			return enumerable;
		}

		// Token: 0x0600D671 RID: 54897 RVA: 0x004D1508 File Offset: 0x004CF708
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			IEnumerable<SaveDataManagedPath> enumerable;
			using (this.CreateOperationScope("ManagedDirectoryEnumerateFileSystemEntries"))
			{
				enumerable = this.ManagedDirectoryEnumerateInternal(path, searchPattern, searchOption, true, true).ToArray<SaveDataManagedPath>();
			}
			return enumerable;
		}

		// Token: 0x0600D672 RID: 54898 RVA: 0x004D1554 File Offset: 0x004CF754
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateInternal(SaveDataManagedPath path, string searchPattern, SearchOption searchOption, bool includeDirectories, bool includeFiles)
		{
			SaveContainer saveContainer;
			string text;
			this.GetSaveContainerAndRelativePath(path, out saveContainer, out text);
			IEnumerable<string> enumerable = saveContainer.DirectoryEnumerate(text, searchPattern, searchOption == SearchOption.AllDirectories, includeDirectories, includeFiles);
			return SaveGameProviderHelper.GetManagedPathsFromBaseAndSubPaths(path, text, enumerable);
		}

		// Token: 0x0600D673 RID: 54899 RVA: 0x004D1588 File Offset: 0x004CF788
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

		// Token: 0x0600D674 RID: 54900 RVA: 0x004D15DC File Offset: 0x004CF7DC
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

		// Token: 0x0600D675 RID: 54901 RVA: 0x004D1630 File Offset: 0x004CF830
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

		// Token: 0x0600D676 RID: 54902 RVA: 0x004D16A0 File Offset: 0x004CF8A0
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

		// Token: 0x0600D677 RID: 54903 RVA: 0x004D1710 File Offset: 0x004CF910
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

		// Token: 0x0400A2C0 RID: 41664
		[PublicizedFrom(EAccessModifier.Private)]
		public SingleThreadTaskScheduler m_taskScheduler;

		// Token: 0x0400A2C1 RID: 41665
		[PublicizedFrom(EAccessModifier.Private)]
		public Action m_initializedDelegate;

		// Token: 0x0400A2C2 RID: 41666
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_initializedDelegateLock = new object();

		// Token: 0x0400A2C3 RID: 41667
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform m_owner;

		// Token: 0x0400A2C4 RID: 41668
		[PublicizedFrom(EAccessModifier.Private)]
		public IApplicationStateController m_appState;

		// Token: 0x0400A2C5 RID: 41669
		[PublicizedFrom(EAccessModifier.Private)]
		public User m_userClient;

		// Token: 0x0400A2C6 RID: 41670
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_maxStorageSizeBytes;

		// Token: 0x0400A2C7 RID: 41671
		[PublicizedFrom(EAccessModifier.Private)]
		public ISaveStorageProvider m_saveStorageProvider;

		// Token: 0x0400A2C8 RID: 41672
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_suspended;

		// Token: 0x0400A2C9 RID: 41673
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_paused;

		// Token: 0x0400A2CA RID: 41674
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_operations;

		// Token: 0x0400A2CB RID: 41675
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveContainer m_rootSaveContainer;

		// Token: 0x02001C2F RID: 7215
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly struct OperationScope : IDisposable
		{
			// Token: 0x0600D678 RID: 54904 RVA: 0x004D1794 File Offset: 0x004CF994
			public OperationScope(SaveGameProviderGameCore provider)
			{
				this.m_provider = provider;
				Interlocked.Increment(ref this.m_provider.m_operations);
			}

			// Token: 0x0600D679 RID: 54905 RVA: 0x004D17AE File Offset: 0x004CF9AE
			public void Dispose()
			{
				Interlocked.Decrement(ref this.m_provider.m_operations);
			}

			// Token: 0x0400A2CE RID: 41678
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly SaveGameProviderGameCore m_provider;
		}
	}
}
