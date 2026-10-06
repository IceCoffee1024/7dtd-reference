using System;
using System.IO;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL.Save.Storage.Files
{
	// Token: 0x02001C84 RID: 7300
	public sealed class SaveStorageStorageProviderFiles : ISaveStorageProvider, IDisposable
	{
		// Token: 0x0600D853 RID: 55379 RVA: 0x004D9D5C File Offset: 0x004D7F5C
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string text)
		{
			Log.Out("[XBL: SaveStorageStorageProviderFiles] " + text);
		}

		// Token: 0x0600D854 RID: 55380 RVA: 0x004D9D6E File Offset: 0x004D7F6E
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string text)
		{
			Log.Error("[XBL: SaveStorageStorageProviderFiles] " + text);
		}

		// Token: 0x0600D855 RID: 55381 RVA: 0x004D9D80 File Offset: 0x004D7F80
		public void Dispose()
		{
			SaveStorageStorageContainerFiles rootSaveStorageContainer = this.m_rootSaveStorageContainer;
			if (rootSaveStorageContainer != null)
			{
				rootSaveStorageContainer.Dispose();
			}
			this.m_rootSaveStorageContainer = null;
			SizeTracker sizeTracker = this.m_sizeTracker;
			if (sizeTracker != null)
			{
				sizeTracker.Dispose();
			}
			this.m_sizeTracker = null;
			this.m_gameSaveDirectory = null;
			this.m_statusChanged = null;
			this.m_taskScheduler = null;
			this.m_userClient = null;
			this.m_api = null;
			this.m_isDisposed = true;
		}

		// Token: 0x17001B44 RID: 6980
		// (get) Token: 0x0600D856 RID: 55382 RVA: 0x004D9DE7 File Offset: 0x004D7FE7
		public SizeTracker SizeTracker
		{
			get
			{
				return this.m_sizeTracker;
			}
		}

		// Token: 0x17001B45 RID: 6981
		// (get) Token: 0x0600D857 RID: 55383 RVA: 0x004D9DEF File Offset: 0x004D7FEF
		public ISaveStorageContainer RootSaveStorageContainer
		{
			get
			{
				return this.m_rootSaveStorageContainer;
			}
		}

		// Token: 0x0600D858 RID: 55384 RVA: 0x004D9DF8 File Offset: 0x004D7FF8
		public void InitializeAsync(IPlatform owner, long maxSizeBytes, SingleThreadTaskScheduler taskScheduler, OnGameSaveProviderStatusChanged statusChanged)
		{
			this.m_api = (XblPlatformApi)owner.Api;
			this.m_userClient = (User)owner.User;
			this.m_maxSizeBytes = maxSizeBytes;
			this.m_taskScheduler = taskScheduler;
			this.m_statusChanged = statusChanged;
			SDK.XGameSaveFilesGetFolderWithUiAsync(this.m_userClient.UserHandle, this.m_api.SCID, new XGameSaveFilesGetFolderWithUiCompleted(this.OnGameSaveFilesGetFolderWithUiCompleted));
		}

		// Token: 0x0600D859 RID: 55385 RVA: 0x004D9E65 File Offset: 0x004D8065
		public void Flush(bool waitForFlush)
		{
			SaveStorageStorageContainerFiles rootSaveStorageContainer = this.m_rootSaveStorageContainer;
			if (rootSaveStorageContainer == null)
			{
				return;
			}
			rootSaveStorageContainer.Flush(waitForFlush);
		}

		// Token: 0x0600D85A RID: 55386 RVA: 0x004D9E78 File Offset: 0x004D8078
		[PublicizedFrom(EAccessModifier.Private)]
		public void GetRemainingQuotaAsync(SizeTrackerGetRemainingQuotaCompleted completionRoutine)
		{
			this.m_taskScheduler.ExecuteNoWait(delegate
			{
				ulong num2;
				int num = SDK.XGameSaveFilesGetRemainingQuota(this.m_userClient.UserHandle, this.m_api.SCID, out num2);
				completionRoutine(num, (long)num2);
			});
		}

		// Token: 0x0600D85B RID: 55387 RVA: 0x004D9EB4 File Offset: 0x004D80B4
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnGameSaveFilesGetFolderWithUiCompleted(int hr, string folderResult)
		{
			XblHelpers.LogHR(hr, "Initialize Game Saves (Files).", false);
			if (this.m_isDisposed)
			{
				SaveStorageStorageProviderFiles.LogInfo("Disposed before XGameSaveFilesGetFolderWithUiAsync completed, cancelling setup");
				return;
			}
			if (!Unity.XGamingRuntime.Interop.HR.FAILED(hr))
			{
				this.m_gameSaveDirectory = Path.GetFullPath(folderResult);
				SaveStorageStorageProviderFiles.LogInfo("Game Save Files Directory: " + this.m_gameSaveDirectory);
				this.m_taskScheduler.ExecuteNoWait(new Action(this.OnGameSaveDirectoryReady));
				return;
			}
			if (hr == -2138898428)
			{
				this.m_statusChanged(ESaveGameProviderStatus.TemporaryError);
				SaveStorageStorageProviderFiles.LogInfo("User cancelled sync, retrying...");
				SDK.XGameSaveFilesGetFolderWithUiAsync(this.m_userClient.UserHandle, this.m_api.SCID, new XGameSaveFilesGetFolderWithUiCompleted(this.OnGameSaveFilesGetFolderWithUiCompleted));
				return;
			}
			SaveStorageStorageProviderFiles.LogError("Could not Initialize. This is a FATAL error!");
			this.m_statusChanged(ESaveGameProviderStatus.PermanentError);
		}

		// Token: 0x0600D85C RID: 55388 RVA: 0x004D9F80 File Offset: 0x004D8180
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnGameSaveDirectoryReady()
		{
			this.m_sizeTracker = new SizeTracker(this.m_maxSizeBytes, 0L, new SizeTrackerGetRemainingQuotaAsync(this.GetRemainingQuotaAsync), false);
			this.m_sizeTracker.RefreshSync();
			this.m_rootSaveStorageContainer = new SaveStorageStorageContainerFiles(this.m_gameSaveDirectory, "root");
			this.m_statusChanged(ESaveGameProviderStatus.Ok);
		}

		// Token: 0x0400A45D RID: 42077
		[PublicizedFrom(EAccessModifier.Private)]
		public XblPlatformApi m_api;

		// Token: 0x0400A45E RID: 42078
		[PublicizedFrom(EAccessModifier.Private)]
		public User m_userClient;

		// Token: 0x0400A45F RID: 42079
		[PublicizedFrom(EAccessModifier.Private)]
		public SingleThreadTaskScheduler m_taskScheduler;

		// Token: 0x0400A460 RID: 42080
		[PublicizedFrom(EAccessModifier.Private)]
		public OnGameSaveProviderStatusChanged m_statusChanged;

		// Token: 0x0400A461 RID: 42081
		[PublicizedFrom(EAccessModifier.Private)]
		public string m_gameSaveDirectory;

		// Token: 0x0400A462 RID: 42082
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_maxSizeBytes;

		// Token: 0x0400A463 RID: 42083
		[PublicizedFrom(EAccessModifier.Private)]
		public SizeTracker m_sizeTracker;

		// Token: 0x0400A464 RID: 42084
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveStorageStorageContainerFiles m_rootSaveStorageContainer;

		// Token: 0x0400A465 RID: 42085
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_isDisposed;
	}
}
