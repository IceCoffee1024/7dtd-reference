using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D8C RID: 7564
	[Serializable]
	public struct PhysicsMoverState
	{
		// Token: 0x0400A95F RID: 43359
		public Vector3 Position;

		// Token: 0x0400A960 RID: 43360
		public Quaternion Rotation;

		// Token: 0x0400A961 RID: 43361
		public Vector3 Velocity;

		// Token: 0x0400A962 RID: 43362
		public Vector3 AngularVelocity;
	}
}
