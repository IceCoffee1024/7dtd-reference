using System;
using System.Diagnostics;
using System.Threading;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL.Save
{
	// Token: 0x02001CAD RID: 7341
	public sealed class SizeTracker : IDisposable
	{
		// Token: 0x0600D93A RID: 55610 RVA: 0x004DD0B6 File Offset: 0x004DB2B6
		[Conditional("DEBUG_SAVE_DATA_MANAGER")]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogTrace(string text)
		{
			Log.Out("[XBL: SizeTracker] " + text);
		}

		// Token: 0x0600D93B RID: 55611 RVA: 0x004DD0C8 File Offset: 0x004DB2C8
		public SizeTracker(long max, long used, SizeTrackerGetRemainingQuotaAsync getRemainingQuotaAsync, bool shouldUpdateSizesOnEstimate)
		{
			this.m_maxBytes = max;
			this.m_getRemainingQuotaAsync = getRemainingQuotaAsync;
			this.m_shouldUpdateSizesOnEstimate = shouldUpdateSizesOnEstimate;
			this.m_sizes = new SaveDataSizes(max, max - used);
		}

		// Token: 0x0600D93C RID: 55612 RVA: 0x004DD100 File Offset: 0x004DB300
		public void Dispose()
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.m_disposed = true;
			}
		}

		// Token: 0x17001B83 RID: 7043
		// (get) Token: 0x0600D93D RID: 55613 RVA: 0x004DD144 File Offset: 0x004DB344
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

		// Token: 0x0600D93E RID: 55614 RVA: 0x004DD188 File Offset: 0x004DB388
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

		// Token: 0x0600D93F RID: 55615 RVA: 0x004DD204 File Offset: 0x004DB404
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

		// Token: 0x0600D940 RID: 55616 RVA: 0x004DD254 File Offset: 0x004DB454
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

		// Token: 0x0600D941 RID: 55617 RVA: 0x004DD2E0 File Offset: 0x004DB4E0
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

		// Token: 0x0400A533 RID: 42291
		[PublicizedFrom(EAccessModifier.Private)]
		public const int DefaultMaxSaveDataBytes = 1073741824;

		// Token: 0x0400A534 RID: 42292
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly long m_maxBytes;

		// Token: 0x0400A535 RID: 42293
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SizeTrackerGetRemainingQuotaAsync m_getRemainingQuotaAsync;

		// Token: 0x0400A536 RID: 42294
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_shouldUpdateSizesOnEstimate;

		// Token: 0x0400A537 RID: 42295
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposed;

		// Token: 0x0400A538 RID: 42296
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_lock = new object();

		// Token: 0x0400A539 RID: 42297
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_refreshCount;

		// Token: 0x0400A53A RID: 42298
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_refreshCountExpected;

		// Token: 0x0400A53B RID: 42299
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataSizes m_sizes;
	}
}
