using System;

namespace Unity.Profiling.Memory
{
	// Token: 0x02000071 RID: 113
	[Flags]
	public enum CaptureFlags : uint
	{
		// Token: 0x0400019F RID: 415
		ManagedObjects = 1U,
		// Token: 0x040001A0 RID: 416
		NativeObjects = 2U,
		// Token: 0x040001A1 RID: 417
		NativeAllocations = 4U,
		// Token: 0x040001A2 RID: 418
		NativeAllocationSites = 8U,
		// Token: 0x040001A3 RID: 419
		NativeStackTraces = 16U
	}
}
