using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000403 RID: 1027
	public enum PassType
	{
		// Token: 0x04000C0E RID: 3086
		Normal,
		// Token: 0x04000C0F RID: 3087
		Vertex,
		// Token: 0x04000C10 RID: 3088
		VertexLM,
		// Token: 0x04000C11 RID: 3089
		[Obsolete("VertexLMRGBM PassType is obsolete. Please use VertexLM PassType together with DecodeLightmap shader function.")]
		VertexLMRGBM,
		// Token: 0x04000C12 RID: 3090
		ForwardBase,
		// Token: 0x04000C13 RID: 3091
		ForwardAdd,
		// Token: 0x04000C14 RID: 3092
		[Obsolete("Deferred Lighting was removed, so LightPrePassBase pass type is never used anymore.")]
		LightPrePassBase,
		// Token: 0x04000C15 RID: 3093
		[Obsolete("Deferred Lighting was removed, so LightPrePassFinal pass type is never used anymore.")]
		LightPrePassFinal,
		// Token: 0x04000C16 RID: 3094
		ShadowCaster,
		// Token: 0x04000C17 RID: 3095
		Deferred = 10,
		// Token: 0x04000C18 RID: 3096
		Meta,
		// Token: 0x04000C19 RID: 3097
		MotionVectors,
		// Token: 0x04000C1A RID: 3098
		ScriptableRenderPipeline,
		// Token: 0x04000C1B RID: 3099
		ScriptableRenderPipelineDefaultUnlit,
		// Token: 0x04000C1C RID: 3100
		GrabPass
	}
}
