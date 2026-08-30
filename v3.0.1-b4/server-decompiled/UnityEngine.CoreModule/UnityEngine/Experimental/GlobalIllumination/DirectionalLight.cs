using System;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x020004BF RID: 1215
	public struct DirectionalLight
	{
		// Token: 0x04000FC8 RID: 4040
		public int instanceID;

		// Token: 0x04000FC9 RID: 4041
		public bool shadow;

		// Token: 0x04000FCA RID: 4042
		public LightMode mode;

		// Token: 0x04000FCB RID: 4043
		public Vector3 position;

		// Token: 0x04000FCC RID: 4044
		public Quaternion orientation;

		// Token: 0x04000FCD RID: 4045
		public LinearColor color;

		// Token: 0x04000FCE RID: 4046
		public LinearColor indirectColor;

		// Token: 0x04000FCF RID: 4047
		public float penumbraWidthRadian;

		// Token: 0x04000FD0 RID: 4048
		[Obsolete("Directional lights support cookies now. In order to position the cookie projection in the world, a position and full orientation are necessary. Use the position and orientation members instead of the direction parameter.", true)]
		public Vector3 direction;
	}
}
