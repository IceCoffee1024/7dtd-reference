using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200041C RID: 1052
	[Flags]
	public enum CopyTextureSupport
	{
		// Token: 0x04000CCD RID: 3277
		None = 0,
		// Token: 0x04000CCE RID: 3278
		Basic = 1,
		// Token: 0x04000CCF RID: 3279
		Copy3D = 2,
		// Token: 0x04000CD0 RID: 3280
		DifferentTypes = 4,
		// Token: 0x04000CD1 RID: 3281
		TextureToRT = 8,
		// Token: 0x04000CD2 RID: 3282
		RTToTexture = 16
	}
}
