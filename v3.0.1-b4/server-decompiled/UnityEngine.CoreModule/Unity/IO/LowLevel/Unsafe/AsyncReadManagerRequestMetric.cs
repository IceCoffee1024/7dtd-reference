using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace Unity.IO.LowLevel.Unsafe
{
	// Token: 0x02000081 RID: 129
	[NativeConditional("ENABLE_PROFILER")]
	[RequiredByNativeCode]
	public struct AsyncReadManagerRequestMetric
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00004610 File Offset: 0x00002810
		[NativeName("assetName")]
		public readonly string AssetName { get; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000220 RID: 544 RVA: 0x00004618 File Offset: 0x00002818
		[NativeName("fileName")]
		public readonly string FileName { get; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00004620 File Offset: 0x00002820
		[NativeName("offsetBytes")]
		public readonly ulong OffsetBytes { get; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00004628 File Offset: 0x00002828
		[NativeName("sizeBytes")]
		public readonly ulong SizeBytes { get; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000223 RID: 547 RVA: 0x00004630 File Offset: 0x00002830
		[NativeName("assetTypeId")]
		public readonly ulong AssetTypeId { get; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00004638 File Offset: 0x00002838
		[NativeName("currentBytesRead")]
		public readonly ulong CurrentBytesRead { get; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000225 RID: 549 RVA: 0x00004640 File Offset: 0x00002840
		[NativeName("batchReadCount")]
		public readonly uint BatchReadCount { get; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000226 RID: 550 RVA: 0x00004648 File Offset: 0x00002848
		[NativeName("isBatchRead")]
		public readonly bool IsBatchRead { get; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00004650 File Offset: 0x00002850
		[NativeName("state")]
		public readonly ProcessingState State { get; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000228 RID: 552 RVA: 0x00004658 File Offset: 0x00002858
		[NativeName("readType")]
		public readonly FileReadType ReadType { get; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000229 RID: 553 RVA: 0x00004660 File Offset: 0x00002860
		[NativeName("priorityLevel")]
		public readonly Priority PriorityLevel { get; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600022A RID: 554 RVA: 0x00004668 File Offset: 0x00002868
		[NativeName("subsystem")]
		public readonly AssetLoadingSubsystem Subsystem { get; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00004670 File Offset: 0x00002870
		[NativeName("requestTimeMicroseconds")]
		public readonly double RequestTimeMicroseconds { get; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600022C RID: 556 RVA: 0x00004678 File Offset: 0x00002878
		[NativeName("timeInQueueMicroseconds")]
		public readonly double TimeInQueueMicroseconds { get; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600022D RID: 557 RVA: 0x00004680 File Offset: 0x00002880
		[NativeName("totalTimeMicroseconds")]
		public readonly double TotalTimeMicroseconds { get; }
	}
}
