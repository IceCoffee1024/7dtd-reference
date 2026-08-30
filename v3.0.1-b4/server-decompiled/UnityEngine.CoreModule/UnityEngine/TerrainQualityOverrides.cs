using System;

namespace UnityEngine
{
	// Token: 0x020001BB RID: 443
	[Flags]
	public enum TerrainQualityOverrides
	{
		// Token: 0x0400062B RID: 1579
		None = 0,
		// Token: 0x0400062C RID: 1580
		PixelError = 1,
		// Token: 0x0400062D RID: 1581
		BasemapDistance = 2,
		// Token: 0x0400062E RID: 1582
		DetailDensity = 4,
		// Token: 0x0400062F RID: 1583
		DetailDistance = 8,
		// Token: 0x04000630 RID: 1584
		TreeDistance = 16,
		// Token: 0x04000631 RID: 1585
		BillboardStart = 32,
		// Token: 0x04000632 RID: 1586
		FadeLength = 64,
		// Token: 0x04000633 RID: 1587
		MaxTrees = 128
	}
}
