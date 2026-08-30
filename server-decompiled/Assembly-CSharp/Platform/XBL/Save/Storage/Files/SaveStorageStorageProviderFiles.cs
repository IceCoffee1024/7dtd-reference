using System;
using System.IO;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL.Save.Storage.Files
{
	// Token: 0x02001C52 RID: 7250
	public sealed class SaveStorageStorageProviderFiles : ISaveStorageProvider, IDisposable
	{
		// Token: 0x0600D6DA RID: 55002 RVA: 0x004D5220 File Offset: 0x004D3420
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string text)
		{
			Log.Out("[XBL: SaveStorageStorageProviderFiles] " + text);
		}

		// Token: 0x0600D6DB RID: 55003 RVA: 0x004D5232 File Offset: 0x004D3432
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string text)
		{
			Log.Error("[XBL: SaveStorageStorageProviderFiles] " + text);
		}

		// Token: 0x0600D6DC RID: 55004 RVA: 0x004D5244 File Offset: 0x004D3444
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

		// Token: 0x17001A9E RID: 6814
		// (get) Token: 0x0600D6DD RID: 55005 RVA: 0x004D52AB File Offset: 0x004D34AB
		public SizeTracker SizeTracker
		{
			get
			{
				return this.m_sizeTracker;
			}
		}

		// Token: 0x17001A9F RID: 6815
		// (get) Token: 0x0600D6DE RID: 55006 RVA: 0x004D52B3 File Offset: 0x004D34B3
		public ISaveStorageContainer RootSaveStorageContainer
		{
			get
			{
				return this.m_rootSaveStorageContainer;
			}
		}

		// Token: 0x0600D6DF RID: 55007 RVA: 0x004D52BC File Offset: 0x004D34BC
		public void InitializeAsync(IPlatform owner, long maxSizeBytes, SingleThreadTaskScheduler taskScheduler, OnGameSaveProviderStatusChanged statusChanged)
		{
			this.m_api = (XblPlatformApi)owner.Api;
			this.m_userClient = (User)owner.User;
			this.m_maxSizeBytes = maxSizeBytes;
			this.m_taskScheduler = taskScheduler;
			this.m_statusChanged = statusChanged;
			SDK.XGameSaveFilesGetFolderWithUiAsync(this.m_userClient.UserHandle, this.m_api.SCID, new XGameSaveFilesGetFolderWithUiCompleted(this.OnGameSaveFilesGetFolderWithUiCompleted));
		}

		// Token: 0x0600D6E0 RID: 55008 RVA: 0x004D5329 File Offset: 0x004D3529
		public void Flush(bool waitForFlush)
		{
			SaveStorageStorageContainerFiles rootSaveStorageContainer = this.m_rootSaveStorageContainer;
			if (rootSaveStorageContainer == null)
			{
				return;
			}
			rootSaveStorageContainer.Flush(waitForFlush);
		}

		// Token: 0x0600D6E1 RID: 55009 RVA: 0x004D533C File Offset: 0x004D353C
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

		// Token: 0x0600D6E2 RID: 55010 RVA: 0x004D5378 File Offset: 0x004D3578
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

		// Token: 0x0600D6E3 RID: 55011 RVA: 0x004D5444 File Offset: 0x004D3644
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnGameSaveDirectoryReady()
		{
			this.m_sizeTracker = new SizeTracker(this.m_maxSizeBytes, 0L, new SizeTrackerGetRemainingQuotaAsync(this.GetRemainingQuotaAsync), false);
			this.m_sizeTracker.RefreshSync();
			this.m_rootSaveStorageContainer = new SaveStorageStorageContainerFiles(this.m_gameSaveDirectory, "root");
			this.m_statusChanged(ESaveGameProviderStatus.Ok);
		}

		// Token: 0x0400A388 RID: 41864
		[PublicizedFrom(EAccessModifier.Private)]
		public XblPlatformApi m_api;

		// Token: 0x0400A389 RID: 41865
		[PublicizedFrom(EAccessModifier.Private)]
		public User m_userClient;

		// Token: 0x0400A38A RID: 41866
		[PublicizedFrom(EAccessModifier.Private)]
		public SingleThreadTaskScheduler m_taskScheduler;

		// Token: 0x0400A38B RID: 41867
		[PublicizedFrom(EAccessModifier.Private)]
		public OnGameSaveProviderStatusChanged m_statusChanged;

		// Token: 0x0400A38C RID: 41868
		[PublicizedFrom(EAccessModifier.Private)]
		public string m_gameSaveDirectory;

		// Token: 0x0400A38D RID: 41869
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_maxSizeBytes;

		// Token: 0x0400A38E RID: 41870
		[PublicizedFrom(EAccessModifier.Private)]
		public SizeTracker m_sizeTracker;

		// Token: 0x0400A38F RID: 41871
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveStorageStorageContainerFiles m_rootSaveStorageContainer;

		// Token: 0x0400A390 RID: 41872
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_isDisposed;
	}
}
