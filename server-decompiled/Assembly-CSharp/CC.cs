using System;
using KinematicCharacterController;
using UnityEngine;

// Token: 0x02000507 RID: 1287
public class CC : ICharacterController
{
	// Token: 0x060029EF RID: 10735 RVA: 0x0010811C File Offset: 0x0010631C
	public void Move()
	{
		this.collisionFlags = CollisionFlags.None;
		this.hadWallOverlap = false;
		this.tickCount++;
		this.motor.UpdatePhase1(0.05f);
		this.motor.UpdatePhase2(0.05f);
		this.motor.Transform.SetPositionAndRotation(this.motor.TransientPosition, this.motor.TransientRotation);
	}

	// Token: 0x060029F0 RID: 10736 RVA: 0x000027FC File Offset: 0x000009FC
	public void BeforeCharacterUpdate(float deltaTime)
	{
	}

	// Token: 0x060029F1 RID: 10737 RVA: 0x0010818C File Offset: 0x0010638C
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

	// Token: 0x060029F2 RID: 10738 RVA: 0x0010826C File Offset: 0x0010646C
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

	// Token: 0x060029F3 RID: 10739 RVA: 0x000027FC File Offset: 0x000009FC
	public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
	{
	}

	// Token: 0x060029F4 RID: 10740 RVA: 0x001082A8 File Offset: 0x001064A8
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

	// Token: 0x060029F5 RID: 10741 RVA: 0x000027FC File Offset: 0x000009FC
	public void AfterCharacterUpdate(float deltaTime)
	{
	}

	// Token: 0x060029F6 RID: 10742 RVA: 0x00020119 File Offset: 0x0001E319
	public bool IsColliderValidForCollisions(Collider coll)
	{
		return true;
	}

	// Token: 0x060029F7 RID: 10743 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnDiscreteCollisionDetected(Collider hitCollider)
	{
	}

	// Token: 0x060029F8 RID: 10744 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
	{
	}

	// Token: 0x060029F9 RID: 10745 RVA: 0x001083A2 File Offset: 0x001065A2
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

	// Token: 0x060029FA RID: 10746 RVA: 0x000027FC File Offset: 0x000009FC
	public void PostGroundingUpdate(float deltaTime)
	{
	}

	// Token: 0x060029FB RID: 10747 RVA: 0x000027FC File Offset: 0x000009FC
	public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
	{
	}

	// Token: 0x04001FF0 RID: 8176
	public Entity entity;

	// Token: 0x04001FF1 RID: 8177
	public KinematicCharacterMotor motor;

	// Token: 0x04001FF2 RID: 8178
	public CollisionFlags collisionFlags;

	// Token: 0x04001FF3 RID: 8179
	public Vector3 vel;

	// Token: 0x04001FF4 RID: 8180
	[PublicizedFrom(EAccessModifier.Private)]
	public int tickCount;

	// Token: 0x04001FF5 RID: 8181
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hadWallOverlap;
}
