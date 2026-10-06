using System;
using System.Diagnostics;
using System.Threading;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL.Save
{
	// Token: 0x02001C7C RID: 7292
	public sealed class SizeTracker : IDisposable
	{
		// Token: 0x0600D826 RID: 55334 RVA: 0x004D9566 File Offset: 0x004D7766
		[Conditional("DEBUG_SAVE_DATA_MANAGER")]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogTrace(string text)
		{
			Log.Out("[XBL: SizeTracker] " + text);
		}

		// Token: 0x0600D827 RID: 55335 RVA: 0x004D9578 File Offset: 0x004D7778
		public SizeTracker(long max, long used, SizeTrackerGetRemainingQuotaAsync getRemainingQuotaAsync, bool shouldUpdateSizesOnEstimate)
		{
			this.m_maxBytes = max;
			this.m_getRemainingQuotaAsync = getRemainingQuotaAsync;
			this.m_shouldUpdateSizesOnEstimate = shouldUpdateSizesOnEstimate;
			this.m_sizes = new SaveDataSizes(max, max - used);
		}

		// Token: 0x0600D828 RID: 55336 RVA: 0x004D95B0 File Offset: 0x004D77B0
		public void Dispose()
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.m_disposed = true;
			}
		}

		// Token: 0x17001B3B RID: 6971
		// (get) Token: 0x0600D829 RID: 55337 RVA: 0x004D95F4 File Offset: 0x004D77F4
		public SaveDataSizes Sizes
		{
			get
			{
				object @lock = this.m_lock;
				SaveDataSizes sizes;
				lock (@lock)
				{
					sizes = this.m_sizes;
				}
				return sizes;
			}
		}

		// Token: 0x0600D82A RID: 55338 RVA: 0x004D9638 File Offset: 0x004D7838
		public void UpdateUsedEstimate(long deltaUsed)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				if (this.m_disposed)
				{
					return;
				}
				this.m_sizes = new SaveDataSizes(this.m_sizes.Total, Math.Max(this.m_sizes.Remaining - deltaUsed, 0L));
			}
			if (this.m_shouldUpdateSizesOnEstimate)
			{
				this.RefreshAsync();
			}
		}

		// Token: 0x0600D82B RID: 55339 RVA: 0x004D96B4 File Offset: 0x004D78B4
		public void RefreshSync()
		{
			if (this.m_disposed)
			{
				return;
			}
			long num = Interlocked.Increment(ref this.m_refreshCountExpected);
			this.m_getRemainingQuotaAsync(new SizeTrackerGetRemainingQuotaCompleted(this.XGameSaveGetRemainingQuotaAsyncCompleted));
			while (Interlocked.Read(ref this.m_refreshCount) < num)
			{
				Thread.Sleep(16);
			}
		}

		// Token: 0x0600D82C RID: 55340 RVA: 0x004D9704 File Offset: 0x004D7904
		public void RefreshAsync()
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				if (this.m_disposed)
				{
					return;
				}
			}
			long num = Interlocked.Read(ref this.m_refreshCountExpected);
			if (Interlocked.Read(ref this.m_refreshCount) >= num)
			{
				return;
			}
			if (Interlocked.CompareExchange(ref this.m_refreshCountExpected, num + 1L, num) == num)
			{
				this.m_getRemainingQuotaAsync(new SizeTrackerGetRemainingQuotaCompleted(this.XGameSaveGetRemainingQuotaAsyncCompleted));
			}
		}

		// Token: 0x0600D82D RID: 55341 RVA: 0x004D9790 File Offset: 0x004D7990
		[PublicizedFrom(EAccessModifier.Private)]
		public void XGameSaveGetRemainingQuotaAsyncCompleted(int hrXGameSaveGetRemainingQuota, long remaining)
		{
			if (HR.FAILED(hrXGameSaveGetRemainingQuota))
			{
				GameCoreSaveHelpers.NonTraceLogHR(hrXGameSaveGetRemainingQuota, "SizeTracker#XGameSaveGetRemainingQuotaAsync");
				Interlocked.Increment(ref this.m_refreshCount);
				return;
			}
			object obj = this.m_lock;
			lock (obj)
			{
				if (this.m_disposed)
				{
					Interlocked.Increment(ref this.m_refreshCount);
					return;
				}
			}
			long total = this.m_sizes.Total;
			long total2 = this.m_sizes.Total;
			obj = this.m_lock;
			lock (obj)
			{
				if (this.m_disposed)
				{
					Interlocked.Increment(ref this.m_refreshCount);
					return;
				}
				this.m_sizes = new SaveDataSizes(total2, remaining);
			}
			Interlocked.Increment(ref this.m_refreshCount);
		}

		// Token: 0x0400A44A RID: 42058
		[PublicizedFrom(EAccessModifier.Private)]
		public const int DefaultMaxSaveDataBytes = 1073741824;

		// Token: 0x0400A44B RID: 42059
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly long m_maxBytes;

		// Token: 0x0400A44C RID: 42060
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SizeTrackerGetRemainingQuotaAsync m_getRemainingQuotaAsync;

		// Token: 0x0400A44D RID: 42061
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_shouldUpdateSizesOnEstimate;

		// Token: 0x0400A44E RID: 42062
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposed;

		// Token: 0x0400A44F RID: 42063
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_lock = new object();

		// Token: 0x0400A450 RID: 42064
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_refreshCount;

		// Token: 0x0400A451 RID: 42065
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_refreshCountExpected;

		// Token: 0x0400A452 RID: 42066
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataSizes m_sizes;
	}
}
