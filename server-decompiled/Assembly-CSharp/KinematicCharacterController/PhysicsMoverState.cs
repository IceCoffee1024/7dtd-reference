using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001DBD RID: 7613
	[Serializable]
	public struct PhysicsMoverState
	{
		// Token: 0x0400AA48 RID: 43592
		public Vector3 Position;

		// Token: 0x0400AA49 RID: 43593
		public Quaternion Rotation;

		// Token: 0x0400AA4A RID: 43594
		public Vector3 Velocity;

		// Token: 0x0400AA4B RID: 43595
		public Vector3 AngularVelocity;
	}
}
