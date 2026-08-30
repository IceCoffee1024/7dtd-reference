using System;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace Unity.IO.LowLevel.Unsafe
{
	// Token: 0x02000084 RID: 132
	[NativeConditional("ENABLE_PROFILER")]
	[NativeAsStruct]
	[StructLayout(LayoutKind.Sequential)]
	public class AsyncReadManagerSummaryMetrics
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000248 RID: 584 RVA: 0x000047D1 File Offset: 0x000029D1
		[NativeName("totalBytesRead")]
		public ulong TotalBytesRead { get; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000249 RID: 585 RVA: 0x000047D9 File Offset: 0x000029D9
		[NativeName("averageBandwidthMBPerSecond")]
		public float AverageBandwidthMBPerSecond { get; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600024A RID: 586 RVA: 0x000047E1 File Offset: 0x000029E1
		[NativeName("averageReadSizeInBytes")]
		public float AverageReadSizeInBytes { get; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600024B RID: 587 RVA: 0x000047E9 File Offset: 0x000029E9
		[NativeName("averageWaitTimeMicroseconds")]
		public float AverageWaitTimeMicroseconds { get; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600024C RID: 588 RVA: 0x000047F1 File Offset: 0x000029F1
		[NativeName("averageReadTimeMicroseconds")]
		public float AverageReadTimeMicroseconds { get; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600024D RID: 589 RVA: 0x000047F9 File Offset: 0x000029F9
		[NativeName("averageTotalRequestTimeMicroseconds")]
		public float AverageTotalRequestTimeMicroseconds { get; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00004801 File Offset: 0x00002A01
		[NativeName("averageThroughputMBPerSecond")]
		public float AverageThroughputMBPerSecond { get; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600024F RID: 591 RVA: 0x00004809 File Offset: 0x00002A09
		[NativeName("longestWaitTimeMicroseconds")]
		public float LongestWaitTimeMicroseconds { get; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00004811 File Offset: 0x00002A11
		[NativeName("longestReadTimeMicroseconds")]
		public float LongestReadTimeMicroseconds { get; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000251 RID: 593 RVA: 0x00004819 File Offset: 0x00002A19
		[NativeName("longestReadAssetType")]
		public ulong LongestReadAssetType { get; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000252 RID: 594 RVA: 0x00004821 File Offset: 0x00002A21
		[NativeName("longestWaitAssetType")]
		public ulong LongestWaitAssetType { get; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000253 RID: 595 RVA: 0x00004829 File Offset: 0x00002A29
		[NativeName("longestReadSubsystem")]
		public AssetLoadingSubsystem LongestReadSubsystem { get; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00004831 File Offset: 0x00002A31
		[NativeName("longestWaitSubsystem")]
		public AssetLoadingSubsystem LongestWaitSubsystem { get; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000255 RID: 597 RVA: 0x00004839 File Offset: 0x00002A39
		[NativeName("numberOfInProgressRequests")]
		public int NumberOfInProgressRequests { get; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000256 RID: 598 RVA: 0x00004841 File Offset: 0x00002A41
		[NativeName("numberOfCompletedRequests")]
		public int NumberOfCompletedRequests { get; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000257 RID: 599 RVA: 0x00004849 File Offset: 0x00002A49
		[NativeName("numberOfFailedRequests")]
		public int NumberOfFailedRequests { get; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000258 RID: 600 RVA: 0x00004851 File Offset: 0x00002A51
		[NativeName("numberOfWaitingRequests")]
		public int NumberOfWaitingRequests { get; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000259 RID: 601 RVA: 0x00004859 File Offset: 0x00002A59
		[NativeName("numberOfCanceledRequests")]
		public int NumberOfCanceledRequests { get; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00004861 File Offset: 0x00002A61
		[NativeName("totalNumberOfRequests")]
		public int TotalNumberOfRequests { get; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600025B RID: 603 RVA: 0x00004869 File Offset: 0x00002A69
		[NativeName("numberOfCachedReads")]
		public int NumberOfCachedReads { get; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00004871 File Offset: 0x00002A71
		[NativeName("numberOfAsyncReads")]
		public int NumberOfAsyncReads { get; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600025D RID: 605 RVA: 0x00004879 File Offset: 0x00002A79
		[NativeName("numberOfSyncReads")]
		public int NumberOfSyncReads { get; }
	}
}
