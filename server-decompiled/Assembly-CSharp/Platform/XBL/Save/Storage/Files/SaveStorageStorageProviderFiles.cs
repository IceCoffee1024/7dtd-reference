using System;
using System.IO;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL.Save.Storage.Files
{
	// Token: 0x02001CB5 RID: 7349
	public sealed class SaveStorageStorageProviderFiles : ISaveStorageProvider, IDisposable
	{
		// Token: 0x0600D967 RID: 55655 RVA: 0x004DD8AC File Offset: 0x004DBAAC
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string text)
		{
			Log.Out("[XBL: SaveStorageStorageProviderFiles] " + text);
		}

		// Token: 0x0600D968 RID: 55656 RVA: 0x004DD8BE File Offset: 0x004DBABE
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string text)
		{
			Log.Error("[XBL: SaveStorageStorageProviderFiles] " + text);
		}

		// Token: 0x0600D969 RID: 55657 RVA: 0x004DD8D0 File Offset: 0x004DBAD0
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

		// Token: 0x17001B8C RID: 7052
		// (get) Token: 0x0600D96A RID: 55658 RVA: 0x004DD937 File Offset: 0x004DBB37
		public SizeTracker SizeTracker
		{
			get
			{
				return this.m_sizeTracker;
			}
		}

		// Token: 0x17001B8D RID: 7053
		// (get) Token: 0x0600D96B RID: 55659 RVA: 0x004DD93F File Offset: 0x004DBB3F
		public ISaveStorageContainer RootSaveStorageContainer
		{
			get
			{
				return this.m_rootSaveStorageContainer;
			}
		}

		// Token: 0x0600D96C RID: 55660 RVA: 0x004DD948 File Offset: 0x004DBB48
		public void InitializeAsync(IPlatform owner, long maxSizeBytes, SingleThreadTaskScheduler taskScheduler, OnGameSaveProviderStatusChanged statusChanged)
		{
			this.m_api = (XblPlatformApi)owner.Api;
			this.m_userClient = (User)owner.User;
			this.m_maxSizeBytes = maxSizeBytes;
			this.m_taskScheduler = taskScheduler;
			this.m_statusChanged = statusChanged;
			SDK.XGameSaveFilesGetFolderWithUiAsync(this.m_userClient.UserHandle, this.m_api.SCID, new XGameSaveFilesGetFolderWithUiCompleted(this.OnGameSaveFilesGetFolderWithUiCompleted));
		}

		// Token: 0x0600D96D RID: 55661 RVA: 0x004DD9B5 File Offset: 0x004DBBB5
		public void Flush(bool waitForFlush)
		{
			SaveStorageStorageContainerFiles rootSaveStorageContainer = this.m_rootSaveStorageContainer;
			if (rootSaveStorageContainer == null)
			{
				return;
			}
			rootSaveStorageContainer.Flush(waitForFlush);
		}

		// Token: 0x0600D96E RID: 55662 RVA: 0x004DD9C8 File Offset: 0x004DBBC8
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

		// Token: 0x0600D96F RID: 55663 RVA: 0x004DDA04 File Offset: 0x004DBC04
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

		// Token: 0x0600D970 RID: 55664 RVA: 0x004DDAD0 File Offset: 0x004DBCD0
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnGameSaveDirectoryReady()
		{
			this.m_sizeTracker = new SizeTracker(this.m_maxSizeBytes, 0L, new SizeTrackerGetRemainingQuotaAsync(this.GetRemainingQuotaAsync), false);
			this.m_sizeTracker.RefreshSync();
			this.m_rootSaveStorageContainer = new SaveStorageStorageContainerFiles(this.m_gameSaveDirectory, "root");
			this.m_statusChanged(ESaveGameProviderStatus.Ok);
		}

		// Token: 0x0400A546 RID: 42310
		[PublicizedFrom(EAccessModifier.Private)]
		public XblPlatformApi m_api;

		// Token: 0x0400A547 RID: 42311
		[PublicizedFrom(EAccessModifier.Private)]
		public User m_userClient;

		// Token: 0x0400A548 RID: 42312
		[PublicizedFrom(EAccessModifier.Private)]
		public SingleThreadTaskScheduler m_taskScheduler;

		// Token: 0x0400A549 RID: 42313
		[PublicizedFrom(EAccessModifier.Private)]
		public OnGameSaveProviderStatusChanged m_statusChanged;

		// Token: 0x0400A54A RID: 42314
		[PublicizedFrom(EAccessModifier.Private)]
		public string m_gameSaveDirectory;

		// Token: 0x0400A54B RID: 42315
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_maxSizeBytes;

		// Token: 0x0400A54C RID: 42316
		[PublicizedFrom(EAccessModifier.Private)]
		public SizeTracker m_sizeTracker;

		// Token: 0x0400A54D RID: 42317
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveStorageStorageContainerFiles m_rootSaveStorageContainer;

		// Token: 0x0400A54E RID: 42318
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_isDisposed;
	}
}
