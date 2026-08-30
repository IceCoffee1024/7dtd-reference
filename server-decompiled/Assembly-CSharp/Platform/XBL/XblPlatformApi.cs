using System;
using System.Threading;
using Unity.Profiling;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;
using UnityEngine;
using UnityEngine.Profiling;

namespace Platform.XBL
{
	// Token: 0x02001BF7 RID: 7159
	public abstract class XblPlatformApi : IPlatformApi
	{
		// Token: 0x17001A1B RID: 6683
		// (get) Token: 0x0600D43A RID: 54330
		public abstract string SCID { get; }

		// Token: 0x17001A1C RID: 6684
		// (get) Token: 0x0600D43B RID: 54331 RVA: 0x004C91E6 File Offset: 0x004C73E6
		// (set) Token: 0x0600D43C RID: 54332 RVA: 0x004C91EE File Offset: 0x004C73EE
		public EApiStatus ClientApiStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EApiStatus.Uninitialized;

		// Token: 0x1400012F RID: 303
		// (add) Token: 0x0600D43D RID: 54333 RVA: 0x004C91F8 File Offset: 0x004C73F8
		// (remove) Token: 0x0600D43E RID: 54334 RVA: 0x004C9254 File Offset: 0x004C7454
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

		// Token: 0x0600D43F RID: 54335
		public abstract void Init(IPlatform owner);

		// Token: 0x0600D440 RID: 54336 RVA: 0x004C92A0 File Offset: 0x004C74A0
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

		// Token: 0x0600D441 RID: 54337
		public abstract bool InitServerApis();

		// Token: 0x0600D442 RID: 54338
		public abstract void ServerApiLoaded();

		// Token: 0x0600D443 RID: 54339 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600D444 RID: 54340 RVA: 0x004C938C File Offset: 0x004C758C
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

		// Token: 0x0600D445 RID: 54341 RVA: 0x00040D24 File Offset: 0x0003EF24
		public float GetScreenBoundsValueFromSystem()
		{
			return 1f;
		}

		// Token: 0x0600D446 RID: 54342 RVA: 0x004C9415 File Offset: 0x004C7615
		[PublicizedFrom(EAccessModifier.Protected)]
		public XblPlatformApi()
		{
		}

		// Token: 0x0400A1B0 RID: 41392
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ProfilerMarker s_TaskQueueDispatchMarker = new ProfilerMarker("XTaskQueueDispatch");

		// Token: 0x0400A1B1 RID: 41393
		[PublicizedFrom(EAccessModifier.Private)]
		public Thread m_dispatchGXDKTaskQueueThread;

		// Token: 0x0400A1B2 RID: 41394
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_dispatchGXDKTaskQueueRunning;

		// Token: 0x0400A1B3 RID: 41395
		[PublicizedFrom(EAccessModifier.Private)]
		public Action m_clientApiInitialized;
	}
}
