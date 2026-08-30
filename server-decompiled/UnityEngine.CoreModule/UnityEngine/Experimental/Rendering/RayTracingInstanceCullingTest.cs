using System;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x020004E4 RID: 1252
	public struct RayTracingInstanceCullingTest
	{
		// Token: 0x040010FA RID: 4346
		public uint instanceMask;

		// Token: 0x040010FB RID: 4347
		public int layerMask;

		// Token: 0x040010FC RID: 4348
		public int shadowCastingModeMask;

		// Token: 0x040010FD RID: 4349
		public bool allowOpaqueMaterials;

		// Token: 0x040010FE RID: 4350
		public bool allowTransparentMaterials;

		// Token: 0x040010FF RID: 4351
		public bool allowAlphaTestedMaterials;
	}
}
