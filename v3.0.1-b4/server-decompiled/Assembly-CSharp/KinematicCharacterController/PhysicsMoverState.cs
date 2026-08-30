using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D32 RID: 7474
	[Serializable]
	public struct PhysicsMoverState
	{
		// Token: 0x0400A753 RID: 42835
		public Vector3 Position;

		// Token: 0x0400A754 RID: 42836
		public Quaternion Rotation;

		// Token: 0x0400A755 RID: 42837
		public Vector3 Velocity;

		// Token: 0x0400A756 RID: 42838
		public Vector3 AngularVelocity;
	}
}
