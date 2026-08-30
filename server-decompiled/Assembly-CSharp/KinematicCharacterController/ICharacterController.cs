using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D25 RID: 7461
	public interface ICharacterController
	{
		// Token: 0x0600DC1F RID: 56351
		void UpdateRotation(ref Quaternion currentRotation, float deltaTime);

		// Token: 0x0600DC20 RID: 56352
		void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime);

		// Token: 0x0600DC21 RID: 56353
		void BeforeCharacterUpdate(float deltaTime);

		// Token: 0x0600DC22 RID: 56354
		void PostGroundingUpdate(float deltaTime);

		// Token: 0x0600DC23 RID: 56355
		void AfterCharacterUpdate(float deltaTime);

		// Token: 0x0600DC24 RID: 56356
		bool IsColliderValidForCollisions(Collider coll);

		// Token: 0x0600DC25 RID: 56357
		void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport);

		// Token: 0x0600DC26 RID: 56358
		void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport);

		// Token: 0x0600DC27 RID: 56359
		void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport);

		// Token: 0x0600DC28 RID: 56360
		void OnDiscreteCollisionDetected(Collider hitCollider);

		// Token: 0x0600DC29 RID: 56361
		bool OnCollisionOverlap(int nbOverlaps, Collider[] _internalProbedColliders);

		// Token: 0x0600DC2A RID: 56362
		float GetCollisionOverlapScale(Transform overlappedTransform);
	}
}
