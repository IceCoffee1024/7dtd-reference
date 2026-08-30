using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200048B RID: 1163
	[Flags]
	public enum ShaderPropertyFlags
	{
		// Token: 0x04000F2B RID: 3883
		None = 0,
		// Token: 0x04000F2C RID: 3884
		HideInInspector = 1,
		// Token: 0x04000F2D RID: 3885
		PerRendererData = 2,
		// Token: 0x04000F2E RID: 3886
		NoScaleOffset = 4,
		// Token: 0x04000F2F RID: 3887
		Normal = 8,
		// Token: 0x04000F30 RID: 3888
		HDR = 16,
		// Token: 0x04000F31 RID: 3889
		Gamma = 32,
		// Token: 0x04000F32 RID: 3890
		NonModifiableTextureData = 64,
		// Token: 0x04000F33 RID: 3891
		MainTexture = 128,
		// Token: 0x04000F34 RID: 3892
		MainColor = 256
	}
}
