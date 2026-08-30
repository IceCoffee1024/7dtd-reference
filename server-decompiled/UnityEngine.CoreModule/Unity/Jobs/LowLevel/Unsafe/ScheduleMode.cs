using System;

namespace Unity.Jobs.LowLevel.Unsafe
{
	// Token: 0x02000051 RID: 81
	public enum ScheduleMode
	{
		// Token: 0x04000106 RID: 262
		Run,
		// Token: 0x04000107 RID: 263
		[Obsolete("Batched is obsolete, use Parallel or Single depending on job type. (UnityUpgradable) -> Parallel", false)]
		Batched,
		// Token: 0x04000108 RID: 264
		Parallel = 1,
		// Token: 0x04000109 RID: 265
		Single
	}
}
