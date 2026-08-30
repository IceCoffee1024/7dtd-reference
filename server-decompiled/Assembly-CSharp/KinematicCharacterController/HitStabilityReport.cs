using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D56 RID: 7510
	public struct HitStabilityReport
	{
		// Token: 0x0400A811 RID: 43025
		public bool IsStable;

		// Token: 0x0400A812 RID: 43026
		public Vector3 InnerNormal;

		// Token: 0x0400A813 RID: 43027
		public Vector3 OuterNormal;

		// Token: 0x0400A814 RID: 43028
		public bool ValidStepDetected;

		// Token: 0x0400A815 RID: 43029
		public Collider SteppedCollider;

		// Token: 0x0400A816 RID: 43030
		public bool LedgeDetected;

		// Token: 0x0400A817 RID: 43031
		public bool IsOnEmptySideOfLedge;

		// Token: 0x0400A818 RID: 43032
		public float DistanceFromLedge;

		// Token: 0x0400A819 RID: 43033
		public bool IsMovingTowardsEmptySideOfLedge;

		// Token: 0x0400A81A RID: 43034
		public Vector3 LedgeGroundNormal;

		// Token: 0x0400A81B RID: 43035
		public Vector3 LedgeRightDirection;

		// Token: 0x0400A81C RID: 43036
		public Vector3 LedgeFacingDirection;
	}
}
