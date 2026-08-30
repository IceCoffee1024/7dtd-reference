using System;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x020004C2 RID: 1218
	public struct RectangleLight
	{
		// Token: 0x04000FE8 RID: 4072
		public int instanceID;

		// Token: 0x04000FE9 RID: 4073
		public bool shadow;

		// Token: 0x04000FEA RID: 4074
		public LightMode mode;

		// Token: 0x04000FEB RID: 4075
		public Vector3 position;

		// Token: 0x04000FEC RID: 4076
		public Quaternion orientation;

		// Token: 0x04000FED RID: 4077
		public LinearColor color;

		// Token: 0x04000FEE RID: 4078
		public LinearColor indirectColor;

		// Token: 0x04000FEF RID: 4079
		public float range;

		// Token: 0x04000FF0 RID: 4080
		public float width;

		// Token: 0x04000FF1 RID: 4081
		public float height;

		// Token: 0x04000FF2 RID: 4082
		public FalloffType falloff;
	}
}
