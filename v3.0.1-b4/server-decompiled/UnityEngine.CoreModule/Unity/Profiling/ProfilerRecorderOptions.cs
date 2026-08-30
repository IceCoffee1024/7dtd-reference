using System;

namespace Unity.Profiling
{
	// Token: 0x02000062 RID: 98
	[Flags]
	public enum ProfilerRecorderOptions
	{
		// Token: 0x04000140 RID: 320
		None = 0,
		// Token: 0x04000141 RID: 321
		StartImmediately = 1,
		// Token: 0x04000142 RID: 322
		KeepAliveDuringDomainReload = 2,
		// Token: 0x04000143 RID: 323
		CollectOnlyOnCurrentThread = 4,
		// Token: 0x04000144 RID: 324
		WrapAroundWhenCapacityReached = 8,
		// Token: 0x04000145 RID: 325
		SumAllSamplesInFrame = 16,
		// Token: 0x04000146 RID: 326
		GpuRecorder = 64,
		// Token: 0x04000147 RID: 327
		Default = 24
	}
}
