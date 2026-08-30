using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000402 RID: 1026
	public enum BuiltinRenderTextureType
	{
		// Token: 0x04000BF6 RID: 3062
		PropertyName = -4,
		// Token: 0x04000BF7 RID: 3063
		BufferPtr,
		// Token: 0x04000BF8 RID: 3064
		RenderTexture,
		// Token: 0x04000BF9 RID: 3065
		BindableTexture,
		// Token: 0x04000BFA RID: 3066
		None,
		// Token: 0x04000BFB RID: 3067
		CurrentActive,
		// Token: 0x04000BFC RID: 3068
		CameraTarget,
		// Token: 0x04000BFD RID: 3069
		Depth,
		// Token: 0x04000BFE RID: 3070
		DepthNormals,
		// Token: 0x04000BFF RID: 3071
		ResolvedDepth,
		// Token: 0x04000C00 RID: 3072
		[Obsolete("Deferred Lighting has been removed, so PrepassNormalsSpec built-in render texture type is never used now.", false)]
		PrepassNormalsSpec = 7,
		// Token: 0x04000C01 RID: 3073
		[Obsolete("Deferred Lighting has been removed, so PrepassLight built-in render texture type is never used now.", false)]
		PrepassLight,
		// Token: 0x04000C02 RID: 3074
		[Obsolete("Deferred Lighting has been removed, so PrepassLightSpec built-in render texture type is never used now.", false)]
		PrepassLightSpec,
		// Token: 0x04000C03 RID: 3075
		GBuffer0,
		// Token: 0x04000C04 RID: 3076
		GBuffer1,
		// Token: 0x04000C05 RID: 3077
		GBuffer2,
		// Token: 0x04000C06 RID: 3078
		GBuffer3,
		// Token: 0x04000C07 RID: 3079
		Reflections,
		// Token: 0x04000C08 RID: 3080
		MotionVectors,
		// Token: 0x04000C09 RID: 3081
		GBuffer4,
		// Token: 0x04000C0A RID: 3082
		GBuffer5,
		// Token: 0x04000C0B RID: 3083
		GBuffer6,
		// Token: 0x04000C0C RID: 3084
		GBuffer7
	}
}
