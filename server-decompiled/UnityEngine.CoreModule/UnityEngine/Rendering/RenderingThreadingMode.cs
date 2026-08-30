using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering
{
	// Token: 0x02000427 RID: 1063
	[MovedFrom("UnityEngine.Experimental.Rendering")]
	public enum RenderingThreadingMode
	{
		// Token: 0x04000D00 RID: 3328
		Direct,
		// Token: 0x04000D01 RID: 3329
		SingleThreaded,
		// Token: 0x04000D02 RID: 3330
		MultiThreaded,
		// Token: 0x04000D03 RID: 3331
		LegacyJobified,
		// Token: 0x04000D04 RID: 3332
		NativeGraphicsJobs,
		// Token: 0x04000D05 RID: 3333
		NativeGraphicsJobsWithoutRenderThread
	}
}
