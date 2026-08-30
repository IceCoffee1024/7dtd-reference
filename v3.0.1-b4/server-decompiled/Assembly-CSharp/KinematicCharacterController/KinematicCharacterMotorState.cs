using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D2A RID: 7466
	[Serializable]
	public struct KinematicCharacterMotorState
	{
		// Token: 0x0400A6C3 RID: 42691
		public Vector3 Position;

		// Token: 0x0400A6C4 RID: 42692
		public Quaternion Rotation;

		// Token: 0x0400A6C5 RID: 42693
		public Vector3 BaseVelocity;

		// Token: 0x0400A6C6 RID: 42694
		public bool MustUnground;

		// Token: 0x0400A6C7 RID: 42695
		public float MustUngroundTime;

		// Token: 0x0400A6C8 RID: 42696
		public bool LastMovementIterationFoundAnyGround;

		// Token: 0x0400A6C9 RID: 42697
		public CharacterTransientGroundingReport GroundingStatus;

		// Token: 0x0400A6CA RID: 42698
		public Rigidbody AttachedRigidbody;

		// Token: 0x0400A6CB RID: 42699
		public Vector3 AttachedRigidbodyVelocity;
	}
}
