using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200043E RID: 1086
	public struct BatchDrawCommand
	{
		// Token: 0x04000D60 RID: 3424
		public uint visibleOffset;

		// Token: 0x04000D61 RID: 3425
		public uint visibleCount;

		// Token: 0x04000D62 RID: 3426
		public BatchID batchID;

		// Token: 0x04000D63 RID: 3427
		public BatchMaterialID materialID;

		// Token: 0x04000D64 RID: 3428
		public BatchMeshID meshID;

		// Token: 0x04000D65 RID: 3429
		public ushort submeshIndex;

		// Token: 0x04000D66 RID: 3430
		public ushort splitVisibilityMask;

		// Token: 0x04000D67 RID: 3431
		public BatchDrawCommandFlags flags;

		// Token: 0x04000D68 RID: 3432
		public int sortingPosition;
	}
}
