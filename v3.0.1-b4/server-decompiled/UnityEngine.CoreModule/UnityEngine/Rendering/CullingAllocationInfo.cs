using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000456 RID: 1110
	internal struct CullingAllocationInfo
	{
		// Token: 0x04000E0B RID: 3595
		public unsafe VisibleLight* visibleLightsPtr;

		// Token: 0x04000E0C RID: 3596
		public unsafe VisibleLight* visibleOffscreenVertexLightsPtr;

		// Token: 0x04000E0D RID: 3597
		public unsafe VisibleReflectionProbe* visibleReflectionProbesPtr;

		// Token: 0x04000E0E RID: 3598
		public int visibleLightCount;

		// Token: 0x04000E0F RID: 3599
		public int visibleOffscreenVertexLightCount;

		// Token: 0x04000E10 RID: 3600
		public int visibleReflectionProbeCount;
	}
}
