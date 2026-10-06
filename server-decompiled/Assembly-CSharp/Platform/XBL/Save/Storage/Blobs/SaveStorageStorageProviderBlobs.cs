using System;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL.Save.Storage.Blobs
{
	// Token: 0x02001C92 RID: 7314
	public sealed class SaveStorageStorageProviderBlobs : ISaveStorageProvider, IDisposable
	{
		// Token: 0x0600D897 RID: 55447 RVA: 0x004DB6B2 File Offset: 0x004D98B2
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string text)
		{
			Log.Out("[XBL: SaveStorageStorageProviderBlobs] " + text);
		}

		// Token: 0x0600D898 RID: 55448 RVA: 0x004DB6C4 File Offset: 0x004D98C4
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string text)
		{
			Log.Error("[XBL: SaveStorageStorageProviderBlobs] " + text);
		}

		// Token: 0x0600D899 RID: 55449 RVA: 0x004DB6D8 File Offset: 0x004D98D8
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

		// Token: 0x17001B4D RID: 6989
		// (get) Token: 0x0600D89A RID: 55450 RVA: 0x004DB764 File Offset: 0x004D9964
		public SizeTracker SizeTracker
		{
			get
			{
				return this.m_sizeTracker;
			}
		}

		// Token: 0x17001B4E RID: 6990
		// (get) Token: 0x0600D89B RID: 55451 RVA: 0x004DB76C File Offset: 0x004D996C
		public ISaveStorageContainer RootSaveStorageContainer
		{
			get
			{
				return this.m_rootSaveStorageContainer;
			}
		}

		// Token: 0x0600D89C RID: 55452 RVA: 0x004DB774 File Offset: 0x004D9974
		public void InitializeAsync(IPlatform owner, long maxSizeBytes, SingleThreadTaskScheduler taskScheduler, OnGameSaveProviderStatusChanged statusChanged)
		{
			this.m_api = (XblPlatformApi)owner.Api;
			this.m_userClient = (User)owner.User;
			this.m_maxSizeBytes = maxSizeBytes;
			this.m_taskScheduler = taskScheduler;
			this.m_statusChanged = statusChanged;
			SDK.XGameSaveInitializeProviderAsync(this.m_userClient.UserHandle, this.m_api.SCID, false, new XGameSaveInitializeProviderCompleted(this.OnXGameSaveInitializeProviderCompleted));
		}

		// Token: 0x0600D89D RID: 55453 RVA: 0x004DB7E2 File Offset: 0x004D99E2
		public void Flush(bool waitForFlush)
		{
			SaveStorageStorageContainerBlobs rootSaveStorageContainer = this.m_rootSaveStorageContainer;
			if (rootSaveStorageContainer == null)
			{
				return;
			}
			rootSaveStorageContainer.Flush(waitForFlush);
		}

		// Token: 0x0600D89E RID: 55454 RVA: 0x004DB7F8 File Offset: 0x004D99F8
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

		// Token: 0x0600D89F RID: 55455 RVA: 0x004DB838 File Offset: 0x004D9A38
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

		// Token: 0x0600D8A0 RID: 55456 RVA: 0x004DB8F8 File Offset: 0x004D9AF8
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

		// Token: 0x0400A49D RID: 42141
		[PublicizedFrom(EAccessModifier.Private)]
		public XblPlatformApi m_api;

		// Token: 0x0400A49E RID: 42142
		[PublicizedFrom(EAccessModifier.Private)]
		public User m_userClient;

		// Token: 0x0400A49F RID: 42143
		[PublicizedFrom(EAccessModifier.Private)]
		public SingleThreadTaskScheduler m_taskScheduler;

		// Token: 0x0400A4A0 RID: 42144
		[PublicizedFrom(EAccessModifier.Private)]
		public OnGameSaveProviderStatusChanged m_statusChanged;

		// Token: 0x0400A4A1 RID: 42145
		[PublicizedFrom(EAccessModifier.Private)]
		public XGameSaveProviderHandle m_gameSaveProviderHandle;

		// Token: 0x0400A4A2 RID: 42146
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_maxSizeBytes;

		// Token: 0x0400A4A3 RID: 42147
		[PublicizedFrom(EAccessModifier.Private)]
		public SizeTracker m_sizeTracker;

		// Token: 0x0400A4A4 RID: 42148
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveStorageStorageContainerBlobs m_rootSaveStorageContainer;

		// Token: 0x0400A4A5 RID: 42149
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_isDisposed;
	}
}
