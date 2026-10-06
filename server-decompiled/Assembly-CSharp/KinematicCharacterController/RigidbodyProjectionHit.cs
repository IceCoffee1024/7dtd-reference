using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D89 RID: 7561
	public struct RigidbodyProjectionHit
	{
		// Token: 0x0400A8F4 RID: 43252
		public Rigidbody Rigidbody;

		// Token: 0x0400A8F5 RID: 43253
		public Vector3 HitPoint;

		// Token: 0x0400A8F6 RID: 43254
		public Vector3 EffectiveHitNormal;

		// Token: 0x0400A8F7 RID: 43255
		public Vector3 HitVelocity;

		// Token: 0x0400A8F8 RID: 43256
		public bool StableOnHit;
	}
}
