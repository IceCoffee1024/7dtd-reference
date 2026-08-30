using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000477 RID: 1143
	[Flags]
	public enum SortingCriteria
	{
		// Token: 0x04000EAD RID: 3757
		None = 0,
		// Token: 0x04000EAE RID: 3758
		SortingLayer = 1,
		// Token: 0x04000EAF RID: 3759
		RenderQueue = 2,
		// Token: 0x04000EB0 RID: 3760
		BackToFront = 4,
		// Token: 0x04000EB1 RID: 3761
		QuantizedFrontToBack = 8,
		// Token: 0x04000EB2 RID: 3762
		OptimizeStateChanges = 16,
		// Token: 0x04000EB3 RID: 3763
		CanvasOrder = 32,
		// Token: 0x04000EB4 RID: 3764
		RendererPriority = 64,
		// Token: 0x04000EB5 RID: 3765
		CommonOpaque = 59,
		// Token: 0x04000EB6 RID: 3766
		CommonTransparent = 23
	}
}
