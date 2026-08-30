using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D2E RID: 7470
	public struct HitStabilityReport
	{
		// Token: 0x0400A6DC RID: 42716
		public bool IsStable;

		// Token: 0x0400A6DD RID: 42717
		public Vector3 InnerNormal;

		// Token: 0x0400A6DE RID: 42718
		public Vector3 OuterNormal;

		// Token: 0x0400A6DF RID: 42719
		public bool ValidStepDetected;

		// Token: 0x0400A6E0 RID: 42720
		public Collider SteppedCollider;

		// Token: 0x0400A6E1 RID: 42721
		public bool LedgeDetected;

		// Token: 0x0400A6E2 RID: 42722
		public bool IsOnEmptySideOfLedge;

		// Token: 0x0400A6E3 RID: 42723
		public float DistanceFromLedge;

		// Token: 0x0400A6E4 RID: 42724
		public bool IsMovingTowardsEmptySideOfLedge;

		// Token: 0x0400A6E5 RID: 42725
		public Vector3 LedgeGroundNormal;

		// Token: 0x0400A6E6 RID: 42726
		public Vector3 LedgeRightDirection;

		// Token: 0x0400A6E7 RID: 42727
		public Vector3 LedgeFacingDirection;
	}
}
