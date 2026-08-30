using System;

namespace UnityEngine
{
	// Token: 0x0200017B RID: 379
	public enum RenderingPath
	{
		// Token: 0x040004A1 RID: 1185
		UsePlayerSettings = -1,
		// Token: 0x040004A2 RID: 1186
		VertexLit,
		// Token: 0x040004A3 RID: 1187
		Forward,
		// Token: 0x040004A4 RID: 1188
		[Obsolete("DeferredLighting has been removed. Use DeferredShading, Forward or HDRP/URP instead.", false)]
		DeferredLighting,
		// Token: 0x040004A5 RID: 1189
		DeferredShading
	}
}
