using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000401 RID: 1025
	[Flags]
	public enum ShadowMapPass
	{
		// Token: 0x04000BE7 RID: 3047
		PointlightPositiveX = 1,
		// Token: 0x04000BE8 RID: 3048
		PointlightNegativeX = 2,
		// Token: 0x04000BE9 RID: 3049
		PointlightPositiveY = 4,
		// Token: 0x04000BEA RID: 3050
		PointlightNegativeY = 8,
		// Token: 0x04000BEB RID: 3051
		PointlightPositiveZ = 16,
		// Token: 0x04000BEC RID: 3052
		PointlightNegativeZ = 32,
		// Token: 0x04000BED RID: 3053
		DirectionalCascade0 = 64,
		// Token: 0x04000BEE RID: 3054
		DirectionalCascade1 = 128,
		// Token: 0x04000BEF RID: 3055
		DirectionalCascade2 = 256,
		// Token: 0x04000BF0 RID: 3056
		DirectionalCascade3 = 512,
		// Token: 0x04000BF1 RID: 3057
		Spotlight = 1024,
		// Token: 0x04000BF2 RID: 3058
		Pointlight = 63,
		// Token: 0x04000BF3 RID: 3059
		Directional = 960,
		// Token: 0x04000BF4 RID: 3060
		All = 2047
	}
}
