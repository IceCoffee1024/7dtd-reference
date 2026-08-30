using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000441 RID: 1089
	public struct BatchCullingOutputDrawCommands
	{
		// Token: 0x04000D73 RID: 3443
		public unsafe BatchDrawCommand* drawCommands;

		// Token: 0x04000D74 RID: 3444
		public unsafe int* visibleInstances;

		// Token: 0x04000D75 RID: 3445
		public unsafe BatchDrawRange* drawRanges;

		// Token: 0x04000D76 RID: 3446
		public unsafe float* instanceSortingPositions;

		// Token: 0x04000D77 RID: 3447
		public unsafe int* drawCommandPickingInstanceIDs;

		// Token: 0x04000D78 RID: 3448
		public int drawCommandCount;

		// Token: 0x04000D79 RID: 3449
		public int visibleInstanceCount;

		// Token: 0x04000D7A RID: 3450
		public int drawRangeCount;

		// Token: 0x04000D7B RID: 3451
		public int instanceSortingPositionFloatCount;
	}
}
