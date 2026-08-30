using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001D7 RID: 471
	[UsedByNativeCode]
	[Serializable]
	public struct CustomRenderTextureUpdateZone
	{
		// Token: 0x0400065E RID: 1630
		public Vector3 updateZoneCenter;

		// Token: 0x0400065F RID: 1631
		public Vector3 updateZoneSize;

		// Token: 0x04000660 RID: 1632
		public float rotation;

		// Token: 0x04000661 RID: 1633
		public int passIndex;

		// Token: 0x04000662 RID: 1634
		public bool needSwap;
	}
}
