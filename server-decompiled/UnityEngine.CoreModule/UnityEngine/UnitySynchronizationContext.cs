using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200026C RID: 620
	internal sealed class UnitySynchronizationContext : SynchronizationContext
	{
		// Token: 0x06001A11 RID: 6673 RVA: 0x0002BE11 File Offset: 0x0002A011
		private UnitySynchronizationContext(int mainThreadID)
		{
			this.m_AsyncWorkQueue = new List<UnitySynchronizationContext.WorkRequest>(20);
			this.m_MainThreadID = mainThreadID;
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x0002BE43 File Offset: 0x0002A043
		private UnitySynchronizationContext(List<UnitySynchronizationContext.WorkRequest> queue, int mainThreadID)
		{
			this.m_AsyncWorkQueue = queue;
			this.m_MainThreadID = mainThreadID;
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x0002BE70 File Offset: 0x0002A070
		public override void Send(SendOrPostCallback callback, object state)
		{
			bool flag = this.m_MainThreadID == Thread.CurrentThread.ManagedThreadId;
			if (flag)
			{
				callback(state);
			}
			else
			{
				using (ManualResetEvent manualResetEvent = new ManualResetEvent(false))
				{
					List<UnitySynchronizationContext.WorkRequest> asyncWorkQueue = this.m_AsyncWorkQueue;
					lock (asyncWorkQueue)
					{
						this.m_AsyncWorkQueue.Add(new UnitySynchronizationContext.WorkRequest(callback, state, manualResetEvent));
					}
					manualResetEvent.WaitOne();
				}
			}
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x0002BF10 File Offset: 0x0002A110
		public override void OperationStarted()
		{
			Interlocked.Increment(ref this.m_TrackedCount);
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x0002BF1F File Offset: 0x0002A11F
		public override void OperationCompleted()
		{
			Interlocked.Decrement(ref this.m_TrackedCount);
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x0002BF30 File Offset: 0x0002A130
		public override void Post(SendOrPostCallback callback, object state)
		{
			List<UnitySynchronizationContext.WorkRequest> asyncWorkQueue = this.m_AsyncWorkQueue;
			lock (asyncWorkQueue)
			{
				this.m_AsyncWorkQueue.Add(new UnitySynchronizationContext.WorkRequest(callback, state, null));
			}
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x0002BF84 File Offset: 0x0002A184
		public override SynchronizationContext CreateCopy()
		{
			return new UnitySynchronizationContext(this.m_AsyncWorkQueue, this.m_MainThreadID);
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x0002BFA8 File Offset: 0x0002A1A8
		public void Exec()
		{
			List<UnitySynchronizationContext.WorkRequest> asyncWorkQueue = this.m_AsyncWorkQueue;
			lock (asyncWorkQueue)
			{
				this.m_CurrentFrameWork.AddRange(this.m_AsyncWorkQueue);
				this.m_AsyncWorkQueue.Clear();
			}
			while (this.m_CurrentFrameWork.Count > 0)
			{
				UnitySynchronizationContext.WorkRequest workRequest = this.m_CurrentFrameWork[0];
				this.m_CurrentFrameWork.RemoveAt(0);
				workRequest.Invoke();
			}
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x0002C040 File Offset: 0x0002A240
		private bool HasPendingTasks()
		{
			return this.m_AsyncWorkQueue.Count != 0 || this.m_TrackedCount != 0;
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x0002C06B File Offset: 0x0002A26B
		[RequiredByNativeCode]
		private static void InitializeSynchronizationContext()
		{
			SynchronizationContext.SetSynchronizationContext(new UnitySynchronizationContext(Thread.CurrentThread.ManagedThreadId));
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x0002C084 File Offset: 0x0002A284
		[RequiredByNativeCode]
		private static void ExecuteTasks()
		{
			UnitySynchronizationContext unitySynchronizationContext = SynchronizationContext.Current as UnitySynchronizationContext;
			bool flag = unitySynchronizationContext != null;
			if (flag)
			{
				unitySynchronizationContext.Exec();
			}
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x0002C0AC File Offset: 0x0002A2AC
		[RequiredByNativeCode]
		private static bool ExecutePendingTasks(long millisecondsTimeout)
		{
			UnitySynchronizationContext unitySynchronizationContext = SynchronizationContext.Current as UnitySynchronizationContext;
			bool flag = unitySynchronizationContext == null;
			bool flag2;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				while (unitySynchronizationContext.HasPendingTasks())
				{
					bool flag3 = stopwatch.ElapsedMilliseconds > millisecondsTimeout;
					if (flag3)
					{
						break;
					}
					unitySynchronizationContext.Exec();
					Thread.Sleep(1);
				}
				flag2 = !unitySynchronizationContext.HasPendingTasks();
			}
			return flag2;
		}

		// Token: 0x0400090B RID: 2315
		private const int kAwqInitialCapacity = 20;

		// Token: 0x0400090C RID: 2316
		private readonly List<UnitySynchronizationContext.WorkRequest> m_AsyncWorkQueue;

		// Token: 0x0400090D RID: 2317
		private readonly List<UnitySynchronizationContext.WorkRequest> m_CurrentFrameWork = new List<UnitySynchronizationContext.WorkRequest>(20);

		// Token: 0x0400090E RID: 2318
		private readonly int m_MainThreadID;

		// Token: 0x0400090F RID: 2319
		private int m_TrackedCount = 0;

		// Token: 0x0200026D RID: 621
		private struct WorkRequest
		{
			// Token: 0x06001A1D RID: 6685 RVA: 0x0002C11C File Offset: 0x0002A31C
			public WorkRequest(SendOrPostCallback callback, object state, ManualResetEvent waitHandle = null)
			{
				this.m_DelagateCallback = callback;
				this.m_DelagateState = state;
				this.m_WaitHandle = waitHandle;
			}

			// Token: 0x06001A1E RID: 6686 RVA: 0x0002C134 File Offset: 0x0002A334
			public void Invoke()
			{
				try
				{
					this.m_DelagateCallback(this.m_DelagateState);
				}
				finally
				{
					ManualResetEvent waitHandle = this.m_WaitHandle;
					if (waitHandle != null)
					{
						waitHandle.Set();
					}
				}
			}

			// Token: 0x04000910 RID: 2320
			private readonly SendOrPostCallback m_DelagateCallback;

			// Token: 0x04000911 RID: 2321
			private readonly object m_DelagateState;

			// Token: 0x04000912 RID: 2322
			private readonly ManualResetEvent m_WaitHandle;
		}
	}
}
