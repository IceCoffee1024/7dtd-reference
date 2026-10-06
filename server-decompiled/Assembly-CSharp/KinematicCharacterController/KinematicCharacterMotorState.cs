using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D84 RID: 7556
	[Serializable]
	public struct KinematicCharacterMotorState
	{
		// Token: 0x0400A8CF RID: 43215
		public Vector3 Position;

		// Token: 0x0400A8D0 RID: 43216
		public Quaternion Rotation;

		// Token: 0x0400A8D1 RID: 43217
		public Vector3 BaseVelocity;

		// Token: 0x0400A8D2 RID: 43218
		public bool MustUnground;

		// Token: 0x0400A8D3 RID: 43219
		public float MustUngroundTime;

		// Token: 0x0400A8D4 RID: 43220
		public bool LastMovementIterationFoundAnyGround;

		// Token: 0x0400A8D5 RID: 43221
		public CharacterTransientGroundingReport GroundingStatus;

		// Token: 0x0400A8D6 RID: 43222
		public Rigidbody AttachedRigidbody;

		// Token: 0x0400A8D7 RID: 43223
		public Vector3 AttachedRigidbodyVelocity;
	}
}
