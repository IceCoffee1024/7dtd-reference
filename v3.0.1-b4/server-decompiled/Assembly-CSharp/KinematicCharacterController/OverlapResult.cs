using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D2B RID: 7467
	public struct OverlapResult
	{
		// Token: 0x0600DC2C RID: 56364 RVA: 0x004EF0FB File Offset: 0x004ED2FB
		public OverlapResult(Vector3 normal, Collider collider)
		{
			this.Normal = normal;
			this.Collider = collider;
		}

		// Token: 0x0400A6CC RID: 42700
		public Vector3 Normal;

		// Token: 0x0400A6CD RID: 42701
		public Collider Collider;
	}
}
