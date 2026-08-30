using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D26 RID: 7462
	public interface IMoverController
	{
		// Token: 0x0600DC2B RID: 56363
		void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime);
	}
}
