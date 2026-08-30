using System;
using System.Diagnostics;
using System.Threading;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL.Save
{
	// Token: 0x02001C4A RID: 7242
	public sealed class SizeTracker : IDisposable
	{
		// Token: 0x0600D6AD RID: 54957 RVA: 0x004D4A2A File Offset: 0x004D2C2A
		[Conditional("DEBUG_SAVE_DATA_MANAGER")]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogTrace(string text)
		{
			Log.Out("[XBL: SizeTracker] " + text);
		}

		// Token: 0x0600D6AE RID: 54958 RVA: 0x004D4A3C File Offset: 0x004D2C3C
		public SizeTracker(long max, long used, SizeTrackerGetRemainingQuotaAsync getRemainingQuotaAsync, bool shouldUpdateSizesOnEstimate)
		{
			this.m_maxBytes = max;
			this.m_getRemainingQuotaAsync = getRemainingQuotaAsync;
			this.m_shouldUpdateSizesOnEstimate = shouldUpdateSizesOnEstimate;
			this.m_sizes = new SaveDataSizes(max, max - used);
		}

		// Token: 0x0600D6AF RID: 54959 RVA: 0x004D4A74 File Offset: 0x004D2C74
		public void Dispose()
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.m_disposed = true;
			}
		}

		// Token: 0x17001A95 RID: 6805
		// (get) Token: 0x0600D6B0 RID: 54960 RVA: 0x004D4AB8 File Offset: 0x004D2CB8
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

		// Token: 0x0600D6B1 RID: 54961 RVA: 0x004D4AFC File Offset: 0x004D2CFC
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

		// Token: 0x0600D6B2 RID: 54962 RVA: 0x004D4B78 File Offset: 0x004D2D78
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

		// Token: 0x0600D6B3 RID: 54963 RVA: 0x004D4BC8 File Offset: 0x004D2DC8
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

		// Token: 0x0600D6B4 RID: 54964 RVA: 0x004D4C54 File Offset: 0x004D2E54
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

		// Token: 0x0400A375 RID: 41845
		[PublicizedFrom(EAccessModifier.Private)]
		public const int DefaultMaxSaveDataBytes = 1073741824;

		// Token: 0x0400A376 RID: 41846
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly long m_maxBytes;

		// Token: 0x0400A377 RID: 41847
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SizeTrackerGetRemainingQuotaAsync m_getRemainingQuotaAsync;

		// Token: 0x0400A378 RID: 41848
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_shouldUpdateSizesOnEstimate;

		// Token: 0x0400A379 RID: 41849
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposed;

		// Token: 0x0400A37A RID: 41850
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_lock = new object();

		// Token: 0x0400A37B RID: 41851
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_refreshCount;

		// Token: 0x0400A37C RID: 41852
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_refreshCountExpected;

		// Token: 0x0400A37D RID: 41853
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataSizes m_sizes;
	}
}
