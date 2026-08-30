using System;
using KinematicCharacterController;
using UnityEngine;

// Token: 0x020004FB RID: 1275
public class CharacterControllerKinematic : CharacterControllerAbstract
{
	// Token: 0x060029BC RID: 10684 RVA: 0x00106548 File Offset: 0x00104748
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

	// Token: 0x060029BD RID: 10685 RVA: 0x00106618 File Offset: 0x00104818
	public override void Enable(bool isEnabled)
	{
		this.motor.enabled = isEnabled;
	}

	// Token: 0x060029BE RID: 10686 RVA: 0x00106626 File Offset: 0x00104826
	public override void SetStepOffset(float _stepOffset)
	{
		this.motor.MaxStepHeight = _stepOffset + 0.01f;
	}

	// Token: 0x060029BF RID: 10687 RVA: 0x0010663A File Offset: 0x0010483A
	public override float GetStepOffset()
	{
		return this.motor.MaxStepHeight;
	}

	// Token: 0x060029C0 RID: 10688 RVA: 0x00106647 File Offset: 0x00104847
	public override void SetSize(Vector3 _center, float _height, float _radius)
	{
		this.motor.SetCapsuleDimensions(_radius, _height, _center.y);
	}

	// Token: 0x060029C1 RID: 10689 RVA: 0x0010665C File Offset: 0x0010485C
	public override void SetCenter(Vector3 _center)
	{
		this.motor.SetCapsuleDimensions(this.GetRadius(), this.GetHeight(), _center.y);
	}

	// Token: 0x060029C2 RID: 10690 RVA: 0x0010667B File Offset: 0x0010487B
	public override Vector3 GetCenter()
	{
		return this.motor.CharacterTransformToCapsuleCenter;
	}

	// Token: 0x060029C3 RID: 10691 RVA: 0x00106688 File Offset: 0x00104888
	public override void SetRadius(float _radius)
	{
		this.motor.SetCapsuleDimensions(_radius, this.GetHeight(), this.GetCenter().y);
	}

	// Token: 0x060029C4 RID: 10692 RVA: 0x001066A7 File Offset: 0x001048A7
	public override float GetRadius()
	{
		return this.motor.Capsule.radius;
	}

	// Token: 0x060029C5 RID: 10693 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetSkinWidth(float _width)
	{
	}

	// Token: 0x060029C6 RID: 10694 RVA: 0x001066B9 File Offset: 0x001048B9
	public override float GetSkinWidth()
	{
		return 0.08f;
	}

	// Token: 0x060029C7 RID: 10695 RVA: 0x001066C0 File Offset: 0x001048C0
	public override void SetHeight(float _height)
	{
		float radius = this.GetRadius();
		_height = Utils.FastMax(_height, radius * 2f);
		this.motor.SetCapsuleDimensions(radius, _height, _height * 0.5f);
	}

	// Token: 0x060029C8 RID: 10696 RVA: 0x001066F7 File Offset: 0x001048F7
	public override float GetHeight()
	{
		return this.motor.Capsule.height;
	}

	// Token: 0x060029C9 RID: 10697 RVA: 0x00106709 File Offset: 0x00104909
	public override bool IsGrounded()
	{
		return (this.cc.collisionFlags & CollisionFlags.Below) > CollisionFlags.None;
	}

	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x060029CA RID: 10698 RVA: 0x0010671B File Offset: 0x0010491B
	public override Vector3 GroundNormal
	{
		get
		{
			return this.motor.GroundingStatus.GroundNormal;
		}
	}

	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x060029CB RID: 10699 RVA: 0x000876FD File Offset: 0x000858FD
	// (set) Token: 0x060029CC RID: 10700 RVA: 0x000876FD File Offset: 0x000858FD
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

	// Token: 0x060029CD RID: 10701 RVA: 0x0010672D File Offset: 0x0010492D
	public override CollisionFlags Move(Vector3 _dir)
	{
		if (_dir.y >= 0.011f)
		{
			this.motor.ForceUnground(0.11f);
		}
		this.cc.vel = _dir / 0.05f;
		return this.Update();
	}

	// Token: 0x060029CE RID: 10702 RVA: 0x00106768 File Offset: 0x00104968
	public override CollisionFlags Update()
	{
		this.cc.Move();
		if (this.motor.GroundingStatus.FoundAnyGround)
		{
			this.cc.collisionFlags |= CollisionFlags.Below;
		}
		return this.cc.collisionFlags;
	}

	// Token: 0x060029CF RID: 10703 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Rotate(Quaternion _dir)
	{
	}

	// Token: 0x04001FB6 RID: 8118
	[PublicizedFrom(EAccessModifier.Private)]
	public KinematicCharacterSystem cs;

	// Token: 0x04001FB7 RID: 8119
	[PublicizedFrom(EAccessModifier.Private)]
	public KinematicCharacterMotor motor;

	// Token: 0x04001FB8 RID: 8120
	[PublicizedFrom(EAccessModifier.Private)]
	public CC cc;
}
