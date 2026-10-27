using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001DBA RID: 7610
	public struct RigidbodyProjectionHit
	{
		// Token: 0x0400A9DD RID: 43485
		public Rigidbody Rigidbody;

		// Token: 0x0400A9DE RID: 43486
		public Vector3 HitPoint;

		// Token: 0x0400A9DF RID: 43487
		public Vector3 EffectiveHitNormal;

		// Token: 0x0400A9E0 RID: 43488
		public Vector3 HitVelocity;

		// Token: 0x0400A9E1 RID: 43489
		public bool StableOnHit;
	}
}
