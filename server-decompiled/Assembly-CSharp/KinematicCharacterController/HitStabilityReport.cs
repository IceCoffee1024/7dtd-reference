using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D88 RID: 7560
	public struct HitStabilityReport
	{
		// Token: 0x0400A8E8 RID: 43240
		public bool IsStable;

		// Token: 0x0400A8E9 RID: 43241
		public Vector3 InnerNormal;

		// Token: 0x0400A8EA RID: 43242
		public Vector3 OuterNormal;

		// Token: 0x0400A8EB RID: 43243
		public bool ValidStepDetected;

		// Token: 0x0400A8EC RID: 43244
		public Collider SteppedCollider;

		// Token: 0x0400A8ED RID: 43245
		public bool LedgeDetected;

		// Token: 0x0400A8EE RID: 43246
		public bool IsOnEmptySideOfLedge;

		// Token: 0x0400A8EF RID: 43247
		public float DistanceFromLedge;

		// Token: 0x0400A8F0 RID: 43248
		public bool IsMovingTowardsEmptySideOfLedge;

		// Token: 0x0400A8F1 RID: 43249
		public Vector3 LedgeGroundNormal;

		// Token: 0x0400A8F2 RID: 43250
		public Vector3 LedgeRightDirection;

		// Token: 0x0400A8F3 RID: 43251
		public Vector3 LedgeFacingDirection;
	}
}
