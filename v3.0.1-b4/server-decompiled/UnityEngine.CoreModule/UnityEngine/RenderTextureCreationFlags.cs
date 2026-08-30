using System;

namespace UnityEngine
{
	// Token: 0x020001A5 RID: 421
	[Flags]
	public enum RenderTextureCreationFlags
	{
		// Token: 0x040005D1 RID: 1489
		MipMap = 1,
		// Token: 0x040005D2 RID: 1490
		AutoGenerateMips = 2,
		// Token: 0x040005D3 RID: 1491
		SRGB = 4,
		// Token: 0x040005D4 RID: 1492
		EyeTexture = 8,
		// Token: 0x040005D5 RID: 1493
		EnableRandomWrite = 16,
		// Token: 0x040005D6 RID: 1494
		CreatedFromScript = 32,
		// Token: 0x040005D7 RID: 1495
		AllowVerticalFlip = 128,
		// Token: 0x040005D8 RID: 1496
		NoResolvedColorSurface = 256,
		// Token: 0x040005D9 RID: 1497
		DynamicallyScalable = 1024,
		// Token: 0x040005DA RID: 1498
		BindMS = 2048
	}
}
