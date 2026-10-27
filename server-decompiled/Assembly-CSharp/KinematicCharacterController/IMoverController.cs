using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001DB1 RID: 7601
	public interface IMoverController
	{
		// Token: 0x0600E05B RID: 57435
		void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime);
	}
}
