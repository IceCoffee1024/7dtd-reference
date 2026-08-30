using System;

namespace Unity.Jobs.LowLevel.Unsafe
{
	// Token: 0x02000052 RID: 82
	[Obsolete("Reflection data is now universal between job types. The parameter can be removed.", false)]
	public enum JobType
	{
		// Token: 0x0400010B RID: 267
		Single,
		// Token: 0x0400010C RID: 268
		ParallelFor
	}
}
