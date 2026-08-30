using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace Unity.IO.LowLevel.Unsafe
{
	// Token: 0x02000082 RID: 130
	[NativeConditional("ENABLE_PROFILER")]
	public static class AsyncReadManagerMetrics
	{
		// Token: 0x0600022E RID: 558
		[FreeFunction("AreMetricsEnabled_Internal")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsEnabled();

		// Token: 0x0600022F RID: 559
		[ThreadSafe]
		[FreeFunction("GetAsyncReadManagerMetrics()->ClearMetrics")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClearMetrics_Internal();

		// Token: 0x06000230 RID: 560 RVA: 0x00004688 File Offset: 0x00002888
		public static void ClearCompletedMetrics()
		{
			AsyncReadManagerMetrics.ClearMetrics_Internal();
		}

		// Token: 0x06000231 RID: 561
		[FreeFunction("GetAsyncReadManagerMetrics()->GetMarshalledMetrics")]
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AsyncReadManagerRequestMetric[] GetMetrics_Internal(bool clear);

		// Token: 0x06000232 RID: 562
		[FreeFunction("GetAsyncReadManagerMetrics()->GetMetrics_NoAlloc")]
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetMetrics_NoAlloc_Internal([NotNull("ArgumentNullException")] List<AsyncReadManagerRequestMetric> metrics, bool clear);

		// Token: 0x06000233 RID: 563
		[FreeFunction("GetAsyncReadManagerMetrics()->GetMarshalledMetrics_Filtered_Managed")]
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AsyncReadManagerRequestMetric[] GetMetrics_Filtered_Internal(AsyncReadManagerMetricsFilters filters, bool clear);

		// Token: 0x06000234 RID: 564
		[FreeFunction("GetAsyncReadManagerMetrics()->GetMetrics_NoAlloc_Filtered_Managed")]
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetMetrics_NoAlloc_Filtered_Internal([NotNull("ArgumentNullException")] List<AsyncReadManagerRequestMetric> metrics, AsyncReadManagerMetricsFilters filters, bool clear);

		// Token: 0x06000235 RID: 565 RVA: 0x00004694 File Offset: 0x00002894
		public static AsyncReadManagerRequestMetric[] GetMetrics(AsyncReadManagerMetricsFilters filters, AsyncReadManagerMetrics.Flags flags)
		{
			bool flag = (flags & AsyncReadManagerMetrics.Flags.ClearOnRead) == AsyncReadManagerMetrics.Flags.ClearOnRead;
			return AsyncReadManagerMetrics.GetMetrics_Filtered_Internal(filters, flag);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x000046B8 File Offset: 0x000028B8
		public static void GetMetrics(List<AsyncReadManagerRequestMetric> outMetrics, AsyncReadManagerMetricsFilters filters, AsyncReadManagerMetrics.Flags flags)
		{
			bool flag = (flags & AsyncReadManagerMetrics.Flags.ClearOnRead) == AsyncReadManagerMetrics.Flags.ClearOnRead;
			AsyncReadManagerMetrics.GetMetrics_NoAlloc_Filtered_Internal(outMetrics, filters, flag);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x000046DC File Offset: 0x000028DC
		public static AsyncReadManagerRequestMetric[] GetMetrics(AsyncReadManagerMetrics.Flags flags)
		{
			bool flag = (flags & AsyncReadManagerMetrics.Flags.ClearOnRead) == AsyncReadManagerMetrics.Flags.ClearOnRead;
			return AsyncReadManagerMetrics.GetMetrics_Internal(flag);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00004700 File Offset: 0x00002900
		public static void GetMetrics(List<AsyncReadManagerRequestMetric> outMetrics, AsyncReadManagerMetrics.Flags flags)
		{
			bool flag = (flags & AsyncReadManagerMetrics.Flags.ClearOnRead) == AsyncReadManagerMetrics.Flags.ClearOnRead;
			AsyncReadManagerMetrics.GetMetrics_NoAlloc_Internal(outMetrics, flag);
		}

		// Token: 0x06000239 RID: 569
		[FreeFunction("GetAsyncReadManagerMetrics()->StartCollecting")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StartCollectingMetrics();

		// Token: 0x0600023A RID: 570
		[FreeFunction("GetAsyncReadManagerMetrics()->StopCollecting")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StopCollectingMetrics();

		// Token: 0x0600023B RID: 571
		[FreeFunction("GetAsyncReadManagerMetrics()->GetCurrentSummaryMetrics")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AsyncReadManagerSummaryMetrics GetSummaryMetrics_Internal(bool clear);

		// Token: 0x0600023C RID: 572 RVA: 0x00004724 File Offset: 0x00002924
		public static AsyncReadManagerSummaryMetrics GetCurrentSummaryMetrics(AsyncReadManagerMetrics.Flags flags)
		{
			bool flag = (flags & AsyncReadManagerMetrics.Flags.ClearOnRead) == AsyncReadManagerMetrics.Flags.ClearOnRead;
			return AsyncReadManagerMetrics.GetSummaryMetrics_Internal(flag);
		}

		// Token: 0x0600023D RID: 573
		[FreeFunction("GetAsyncReadManagerMetrics()->GetCurrentSummaryMetricsWithFilters")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AsyncReadManagerSummaryMetrics GetSummaryMetricsWithFilters_Internal(AsyncReadManagerMetricsFilters metricsFilters, bool clear);

		// Token: 0x0600023E RID: 574 RVA: 0x00004748 File Offset: 0x00002948
		public static AsyncReadManagerSummaryMetrics GetCurrentSummaryMetrics(AsyncReadManagerMetricsFilters metricsFilters, AsyncReadManagerMetrics.Flags flags)
		{
			bool flag = (flags & AsyncReadManagerMetrics.Flags.ClearOnRead) == AsyncReadManagerMetrics.Flags.ClearOnRead;
			return AsyncReadManagerMetrics.GetSummaryMetricsWithFilters_Internal(metricsFilters, flag);
		}

		// Token: 0x0600023F RID: 575
		[FreeFunction("GetAsyncReadManagerMetrics()->GetSummaryOfMetrics_Managed")]
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AsyncReadManagerSummaryMetrics GetSummaryOfMetrics_Internal(AsyncReadManagerRequestMetric[] metrics);

		// Token: 0x06000240 RID: 576 RVA: 0x0000476C File Offset: 0x0000296C
		public static AsyncReadManagerSummaryMetrics GetSummaryOfMetrics(AsyncReadManagerRequestMetric[] metrics)
		{
			return AsyncReadManagerMetrics.GetSummaryOfMetrics_Internal(metrics);
		}

		// Token: 0x06000241 RID: 577
		[ThreadSafe]
		[FreeFunction("GetAsyncReadManagerMetrics()->GetSummaryOfMetrics_FromContainer_Managed", ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AsyncReadManagerSummaryMetrics GetSummaryOfMetrics_FromContainer_Internal(List<AsyncReadManagerRequestMetric> metrics);

		// Token: 0x06000242 RID: 578 RVA: 0x00004784 File Offset: 0x00002984
		public static AsyncReadManagerSummaryMetrics GetSummaryOfMetrics(List<AsyncReadManagerRequestMetric> metrics)
		{
			return AsyncReadManagerMetrics.GetSummaryOfMetrics_FromContainer_Internal(metrics);
		}

		// Token: 0x06000243 RID: 579
		[ThreadSafe]
		[FreeFunction("GetAsyncReadManagerMetrics()->GetSummaryOfMetricsWithFilters_Managed")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AsyncReadManagerSummaryMetrics GetSummaryOfMetricsWithFilters_Internal(AsyncReadManagerRequestMetric[] metrics, AsyncReadManagerMetricsFilters metricsFilters);

		// Token: 0x06000244 RID: 580 RVA: 0x0000479C File Offset: 0x0000299C
		public static AsyncReadManagerSummaryMetrics GetSummaryOfMetrics(AsyncReadManagerRequestMetric[] metrics, AsyncReadManagerMetricsFilters metricsFilters)
		{
			return AsyncReadManagerMetrics.GetSummaryOfMetricsWithFilters_Internal(metrics, metricsFilters);
		}

		// Token: 0x06000245 RID: 581
		[ThreadSafe]
		[FreeFunction("GetAsyncReadManagerMetrics()->GetSummaryOfMetricsWithFilters_FromContainer_Managed", ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AsyncReadManagerSummaryMetrics GetSummaryOfMetricsWithFilters_FromContainer_Internal(List<AsyncReadManagerRequestMetric> metrics, AsyncReadManagerMetricsFilters metricsFilters);

		// Token: 0x06000246 RID: 582 RVA: 0x000047B8 File Offset: 0x000029B8
		public static AsyncReadManagerSummaryMetrics GetSummaryOfMetrics(List<AsyncReadManagerRequestMetric> metrics, AsyncReadManagerMetricsFilters metricsFilters)
		{
			return AsyncReadManagerMetrics.GetSummaryOfMetricsWithFilters_FromContainer_Internal(metrics, metricsFilters);
		}

		// Token: 0x06000247 RID: 583
		[ThreadSafe]
		[FreeFunction("GetAsyncReadManagerMetrics()->GetTotalSizeNonASRMReadsBytes")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ulong GetTotalSizeOfNonASRMReadsBytes(bool emptyAfterRead);

		// Token: 0x02000083 RID: 131
		[Flags]
		public enum Flags
		{
			// Token: 0x040001EA RID: 490
			None = 0,
			// Token: 0x040001EB RID: 491
			ClearOnRead = 1
		}
	}
}
