using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200042B RID: 1067
	public enum SynchronisationStageFlags
	{
		// Token: 0x04000D16 RID: 3350
		VertexProcessing = 1,
		// Token: 0x04000D17 RID: 3351
		PixelProcessing,
		// Token: 0x04000D18 RID: 3352
		ComputeProcessing = 4,
		// Token: 0x04000D19 RID: 3353
		AllGPUOperations = 7
	}
}
