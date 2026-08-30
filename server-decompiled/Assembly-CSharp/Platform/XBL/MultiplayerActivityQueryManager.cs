using System;
using System.Collections.Generic;
using System.Threading;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001BF7 RID: 7159
	public class MultiplayerActivityQueryManager
	{
		// Token: 0x0600D4C0 RID: 54464 RVA: 0x004CBBC1 File Offset: 0x004C9DC1
		public MultiplayerActivityQueryManager(Unity.XGamingRuntime.XblContextHandle xblContextHandle)
		{
			this.xblContextHandle = xblContextHandle;
		}

		// Token: 0x0600D4C1 RID: 54465 RVA: 0x004CBBFC File Offset: 0x004C9DFC
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

		// Token: 0x0600D4C2 RID: 54466 RVA: 0x004CBCC0 File Offset: 0x004C9EC0
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

		// Token: 0x0600D4C3 RID: 54467 RVA: 0x004CBD08 File Offset: 0x004C9F08
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

		// Token: 0x0600D4C4 RID: 54468 RVA: 0x004CBDB8 File Offset: 0x004C9FB8
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

		// Token: 0x0600D4C5 RID: 54469 RVA: 0x004CBE50 File Offset: 0x004CA050
		[PublicizedFrom(EAccessModifier.Private)]
		public void RetryExitHandler(ThreadManager.TaskInfo _ti, Exception _e)
		{
			object obj = this.retryLock;
			lock (obj)
			{
				this.retryTask = null;
			}
		}

		// Token: 0x0400A1D7 RID: 41431
		[PublicizedFrom(EAccessModifier.Private)]
		public const int batchMax = 30;

		// Token: 0x0400A1D8 RID: 41432
		[PublicizedFrom(EAccessModifier.Private)]
		public const int backoffSeconds = 20;

		// Token: 0x0400A1D9 RID: 41433
		[PublicizedFrom(EAccessModifier.Private)]
		public const int burstLimit = 20;

		// Token: 0x0400A1DA RID: 41434
		[PublicizedFrom(EAccessModifier.Private)]
		public static int nextRequestId;

		// Token: 0x0400A1DB RID: 41435
		[PublicizedFrom(EAccessModifier.Private)]
		public Unity.XGamingRuntime.XblContextHandle xblContextHandle;

		// Token: 0x0400A1DC RID: 41436
		[PublicizedFrom(EAccessModifier.Private)]
		public object pendingLock = new object();

		// Token: 0x0400A1DD RID: 41437
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, MultiplayerActivityQueryManager.Request> pendingRequests = new Dictionary<int, MultiplayerActivityQueryManager.Request>();

		// Token: 0x0400A1DE RID: 41438
		[PublicizedFrom(EAccessModifier.Private)]
		public object retryLock = new object();

		// Token: 0x0400A1DF RID: 41439
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<MultiplayerActivityQueryManager.RequestBatch> retryQueue = new Queue<MultiplayerActivityQueryManager.RequestBatch>();

		// Token: 0x0400A1E0 RID: 41440
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.TaskInfo retryTask;

		// Token: 0x02001BF8 RID: 7160
		[PublicizedFrom(EAccessModifier.Private)]
		public class Request
		{
			// Token: 0x0600D4C6 RID: 54470 RVA: 0x004CBE94 File Offset: 0x004CA094
			public Request(ulong[] xuids, int batchCount, MultiplayerActivityQueryManager.OnGetActivityComplete callback)
			{
				this.id = Interlocked.Increment(ref MultiplayerActivityQueryManager.nextRequestId);
				this.xuids = xuids;
				this.batchesPending = batchCount;
				this.callback = callback;
			}

			// Token: 0x0400A1E1 RID: 41441
			public int id;

			// Token: 0x0400A1E2 RID: 41442
			public ulong[] xuids;

			// Token: 0x0400A1E3 RID: 41443
			public MultiplayerActivityQueryManager.OnGetActivityComplete callback;

			// Token: 0x0400A1E4 RID: 41444
			public List<XblMultiplayerActivityInfo> results = new List<XblMultiplayerActivityInfo>();

			// Token: 0x0400A1E5 RID: 41445
			public int batchesPending;
		}

		// Token: 0x02001BF9 RID: 7161
		[PublicizedFrom(EAccessModifier.Private)]
		public struct RequestBatch
		{
			// Token: 0x0600D4C7 RID: 54471 RVA: 0x004CBECC File Offset: 0x004CA0CC
			public RequestBatch(ulong[] xuidBatch, MultiplayerActivityQueryManager.Request request)
			{
				this.xuids = xuidBatch;
				this.request = request;
			}

			// Token: 0x0400A1E6 RID: 41446
			public ulong[] xuids;

			// Token: 0x0400A1E7 RID: 41447
			public MultiplayerActivityQueryManager.Request request;
		}

		// Token: 0x02001BFA RID: 7162
		// (Invoke) Token: 0x0600D4C9 RID: 54473
		public delegate void OnGetActivityComplete(ulong[] requestedXuids, List<XblMultiplayerActivityInfo> results);
	}
}
