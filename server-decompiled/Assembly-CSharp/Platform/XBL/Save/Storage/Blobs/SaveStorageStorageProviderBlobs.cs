using System;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL.Save.Storage.Blobs
{
	// Token: 0x02001CC3 RID: 7363
	public sealed class SaveStorageStorageProviderBlobs : ISaveStorageProvider, IDisposable
	{
		// Token: 0x0600D9AB RID: 55723 RVA: 0x004DF202 File Offset: 0x004DD402
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string text)
		{
			Log.Out("[XBL: SaveStorageStorageProviderBlobs] " + text);
		}

		// Token: 0x0600D9AC RID: 55724 RVA: 0x004DF214 File Offset: 0x004DD414
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string text)
		{
			Log.Error("[XBL: SaveStorageStorageProviderBlobs] " + text);
		}

		// Token: 0x0600D9AD RID: 55725 RVA: 0x004DF228 File Offset: 0x004DD428
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

		// Token: 0x17001B95 RID: 7061
		// (get) Token: 0x0600D9AE RID: 55726 RVA: 0x004DF2B4 File Offset: 0x004DD4B4
		public SizeTracker SizeTracker
		{
			get
			{
				return this.m_sizeTracker;
			}
		}

		// Token: 0x17001B96 RID: 7062
		// (get) Token: 0x0600D9AF RID: 55727 RVA: 0x004DF2BC File Offset: 0x004DD4BC
		public ISaveStorageContainer RootSaveStorageContainer
		{
			get
			{
				return this.m_rootSaveStorageContainer;
			}
		}

		// Token: 0x0600D9B0 RID: 55728 RVA: 0x004DF2C4 File Offset: 0x004DD4C4
		public void InitializeAsync(IPlatform owner, long maxSizeBytes, SingleThreadTaskScheduler taskScheduler, OnGameSaveProviderStatusChanged statusChanged)
		{
			this.m_api = (XblPlatformApi)owner.Api;
			this.m_userClient = (User)owner.User;
			this.m_maxSizeBytes = maxSizeBytes;
			this.m_taskScheduler = taskScheduler;
			this.m_statusChanged = statusChanged;
			SDK.XGameSaveInitializeProviderAsync(this.m_userClient.UserHandle, this.m_api.SCID, false, new XGameSaveInitializeProviderCompleted(this.OnXGameSaveInitializeProviderCompleted));
		}

		// Token: 0x0600D9B1 RID: 55729 RVA: 0x004DF332 File Offset: 0x004DD532
		public void Flush(bool waitForFlush)
		{
			SaveStorageStorageContainerBlobs rootSaveStorageContainer = this.m_rootSaveStorageContainer;
			if (rootSaveStorageContainer == null)
			{
				return;
			}
			rootSaveStorageContainer.Flush(waitForFlush);
		}

		// Token: 0x0600D9B2 RID: 55730 RVA: 0x004DF348 File Offset: 0x004DD548
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

		// Token: 0x0600D9B3 RID: 55731 RVA: 0x004DF388 File Offset: 0x004DD588
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

		// Token: 0x0600D9B4 RID: 55732 RVA: 0x004DF448 File Offset: 0x004DD648
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

		// Token: 0x0400A586 RID: 42374
		[PublicizedFrom(EAccessModifier.Private)]
		public XblPlatformApi m_api;

		// Token: 0x0400A587 RID: 42375
		[PublicizedFrom(EAccessModifier.Private)]
		public User m_userClient;

		// Token: 0x0400A588 RID: 42376
		[PublicizedFrom(EAccessModifier.Private)]
		public SingleThreadTaskScheduler m_taskScheduler;

		// Token: 0x0400A589 RID: 42377
		[PublicizedFrom(EAccessModifier.Private)]
		public OnGameSaveProviderStatusChanged m_statusChanged;

		// Token: 0x0400A58A RID: 42378
		[PublicizedFrom(EAccessModifier.Private)]
		public XGameSaveProviderHandle m_gameSaveProviderHandle;

		// Token: 0x0400A58B RID: 42379
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_maxSizeBytes;

		// Token: 0x0400A58C RID: 42380
		[PublicizedFrom(EAccessModifier.Private)]
		public SizeTracker m_sizeTracker;

		// Token: 0x0400A58D RID: 42381
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveStorageStorageContainerBlobs m_rootSaveStorageContainer;

		// Token: 0x0400A58E RID: 42382
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_isDisposed;
	}
}
