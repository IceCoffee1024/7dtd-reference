using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000425 RID: 1061
	[Flags]
	public enum RTClearFlags
	{
		// Token: 0x04000CF2 RID: 3314
		None = 0,
		// Token: 0x04000CF3 RID: 3315
		Color = 1,
		// Token: 0x04000CF4 RID: 3316
		Depth = 2,
		// Token: 0x04000CF5 RID: 3317
		Stencil = 4,
		// Token: 0x04000CF6 RID: 3318
		All = 7,
		// Token: 0x04000CF7 RID: 3319
		DepthStencil = 6,
		// Token: 0x04000CF8 RID: 3320
		ColorDepth = 3,
		// Token: 0x04000CF9 RID: 3321
		ColorStencil = 5
	}
}
