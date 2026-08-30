using System;

namespace UnityEngine
{
	// Token: 0x0200029A RID: 666
	[Flags]
	public enum DrivenTransformProperties
	{
		// Token: 0x04000979 RID: 2425
		None = 0,
		// Token: 0x0400097A RID: 2426
		All = -1,
		// Token: 0x0400097B RID: 2427
		AnchoredPositionX = 2,
		// Token: 0x0400097C RID: 2428
		AnchoredPositionY = 4,
		// Token: 0x0400097D RID: 2429
		AnchoredPositionZ = 8,
		// Token: 0x0400097E RID: 2430
		Rotation = 16,
		// Token: 0x0400097F RID: 2431
		ScaleX = 32,
		// Token: 0x04000980 RID: 2432
		ScaleY = 64,
		// Token: 0x04000981 RID: 2433
		ScaleZ = 128,
		// Token: 0x04000982 RID: 2434
		AnchorMinX = 256,
		// Token: 0x04000983 RID: 2435
		AnchorMinY = 512,
		// Token: 0x04000984 RID: 2436
		AnchorMaxX = 1024,
		// Token: 0x04000985 RID: 2437
		AnchorMaxY = 2048,
		// Token: 0x04000986 RID: 2438
		SizeDeltaX = 4096,
		// Token: 0x04000987 RID: 2439
		SizeDeltaY = 8192,
		// Token: 0x04000988 RID: 2440
		PivotX = 16384,
		// Token: 0x04000989 RID: 2441
		PivotY = 32768,
		// Token: 0x0400098A RID: 2442
		AnchoredPosition = 6,
		// Token: 0x0400098B RID: 2443
		AnchoredPosition3D = 14,
		// Token: 0x0400098C RID: 2444
		Scale = 224,
		// Token: 0x0400098D RID: 2445
		AnchorMin = 768,
		// Token: 0x0400098E RID: 2446
		AnchorMax = 3072,
		// Token: 0x0400098F RID: 2447
		Anchors = 3840,
		// Token: 0x04000990 RID: 2448
		SizeDelta = 12288,
		// Token: 0x04000991 RID: 2449
		Pivot = 49152
	}
}
