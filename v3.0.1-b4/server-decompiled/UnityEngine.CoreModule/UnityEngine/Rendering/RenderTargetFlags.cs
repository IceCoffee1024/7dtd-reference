using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200040C RID: 1036
	[Flags]
	public enum RenderTargetFlags
	{
		// Token: 0x04000C60 RID: 3168
		None = 0,
		// Token: 0x04000C61 RID: 3169
		ReadOnlyDepth = 1,
		// Token: 0x04000C62 RID: 3170
		ReadOnlyStencil = 2,
		// Token: 0x04000C63 RID: 3171
		ReadOnlyDepthStencil = 3
	}
}
