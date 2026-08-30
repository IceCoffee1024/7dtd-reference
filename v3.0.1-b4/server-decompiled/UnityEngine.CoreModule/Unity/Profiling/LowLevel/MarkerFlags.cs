using System;

namespace Unity.Profiling.LowLevel
{
	// Token: 0x02000069 RID: 105
	[Flags]
	public enum MarkerFlags : ushort
	{
		// Token: 0x0400015D RID: 349
		Default = 0,
		// Token: 0x0400015E RID: 350
		Script = 2,
		// Token: 0x0400015F RID: 351
		ScriptInvoke = 32,
		// Token: 0x04000160 RID: 352
		ScriptDeepProfiler = 64,
		// Token: 0x04000161 RID: 353
		AvailabilityEditor = 4,
		// Token: 0x04000162 RID: 354
		AvailabilityNonDevelopment = 8,
		// Token: 0x04000163 RID: 355
		Warning = 16,
		// Token: 0x04000164 RID: 356
		Counter = 128,
		// Token: 0x04000165 RID: 357
		SampleGPU = 256
	}
}
