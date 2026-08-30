using System;
using System.Threading;
using Unity.Profiling;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;
using UnityEngine;
using UnityEngine.Profiling;

namespace Platform.XBL
{
	// Token: 0x02001C1E RID: 7198
	public abstract class XblPlatformApi : IPlatformApi
	{
		// Token: 0x17001A8D RID: 6797
		// (get) Token: 0x0600D5C8 RID: 54728
		public abstract string SCID { get; }

		// Token: 0x17001A8E RID: 6798
		// (get) Token: 0x0600D5C9 RID: 54729 RVA: 0x004CF5DA File Offset: 0x004CD7DA
		// (set) Token: 0x0600D5CA RID: 54730 RVA: 0x004CF5E2 File Offset: 0x004CD7E2
		public EApiStatus ClientApiStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EApiStatus.Uninitialized;

		// Token: 0x14000130 RID: 304
		// (add) Token: 0x0600D5CB RID: 54731 RVA: 0x004CF5EC File Offset: 0x004CD7EC
		// (remove) Token: 0x0600D5CC RID: 54732 RVA: 0x004CF648 File Offset: 0x004CD848
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

		// Token: 0x0600D5CD RID: 54733
		public abstract void Init(IPlatform owner);

		// Token: 0x0600D5CE RID: 54734 RVA: 0x004CF694 File Offset: 0x004CD894
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

		// Token: 0x0600D5CF RID: 54735
		public abstract bool InitServerApis();

		// Token: 0x0600D5D0 RID: 54736
		public abstract void ServerApiLoaded();

		// Token: 0x0600D5D1 RID: 54737 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600D5D2 RID: 54738 RVA: 0x004CF780 File Offset: 0x004CD980
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

		// Token: 0x0600D5D3 RID: 54739 RVA: 0x00040F84 File Offset: 0x0003F184
		public float GetScreenBoundsValueFromSystem()
		{
			return 1f;
		}

		// Token: 0x0600D5D4 RID: 54740 RVA: 0x004CF809 File Offset: 0x004CDA09
		[PublicizedFrom(EAccessModifier.Protected)]
		public XblPlatformApi()
		{
		}

		// Token: 0x0400A2E1 RID: 41697
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ProfilerMarker s_TaskQueueDispatchMarker = new ProfilerMarker("XTaskQueueDispatch");

		// Token: 0x0400A2E2 RID: 41698
		[PublicizedFrom(EAccessModifier.Private)]
		public Thread m_dispatchGXDKTaskQueueThread;

		// Token: 0x0400A2E3 RID: 41699
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_dispatchGXDKTaskQueueRunning;

		// Token: 0x0400A2E4 RID: 41700
		[PublicizedFrom(EAccessModifier.Private)]
		public Action m_clientApiInitialized;
	}
}
