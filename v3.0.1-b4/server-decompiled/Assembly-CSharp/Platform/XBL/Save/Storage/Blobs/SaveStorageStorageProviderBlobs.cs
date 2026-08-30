using System;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL.Save.Storage.Blobs
{
	// Token: 0x02001C39 RID: 7225
	public sealed class SaveStorageStorageProviderBlobs : ISaveStorageProvider, IDisposable
	{
		// Token: 0x0600D58E RID: 54670 RVA: 0x004D0752 File Offset: 0x004CE952
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string text)
		{
			Log.Out("[XBL: SaveStorageStorageProviderBlobs] " + text);
		}

		// Token: 0x0600D58F RID: 54671 RVA: 0x004D0764 File Offset: 0x004CE964
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string text)
		{
			Log.Error("[XBL: SaveStorageStorageProviderBlobs] " + text);
		}

		// Token: 0x0600D590 RID: 54672 RVA: 0x004D0778 File Offset: 0x004CE978
		public void Dispose()
		{
			SaveStorageStorageContainerBlobs rootSaveStorageContainer = this.m_rootSaveStorageContainer;
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
			if (this.m_gameSaveProviderHandle != null)
			{
				SDK.XGameSaveCloseProvider(this.m_gameSaveProviderHandle);
				XblHelpers.LogHR(0, "Uninitialize Game Saves.", false);
				this.m_gameSaveProviderHandle = null;
			}
			this.m_statusChanged = null;
			this.m_taskScheduler = null;
			this.m_userClient = null;
			this.m_api = null;
			this.m_isDisposed = true;
		}

		// Token: 0x17001A35 RID: 6709
		// (get) Token: 0x0600D591 RID: 54673 RVA: 0x004D0804 File Offset: 0x004CEA04
		public SizeTracker SizeTracker
		{
			get
			{
				return this.m_sizeTracker;
			}
		}

		// Token: 0x17001A36 RID: 6710
		// (get) Token: 0x0600D592 RID: 54674 RVA: 0x004D080C File Offset: 0x004CEA0C
		public ISaveStorageContainer RootSaveStorageContainer
		{
			get
			{
				return this.m_rootSaveStorageContainer;
			}
		}

		// Token: 0x0600D593 RID: 54675 RVA: 0x004D0814 File Offset: 0x004CEA14
		public void InitializeAsync(IPlatform owner, long maxSizeBytes, SingleThreadTaskScheduler taskScheduler, OnGameSaveProviderStatusChanged statusChanged)
		{
			this.m_api = (XblPlatformApi)owner.Api;
			this.m_userClient = (User)owner.User;
			this.m_maxSizeBytes = maxSizeBytes;
			this.m_taskScheduler = taskScheduler;
			this.m_statusChanged = statusChanged;
			SDK.XGameSaveInitializeProviderAsync(this.m_userClient.UserHandle, this.m_api.SCID, false, new XGameSaveInitializeProviderCompleted(this.OnXGameSaveInitializeProviderCompleted));
		}

		// Token: 0x0600D594 RID: 54676 RVA: 0x004D0882 File Offset: 0x004CEA82
		public void Flush(bool waitForFlush)
		{
			SaveStorageStorageContainerBlobs rootSaveStorageContainer = this.m_rootSaveStorageContainer;
			if (rootSaveStorageContainer == null)
			{
				return;
			}
			rootSaveStorageContainer.Flush(waitForFlush);
		}

		// Token: 0x0600D595 RID: 54677 RVA: 0x004D0898 File Offset: 0x004CEA98
		[PublicizedFrom(EAccessModifier.Private)]
		public void GetRemainingQuotaAsync(SizeTrackerGetRemainingQuotaCompleted completionRoutine)
		{
			SingleThreadTaskScheduler taskScheduler = this.m_taskScheduler;
			if (taskScheduler == null)
			{
				return;
			}
			XGameSaveGetRemainingQuotaCompleted <>9__1;
			taskScheduler.ExecuteNoWait(delegate
			{
				XGameSaveProviderHandle gameSaveProviderHandle = this.m_gameSaveProviderHandle;
				XGameSaveGetRemainingQuotaCompleted xgameSaveGetRemainingQuotaCompleted;
				if ((xgameSaveGetRemainingQuotaCompleted = <>9__1) == null)
				{
					xgameSaveGetRemainingQuotaCompleted = (<>9__1 = delegate(int hr, long remaining)
					{
						SingleThreadTaskScheduler taskScheduler2 = this.m_taskScheduler;
						if (taskScheduler2 == null)
						{
							return;
						}
						taskScheduler2.ExecuteNoWait(delegate
						{
							if (this.m_rootSaveStorageContainer != null)
							{
								remaining = Math.Max(0L, remaining - (long)this.m_rootSaveStorageContainer.GetQueuedUsed());
							}
							completionRoutine(hr, remaining);
						});
					});
				}
				SDK.XGameSaveGetRemainingQuotaAsync(gameSaveProviderHandle, xgameSaveGetRemainingQuotaCompleted);
			});
		}

		// Token: 0x0600D596 RID: 54678 RVA: 0x004D08D8 File Offset: 0x004CEAD8
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnXGameSaveInitializeProviderCompleted(int hr, XGameSaveProviderHandle gameSaveProviderHandle)
		{
			XblHelpers.LogHR(hr, "Initialize Game Saves (Blobs).", false);
			if (this.m_isDisposed)
			{
				SaveStorageStorageProviderBlobs.LogInfo("Disposed before XGameSaveInitializeProviderAsync completed, cancelling setup");
				if (Unity.XGamingRuntime.Interop.HR.SUCCEEDED(hr))
				{
					SDK.XGameSaveCloseProvider(gameSaveProviderHandle);
				}
				return;
			}
			if (!Unity.XGamingRuntime.Interop.HR.FAILED(hr))
			{
				this.m_gameSaveProviderHandle = gameSaveProviderHandle;
				this.m_taskScheduler.ExecuteNoWait(new Action(this.OnXGameSaveProviderHandleReady));
				return;
			}
			if (hr == -2138898428)
			{
				this.m_statusChanged(ESaveGameProviderStatus.TemporaryError);
				SaveStorageStorageProviderBlobs.LogInfo("User cancelled sync, retrying...");
				SDK.XGameSaveInitializeProviderAsync(this.m_userClient.UserHandle, this.m_api.SCID, false, new XGameSaveInitializeProviderCompleted(this.OnXGameSaveInitializeProviderCompleted));
				return;
			}
			SaveStorageStorageProviderBlobs.LogError("Could not Initialize. This is a FATAL error!");
			this.m_statusChanged(ESaveGameProviderStatus.PermanentError);
		}

		// Token: 0x0600D597 RID: 54679 RVA: 0x004D0998 File Offset: 0x004CEB98
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnXGameSaveProviderHandleReady()
		{
			ulong num = 0UL;
			XGameSaveContainerInfo[] array;
			if (Unity.XGamingRuntime.Interop.HR.SUCCEEDED(SDK.XGameSaveEnumerateContainerInfo(this.m_gameSaveProviderHandle, out array)))
			{
				foreach (XGameSaveContainerInfo xgameSaveContainerInfo in array)
				{
					SaveStorageStorageProviderBlobs.LogInfo(string.Format("Container '{0}' (display name '{1}') has {2} blobs. NeedsSync:{3} TotalSize:{4} LastModifiedTime:{5}", new object[] { xgameSaveContainerInfo.Name, xgameSaveContainerInfo.DisplayName, xgameSaveContainerInfo.BlobCount, xgameSaveContainerInfo.NeedsSync, xgameSaveContainerInfo.TotalSize, xgameSaveContainerInfo.LastModifiedTime }));
					num += xgameSaveContainerInfo.TotalSize;
				}
			}
			this.m_sizeTracker = new SizeTracker(this.m_maxSizeBytes, (long)num, new SizeTrackerGetRemainingQuotaAsync(this.GetRemainingQuotaAsync), true);
			this.m_sizeTracker.RefreshAsync();
			this.m_rootSaveStorageContainer = new SaveStorageStorageContainerBlobs("root", this.m_gameSaveProviderHandle, this.m_taskScheduler);
			this.m_statusChanged(ESaveGameProviderStatus.Ok);
		}

		// Token: 0x0400A297 RID: 41623
		[PublicizedFrom(EAccessModifier.Private)]
		public XblPlatformApi m_api;

		// Token: 0x0400A298 RID: 41624
		[PublicizedFrom(EAccessModifier.Private)]
		public User m_userClient;

		// Token: 0x0400A299 RID: 41625
		[PublicizedFrom(EAccessModifier.Private)]
		public SingleThreadTaskScheduler m_taskScheduler;

		// Token: 0x0400A29A RID: 41626
		[PublicizedFrom(EAccessModifier.Private)]
		public OnGameSaveProviderStatusChanged m_statusChanged;

		// Token: 0x0400A29B RID: 41627
		[PublicizedFrom(EAccessModifier.Private)]
		public XGameSaveProviderHandle m_gameSaveProviderHandle;

		// Token: 0x0400A29C RID: 41628
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_maxSizeBytes;

		// Token: 0x0400A29D RID: 41629
		[PublicizedFrom(EAccessModifier.Private)]
		public SizeTracker m_sizeTracker;

		// Token: 0x0400A29E RID: 41630
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveStorageStorageContainerBlobs m_rootSaveStorageContainer;

		// Token: 0x0400A29F RID: 41631
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_isDisposed;
	}
}
