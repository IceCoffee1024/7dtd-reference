using System;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x020004C5 RID: 1221
	public struct SpotLightPyramidShape
	{
		// Token: 0x04001007 RID: 4103
		public int instanceID;

		// Token: 0x04001008 RID: 4104
		public bool shadow;

		// Token: 0x04001009 RID: 4105
		public LightMode mode;

		// Token: 0x0400100A RID: 4106
		public Vector3 position;

		// Token: 0x0400100B RID: 4107
		public Quaternion orientation;

		// Token: 0x0400100C RID: 4108
		public LinearColor color;

		// Token: 0x0400100D RID: 4109
		public LinearColor indirectColor;

		// Token: 0x0400100E RID: 4110
		public float range;

		// Token: 0x0400100F RID: 4111
		public float angle;

		// Token: 0x04001010 RID: 4112
		public float aspectRatio;

		// Token: 0x04001011 RID: 4113
		public FalloffType falloff;
	}
}
