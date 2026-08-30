using System;
using Unity.Jobs;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000446 RID: 1094
	[UsedByNativeCode]
	[NativeHeader("Runtime/Camera/BatchRendererGroup.h")]
	internal struct BatchRendererCullingOutput
	{
		// Token: 0x04000D93 RID: 3475
		public JobHandle cullingJobsFence;

		// Token: 0x04000D94 RID: 3476
		public Matrix4x4 localToWorldMatrix;

		// Token: 0x04000D95 RID: 3477
		public unsafe Plane* cullingPlanes;

		// Token: 0x04000D96 RID: 3478
		public int cullingPlaneCount;

		// Token: 0x04000D97 RID: 3479
		public int receiverPlaneOffset;

		// Token: 0x04000D98 RID: 3480
		public int receiverPlaneCount;

		// Token: 0x04000D99 RID: 3481
		public unsafe CullingSplit* cullingSplits;

		// Token: 0x04000D9A RID: 3482
		public int cullingSplitCount;

		// Token: 0x04000D9B RID: 3483
		public BatchCullingViewType viewType;

		// Token: 0x04000D9C RID: 3484
		public BatchCullingProjectionType projectionType;

		// Token: 0x04000D9D RID: 3485
		public BatchCullingFlags cullingFlags;

		// Token: 0x04000D9E RID: 3486
		public ulong viewID;

		// Token: 0x04000D9F RID: 3487
		public uint cullingLayerMask;

		// Token: 0x04000DA0 RID: 3488
		public ulong sceneCullingMask;

		// Token: 0x04000DA1 RID: 3489
		public unsafe BatchCullingOutputDrawCommands* drawCommands;
	}
}
