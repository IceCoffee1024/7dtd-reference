using System;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x020004C1 RID: 1217
	public struct SpotLight
	{
		// Token: 0x04000FDB RID: 4059
		public int instanceID;

		// Token: 0x04000FDC RID: 4060
		public bool shadow;

		// Token: 0x04000FDD RID: 4061
		public LightMode mode;

		// Token: 0x04000FDE RID: 4062
		public Vector3 position;

		// Token: 0x04000FDF RID: 4063
		public Quaternion orientation;

		// Token: 0x04000FE0 RID: 4064
		public LinearColor color;

		// Token: 0x04000FE1 RID: 4065
		public LinearColor indirectColor;

		// Token: 0x04000FE2 RID: 4066
		public float range;

		// Token: 0x04000FE3 RID: 4067
		public float sphereRadius;

		// Token: 0x04000FE4 RID: 4068
		public float coneAngle;

		// Token: 0x04000FE5 RID: 4069
		public float innerConeAngle;

		// Token: 0x04000FE6 RID: 4070
		public FalloffType falloff;

		// Token: 0x04000FE7 RID: 4071
		public AngularFalloffType angularFalloff;
	}
}
