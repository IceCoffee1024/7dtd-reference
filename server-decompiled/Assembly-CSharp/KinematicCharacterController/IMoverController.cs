using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D80 RID: 7552
	public interface IMoverController
	{
		// Token: 0x0600DF47 RID: 57159
		void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime);
	}
}
