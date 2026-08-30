using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x020004E2 RID: 1250
	[UsedByNativeCode]
	[NativeHeader("Runtime/Shaders/RayTracingAccelerationStructure.h")]
	[Flags]
	[NativeHeader("Runtime/Export/Graphics/RayTracingAccelerationStructure.bindings.h")]
	public enum RayTracingSubMeshFlags
	{
		// Token: 0x040010EF RID: 4335
		Disabled = 0,
		// Token: 0x040010F0 RID: 4336
		Enabled = 1,
		// Token: 0x040010F1 RID: 4337
		ClosestHitOnly = 2,
		// Token: 0x040010F2 RID: 4338
		UniqueAnyHitCalls = 4
	}
}
