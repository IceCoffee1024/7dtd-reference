using System;
using System.Threading;
using Unity.Profiling;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;
using UnityEngine;
using UnityEngine.Profiling;

namespace Platform.XBL
{
	// Token: 0x02001C81 RID: 7297
	public abstract class XblPlatformApi : IPlatformApi
	{
		// Token: 0x17001B7B RID: 7035
		// (get) Token: 0x0600D854 RID: 55380
		public abstract string SCID { get; }

		// Token: 0x17001B7C RID: 7036
		// (get) Token: 0x0600D855 RID: 55381 RVA: 0x004D7BEA File Offset: 0x004D5DEA
		// (set) Token: 0x0600D856 RID: 55382 RVA: 0x004D7BF2 File Offset: 0x004D5DF2
		public EApiStatus ClientApiStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EApiStatus.Uninitialized;

		// Token: 0x1400012F RID: 303
		// (add) Token: 0x0600D857 RID: 55383 RVA: 0x004D7BFC File Offset: 0x004D5DFC
		// (remove) Token: 0x0600D858 RID: 55384 RVA: 0x004D7C58 File Offset: 0x004D5E58
		public event Action ClientApiInitialized
		{
			add
			{
				lock (this)
				{
					this.m_clientApiInitialized = (Action)Delegate.Combine(this.m_clientApiInitialized, value);
					if (this.ClientApiStatus == EApiStatus.Ok)
					{
						value();
					}
				}
			}
			remove
			{
				lock (this)
				{
					this.m_clientApiInitialized = (Action)Delegate.Remove(this.m_clientApiInitialized, value);
				}
			}
		}

		// Token: 0x0600D859 RID: 55385
		public abstract void Init(IPlatform owner);

		// Token: 0x0600D85A RID: 55386 RVA: 0x004D7CA4 File Offset: 0x004D5EA4
		public bool InitClientApis()
		{
			if (this.ClientApiStatus == EApiStatus.Ok)
			{
				return true;
			}
			if (Application.isEditor)
			{
				this.ClientApiStatus = EApiStatus.Ok;
				Action clientApiInitialized = this.m_clientApiInitialized;
				if (clientApiInitialized != null)
				{
					clientApiInitialized();
				}
				return true;
			}
			int num = SDK.XGameRuntimeInitialize();
			XblHelpers.LogHR(num, "Initialize Gaming Runtime.", false);
			if (Unity.XGamingRuntime.Interop.HR.FAILED(num))
			{
				this.ClientApiStatus = EApiStatus.PermanentError;
				return false;
			}
			if (Unity.XGamingRuntime.Interop.HR.FAILED(SDK.CreateDefaultTaskQueue()))
			{
				this.ClientApiStatus = EApiStatus.PermanentError;
				return false;
			}
			this.m_dispatchGXDKTaskQueueRunning = true;
			this.m_dispatchGXDKTaskQueueThread = new Thread(delegate
			{
				Log.Out("[XBL] Started Thread: " + Thread.CurrentThread.Name);
				try
				{
					while (this.m_dispatchGXDKTaskQueueRunning)
					{
						try
						{
							SDK.XTaskQueueDispatch(32U);
						}
						catch (ThreadInterruptedException)
						{
							break;
						}
						catch (Exception ex)
						{
							Log.Exception(ex);
						}
						finally
						{
						}
					}
				}
				finally
				{
					Profiler.EndThreadProfiling();
					Log.Out("[XBL] Stopped Thread: " + Thread.CurrentThread.Name);
				}
			})
			{
				Name = "GXDK Task Queue Dispatch Completion",
				IsBackground = true
			};
			this.m_dispatchGXDKTaskQueueThread.Start();
			int num2 = SDK.XBL.XblInitialize(this.SCID);
			XblHelpers.LogHR(num2, "Initialize Xbox Live.", false);
			if (Unity.XGamingRuntime.Interop.HR.FAILED(num2))
			{
				return false;
			}
			Log.Out("[XBL] API loaded.");
			this.ClientApiStatus = EApiStatus.Ok;
			Action clientApiInitialized2 = this.m_clientApiInitialized;
			if (clientApiInitialized2 != null)
			{
				clientApiInitialized2();
			}
			return true;
		}

		// Token: 0x0600D85B RID: 55387
		public abstract bool InitServerApis();

		// Token: 0x0600D85C RID: 55388
		public abstract void ServerApiLoaded();

		// Token: 0x0600D85D RID: 55389 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600D85E RID: 55390 RVA: 0x004D7D90 File Offset: 0x004D5F90
		public void Destroy()
		{
			if (Application.isEditor)
			{
				return;
			}
			this.m_dispatchGXDKTaskQueueRunning = false;
			SDK.CloseDefaultXTaskQueue();
			SDK.XBL.XblCleanup(delegate(int hr)
			{
				XblHelpers.LogHR(hr, "Uninitialize Xbox Live.", false);
			});
			Thread dispatchGXDKTaskQueueThread = this.m_dispatchGXDKTaskQueueThread;
			if (dispatchGXDKTaskQueueThread != null)
			{
				dispatchGXDKTaskQueueThread.Interrupt();
			}
			Thread dispatchGXDKTaskQueueThread2 = this.m_dispatchGXDKTaskQueueThread;
			if (dispatchGXDKTaskQueueThread2 != null)
			{
				dispatchGXDKTaskQueueThread2.Join();
			}
			this.m_dispatchGXDKTaskQueueThread = null;
			SDK.XGameRuntimeUninitialize();
			XblHelpers.LogHR(0, "Uninitialize Gaming Runtime.", false);
			Log.Out("[XBL] API Destroyed.");
		}

		// Token: 0x0600D85F RID: 55391 RVA: 0x00041024 File Offset: 0x0003F224
		public float GetScreenBoundsValueFromSystem()
		{
			return 1f;
		}

		// Token: 0x0600D860 RID: 55392 RVA: 0x004D7E19 File Offset: 0x004D6019
		[PublicizedFrom(EAccessModifier.Protected)]
		public XblPlatformApi()
		{
		}

		// Token: 0x0400A49F RID: 42143
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ProfilerMarker s_TaskQueueDispatchMarker = new ProfilerMarker("XTaskQueueDispatch");

		// Token: 0x0400A4A0 RID: 42144
		[PublicizedFrom(EAccessModifier.Private)]
		public Thread m_dispatchGXDKTaskQueueThread;

		// Token: 0x0400A4A1 RID: 42145
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_dispatchGXDKTaskQueueRunning;

		// Token: 0x0400A4A2 RID: 42146
		[PublicizedFrom(EAccessModifier.Private)]
		public Action m_clientApiInitialized;
	}
}
