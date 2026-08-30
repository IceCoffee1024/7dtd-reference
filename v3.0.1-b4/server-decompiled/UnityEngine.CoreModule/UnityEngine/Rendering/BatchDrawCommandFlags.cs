using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000438 RID: 1080
	[Flags]
	public enum BatchDrawCommandFlags
	{
		// Token: 0x04000D47 RID: 3399
		None = 0,
		// Token: 0x04000D48 RID: 3400
		FlipWinding = 1,
		// Token: 0x04000D49 RID: 3401
		HasMotion = 2,
		// Token: 0x04000D4A RID: 3402
		IsLightMapped = 4,
		// Token: 0x04000D4B RID: 3403
		HasSortingPosition = 8,
		// Token: 0x04000D4C RID: 3404
		LODCrossFade = 16
	}
}
