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
	// Token: 0x02001C5F RID: 7263
	public class SaveGameProviderGameCore : IPlatformSaveGameProvider, IPlatformSaveGameIOProvider
	{
		// Token: 0x0600D75A RID: 55130 RVA: 0x004D45D8 File Offset: 0x004D27D8
		public SaveGameProviderGameCore(long maxStorageSizeBytes = 1073741824L)
		{
			this.m_maxStorageSizeBytes = maxStorageSizeBytes;
		}

		// Token: 0x0600D75B RID: 55131 RVA: 0x004D45F2 File Offset: 0x004D27F2
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

		// Token: 0x0600D75C RID: 55132 RVA: 0x004D4634 File Offset: 0x004D2834
		[PublicizedFrom(EAccessModifier.Private)]
		public void InitializeSaveStorageProvider()
		{
			SaveStorageProvider value = LaunchPrefs.GameCoreSaveStorageProvider.Value;
			SaveGameProviderGameCore.LogInfo(string.Format("[GDK] Initializing Save Storage Provider '{0}'.", value));
			this.m_saveStorageProvider = value.Create();
			this.m_saveStorageProvider.InitializeAsync(this.m_owner, this.m_maxStorageSizeBytes, this.m_taskScheduler, new OnGameSaveProviderStatusChanged(this.OnGameSaveProviderStatusChanged));
		}

		// Token: 0x0600D75D RID: 55133 RVA: 0x004D4698 File Offset: 0x004D2898
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

		// Token: 0x0600D75E RID: 55134 RVA: 0x004D4714 File Offset: 0x004D2914
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnGameSaveProviderReady()
		{
			this.m_rootSaveContainer = new SaveContainer(this.m_saveStorageProvider.RootSaveStorageContainer, this.m_saveStorageProvider.SizeTracker);
			this.ResumeIO();
		}

		// Token: 0x0600D75F RID: 55135 RVA: 0x004D4740 File Offset: 0x004D2940
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

		// Token: 0x0600D760 RID: 55136 RVA: 0x004D4772 File Offset: 0x004D2972
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnSuspend()
		{
			this.PauseIO();
		}

		// Token: 0x0600D761 RID: 55137 RVA: 0x004D477C File Offset: 0x004D297C
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

		// Token: 0x0600D762 RID: 55138 RVA: 0x004D47E0 File Offset: 0x004D29E0
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

		// Token: 0x0600D763 RID: 55139 RVA: 0x004D4839 File Offset: 0x004D2A39
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

		// Token: 0x0600D764 RID: 55140 RVA: 0x004D4855 File Offset: 0x004D2A55
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

		// Token: 0x0600D765 RID: 55141 RVA: 0x004D4876 File Offset: 0x004D2A76
		[PublicizedFrom(EAccessModifier.Private)]
		public void GetSaveContainerAndRelativePath(SaveDataManagedPath path, out SaveContainer saveContainer, out string relativePath)
		{
			saveContainer = this.m_rootSaveContainer;
			relativePath = path.PathRelativeToRoot;
		}

		// Token: 0x0600D766 RID: 55142 RVA: 0x004D4888 File Offset: 0x004D2A88
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string text)
		{
			Log.Out("[XBL: SaveGameProvider] " + text);
		}

		// Token: 0x0600D767 RID: 55143 RVA: 0x004D489A File Offset: 0x004D2A9A
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogWarning(string text)
		{
			Log.Warning("[XBL: SaveGameProvider] " + text);
		}

		// Token: 0x0600D768 RID: 55144 RVA: 0x004D48AC File Offset: 0x004D2AAC
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string text)
		{
			Log.Error("[XBL: SaveGameProvider] " + text);
		}

		// Token: 0x0600D769 RID: 55145 RVA: 0x004D4888 File Offset: 0x004D2A88
		[Conditional("DEBUG_SAVE_DATA_MANAGER")]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogTrace(string text)
		{
			Log.Out("[XBL: SaveGameProvider] " + text);
		}

		// Token: 0x17001B56 RID: 6998
		// (get) Token: 0x0600D76A RID: 55146 RVA: 0x004D48BE File Offset: 0x004D2ABE
		// (set) Token: 0x0600D76B RID: 55147 RVA: 0x004D48C6 File Offset: 0x004D2AC6
		public ESaveGameProviderStatus Status
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B57 RID: 6999
		// (get) Token: 0x0600D76C RID: 55148 RVA: 0x004D48CF File Offset: 0x004D2ACF
		// (set) Token: 0x0600D76D RID: 55149 RVA: 0x004D48D7 File Offset: 0x004D2AD7
		public IPlatformSaveGameIOProvider Cache
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1400012A RID: 298
		// (add) Token: 0x0600D76E RID: 55150 RVA: 0x004D48E0 File Offset: 0x004D2AE0
		// (remove) Token: 0x0600D76F RID: 55151 RVA: 0x004D4940 File Offset: 0x004D2B40
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

		// Token: 0x0600D770 RID: 55152 RVA: 0x004D4994 File Offset: 0x004D2B94
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

		// Token: 0x0600D771 RID: 55153 RVA: 0x004D4A34 File Offset: 0x004D2C34
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

		// Token: 0x0600D772 RID: 55154 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool ShouldBackup()
		{
			return false;
		}

		// Token: 0x0600D773 RID: 55155 RVA: 0x004D4ADE File Offset: 0x004D2CDE
		public bool ShouldCommit()
		{
			return !this.m_paused;
		}

		// Token: 0x0600D774 RID: 55156 RVA: 0x004D4AE9 File Offset: 0x004D2CE9
		public double GetCommitProgress()
		{
			return 1.0;
		}

		// Token: 0x0600D775 RID: 55157 RVA: 0x004D4AF4 File Offset: 0x004D2CF4
		public void Flush(bool waitForFlush)
		{
			ISaveStorageProvider saveStorageProvider = this.m_saveStorageProvider;
			if (saveStorageProvider == null)
			{
				return;
			}
			saveStorageProvider.Flush(waitForFlush);
		}

		// Token: 0x0600D776 RID: 55158 RVA: 0x00020119 File Offset: 0x0001E319
		public bool ShouldLimitSize()
		{
			return true;
		}

		// Token: 0x0600D777 RID: 55159 RVA: 0x004D4B08 File Offset: 0x004D2D08
		public void UpdateSizes()
		{
			using (this.CreateOperationScope("UpdateSizes"))
			{
				this.m_saveStorageProvider.SizeTracker.RefreshSync();
			}
		}

		// Token: 0x0600D778 RID: 55160 RVA: 0x004D4B54 File Offset: 0x004D2D54
		public SaveDataSizes GetSizes()
		{
			SaveDataSizes sizes;
			using (this.CreateOperationScope("GetSizes"))
			{
				sizes = this.m_saveStorageProvider.SizeTracker.Sizes;
			}
			return sizes;
		}

		// Token: 0x0600D779 RID: 55161 RVA: 0x004D4BA0 File Offset: 0x004D2DA0
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

		// Token: 0x0600D77A RID: 55162 RVA: 0x004D4BF4 File Offset: 0x004D2DF4
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

		// Token: 0x0600D77B RID: 55163 RVA: 0x004D4C48 File Offset: 0x004D2E48
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

		// Token: 0x0600D77C RID: 55164 RVA: 0x004D4D00 File Offset: 0x004D2F00
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

		// Token: 0x0600D77D RID: 55165 RVA: 0x004D4D50 File Offset: 0x004D2F50
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

		// Token: 0x0600D77E RID: 55166 RVA: 0x004D4DA4 File Offset: 0x004D2FA4
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

		// Token: 0x0600D77F RID: 55167 RVA: 0x004D4DF8 File Offset: 0x004D2FF8
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

		// Token: 0x0600D780 RID: 55168 RVA: 0x004D4E90 File Offset: 0x004D3090
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

		// Token: 0x0600D781 RID: 55169 RVA: 0x004D4EF0 File Offset: 0x004D30F0
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

		// Token: 0x0600D782 RID: 55170 RVA: 0x004D4F44 File Offset: 0x004D3144
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

		// Token: 0x0600D783 RID: 55171 RVA: 0x004D4F98 File Offset: 0x004D3198
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			IEnumerable<SaveDataManagedPath> enumerable;
			using (this.CreateOperationScope("ManagedDirectoryEnumerateDirectories"))
			{
				enumerable = this.ManagedDirectoryEnumerateInternal(path, searchPattern, searchOption, true, false).ToArray<SaveDataManagedPath>();
			}
			return enumerable;
		}

		// Token: 0x0600D784 RID: 55172 RVA: 0x004D4FE4 File Offset: 0x004D31E4
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			IEnumerable<SaveDataManagedPath> enumerable;
			using (this.CreateOperationScope("ManagedDirectoryEnumerateFiles"))
			{
				enumerable = this.ManagedDirectoryEnumerateInternal(path, searchPattern, searchOption, false, true).ToArray<SaveDataManagedPath>();
			}
			return enumerable;
		}

		// Token: 0x0600D785 RID: 55173 RVA: 0x004D5030 File Offset: 0x004D3230
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			IEnumerable<SaveDataManagedPath> enumerable;
			using (this.CreateOperationScope("ManagedDirectoryEnumerateFileSystemEntries"))
			{
				enumerable = this.ManagedDirectoryEnumerateInternal(path, searchPattern, searchOption, true, true).ToArray<SaveDataManagedPath>();
			}
			return enumerable;
		}

		// Token: 0x0600D786 RID: 55174 RVA: 0x004D507C File Offset: 0x004D327C
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateInternal(SaveDataManagedPath path, string searchPattern, SearchOption searchOption, bool includeDirectories, bool includeFiles)
		{
			SaveContainer saveContainer;
			string text;
			this.GetSaveContainerAndRelativePath(path, out saveContainer, out text);
			IEnumerable<string> enumerable = saveContainer.DirectoryEnumerate(text, searchPattern, searchOption == SearchOption.AllDirectories, includeDirectories, includeFiles);
			return SaveGameProviderHelper.GetManagedPathsFromBaseAndSubPaths(path, text, enumerable);
		}

		// Token: 0x0600D787 RID: 55175 RVA: 0x004D50B0 File Offset: 0x004D32B0
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

		// Token: 0x0600D788 RID: 55176 RVA: 0x004D5104 File Offset: 0x004D3304
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

		// Token: 0x0600D789 RID: 55177 RVA: 0x004D5158 File Offset: 0x004D3358
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

		// Token: 0x0600D78A RID: 55178 RVA: 0x004D51C8 File Offset: 0x004D33C8
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

		// Token: 0x0600D78B RID: 55179 RVA: 0x004D5238 File Offset: 0x004D3438
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

		// Token: 0x0400A3A9 RID: 41897
		[PublicizedFrom(EAccessModifier.Private)]
		public SingleThreadTaskScheduler m_taskScheduler;

		// Token: 0x0400A3AA RID: 41898
		[PublicizedFrom(EAccessModifier.Private)]
		public Action m_initializedDelegate;

		// Token: 0x0400A3AB RID: 41899
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_initializedDelegateLock = new object();

		// Token: 0x0400A3AC RID: 41900
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform m_owner;

		// Token: 0x0400A3AD RID: 41901
		[PublicizedFrom(EAccessModifier.Private)]
		public IApplicationStateController m_appState;

		// Token: 0x0400A3AE RID: 41902
		[PublicizedFrom(EAccessModifier.Private)]
		public User m_userClient;

		// Token: 0x0400A3AF RID: 41903
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_maxStorageSizeBytes;

		// Token: 0x0400A3B0 RID: 41904
		[PublicizedFrom(EAccessModifier.Private)]
		public ISaveStorageProvider m_saveStorageProvider;

		// Token: 0x0400A3B1 RID: 41905
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_suspended;

		// Token: 0x0400A3B2 RID: 41906
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_paused;

		// Token: 0x0400A3B3 RID: 41907
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_operations;

		// Token: 0x0400A3B4 RID: 41908
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveContainer m_rootSaveContainer;

		// Token: 0x02001C60 RID: 7264
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly struct OperationScope : IDisposable
		{
			// Token: 0x0600D78C RID: 55180 RVA: 0x004D52BC File Offset: 0x004D34BC
			public OperationScope(SaveGameProviderGameCore provider)
			{
				this.m_provider = provider;
				Interlocked.Increment(ref this.m_provider.m_operations);
			}

			// Token: 0x0600D78D RID: 55181 RVA: 0x004D52D6 File Offset: 0x004D34D6
			public void Dispose()
			{
				Interlocked.Decrement(ref this.m_provider.m_operations);
			}

			// Token: 0x0400A3B7 RID: 41911
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly SaveGameProviderGameCore m_provider;
		}
	}
}
