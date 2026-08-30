using System;

namespace Unity.Profiling
{
	// Token: 0x0200005F RID: 95
	[Flags]
	public enum ProfilerCounterOptions : ushort
	{
		// Token: 0x04000139 RID: 313
		None = 0,
		// Token: 0x0400013A RID: 314
		FlushOnEndOfFrame = 2,
		// Token: 0x0400013B RID: 315
		ResetToZeroOnFlush = 4
	}
}
