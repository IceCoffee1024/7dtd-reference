using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000460 RID: 1120
	[Flags]
	public enum PerObjectData
	{
		// Token: 0x04000E39 RID: 3641
		None = 0,
		// Token: 0x04000E3A RID: 3642
		LightProbe = 1,
		// Token: 0x04000E3B RID: 3643
		ReflectionProbes = 2,
		// Token: 0x04000E3C RID: 3644
		LightProbeProxyVolume = 4,
		// Token: 0x04000E3D RID: 3645
		Lightmaps = 8,
		// Token: 0x04000E3E RID: 3646
		LightData = 16,
		// Token: 0x04000E3F RID: 3647
		MotionVectors = 32,
		// Token: 0x04000E40 RID: 3648
		LightIndices = 64,
		// Token: 0x04000E41 RID: 3649
		ReflectionProbeData = 128,
		// Token: 0x04000E42 RID: 3650
		OcclusionProbe = 256,
		// Token: 0x04000E43 RID: 3651
		OcclusionProbeProxyVolume = 512,
		// Token: 0x04000E44 RID: 3652
		ShadowMask = 1024
	}
}
