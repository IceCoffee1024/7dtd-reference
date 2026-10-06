using System;
using System.Collections.Generic;
using System.Threading;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001C29 RID: 7209
	public class MultiplayerActivityQueryManager
	{
		// Token: 0x0600D638 RID: 54840 RVA: 0x004D06A9 File Offset: 0x004CE8A9
		public MultiplayerActivityQueryManager(Unity.XGamingRuntime.XblContextHandle xblContextHandle)
		{
			this.xblContextHandle = xblContextHandle;
		}

		// Token: 0x0600D639 RID: 54841 RVA: 0x004D06E4 File Offset: 0x004CE8E4
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

		// Token: 0x0600D63A RID: 54842 RVA: 0x004D07A8 File Offset: 0x004CE9A8
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

		// Token: 0x0600D63B RID: 54843 RVA: 0x004D07F0 File Offset: 0x004CE9F0
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

		// Token: 0x0600D63C RID: 54844 RVA: 0x004D08A0 File Offset: 0x004CEAA0
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

		// Token: 0x0600D63D RID: 54845 RVA: 0x004D0938 File Offset: 0x004CEB38
		[PublicizedFrom(EAccessModifier.Private)]
		public void RetryExitHandler(ThreadManager.TaskInfo _ti, Exception _e)
		{
			object obj = this.retryLock;
			lock (obj)
			{
				this.retryTask = null;
			}
		}

		// Token: 0x0400A2AC RID: 41644
		[PublicizedFrom(EAccessModifier.Private)]
		public const int batchMax = 30;

		// Token: 0x0400A2AD RID: 41645
		[PublicizedFrom(EAccessModifier.Private)]
		public const int backoffSeconds = 20;

		// Token: 0x0400A2AE RID: 41646
		[PublicizedFrom(EAccessModifier.Private)]
		public const int burstLimit = 20;

		// Token: 0x0400A2AF RID: 41647
		[PublicizedFrom(EAccessModifier.Private)]
		public static int nextRequestId;

		// Token: 0x0400A2B0 RID: 41648
		[PublicizedFrom(EAccessModifier.Private)]
		public Unity.XGamingRuntime.XblContextHandle xblContextHandle;

		// Token: 0x0400A2B1 RID: 41649
		[PublicizedFrom(EAccessModifier.Private)]
		public object pendingLock = new object();

		// Token: 0x0400A2B2 RID: 41650
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, MultiplayerActivityQueryManager.Request> pendingRequests = new Dictionary<int, MultiplayerActivityQueryManager.Request>();

		// Token: 0x0400A2B3 RID: 41651
		[PublicizedFrom(EAccessModifier.Private)]
		public object retryLock = new object();

		// Token: 0x0400A2B4 RID: 41652
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<MultiplayerActivityQueryManager.RequestBatch> retryQueue = new Queue<MultiplayerActivityQueryManager.RequestBatch>();

		// Token: 0x0400A2B5 RID: 41653
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.TaskInfo retryTask;

		// Token: 0x02001C2A RID: 7210
		[PublicizedFrom(EAccessModifier.Private)]
		public class Request
		{
			// Token: 0x0600D63E RID: 54846 RVA: 0x004D097C File Offset: 0x004CEB7C
			public Request(ulong[] xuids, int batchCount, MultiplayerActivityQueryManager.OnGetActivityComplete callback)
			{
				this.id = Interlocked.Increment(ref MultiplayerActivityQueryManager.nextRequestId);
				this.xuids = xuids;
				this.batchesPending = batchCount;
				this.callback = callback;
			}

			// Token: 0x0400A2B6 RID: 41654
			public int id;

			// Token: 0x0400A2B7 RID: 41655
			public ulong[] xuids;

			// Token: 0x0400A2B8 RID: 41656
			public MultiplayerActivityQueryManager.OnGetActivityComplete callback;

			// Token: 0x0400A2B9 RID: 41657
			public List<XblMultiplayerActivityInfo> results = new List<XblMultiplayerActivityInfo>();

			// Token: 0x0400A2BA RID: 41658
			public int batchesPending;
		}

		// Token: 0x02001C2B RID: 7211
		[PublicizedFrom(EAccessModifier.Private)]
		public struct RequestBatch
		{
			// Token: 0x0600D63F RID: 54847 RVA: 0x004D09B4 File Offset: 0x004CEBB4
			public RequestBatch(ulong[] xuidBatch, MultiplayerActivityQueryManager.Request request)
			{
				this.xuids = xuidBatch;
				this.request = request;
			}

			// Token: 0x0400A2BB RID: 41659
			public ulong[] xuids;

			// Token: 0x0400A2BC RID: 41660
			public MultiplayerActivityQueryManager.Request request;
		}

		// Token: 0x02001C2C RID: 7212
		// (Invoke) Token: 0x0600D641 RID: 54849
		public delegate void OnGetActivityComplete(ulong[] requestedXuids, List<XblMultiplayerActivityInfo> results);
	}
}
