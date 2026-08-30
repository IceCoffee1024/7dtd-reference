using System;
using UnityEngine;

namespace WaterClippingTool
{
	// Token: 0x02001646 RID: 5702
	public class WaterClippingPlanePlacer : MonoBehaviour
	{
		// Token: 0x0600B2F8 RID: 45816 RVA: 0x0042EDED File Offset: 0x0042CFED
		public Plane GetPlane()
		{
			return new Plane(base.transform.forward, base.transform.position);
		}

		// Token: 0x04008614 RID: 34324
		public static readonly Plane DisabledPlane = new Plane(Vector3.up, 1000f);

		// Token: 0x04008615 RID: 34325
		public static readonly Vector4 DisabledPlaneVec = new Vector4(0f, 1f, 0f, 1000f);

		// Token: 0x04008616 RID: 34326
		public static readonly Vector3 DefaultModelOffset = new Vector3(1f, 0f, 1f);

		// Token: 0x04008617 RID: 34327
		public ShapeSettings liveSettings;
	}
}
