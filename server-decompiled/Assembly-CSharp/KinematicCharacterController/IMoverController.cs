using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D4E RID: 7502
	public interface IMoverController
	{
		// Token: 0x0600DDCC RID: 56780
		void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime);
	}
}
