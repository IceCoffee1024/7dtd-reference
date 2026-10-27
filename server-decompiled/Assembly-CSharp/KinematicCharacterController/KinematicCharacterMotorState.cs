using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001DB5 RID: 7605
	[Serializable]
	public struct KinematicCharacterMotorState
	{
		// Token: 0x0400A9B8 RID: 43448
		public Vector3 Position;

		// Token: 0x0400A9B9 RID: 43449
		public Quaternion Rotation;

		// Token: 0x0400A9BA RID: 43450
		public Vector3 BaseVelocity;

		// Token: 0x0400A9BB RID: 43451
		public bool MustUnground;

		// Token: 0x0400A9BC RID: 43452
		public float MustUngroundTime;

		// Token: 0x0400A9BD RID: 43453
		public bool LastMovementIterationFoundAnyGround;

		// Token: 0x0400A9BE RID: 43454
		public CharacterTransientGroundingReport GroundingStatus;

		// Token: 0x0400A9BF RID: 43455
		public Rigidbody AttachedRigidbody;

		// Token: 0x0400A9C0 RID: 43456
		public Vector3 AttachedRigidbodyVelocity;
	}
}
