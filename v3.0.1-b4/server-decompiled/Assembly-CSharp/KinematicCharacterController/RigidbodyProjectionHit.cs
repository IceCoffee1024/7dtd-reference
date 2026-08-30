using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D2F RID: 7471
	public struct RigidbodyProjectionHit
	{
		// Token: 0x0400A6E8 RID: 42728
		public Rigidbody Rigidbody;

		// Token: 0x0400A6E9 RID: 42729
		public Vector3 HitPoint;

		// Token: 0x0400A6EA RID: 42730
		public Vector3 EffectiveHitNormal;

		// Token: 0x0400A6EB RID: 42731
		public Vector3 HitVelocity;

		// Token: 0x0400A6EC RID: 42732
		public bool StableOnHit;
	}
}
