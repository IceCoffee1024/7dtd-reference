using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D5A RID: 7514
	[Serializable]
	public struct PhysicsMoverState
	{
		// Token: 0x0400A888 RID: 43144
		public Vector3 Position;

		// Token: 0x0400A889 RID: 43145
		public Quaternion Rotation;

		// Token: 0x0400A88A RID: 43146
		public Vector3 Velocity;

		// Token: 0x0400A88B RID: 43147
		public Vector3 AngularVelocity;
	}
}
