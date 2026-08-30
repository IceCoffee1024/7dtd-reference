using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x020004EA RID: 1258
	public struct RayTracingInstanceCullingConfig
	{
		// Token: 0x0400110F RID: 4367
		public RayTracingInstanceCullingFlags flags;

		// Token: 0x04001110 RID: 4368
		public Vector3 sphereCenter;

		// Token: 0x04001111 RID: 4369
		public float sphereRadius;

		// Token: 0x04001112 RID: 4370
		public Plane[] planes;

		// Token: 0x04001113 RID: 4371
		public RayTracingInstanceCullingTest[] instanceTests;

		// Token: 0x04001114 RID: 4372
		public RayTracingInstanceCullingMaterialTest materialTest;

		// Token: 0x04001115 RID: 4373
		public RayTracingInstanceMaterialConfig transparentMaterialConfig;

		// Token: 0x04001116 RID: 4374
		public RayTracingInstanceMaterialConfig alphaTestedMaterialConfig;

		// Token: 0x04001117 RID: 4375
		public RayTracingSubMeshFlagsConfig subMeshFlagsConfig;

		// Token: 0x04001118 RID: 4376
		public RayTracingInstanceTriangleCullingConfig triangleCullingConfig;

		// Token: 0x04001119 RID: 4377
		public LODParameters lodParameters;
	}
}
