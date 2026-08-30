using System;
using System.Collections.Generic;
using System.Threading;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001BD0 RID: 7120
	public class MultiplayerActivityQueryManager
	{
		// Token: 0x0600D332 RID: 54066 RVA: 0x004C57CD File Offset: 0x004C39CD
		public MultiplayerActivityQueryManager(Unity.XGamingRuntime.XblContextHandle xblContextHandle)
		{
			this.xblContextHandle = xblContextHandle;
		}

		// Token: 0x0600D333 RID: 54067 RVA: 0x004C5808 File Offset: 0x004C3A08
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

		// Token: 0x0600D334 RID: 54068 RVA: 0x004C58CC File Offset: 0x004C3ACC
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

		// Token: 0x0600D335 RID: 54069 RVA: 0x004C5914 File Offset: 0x004C3B14
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

		// Token: 0x0600D336 RID: 54070 RVA: 0x004C59C4 File Offset: 0x004C3BC4
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

		// Token: 0x0600D337 RID: 54071 RVA: 0x004C5A5C File Offset: 0x004C3C5C
		[PublicizedFrom(EAccessModifier.Private)]
		public void RetryExitHandler(ThreadManager.TaskInfo _ti, Exception _e)
		{
			object obj = this.retryLock;
			lock (obj)
			{
				this.retryTask = null;
			}
		}

		// Token: 0x0400A0A6 RID: 41126
		[PublicizedFrom(EAccessModifier.Private)]
		public const int batchMax = 30;

		// Token: 0x0400A0A7 RID: 41127
		[PublicizedFrom(EAccessModifier.Private)]
		public const int backoffSeconds = 20;

		// Token: 0x0400A0A8 RID: 41128
		[PublicizedFrom(EAccessModifier.Private)]
		public const int burstLimit = 20;

		// Token: 0x0400A0A9 RID: 41129
		[PublicizedFrom(EAccessModifier.Private)]
		public static int nextRequestId;

		// Token: 0x0400A0AA RID: 41130
		[PublicizedFrom(EAccessModifier.Private)]
		public Unity.XGamingRuntime.XblContextHandle xblContextHandle;

		// Token: 0x0400A0AB RID: 41131
		[PublicizedFrom(EAccessModifier.Private)]
		public object pendingLock = new object();

		// Token: 0x0400A0AC RID: 41132
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, MultiplayerActivityQueryManager.Request> pendingRequests = new Dictionary<int, MultiplayerActivityQueryManager.Request>();

		// Token: 0x0400A0AD RID: 41133
		[PublicizedFrom(EAccessModifier.Private)]
		public object retryLock = new object();

		// Token: 0x0400A0AE RID: 41134
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<MultiplayerActivityQueryManager.RequestBatch> retryQueue = new Queue<MultiplayerActivityQueryManager.RequestBatch>();

		// Token: 0x0400A0AF RID: 41135
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.TaskInfo retryTask;

		// Token: 0x02001BD1 RID: 7121
		[PublicizedFrom(EAccessModifier.Private)]
		public class Request
		{
			// Token: 0x0600D338 RID: 54072 RVA: 0x004C5AA0 File Offset: 0x004C3CA0
			public Request(ulong[] xuids, int batchCount, MultiplayerActivityQueryManager.OnGetActivityComplete callback)
			{
				this.id = Interlocked.Increment(ref MultiplayerActivityQueryManager.nextRequestId);
				this.xuids = xuids;
				this.batchesPending = batchCount;
				this.callback = callback;
			}

			// Token: 0x0400A0B0 RID: 41136
			public int id;

			// Token: 0x0400A0B1 RID: 41137
			public ulong[] xuids;

			// Token: 0x0400A0B2 RID: 41138
			public MultiplayerActivityQueryManager.OnGetActivityComplete callback;

			// Token: 0x0400A0B3 RID: 41139
			public List<XblMultiplayerActivityInfo> results = new List<XblMultiplayerActivityInfo>();

			// Token: 0x0400A0B4 RID: 41140
			public int batchesPending;
		}

		// Token: 0x02001BD2 RID: 7122
		[PublicizedFrom(EAccessModifier.Private)]
		public struct RequestBatch
		{
			// Token: 0x0600D339 RID: 54073 RVA: 0x004C5AD8 File Offset: 0x004C3CD8
			public RequestBatch(ulong[] xuidBatch, MultiplayerActivityQueryManager.Request request)
			{
				this.xuids = xuidBatch;
				this.request = request;
			}

			// Token: 0x0400A0B5 RID: 41141
			public ulong[] xuids;

			// Token: 0x0400A0B6 RID: 41142
			public MultiplayerActivityQueryManager.Request request;
		}

		// Token: 0x02001BD3 RID: 7123
		// (Invoke) Token: 0x0600D33B RID: 54075
		public delegate void OnGetActivityComplete(ulong[] requestedXuids, List<XblMultiplayerActivityInfo> results);
	}
}
