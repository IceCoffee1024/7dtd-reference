using System;

namespace Unity.Profiling.LowLevel
{
	// Token: 0x0200006A RID: 106
	public enum ProfilerMarkerDataType : byte
	{
		// Token: 0x04000167 RID: 359
		InstanceId = 1,
		// Token: 0x04000168 RID: 360
		Int32,
		// Token: 0x04000169 RID: 361
		UInt32,
		// Token: 0x0400016A RID: 362
		Int64,
		// Token: 0x0400016B RID: 363
		UInt64,
		// Token: 0x0400016C RID: 364
		Float,
		// Token: 0x0400016D RID: 365
		Double,
		// Token: 0x0400016E RID: 366
		String16 = 9,
		// Token: 0x0400016F RID: 367
		Blob8 = 11,
		// Token: 0x04000170 RID: 368
		GfxResourceId
	}
}
