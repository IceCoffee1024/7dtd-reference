using System;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x020004C3 RID: 1219
	public struct DiscLight
	{
		// Token: 0x04000FF3 RID: 4083
		public int instanceID;

		// Token: 0x04000FF4 RID: 4084
		public bool shadow;

		// Token: 0x04000FF5 RID: 4085
		public LightMode mode;

		// Token: 0x04000FF6 RID: 4086
		public Vector3 position;

		// Token: 0x04000FF7 RID: 4087
		public Quaternion orientation;

		// Token: 0x04000FF8 RID: 4088
		public LinearColor color;

		// Token: 0x04000FF9 RID: 4089
		public LinearColor indirectColor;

		// Token: 0x04000FFA RID: 4090
		public float range;

		// Token: 0x04000FFB RID: 4091
		public float radius;

		// Token: 0x04000FFC RID: 4092
		public FalloffType falloff;
	}
}
