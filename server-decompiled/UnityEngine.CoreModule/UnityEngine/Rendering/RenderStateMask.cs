using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200046D RID: 1133
	[Flags]
	public enum RenderStateMask
	{
		// Token: 0x04000E85 RID: 3717
		Nothing = 0,
		// Token: 0x04000E86 RID: 3718
		Blend = 1,
		// Token: 0x04000E87 RID: 3719
		Raster = 2,
		// Token: 0x04000E88 RID: 3720
		Depth = 4,
		// Token: 0x04000E89 RID: 3721
		Stencil = 8,
		// Token: 0x04000E8A RID: 3722
		Everything = 15
	}
}
