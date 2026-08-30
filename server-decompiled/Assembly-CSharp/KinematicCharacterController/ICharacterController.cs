using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D4D RID: 7501
	public interface ICharacterController
	{
		// Token: 0x0600DDC0 RID: 56768
		void UpdateRotation(ref Quaternion currentRotation, float deltaTime);

		// Token: 0x0600DDC1 RID: 56769
		void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime);

		// Token: 0x0600DDC2 RID: 56770
		void BeforeCharacterUpdate(float deltaTime);

		// Token: 0x0600DDC3 RID: 56771
		void PostGroundingUpdate(float deltaTime);

		// Token: 0x0600DDC4 RID: 56772
		void AfterCharacterUpdate(float deltaTime);

		// Token: 0x0600DDC5 RID: 56773
		bool IsColliderValidForCollisions(Collider coll);

		// Token: 0x0600DDC6 RID: 56774
		void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport);

		// Token: 0x0600DDC7 RID: 56775
		void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport);

		// Token: 0x0600DDC8 RID: 56776
		void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport);

		// Token: 0x0600DDC9 RID: 56777
		void OnDiscreteCollisionDetected(Collider hitCollider);

		// Token: 0x0600DDCA RID: 56778
		bool OnCollisionOverlap(int nbOverlaps, Collider[] _internalProbedColliders);

		// Token: 0x0600DDCB RID: 56779
		float GetCollisionOverlapScale(Transform overlappedTransform);
	}
}
