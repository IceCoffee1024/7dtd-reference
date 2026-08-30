using System;
using UnityEngine;

namespace WaterClippingTool
{
	// Token: 0x02001661 RID: 5729
	public class WaterClippingPlanePlacer : MonoBehaviour
	{
		// Token: 0x0600B3F5 RID: 46069 RVA: 0x0043412D File Offset: 0x0043232D
		public Plane GetPlane()
		{
			return new Plane(base.transform.forward, base.transform.position);
		}

		// Token: 0x040086ED RID: 34541
		public static readonly Plane DisabledPlane = new Plane(Vector3.up, 1000f);

		// Token: 0x040086EE RID: 34542
		public static readonly Vector4 DisabledPlaneVec = new Vector4(0f, 1f, 0f, 1000f);

		// Token: 0x040086EF RID: 34543
		public static readonly Vector3 DefaultModelOffset = new Vector3(1f, 0f, 1f);

		// Token: 0x040086F0 RID: 34544
		public ShapeSettings liveSettings;
	}
}
