using System;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x020004DD RID: 1245
	public enum FormatUsage
	{
		// Token: 0x0400103E RID: 4158
		Sample,
		// Token: 0x0400103F RID: 4159
		Linear,
		// Token: 0x04001040 RID: 4160
		Sparse,
		// Token: 0x04001041 RID: 4161
		Render = 4,
		// Token: 0x04001042 RID: 4162
		Blend,
		// Token: 0x04001043 RID: 4163
		GetPixels,
		// Token: 0x04001044 RID: 4164
		SetPixels,
		// Token: 0x04001045 RID: 4165
		SetPixels32,
		// Token: 0x04001046 RID: 4166
		ReadPixels,
		// Token: 0x04001047 RID: 4167
		LoadStore,
		// Token: 0x04001048 RID: 4168
		MSAA2x,
		// Token: 0x04001049 RID: 4169
		MSAA4x,
		// Token: 0x0400104A RID: 4170
		MSAA8x,
		// Token: 0x0400104B RID: 4171
		StencilSampling = 16
	}
}
