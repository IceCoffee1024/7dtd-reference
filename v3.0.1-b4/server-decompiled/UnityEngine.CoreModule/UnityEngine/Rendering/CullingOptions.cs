using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000451 RID: 1105
	[Flags]
	public enum CullingOptions
	{
		// Token: 0x04000DE1 RID: 3553
		None = 0,
		// Token: 0x04000DE2 RID: 3554
		ForceEvenIfCameraIsNotActive = 1,
		// Token: 0x04000DE3 RID: 3555
		OcclusionCull = 2,
		// Token: 0x04000DE4 RID: 3556
		NeedsLighting = 4,
		// Token: 0x04000DE5 RID: 3557
		NeedsReflectionProbes = 8,
		// Token: 0x04000DE6 RID: 3558
		Stereo = 16,
		// Token: 0x04000DE7 RID: 3559
		DisablePerObjectCulling = 32,
		// Token: 0x04000DE8 RID: 3560
		ShadowCasters = 64
	}
}
