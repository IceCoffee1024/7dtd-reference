using System;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x020004E3 RID: 1251
	[Flags]
	public enum RayTracingInstanceCullingFlags
	{
		// Token: 0x040010F4 RID: 4340
		None = 0,
		// Token: 0x040010F5 RID: 4341
		EnableSphereCulling = 1,
		// Token: 0x040010F6 RID: 4342
		EnablePlaneCulling = 2,
		// Token: 0x040010F7 RID: 4343
		EnableLODCulling = 4,
		// Token: 0x040010F8 RID: 4344
		ComputeMaterialsCRC = 8,
		// Token: 0x040010F9 RID: 4345
		IgnoreReflectionProbes = 16
	}
}
