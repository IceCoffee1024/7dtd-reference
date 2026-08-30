using System;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL.Save.Storage.Blobs
{
	// Token: 0x02001C60 RID: 7264
	public sealed class SaveStorageStorageProviderBlobs : ISaveStorageProvider, IDisposable
	{
		// Token: 0x0600D71E RID: 55070 RVA: 0x004D6B76 File Offset: 0x004D4D76
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string text)
		{
			Log.Out("[XBL: SaveStorageStorageProviderBlobs] " + text);
		}

		// Token: 0x0600D71F RID: 55071 RVA: 0x004D6B88 File Offset: 0x004D4D88
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string text)
		{
			Log.Error("[XBL: SaveStorageStorageProviderBlobs] " + text);
		}

		// Token: 0x0600D720 RID: 55072 RVA: 0x004D6B9C File Offset: 0x004D4D9C
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

		// Token: 0x17001AA7 RID: 6823
		// (get) Token: 0x0600D721 RID: 55073 RVA: 0x004D6C28 File Offset: 0x004D4E28
		public SizeTracker SizeTracker
		{
			get
			{
				return this.m_sizeTracker;
			}
		}

		// Token: 0x17001AA8 RID: 6824
		// (get) Token: 0x0600D722 RID: 55074 RVA: 0x004D6C30 File Offset: 0x004D4E30
		public ISaveStorageContainer RootSaveStorageContainer
		{
			get
			{
				return this.m_rootSaveStorageContainer;
			}
		}

		// Token: 0x0600D723 RID: 55075 RVA: 0x004D6C38 File Offset: 0x004D4E38
		public void InitializeAsync(IPlatform owner, long maxSizeBytes, SingleThreadTaskScheduler taskScheduler, OnGameSaveProviderStatusChanged statusChanged)
		{
			this.m_api = (XblPlatformApi)owner.Api;
			this.m_userClient = (User)owner.User;
			this.m_maxSizeBytes = maxSizeBytes;
			this.m_taskScheduler = taskScheduler;
			this.m_statusChanged = statusChanged;
			SDK.XGameSaveInitializeProviderAsync(this.m_userClient.UserHandle, this.m_api.SCID, false, new XGameSaveInitializeProviderCompleted(this.OnXGameSaveInitializeProviderCompleted));
		}

		// Token: 0x0600D724 RID: 55076 RVA: 0x004D6CA6 File Offset: 0x004D4EA6
		public void Flush(bool waitForFlush)
		{
			SaveStorageStorageContainerBlobs rootSaveStorageContainer = this.m_rootSaveStorageContainer;
			if (rootSaveStorageContainer == null)
			{
				return;
			}
			rootSaveStorageContainer.Flush(waitForFlush);
		}

		// Token: 0x0600D725 RID: 55077 RVA: 0x004D6CBC File Offset: 0x004D4EBC
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

		// Token: 0x0600D726 RID: 55078 RVA: 0x004D6CFC File Offset: 0x004D4EFC
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

		// Token: 0x0600D727 RID: 55079 RVA: 0x004D6DBC File Offset: 0x004D4FBC
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

		// Token: 0x0400A3C8 RID: 41928
		[PublicizedFrom(EAccessModifier.Private)]
		public XblPlatformApi m_api;

		// Token: 0x0400A3C9 RID: 41929
		[PublicizedFrom(EAccessModifier.Private)]
		public User m_userClient;

		// Token: 0x0400A3CA RID: 41930
		[PublicizedFrom(EAccessModifier.Private)]
		public SingleThreadTaskScheduler m_taskScheduler;

		// Token: 0x0400A3CB RID: 41931
		[PublicizedFrom(EAccessModifier.Private)]
		public OnGameSaveProviderStatusChanged m_statusChanged;

		// Token: 0x0400A3CC RID: 41932
		[PublicizedFrom(EAccessModifier.Private)]
		public XGameSaveProviderHandle m_gameSaveProviderHandle;

		// Token: 0x0400A3CD RID: 41933
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_maxSizeBytes;

		// Token: 0x0400A3CE RID: 41934
		[PublicizedFrom(EAccessModifier.Private)]
		public SizeTracker m_sizeTracker;

		// Token: 0x0400A3CF RID: 41935
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveStorageStorageContainerBlobs m_rootSaveStorageContainer;

		// Token: 0x0400A3D0 RID: 41936
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_isDisposed;
	}
}
