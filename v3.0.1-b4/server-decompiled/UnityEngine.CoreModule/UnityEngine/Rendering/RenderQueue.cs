using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003F1 RID: 1009
	public enum RenderQueue
	{
		// Token: 0x04000B56 RID: 2902
		Background = 1000,
		// Token: 0x04000B57 RID: 2903
		Geometry = 2000,
		// Token: 0x04000B58 RID: 2904
		AlphaTest = 2450,
		// Token: 0x04000B59 RID: 2905
		GeometryLast = 2500,
		// Token: 0x04000B5A RID: 2906
		Transparent = 3000,
		// Token: 0x04000B5B RID: 2907
		Overlay = 4000
	}
}
