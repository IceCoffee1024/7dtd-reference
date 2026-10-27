using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001DB0 RID: 7600
	public interface ICharacterController
	{
		// Token: 0x0600E04F RID: 57423
		void UpdateRotation(ref Quaternion currentRotation, float deltaTime);

		// Token: 0x0600E050 RID: 57424
		void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime);

		// Token: 0x0600E051 RID: 57425
		void BeforeCharacterUpdate(float deltaTime);

		// Token: 0x0600E052 RID: 57426
		void PostGroundingUpdate(float deltaTime);

		// Token: 0x0600E053 RID: 57427
		void AfterCharacterUpdate(float deltaTime);

		// Token: 0x0600E054 RID: 57428
		bool IsColliderValidForCollisions(Collider coll);

		// Token: 0x0600E055 RID: 57429
		void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport);

		// Token: 0x0600E056 RID: 57430
		void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport);

		// Token: 0x0600E057 RID: 57431
		void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport);

		// Token: 0x0600E058 RID: 57432
		void OnDiscreteCollisionDetected(Collider hitCollider);

		// Token: 0x0600E059 RID: 57433
		bool OnCollisionOverlap(int nbOverlaps, Collider[] _internalProbedColliders);

		// Token: 0x0600E05A RID: 57434
		float GetCollisionOverlapScale(Transform overlappedTransform);
	}
}
