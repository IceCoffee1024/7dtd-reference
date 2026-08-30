using System;
using KinematicCharacterController;
using UnityEngine;

// Token: 0x02000500 RID: 1280
public class CharacterControllerKinematic : CharacterControllerAbstract
{
	// Token: 0x060029E6 RID: 10726 RVA: 0x00107584 File Offset: 0x00105784
	public CharacterControllerKinematic(Entity _entity)
	{
		GameObject gameObject = _entity.PhysicsTransform.gameObject;
		KinematicCharacterSystem.EnsureCreation();
		this.cs = KinematicCharacterSystem.GetInstance();
		KinematicCharacterSystem.AutoSimulation = false;
		KinematicCharacterSystem.Interpolate = false;
		this.motor = gameObject.AddComponent<KinematicCharacterMotor>();
		this.motor.StepHandling = StepHandlingMethod.Extra;
		this.motor.AllowSteppingWithoutStableGrounding = true;
		this.motor.InteractiveRigidbodyHandling = false;
		this.motor.LedgeAndDenivelationHandling = false;
		this.motor.MaxStableSlopeAngle = 63.8f;
		this.cc = new CC();
		this.cc.entity = _entity;
		this.cc.motor = this.motor;
		this.motor.CharacterController = this.cc;
		this.motor.ForceUnground(0.1f);
	}

	// Token: 0x060029E7 RID: 10727 RVA: 0x00107654 File Offset: 0x00105854
	public override void Enable(bool isEnabled)
	{
		this.motor.enabled = isEnabled;
	}

	// Token: 0x060029E8 RID: 10728 RVA: 0x00107662 File Offset: 0x00105862
	public override void SetStepOffset(float _stepOffset)
	{
		this.motor.MaxStepHeight = _stepOffset + 0.01f;
	}

	// Token: 0x060029E9 RID: 10729 RVA: 0x00107676 File Offset: 0x00105876
	public override float GetStepOffset()
	{
		return this.motor.MaxStepHeight;
	}

	// Token: 0x060029EA RID: 10730 RVA: 0x00107683 File Offset: 0x00105883
	public override void SetSize(Vector3 _center, float _height, float _radius)
	{
		this.motor.SetCapsuleDimensions(_radius, _height, _center.y);
	}

	// Token: 0x060029EB RID: 10731 RVA: 0x00107698 File Offset: 0x00105898
	public override void SetCenter(Vector3 _center)
	{
		this.motor.SetCapsuleDimensions(this.GetRadius(), this.GetHeight(), _center.y);
	}

	// Token: 0x060029EC RID: 10732 RVA: 0x001076B7 File Offset: 0x001058B7
	public override Vector3 GetCenter()
	{
		return this.motor.CharacterTransformToCapsuleCenter;
	}

	// Token: 0x060029ED RID: 10733 RVA: 0x001076C4 File Offset: 0x001058C4
	public override void SetRadius(float _radius)
	{
		this.motor.SetCapsuleDimensions(_radius, this.GetHeight(), this.GetCenter().y);
	}

	// Token: 0x060029EE RID: 10734 RVA: 0x001076E3 File Offset: 0x001058E3
	public override float GetRadius()
	{
		return this.motor.Capsule.radius;
	}

	// Token: 0x060029EF RID: 10735 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetSkinWidth(float _width)
	{
	}

	// Token: 0x060029F0 RID: 10736 RVA: 0x001076F5 File Offset: 0x001058F5
	public override float GetSkinWidth()
	{
		return 0.08f;
	}

	// Token: 0x060029F1 RID: 10737 RVA: 0x001076FC File Offset: 0x001058FC
	public override void SetHeight(float _height)
	{
		float radius = this.GetRadius();
		_height = Utils.FastMax(_height, radius * 2f);
		this.motor.SetCapsuleDimensions(radius, _height, _height * 0.5f);
	}

	// Token: 0x060029F2 RID: 10738 RVA: 0x00107733 File Offset: 0x00105933
	public override float GetHeight()
	{
		return this.motor.Capsule.height;
	}

	// Token: 0x060029F3 RID: 10739 RVA: 0x00107745 File Offset: 0x00105945
	public override bool IsGrounded()
	{
		return (this.cc.collisionFlags & CollisionFlags.Below) > CollisionFlags.None;
	}

	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x060029F4 RID: 10740 RVA: 0x00107757 File Offset: 0x00105957
	public override Vector3 GroundNormal
	{
		get
		{
			return this.motor.GroundingStatus.GroundNormal;
		}
	}

	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x060029F5 RID: 10741 RVA: 0x00087DB8 File Offset: 0x00085FB8
	// (set) Token: 0x060029F6 RID: 10742 RVA: 0x00087DB8 File Offset: 0x00085FB8
	public override bool enableOverlapRecovery
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x060029F7 RID: 10743 RVA: 0x00107769 File Offset: 0x00105969
	public override CollisionFlags Move(Vector3 _dir)
	{
		if (_dir.y >= 0.011f)
		{
			this.motor.ForceUnground(0.11f);
		}
		this.cc.vel = _dir / 0.05f;
		return this.Update();
	}

	// Token: 0x060029F8 RID: 10744 RVA: 0x001077A4 File Offset: 0x001059A4
	public override CollisionFlags Update()
	{
		this.cc.Move();
		if (this.motor.GroundingStatus.FoundAnyGround)
		{
			this.cc.collisionFlags |= CollisionFlags.Below;
		}
		return this.cc.collisionFlags;
	}

	// Token: 0x060029F9 RID: 10745 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Rotate(Quaternion _dir)
	{
	}

	// Token: 0x04001FE5 RID: 8165
	[PublicizedFrom(EAccessModifier.Private)]
	public KinematicCharacterSystem cs;

	// Token: 0x04001FE6 RID: 8166
	[PublicizedFrom(EAccessModifier.Private)]
	public KinematicCharacterMotor motor;

	// Token: 0x04001FE7 RID: 8167
	[PublicizedFrom(EAccessModifier.Private)]
	public CC cc;
}
