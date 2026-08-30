using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000417 RID: 1047
	public enum BuiltinShaderType
	{
		// Token: 0x04000C8E RID: 3214
		DeferredShading,
		// Token: 0x04000C8F RID: 3215
		DeferredReflections,
		// Token: 0x04000C90 RID: 3216
		[Obsolete("LegacyDeferredLighting has been removed.", false)]
		LegacyDeferredLighting,
		// Token: 0x04000C91 RID: 3217
		ScreenSpaceShadows,
		// Token: 0x04000C92 RID: 3218
		DepthNormals,
		// Token: 0x04000C93 RID: 3219
		MotionVectors,
		// Token: 0x04000C94 RID: 3220
		LightHalo,
		// Token: 0x04000C95 RID: 3221
		LensFlare
	}
}
