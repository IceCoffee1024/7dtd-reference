using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D7F RID: 7551
	public interface ICharacterController
	{
		// Token: 0x0600DF3B RID: 57147
		void UpdateRotation(ref Quaternion currentRotation, float deltaTime);

		// Token: 0x0600DF3C RID: 57148
		void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime);

		// Token: 0x0600DF3D RID: 57149
		void BeforeCharacterUpdate(float deltaTime);

		// Token: 0x0600DF3E RID: 57150
		void PostGroundingUpdate(float deltaTime);

		// Token: 0x0600DF3F RID: 57151
		void AfterCharacterUpdate(float deltaTime);

		// Token: 0x0600DF40 RID: 57152
		bool IsColliderValidForCollisions(Collider coll);

		// Token: 0x0600DF41 RID: 57153
		void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport);

		// Token: 0x0600DF42 RID: 57154
		void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport);

		// Token: 0x0600DF43 RID: 57155
		void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport);

		// Token: 0x0600DF44 RID: 57156
		void OnDiscreteCollisionDetected(Collider hitCollider);

		// Token: 0x0600DF45 RID: 57157
		bool OnCollisionOverlap(int nbOverlaps, Collider[] _internalProbedColliders);

		// Token: 0x0600DF46 RID: 57158
		float GetCollisionOverlapScale(Transform overlappedTransform);
	}
}
