using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000443 RID: 1091
	[NativeHeader("Runtime/Camera/BatchRendererGroup.h")]
	[UsedByNativeCode]
	public struct CullingSplit
	{
		// Token: 0x04000D7E RID: 3454
		public Vector3 sphereCenter;

		// Token: 0x04000D7F RID: 3455
		public float sphereRadius;

		// Token: 0x04000D80 RID: 3456
		public int cullingPlaneOffset;

		// Token: 0x04000D81 RID: 3457
		public int cullingPlaneCount;

		// Token: 0x04000D82 RID: 3458
		public float cascadeBlendCullingFactor;

		// Token: 0x04000D83 RID: 3459
		public float nearPlane;

		// Token: 0x04000D84 RID: 3460
		public Matrix4x4 cullingMatrix;
	}
}
