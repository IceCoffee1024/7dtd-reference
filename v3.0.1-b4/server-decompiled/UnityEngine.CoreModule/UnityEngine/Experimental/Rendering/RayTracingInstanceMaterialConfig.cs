using System;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x020004E6 RID: 1254
	public struct RayTracingInstanceMaterialConfig
	{
		// Token: 0x04001102 RID: 4354
		public int renderQueueLowerBound;

		// Token: 0x04001103 RID: 4355
		public int renderQueueUpperBound;

		// Token: 0x04001104 RID: 4356
		public RayTracingInstanceCullingShaderTagConfig[] optionalShaderTags;

		// Token: 0x04001105 RID: 4357
		public string[] optionalShaderKeywords;
	}
}
