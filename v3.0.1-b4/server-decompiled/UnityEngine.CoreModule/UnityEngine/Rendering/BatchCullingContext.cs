using System;
using Unity.Collections;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000444 RID: 1092
	[NativeHeader("Runtime/Camera/BatchRendererGroup.h")]
	[UsedByNativeCode]
	public struct BatchCullingContext
	{
		// Token: 0x06002468 RID: 9320 RVA: 0x0003D264 File Offset: 0x0003B464
		internal BatchCullingContext(NativeArray<Plane> inCullingPlanes, NativeArray<CullingSplit> inCullingSplits, LODParameters inLodParameters, Matrix4x4 inLocalToWorldMatrix, BatchCullingViewType inViewType, BatchCullingProjectionType inProjectionType, BatchCullingFlags inBatchCullingFlags, ulong inViewID, uint inCullingLayerMask, ulong inSceneCullingMask, int inReceiverPlaneOffset, int inReceiverPlaneCount)
		{
			this.cullingPlanes = inCullingPlanes;
			this.cullingSplits = inCullingSplits;
			this.lodParameters = inLodParameters;
			this.localToWorldMatrix = inLocalToWorldMatrix;
			this.viewType = inViewType;
			this.projectionType = inProjectionType;
			this.cullingFlags = inBatchCullingFlags;
			this.viewID = new BatchPackedCullingViewID
			{
				handle = inViewID
			};
			this.cullingLayerMask = inCullingLayerMask;
			this.sceneCullingMask = inSceneCullingMask;
			this.receiverPlaneOffset = inReceiverPlaneOffset;
			this.receiverPlaneCount = inReceiverPlaneCount;
			this.isOrthographic = 0;
		}

		// Token: 0x04000D85 RID: 3461
		public readonly NativeArray<Plane> cullingPlanes;

		// Token: 0x04000D86 RID: 3462
		public readonly NativeArray<CullingSplit> cullingSplits;

		// Token: 0x04000D87 RID: 3463
		public readonly LODParameters lodParameters;

		// Token: 0x04000D88 RID: 3464
		public readonly Matrix4x4 localToWorldMatrix;

		// Token: 0x04000D89 RID: 3465
		public readonly BatchCullingViewType viewType;

		// Token: 0x04000D8A RID: 3466
		public readonly BatchCullingProjectionType projectionType;

		// Token: 0x04000D8B RID: 3467
		public readonly BatchCullingFlags cullingFlags;

		// Token: 0x04000D8C RID: 3468
		public readonly BatchPackedCullingViewID viewID;

		// Token: 0x04000D8D RID: 3469
		public readonly uint cullingLayerMask;

		// Token: 0x04000D8E RID: 3470
		public readonly ulong sceneCullingMask;

		// Token: 0x04000D8F RID: 3471
		[Obsolete("BatchCullingContext.isOrthographic is deprecated. Use BatchCullingContext.projectionType instead.")]
		public readonly byte isOrthographic;

		// Token: 0x04000D90 RID: 3472
		public readonly int receiverPlaneOffset;

		// Token: 0x04000D91 RID: 3473
		public readonly int receiverPlaneCount;
	}
}
