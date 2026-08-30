using System;

namespace UnityEngine
{
	// Token: 0x0200010E RID: 270
	public struct BoundingSphere
	{
		// Token: 0x0600064C RID: 1612 RVA: 0x0000900E File Offset: 0x0000720E
		public BoundingSphere(Vector3 pos, float rad)
		{
			this.position = pos;
			this.radius = rad;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0000901F File Offset: 0x0000721F
		public BoundingSphere(Vector4 packedSphere)
		{
			this.position = new Vector3(packedSphere.x, packedSphere.y, packedSphere.z);
			this.radius = packedSphere.w;
		}

		// Token: 0x04000387 RID: 903
		public Vector3 position;

		// Token: 0x04000388 RID: 904
		public float radius;
	}
}
