using System;
using UnityEngine;

namespace WaterClippingTool
{
	// Token: 0x02001680 RID: 5760
	public class WaterClippingPlanePlacer : MonoBehaviour
	{
		// Token: 0x0600B487 RID: 46215 RVA: 0x00437E09 File Offset: 0x00436009
		public Plane GetPlane()
		{
			return new Plane(base.transform.forward, base.transform.position);
		}

		// Token: 0x04008743 RID: 34627
		public static readonly Plane DisabledPlane = new Plane(Vector3.up, 1000f);

		// Token: 0x04008744 RID: 34628
		public static readonly Vector4 DisabledPlaneVec = new Vector4(0f, 1f, 0f, 1000f);

		// Token: 0x04008745 RID: 34629
		public static readonly Vector3 DefaultModelOffset = new Vector3(1f, 0f, 1f);

		// Token: 0x04008746 RID: 34630
		public ShapeSettings liveSettings;
	}
}
