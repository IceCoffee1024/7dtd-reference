using System;
using KinematicCharacterController;
using UnityEngine;

// Token: 0x02000506 RID: 1286
public class CharacterControllerKinematic : CharacterControllerAbstract
{
	// Token: 0x060029DB RID: 10715 RVA: 0x00107EBC File Offset: 0x001060BC
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

	// Token: 0x060029DC RID: 10716 RVA: 0x00107F8C File Offset: 0x0010618C
	public override void Enable(bool isEnabled)
	{
		this.motor.enabled = isEnabled;
	}

	// Token: 0x060029DD RID: 10717 RVA: 0x00107F9A File Offset: 0x0010619A
	public override void SetStepOffset(float _stepOffset)
	{
		this.motor.MaxStepHeight = _stepOffset + 0.01f;
	}

	// Token: 0x060029DE RID: 10718 RVA: 0x00107FAE File Offset: 0x001061AE
	public override float GetStepOffset()
	{
		return this.motor.MaxStepHeight;
	}

	// Token: 0x060029DF RID: 10719 RVA: 0x00107FBB File Offset: 0x001061BB
	public override void SetSize(Vector3 _center, float _height, float _radius)
	{
		this.motor.SetCapsuleDimensions(_radius, _height, _center.y);
	}

	// Token: 0x060029E0 RID: 10720 RVA: 0x00107FD0 File Offset: 0x001061D0
	public override void SetCenter(Vector3 _center)
	{
		this.motor.SetCapsuleDimensions(this.GetRadius(), this.GetHeight(), _center.y);
	}

	// Token: 0x060029E1 RID: 10721 RVA: 0x00107FEF File Offset: 0x001061EF
	public override Vector3 GetCenter()
	{
		return this.motor.CharacterTransformToCapsuleCenter;
	}

	// Token: 0x060029E2 RID: 10722 RVA: 0x00107FFC File Offset: 0x001061FC
	public override void SetRadius(float _radius)
	{
		this.motor.SetCapsuleDimensions(_radius, this.GetHeight(), this.GetCenter().y);
	}

	// Token: 0x060029E3 RID: 10723 RVA: 0x0010801B File Offset: 0x0010621B
	public override float GetRadius()
	{
		return this.motor.Capsule.radius;
	}

	// Token: 0x060029E4 RID: 10724 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetSkinWidth(float _width)
	{
	}

	// Token: 0x060029E5 RID: 10725 RVA: 0x0010802D File Offset: 0x0010622D
	public override float GetSkinWidth()
	{
		return 0.08f;
	}

	// Token: 0x060029E6 RID: 10726 RVA: 0x00108034 File Offset: 0x00106234
	public override void SetHeight(float _height)
	{
		float radius = this.GetRadius();
		_height = Utils.FastMax(_height, radius * 2f);
		this.motor.SetCapsuleDimensions(radius, _height, _height * 0.5f);
	}

	// Token: 0x060029E7 RID: 10727 RVA: 0x0010806B File Offset: 0x0010626B
	public override float GetHeight()
	{
		return this.motor.Capsule.height;
	}

	// Token: 0x060029E8 RID: 10728 RVA: 0x0010807D File Offset: 0x0010627D
	public override bool IsGrounded()
	{
		return (this.cc.collisionFlags & CollisionFlags.Below) > CollisionFlags.None;
	}

	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x060029E9 RID: 10729 RVA: 0x0010808F File Offset: 0x0010628F
	public override Vector3 GroundNormal
	{
		get
		{
			return this.motor.GroundingStatus.GroundNormal;
		}
	}

	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x060029EA RID: 10730 RVA: 0x0008806C File Offset: 0x0008626C
	// (set) Token: 0x060029EB RID: 10731 RVA: 0x0008806C File Offset: 0x0008626C
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

	// Token: 0x060029EC RID: 10732 RVA: 0x001080A1 File Offset: 0x001062A1
	public override CollisionFlags Move(Vector3 _dir)
	{
		if (_dir.y >= 0.011f)
		{
			this.motor.ForceUnground(0.11f);
		}
		this.cc.vel = _dir / 0.05f;
		return this.Update();
	}

	// Token: 0x060029ED RID: 10733 RVA: 0x001080DC File Offset: 0x001062DC
	public override CollisionFlags Update()
	{
		this.cc.Move();
		if (this.motor.GroundingStatus.FoundAnyGround)
		{
			this.cc.collisionFlags |= CollisionFlags.Below;
		}
		return this.cc.collisionFlags;
	}

	// Token: 0x060029EE RID: 10734 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Rotate(Quaternion _dir)
	{
	}

	// Token: 0x04001FED RID: 8173
	[PublicizedFrom(EAccessModifier.Private)]
	public KinematicCharacterSystem cs;

	// Token: 0x04001FEE RID: 8174
	[PublicizedFrom(EAccessModifier.Private)]
	public KinematicCharacterMotor motor;

	// Token: 0x04001FEF RID: 8175
	[PublicizedFrom(EAccessModifier.Private)]
	public CC cc;
}
