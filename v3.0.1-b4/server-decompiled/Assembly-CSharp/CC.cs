using System;
using KinematicCharacterController;
using UnityEngine;

// Token: 0x020004FC RID: 1276
public class CC : ICharacterController
{
	// Token: 0x060029D0 RID: 10704 RVA: 0x001067A8 File Offset: 0x001049A8
	public void Move()
	{
		this.collisionFlags = CollisionFlags.None;
		this.hadWallOverlap = false;
		this.tickCount++;
		this.motor.UpdatePhase1(0.05f);
		this.motor.UpdatePhase2(0.05f);
		this.motor.Transform.SetPositionAndRotation(this.motor.TransientPosition, this.motor.TransientRotation);
	}

	// Token: 0x060029D1 RID: 10705 RVA: 0x000027FC File Offset: 0x000009FC
	public void BeforeCharacterUpdate(float deltaTime)
	{
	}

	// Token: 0x060029D2 RID: 10706 RVA: 0x00106818 File Offset: 0x00104A18
	public bool OnCollisionOverlap(int nbOverlaps, Collider[] _colliders)
	{
		Vector3 position = this.motor.Transform.position;
		bool flag;
		do
		{
			flag = false;
			for (int i = 0; i < nbOverlaps - 1; i++)
			{
				Collider collider = _colliders[i];
				Collider collider2 = _colliders[i + 1];
				if (collider.gameObject.layer == 15)
				{
					if (collider2.gameObject.layer != 15)
					{
						_colliders[i] = collider2;
						_colliders[i + 1] = collider;
						flag = true;
					}
					else
					{
						float sqrMagnitude = (collider.transform.position - position).sqrMagnitude;
						if ((collider2.transform.position - position).sqrMagnitude < sqrMagnitude)
						{
							_colliders[i] = collider2;
							_colliders[i + 1] = collider;
							flag = true;
						}
					}
				}
			}
		}
		while (flag);
		if (_colliders[0].gameObject.layer != 15)
		{
			this.hadWallOverlap = true;
		}
		else if (this.hadWallOverlap)
		{
			return false;
		}
		return true;
	}

	// Token: 0x060029D3 RID: 10707 RVA: 0x001068F8 File Offset: 0x00104AF8
	public float GetCollisionOverlapScale(Transform overlappedTransform)
	{
		if (overlappedTransform.gameObject.layer != 15)
		{
			return 1f;
		}
		if (((this.entity.entityId + this.tickCount) & 15) != 0)
		{
			return 0.1f;
		}
		return 0.5f;
	}

	// Token: 0x060029D4 RID: 10708 RVA: 0x000027FC File Offset: 0x000009FC
	public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
	{
	}

	// Token: 0x060029D5 RID: 10709 RVA: 0x00106934 File Offset: 0x00104B34
	public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
	{
		if (this.vel.y <= 0.001f)
		{
			if (this.motor.GroundingStatus.IsStableOnGround)
			{
				Vector3 groundNormal = this.motor.GroundingStatus.GroundNormal;
				this.vel.y = 0f;
				float magnitude = this.vel.magnitude;
				Vector3 vector = Vector3.Cross(this.vel, this.motor.CharacterUp);
				this.vel = Vector3.Cross(groundNormal, vector).normalized * magnitude;
				this.vel = this.vel * 0.5f + currentVelocity * 0.5f;
			}
			else
			{
				this.vel = this.vel * 0.5f + currentVelocity * 0.5f;
			}
		}
		currentVelocity = this.vel;
	}

	// Token: 0x060029D6 RID: 10710 RVA: 0x000027FC File Offset: 0x000009FC
	public void AfterCharacterUpdate(float deltaTime)
	{
	}

	// Token: 0x060029D7 RID: 10711 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public bool IsColliderValidForCollisions(Collider coll)
	{
		return true;
	}

	// Token: 0x060029D8 RID: 10712 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnDiscreteCollisionDetected(Collider hitCollider)
	{
	}

	// Token: 0x060029D9 RID: 10713 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
	{
	}

	// Token: 0x060029DA RID: 10714 RVA: 0x00106A2E File Offset: 0x00104C2E
	public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
	{
		if (hitNormal.y >= 0.64f)
		{
			this.collisionFlags |= CollisionFlags.Below;
			return;
		}
		if (hitNormal.y > -0.5f)
		{
			this.collisionFlags |= CollisionFlags.Sides;
		}
	}

	// Token: 0x060029DB RID: 10715 RVA: 0x000027FC File Offset: 0x000009FC
	public void PostGroundingUpdate(float deltaTime)
	{
	}

	// Token: 0x060029DC RID: 10716 RVA: 0x000027FC File Offset: 0x000009FC
	public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
	{
	}

	// Token: 0x04001FB9 RID: 8121
	public Entity entity;

	// Token: 0x04001FBA RID: 8122
	public KinematicCharacterMotor motor;

	// Token: 0x04001FBB RID: 8123
	public CollisionFlags collisionFlags;

	// Token: 0x04001FBC RID: 8124
	public Vector3 vel;

	// Token: 0x04001FBD RID: 8125
	[PublicizedFrom(EAccessModifier.Private)]
	public int tickCount;

	// Token: 0x04001FBE RID: 8126
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hadWallOverlap;
}
