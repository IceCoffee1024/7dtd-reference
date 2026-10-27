using System;
using System.Collections.Generic;
using System.Threading;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001C5A RID: 7258
	public class MultiplayerActivityQueryManager
	{
		// Token: 0x0600D74C RID: 55116 RVA: 0x004D41D1 File Offset: 0x004D23D1
		public MultiplayerActivityQueryManager(Unity.XGamingRuntime.XblContextHandle xblContextHandle)
		{
			this.xblContextHandle = xblContextHandle;
		}

		// Token: 0x0600D74D RID: 55117 RVA: 0x004D420C File Offset: 0x004D240C
		public void GetActivityAsync(ulong[] xuids, MultiplayerActivityQueryManager.OnGetActivityComplete callback)
		{
			int num = (xuids.Length + 30 - 1) / 30;
			MultiplayerActivityQueryManager.Request request = new MultiplayerActivityQueryManager.Request(xuids, num, callback);
			object obj = this.pendingLock;
			lock (obj)
			{
				if (!this.pendingRequests.TryAdd(request.id, request))
				{
					Log.Error("[XBL] could not start GetActivityAsync as request could not be enqueued");
					return;
				}
			}
			if (num == 1)
			{
				this.StartBatch(request, xuids);
				return;
			}
			for (int i = 0; i < num; i++)
			{
				int num2 = i * 30;
				int num3 = Math.Min(xuids.Length - num2, 30);
				ulong[] array = new ulong[num3];
				Array.Copy(xuids, num2, array, 0, num3);
				this.StartBatch(request, array);
			}
		}

		// Token: 0x0600D74E RID: 55118 RVA: 0x004D42D0 File Offset: 0x004D24D0
		[PublicizedFrom(EAccessModifier.Private)]
		public void StartBatch(MultiplayerActivityQueryManager.Request request, ulong[] xuids)
		{
			SDK.XBL.XblMultiplayerActivityGetActivityAsync(this.xblContextHandle, xuids, delegate(int hresult, XblMultiplayerActivityInfo[] results)
			{
				if (Unity.XGamingRuntime.Interop.HR.SUCCEEDED(hresult))
				{
					this.CompleteBatch(request, results);
					return;
				}
				if (hresult == -2145844819)
				{
					object obj = this.retryLock;
					lock (obj)
					{
						this.retryQueue.Enqueue(new MultiplayerActivityQueryManager.RequestBatch(xuids, request));
						if (this.retryTask == null)
						{
							this.retryTask = ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(this.RetryBatchesTask), null, new ThreadManager.ExitCallbackTask(this.RetryExitHandler), true);
						}
					}
					return;
				}
				XblHelpers.LogHR(hresult, "XblMultiplayerActivityGetActivityAsync", false);
				this.CompleteBatch(request, null);
			});
		}

		// Token: 0x0600D74F RID: 55119 RVA: 0x004D4318 File Offset: 0x004D2518
		[PublicizedFrom(EAccessModifier.Private)]
		public void CompleteBatch(MultiplayerActivityQueryManager.Request request, XblMultiplayerActivityInfo[] batchResults)
		{
			if (batchResults != null && batchResults.Length != 0)
			{
				List<XblMultiplayerActivityInfo> results = request.results;
				lock (results)
				{
					request.results.AddRange(batchResults);
				}
			}
			if (Interlocked.Decrement(ref request.batchesPending) == 0)
			{
				request.callback(request.xuids, request.results);
				object obj = this.pendingLock;
				lock (obj)
				{
					MultiplayerActivityQueryManager.Request request2;
					this.pendingRequests.Remove(request.id, out request2);
				}
			}
		}

		// Token: 0x0600D750 RID: 55120 RVA: 0x004D43C8 File Offset: 0x004D25C8
		[PublicizedFrom(EAccessModifier.Private)]
		public void RetryBatchesTask(ThreadManager.TaskInfo taskInfo)
		{
			for (;;)
			{
				Log.Warning(string.Format("[XBL] too many multiplayer activity requests, will try again in {0}s", 20));
				Thread.Sleep(20000);
				object obj = this.retryLock;
				lock (obj)
				{
					int num = 0;
					MultiplayerActivityQueryManager.RequestBatch requestBatch;
					while (this.retryQueue.TryDequeue(out requestBatch) && num < 20)
					{
						this.StartBatch(requestBatch.request, requestBatch.xuids);
						num++;
					}
					if (this.retryQueue.Count != 0)
					{
						continue;
					}
				}
				break;
			}
		}

		// Token: 0x0600D751 RID: 55121 RVA: 0x004D4460 File Offset: 0x004D2660
		[PublicizedFrom(EAccessModifier.Private)]
		public void RetryExitHandler(ThreadManager.TaskInfo _ti, Exception _e)
		{
			object obj = this.retryLock;
			lock (obj)
			{
				this.retryTask = null;
			}
		}

		// Token: 0x0400A395 RID: 41877
		[PublicizedFrom(EAccessModifier.Private)]
		public const int batchMax = 30;

		// Token: 0x0400A396 RID: 41878
		[PublicizedFrom(EAccessModifier.Private)]
		public const int backoffSeconds = 20;

		// Token: 0x0400A397 RID: 41879
		[PublicizedFrom(EAccessModifier.Private)]
		public const int burstLimit = 20;

		// Token: 0x0400A398 RID: 41880
		[PublicizedFrom(EAccessModifier.Private)]
		public static int nextRequestId;

		// Token: 0x0400A399 RID: 41881
		[PublicizedFrom(EAccessModifier.Private)]
		public Unity.XGamingRuntime.XblContextHandle xblContextHandle;

		// Token: 0x0400A39A RID: 41882
		[PublicizedFrom(EAccessModifier.Private)]
		public object pendingLock = new object();

		// Token: 0x0400A39B RID: 41883
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, MultiplayerActivityQueryManager.Request> pendingRequests = new Dictionary<int, MultiplayerActivityQueryManager.Request>();

		// Token: 0x0400A39C RID: 41884
		[PublicizedFrom(EAccessModifier.Private)]
		public object retryLock = new object();

		// Token: 0x0400A39D RID: 41885
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<MultiplayerActivityQueryManager.RequestBatch> retryQueue = new Queue<MultiplayerActivityQueryManager.RequestBatch>();

		// Token: 0x0400A39E RID: 41886
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.TaskInfo retryTask;

		// Token: 0x02001C5B RID: 7259
		[PublicizedFrom(EAccessModifier.Private)]
		public class Request
		{
			// Token: 0x0600D752 RID: 55122 RVA: 0x004D44A4 File Offset: 0x004D26A4
			public Request(ulong[] xuids, int batchCount, MultiplayerActivityQueryManager.OnGetActivityComplete callback)
			{
				this.id = Interlocked.Increment(ref MultiplayerActivityQueryManager.nextRequestId);
				this.xuids = xuids;
				this.batchesPending = batchCount;
				this.callback = callback;
			}

			// Token: 0x0400A39F RID: 41887
			public int id;

			// Token: 0x0400A3A0 RID: 41888
			public ulong[] xuids;

			// Token: 0x0400A3A1 RID: 41889
			public MultiplayerActivityQueryManager.OnGetActivityComplete callback;

			// Token: 0x0400A3A2 RID: 41890
			public List<XblMultiplayerActivityInfo> results = new List<XblMultiplayerActivityInfo>();

			// Token: 0x0400A3A3 RID: 41891
			public int batchesPending;
		}

		// Token: 0x02001C5C RID: 7260
		[PublicizedFrom(EAccessModifier.Private)]
		public struct RequestBatch
		{
			// Token: 0x0600D753 RID: 55123 RVA: 0x004D44DC File Offset: 0x004D26DC
			public RequestBatch(ulong[] xuidBatch, MultiplayerActivityQueryManager.Request request)
			{
				this.xuids = xuidBatch;
				this.request = request;
			}

			// Token: 0x0400A3A4 RID: 41892
			public ulong[] xuids;

			// Token: 0x0400A3A5 RID: 41893
			public MultiplayerActivityQueryManager.Request request;
		}

		// Token: 0x02001C5D RID: 7261
		// (Invoke) Token: 0x0600D755 RID: 55125
		public delegate void OnGetActivityComplete(ulong[] requestedXuids, List<XblMultiplayerActivityInfo> results);
	}
}
