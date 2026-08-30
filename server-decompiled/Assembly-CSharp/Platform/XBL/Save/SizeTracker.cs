using System;
using System.Diagnostics;
using System.Threading;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL.Save
{
	// Token: 0x02001C23 RID: 7203
	public sealed class SizeTracker : IDisposable
	{
		// Token: 0x0600D51D RID: 54557 RVA: 0x004CE606 File Offset: 0x004CC806
		[Conditional("DEBUG_SAVE_DATA_MANAGER")]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogTrace(string text)
		{
			Log.Out("[XBL: SizeTracker] " + text);
		}

		// Token: 0x0600D51E RID: 54558 RVA: 0x004CE618 File Offset: 0x004CC818
		public SizeTracker(long max, long used, SizeTrackerGetRemainingQuotaAsync getRemainingQuotaAsync, bool shouldUpdateSizesOnEstimate)
		{
			this.m_maxBytes = max;
			this.m_getRemainingQuotaAsync = getRemainingQuotaAsync;
			this.m_shouldUpdateSizesOnEstimate = shouldUpdateSizesOnEstimate;
			this.m_sizes = new SaveDataSizes(max, max - used);
		}

		// Token: 0x0600D51F RID: 54559 RVA: 0x004CE650 File Offset: 0x004CC850
		public void Dispose()
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.m_disposed = true;
			}
		}

		// Token: 0x17001A23 RID: 6691
		// (get) Token: 0x0600D520 RID: 54560 RVA: 0x004CE694 File Offset: 0x004CC894
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

		// Token: 0x0600D521 RID: 54561 RVA: 0x004CE6D8 File Offset: 0x004CC8D8
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

		// Token: 0x0600D522 RID: 54562 RVA: 0x004CE754 File Offset: 0x004CC954
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

		// Token: 0x0600D523 RID: 54563 RVA: 0x004CE7A4 File Offset: 0x004CC9A4
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

		// Token: 0x0600D524 RID: 54564 RVA: 0x004CE830 File Offset: 0x004CCA30
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

		// Token: 0x0400A244 RID: 41540
		[PublicizedFrom(EAccessModifier.Private)]
		public const int DefaultMaxSaveDataBytes = 1073741824;

		// Token: 0x0400A245 RID: 41541
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly long m_maxBytes;

		// Token: 0x0400A246 RID: 41542
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SizeTrackerGetRemainingQuotaAsync m_getRemainingQuotaAsync;

		// Token: 0x0400A247 RID: 41543
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_shouldUpdateSizesOnEstimate;

		// Token: 0x0400A248 RID: 41544
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposed;

		// Token: 0x0400A249 RID: 41545
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_lock = new object();

		// Token: 0x0400A24A RID: 41546
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_refreshCount;

		// Token: 0x0400A24B RID: 41547
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_refreshCountExpected;

		// Token: 0x0400A24C RID: 41548
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataSizes m_sizes;
	}
}
