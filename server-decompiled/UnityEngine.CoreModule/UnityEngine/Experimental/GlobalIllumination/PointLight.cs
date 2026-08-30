using System;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x020004C0 RID: 1216
	public struct PointLight
	{
		// Token: 0x04000FD1 RID: 4049
		public int instanceID;

		// Token: 0x04000FD2 RID: 4050
		public bool shadow;

		// Token: 0x04000FD3 RID: 4051
		public LightMode mode;

		// Token: 0x04000FD4 RID: 4052
		public Vector3 position;

		// Token: 0x04000FD5 RID: 4053
		public Quaternion orientation;

		// Token: 0x04000FD6 RID: 4054
		public LinearColor color;

		// Token: 0x04000FD7 RID: 4055
		public LinearColor indirectColor;

		// Token: 0x04000FD8 RID: 4056
		public float range;

		// Token: 0x04000FD9 RID: 4057
		public float sphereRadius;

		// Token: 0x04000FDA RID: 4058
		public FalloffType falloff;
	}
}
