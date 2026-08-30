using System;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000175 RID: 373
	[NativeHeader("Runtime/Camera/SharedLightData.h")]
	public struct LightBakingOutput
	{
		// Token: 0x04000496 RID: 1174
		public int probeOcclusionLightIndex;

		// Token: 0x04000497 RID: 1175
		public int occlusionMaskChannel;

		// Token: 0x04000498 RID: 1176
		[NativeName("lightmapBakeMode.lightmapBakeType")]
		public LightmapBakeType lightmapBakeType;

		// Token: 0x04000499 RID: 1177
		[NativeName("lightmapBakeMode.mixedLightingMode")]
		public MixedLightingMode mixedLightingMode;

		// Token: 0x0400049A RID: 1178
		public bool isBaked;
	}
}
