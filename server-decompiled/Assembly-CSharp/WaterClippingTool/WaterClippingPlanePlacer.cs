using System;
using UnityEngine;

namespace WaterClippingTool
{
	// Token: 0x02001692 RID: 5778
	public class WaterClippingPlanePlacer : MonoBehaviour
	{
		// Token: 0x0600B4DF RID: 46303 RVA: 0x0043A20D File Offset: 0x0043840D
		public Plane GetPlane()
		{
			return new Plane(base.transform.forward, base.transform.position);
		}

		// Token: 0x04008784 RID: 34692
		public static readonly Plane DisabledPlane = new Plane(Vector3.up, 1000f);

		// Token: 0x04008785 RID: 34693
		public static readonly Vector4 DisabledPlaneVec = new Vector4(0f, 1f, 0f, 1000f);

		// Token: 0x04008786 RID: 34694
		public static readonly Vector3 DefaultModelOffset = new Vector3(1f, 0f, 1f);

		// Token: 0x04008787 RID: 34695
		public ShapeSettings liveSettings;
	}
}
