using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001DB9 RID: 7609
	public struct HitStabilityReport
	{
		// Token: 0x0400A9D1 RID: 43473
		public bool IsStable;

		// Token: 0x0400A9D2 RID: 43474
		public Vector3 InnerNormal;

		// Token: 0x0400A9D3 RID: 43475
		public Vector3 OuterNormal;

		// Token: 0x0400A9D4 RID: 43476
		public bool ValidStepDetected;

		// Token: 0x0400A9D5 RID: 43477
		public Collider SteppedCollider;

		// Token: 0x0400A9D6 RID: 43478
		public bool LedgeDetected;

		// Token: 0x0400A9D7 RID: 43479
		public bool IsOnEmptySideOfLedge;

		// Token: 0x0400A9D8 RID: 43480
		public float DistanceFromLedge;

		// Token: 0x0400A9D9 RID: 43481
		public bool IsMovingTowardsEmptySideOfLedge;

		// Token: 0x0400A9DA RID: 43482
		public Vector3 LedgeGroundNormal;

		// Token: 0x0400A9DB RID: 43483
		public Vector3 LedgeRightDirection;

		// Token: 0x0400A9DC RID: 43484
		public Vector3 LedgeFacingDirection;
	}
}
