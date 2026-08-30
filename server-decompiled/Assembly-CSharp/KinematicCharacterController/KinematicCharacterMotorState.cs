using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D52 RID: 7506
	[Serializable]
	public struct KinematicCharacterMotorState
	{
		// Token: 0x0400A7F8 RID: 43000
		public Vector3 Position;

		// Token: 0x0400A7F9 RID: 43001
		public Quaternion Rotation;

		// Token: 0x0400A7FA RID: 43002
		public Vector3 BaseVelocity;

		// Token: 0x0400A7FB RID: 43003
		public bool MustUnground;

		// Token: 0x0400A7FC RID: 43004
		public float MustUngroundTime;

		// Token: 0x0400A7FD RID: 43005
		public bool LastMovementIterationFoundAnyGround;

		// Token: 0x0400A7FE RID: 43006
		public CharacterTransientGroundingReport GroundingStatus;

		// Token: 0x0400A7FF RID: 43007
		public Rigidbody AttachedRigidbody;

		// Token: 0x0400A800 RID: 43008
		public Vector3 AttachedRigidbodyVelocity;
	}
}
