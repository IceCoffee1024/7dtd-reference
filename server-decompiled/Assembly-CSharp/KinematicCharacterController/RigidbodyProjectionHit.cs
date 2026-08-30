using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D57 RID: 7511
	public struct RigidbodyProjectionHit
	{
		// Token: 0x0400A81D RID: 43037
		public Rigidbody Rigidbody;

		// Token: 0x0400A81E RID: 43038
		public Vector3 HitPoint;

		// Token: 0x0400A81F RID: 43039
		public Vector3 EffectiveHitNormal;

		// Token: 0x0400A820 RID: 43040
		public Vector3 HitVelocity;

		// Token: 0x0400A821 RID: 43041
		public bool StableOnHit;
	}
}
