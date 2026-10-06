using System;
using System.Threading;
using Unity.Profiling;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;
using UnityEngine;
using UnityEngine.Profiling;

namespace Platform.XBL
{
	// Token: 0x02001C50 RID: 7248
	public abstract class XblPlatformApi : IPlatformApi
	{
		// Token: 0x17001B33 RID: 6963
		// (get) Token: 0x0600D740 RID: 55104
		public abstract string SCID { get; }

		// Token: 0x17001B34 RID: 6964
		// (get) Token: 0x0600D741 RID: 55105 RVA: 0x004D40C2 File Offset: 0x004D22C2
		// (set) Token: 0x0600D742 RID: 55106 RVA: 0x004D40CA File Offset: 0x004D22CA
		public EApiStatus ClientApiStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EApiStatus.Uninitialized;

		// Token: 0x1400012F RID: 303
		// (add) Token: 0x0600D743 RID: 55107 RVA: 0x004D40D4 File Offset: 0x004D22D4
		// (remove) Token: 0x0600D744 RID: 55108 RVA: 0x004D4130 File Offset: 0x004D2330
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

		// Token: 0x0600D745 RID: 55109
		public abstract void Init(IPlatform owner);

		// Token: 0x0600D746 RID: 55110 RVA: 0x004D417C File Offset: 0x004D237C
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

		// Token: 0x0600D747 RID: 55111
		public abstract bool InitServerApis();

		// Token: 0x0600D748 RID: 55112
		public abstract void ServerApiLoaded();

		// Token: 0x0600D749 RID: 55113 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600D74A RID: 55114 RVA: 0x004D4268 File Offset: 0x004D2468
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

		// Token: 0x0600D74B RID: 55115 RVA: 0x00041064 File Offset: 0x0003F264
		public float GetScreenBoundsValueFromSystem()
		{
			return 1f;
		}

		// Token: 0x0600D74C RID: 55116 RVA: 0x004D42F1 File Offset: 0x004D24F1
		[PublicizedFrom(EAccessModifier.Protected)]
		public XblPlatformApi()
		{
		}

		// Token: 0x0400A3B6 RID: 41910
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ProfilerMarker s_TaskQueueDispatchMarker = new ProfilerMarker("XTaskQueueDispatch");

		// Token: 0x0400A3B7 RID: 41911
		[PublicizedFrom(EAccessModifier.Private)]
		public Thread m_dispatchGXDKTaskQueueThread;

		// Token: 0x0400A3B8 RID: 41912
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_dispatchGXDKTaskQueueRunning;

		// Token: 0x0400A3B9 RID: 41913
		[PublicizedFrom(EAccessModifier.Private)]
		public Action m_clientApiInitialized;
	}
}
