using System;

namespace Unity.Jobs.LowLevel.Unsafe
{
	// Token: 0x02000050 RID: 80
	public struct JobRanges
	{
		// Token: 0x04000101 RID: 257
		internal int BatchSize;

		// Token: 0x04000102 RID: 258
		internal int NumJobs;

		// Token: 0x04000103 RID: 259
		public int TotalIterationCount;

		// Token: 0x04000104 RID: 260
		internal IntPtr StartEndIndex;
	}
}
