using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001614 RID: 5652
[RequireComponent(typeof(vp_FPPlayerEventHandler))]
[RequireComponent(typeof(CharacterController))]
[Preserve]
public class vp_FPController : vp_Component
{
	// Token: 0x17001495 RID: 5269
	// (get) Token: 0x0600B037 RID: 45111 RVA: 0x0041BA51 File Offset: 0x00419C51
	public vp_FPPlayerEventHandler Player
	{
		get
		{
			if (this.m_Player == null && base.EventHandler != null)
			{
				this.m_Player = (vp_FPPlayerEventHandler)base.EventHandler;
			}
			return this.m_Player;
		}
	}

	// Token: 0x17001496 RID: 5270
	// (get) Token: 0x0600B038 RID: 45112 RVA: 0x0041BA86 File Offset: 0x00419C86
	public CharacterController CharacterController
	{
		get
		{
			if (this.m_CharacterController == null)
			{
				this.m_CharacterController = base.gameObject.GetComponent<CharacterController>();
			}
			return this.m_CharacterController;
		}
	}

	// Token: 0x17001497 RID: 5271
	// (get) Token: 0x0600B039 RID: 45113 RVA: 0x0041BAAD File Offset: 0x00419CAD
	public Vector3 SmoothPosition
	{
		get
		{
			return this.m_SmoothPosition;
		}
	}

	// Token: 0x17001498 RID: 5272
	// (get) Token: 0x0600B03A RID: 45114 RVA: 0x0041BAB5 File Offset: 0x00419CB5
	public Vector3 Velocity
	{
		get
		{
			return this.m_CharacterController.velocity;
		}
	}

	// Token: 0x17001499 RID: 5273
	// (get) Token: 0x0600B03B RID: 45115 RVA: 0x0041BAC2 File Offset: 0x00419CC2
	// (set) Token: 0x0600B03C RID: 45116 RVA: 0x0041BACA File Offset: 0x00419CCA
	public float SpeedModifier
	{
		get
		{
			return this.m_SpeedModifier;
		}
		set
		{
			this.m_SpeedModifier = value;
		}
	}

	// Token: 0x0600B03D RID: 45117 RVA: 0x0041BAD3 File Offset: 0x00419CD3
	public void Reposition(Vector3 _deltaVec)
	{
		this.m_SmoothPosition += _deltaVec;
		this.SetPosition(this.m_SmoothPosition);
	}

	// Token: 0x1700149A RID: 5274
	// (get) Token: 0x0600B03E RID: 45118 RVA: 0x0041BAF3 File Offset: 0x00419CF3
	public bool Grounded
	{
		get
		{
			return this.m_Grounded;
		}
	}

	// Token: 0x1700149B RID: 5275
	// (get) Token: 0x0600B03F RID: 45119 RVA: 0x0041BAFB File Offset: 0x00419CFB
	public bool HeadContact
	{
		get
		{
			return this.m_HeadContact;
		}
	}

	// Token: 0x1700149C RID: 5276
	// (get) Token: 0x0600B040 RID: 45120 RVA: 0x0041BB03 File Offset: 0x00419D03
	public Vector3 GroundNormal
	{
		get
		{
			return this.m_GroundHit.normal;
		}
	}

	// Token: 0x1700149D RID: 5277
	// (get) Token: 0x0600B041 RID: 45121 RVA: 0x0041BB10 File Offset: 0x00419D10
	public float GroundAngle
	{
		get
		{
			return Vector3.Angle(this.m_GroundHit.normal, Vector3.up);
		}
	}

	// Token: 0x1700149E RID: 5278
	// (get) Token: 0x0600B042 RID: 45122 RVA: 0x0041BB27 File Offset: 0x00419D27
	public Transform GroundTransform
	{
		get
		{
			return this.m_GroundHit.transform;
		}
	}

	// Token: 0x1700149F RID: 5279
	// (get) Token: 0x0600B043 RID: 45123 RVA: 0x0041BB34 File Offset: 0x00419D34
	public bool IsCollidingWall
	{
		get
		{
			return this.m_WallHit.collider != null;
		}
	}

	// Token: 0x170014A0 RID: 5280
	// (get) Token: 0x0600B044 RID: 45124 RVA: 0x0041BB47 File Offset: 0x00419D47
	public float ProjectedWallMove
	{
		get
		{
			if (!(this.m_WallHit.collider != null))
			{
				return 1f;
			}
			return this.m_ForceMultiplier;
		}
	}

	// Token: 0x170014A1 RID: 5281
	// (get) Token: 0x0600B045 RID: 45125 RVA: 0x0041BB68 File Offset: 0x00419D68
	public Vector3 FixedPosition
	{
		get
		{
			return this.m_FixedPosition;
		}
	}

	// Token: 0x170014A2 RID: 5282
	// (get) Token: 0x0600B046 RID: 45126 RVA: 0x0041BB70 File Offset: 0x00419D70
	public float SkinWidth
	{
		get
		{
			return this.m_SkinWidth;
		}
	}

	// Token: 0x0600B047 RID: 45127 RVA: 0x0041BB78 File Offset: 0x00419D78
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.SyncCharacterController();
		if (this.originalMotorJumpForce == -1f)
		{
			this.originalMotorJumpForce = this.MotorJumpForce;
		}
	}

	// Token: 0x0600B048 RID: 45128 RVA: 0x0041BBA0 File Offset: 0x00419DA0
	public void SyncCharacterController()
	{
		this.m_NormalHeight = this.CharacterController.height;
		this.CharacterController.center = (this.m_NormalCenter = new Vector3(0f, this.m_NormalHeight * 0.5f, 0f));
		this.CharacterController.radius = this.m_NormalHeight * 0.16666f;
		this.m_CrouchHeight = this.m_NormalHeight * this.PhysicsCrouchHeightModifier;
		this.m_CrouchCenter = this.m_NormalCenter * this.PhysicsCrouchHeightModifier;
		this.m_StepHeight = this.CharacterController.stepOffset;
	}

	// Token: 0x0600B049 RID: 45129 RVA: 0x0041BC3F File Offset: 0x00419E3F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		base.OnEnable();
		vp_TargetEvent<Vector3>.Register(this.m_Transform, "ForceImpact", new Action<Vector3>(this.AddForce));
	}

	// Token: 0x0600B04A RID: 45130 RVA: 0x0041BC64 File Offset: 0x00419E64
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
		vp_TargetEvent<Vector3>.Unregister(this.m_Root, "ForceImpact", new Action<Vector3>(this.AddForce));
	}

	// Token: 0x0600B04B RID: 45131 RVA: 0x0041BC89 File Offset: 0x00419E89
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.SetPosition(base.Transform.position);
	}

	// Token: 0x0600B04C RID: 45132 RVA: 0x0041BCA2 File Offset: 0x00419EA2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		this.SmoothMove();
	}

	// Token: 0x0600B04D RID: 45133 RVA: 0x0041BCB0 File Offset: 0x00419EB0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void FixedUpdate()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}
		if (this.Player.Driving.Active)
		{
			this.m_PrevPos = this.m_FixedPosition;
			this.m_FixedPosition = base.Transform.position;
			return;
		}
		base.Transform.position = this.m_FixedPosition;
		this.UpdateMotor();
		this.UpdateJump();
		this.UpdateForces();
		this.UpdateSliding();
		this.UpdateOutOfControl();
		this.FixedMove();
		this.UpdateCollisions();
		this.UpdatePlatformMove();
		this.m_PrevPos = base.Transform.position;
		this.m_FixedPosition = base.Transform.position;
	}

	// Token: 0x0600B04E RID: 45134 RVA: 0x0041BD5D File Offset: 0x00419F5D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateMotor()
	{
		if (!this.MotorFreeFly)
		{
			this.UpdateThrottleWalk();
			this.m_MotorThrottle = vp_MathUtility.SnapToZero(this.m_MotorThrottle, 0.0001f);
			return;
		}
		this.localPlayer.SwimModeUpdateThrottle();
	}

	// Token: 0x0600B04F RID: 45135 RVA: 0x0041BD90 File Offset: 0x00419F90
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateThrottleWalk()
	{
		this.UpdateSlopeFactor();
		this.m_MotorAirSpeedModifier = (this.m_Grounded ? 1f : this.MotorAirSpeed);
		Vector2 vector = this.Player.InputMoveVector.Get();
		if (vector.magnitude > 1f)
		{
			vector.Normalize();
		}
		float num = 0.1f * this.MotorAcceleration * this.m_MotorAirSpeedModifier * this.m_SlopeFactor;
		if (!this.Player.IsFirstPerson.Get() && this.Player.CameraRelativeMovement3P.Get())
		{
			float num2 = vector.y * num;
			float num3 = vector.x * num;
			Vector3 vector2 = new Vector3(num3, 0f, num2);
			this.m_MotorThrottle += vector2;
		}
		else
		{
			float num4 = ((vector.y > 0f) ? vector.y : (vector.y * this.MotorBackwardsSpeed)) * num;
			float num5 = vector.x * this.MotorSidewaysSpeed * num;
			this.m_MotorThrottle += base.Transform.TransformDirection(Vector3.forward) * num4 + base.Transform.TransformDirection(Vector3.right) * num5;
		}
		this.m_MotorThrottle.x = this.m_MotorThrottle.x / (1f + this.MotorDamping * this.m_MotorAirSpeedModifier * Time.timeScale);
		this.m_MotorThrottle.z = this.m_MotorThrottle.z / (1f + this.MotorDamping * this.m_MotorAirSpeedModifier * Time.timeScale);
	}

	// Token: 0x0600B050 RID: 45136 RVA: 0x0041BF34 File Offset: 0x0041A134
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateThrottleFree()
	{
		Vector3 vector = this.Player.CameraLookDirection.Get();
		vector.y *= 2f;
		vector.Normalize();
		this.m_MotorThrottle += this.Player.InputMoveVector.Get().y * vector * (this.MotorAcceleration * 0.1f);
		this.m_MotorThrottle += this.Player.InputMoveVector.Get().x * base.Transform.TransformDirection(Vector3.right * (this.MotorAcceleration * 0.1f));
		this.m_MotorThrottle.x = this.m_MotorThrottle.x / (1f + this.MotorDamping * Time.timeScale);
		this.m_MotorThrottle.z = this.m_MotorThrottle.z / (1f + this.MotorDamping * Time.timeScale);
	}

	// Token: 0x0600B051 RID: 45137 RVA: 0x0041C044 File Offset: 0x0041A244
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateJump()
	{
		if (this.m_HeadContact)
		{
			this.Player.Jump.Stop(1f);
		}
		if (!this.MotorFreeFly)
		{
			this.UpdateJumpForceWalk();
		}
		else
		{
			this.UpdateJumpForceFree();
		}
		this.m_MotorThrottle.y = this.m_MotorThrottle.y + this.m_MotorJumpForceAcc * Time.timeScale;
		this.m_MotorJumpForceAcc /= 1f + this.MotorJumpForceHoldDamping * Time.timeScale;
		this.m_MotorThrottle.y = this.m_MotorThrottle.y / (1f + this.MotorJumpForceDamping * Time.timeScale);
	}

	// Token: 0x0600B052 RID: 45138 RVA: 0x0041C0E0 File Offset: 0x0041A2E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateJumpForceWalk()
	{
		if (this.Player.Jump.Active && !this.m_Grounded)
		{
			if (this.m_MotorJumpForceHoldSkipFrames > 2)
			{
				if (this.Player.Velocity.Get().y >= 0f)
				{
					this.m_MotorJumpForceAcc += this.MotorJumpForceHold;
					return;
				}
			}
			else
			{
				this.m_MotorJumpForceHoldSkipFrames++;
			}
		}
	}

	// Token: 0x0600B053 RID: 45139 RVA: 0x0041C154 File Offset: 0x0041A354
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateJumpForceFree()
	{
		if (this.Player.Jump.Active && this.Player.Crouch.Active)
		{
			return;
		}
		if (this.Player.Jump.Active)
		{
			this.m_MotorJumpForceAcc += this.MotorJumpForceHold;
			return;
		}
		if (this.Player.Crouch.Active && this.Grounded && this.CharacterController.height == this.m_NormalHeight)
		{
			this.CharacterController.height = this.m_CrouchHeight;
			this.CharacterController.center = this.m_CrouchCenter;
		}
	}

	// Token: 0x0600B054 RID: 45140 RVA: 0x0041C1FC File Offset: 0x0041A3FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateForces()
	{
		this.m_GravityForce = Physics.gravity.y * (this.PhysicsGravityModifier * 0.002f) * vp_TimeUtility.AdjustedTimeScale;
		if (this.m_Grounded && this.m_FallSpeed <= 0f)
		{
			this.m_FallSpeed = this.m_GravityForce;
		}
		else
		{
			this.m_FallSpeed += this.m_GravityForce;
		}
		if (this.m_SmoothForceFrame[0] != Vector3.zero)
		{
			this.AddForceInternal(this.m_SmoothForceFrame[0]);
			for (int i = 0; i < 120; i++)
			{
				this.m_SmoothForceFrame[i] = ((i < 119) ? this.m_SmoothForceFrame[i + 1] : Vector3.zero);
				if (this.m_SmoothForceFrame[i] == Vector3.zero)
				{
					break;
				}
			}
		}
		this.m_ExternalForce /= 1f + this.PhysicsForceDamping * vp_TimeUtility.AdjustedTimeScale;
	}

	// Token: 0x0600B055 RID: 45141 RVA: 0x0041C2FC File Offset: 0x0041A4FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateSliding()
	{
		bool slideFast = this.m_SlideFast;
		bool slide = this.m_Slide;
		this.m_Slide = false;
		if (!this.m_Grounded)
		{
			this.m_OnSteepGroundSince = 0f;
			this.m_SlideFast = false;
		}
		else if (this.GroundAngle > this.PhysicsSlopeSlideLimit)
		{
			this.m_Slide = true;
			if (this.GroundAngle <= this.Player.SlopeLimit.Get())
			{
				this.m_SlopeSlideSpeed = Mathf.Max(this.m_SlopeSlideSpeed, this.PhysicsSlopeSlidiness * 0.01f);
				this.m_OnSteepGroundSince = 0f;
				this.m_SlideFast = false;
				this.m_SlopeSlideSpeed = ((Mathf.Abs(this.m_SlopeSlideSpeed) < 0.0001f) ? 0f : (this.m_SlopeSlideSpeed / (1f + 0.05f * vp_TimeUtility.AdjustedTimeScale)));
			}
			else
			{
				if (this.m_SlopeSlideSpeed > 0.01f)
				{
					this.m_SlideFast = true;
				}
				if (this.m_OnSteepGroundSince == 0f)
				{
					this.m_OnSteepGroundSince = Time.time;
					this.slideLastGroundN = Vector3.up;
				}
				this.m_SlopeSlideSpeed += this.PhysicsSlopeSlidiness * 0.01f * 5f * Time.deltaTime * vp_TimeUtility.AdjustedTimeScale;
				this.m_SlopeSlideSpeed *= 0.97f;
				float num = Vector3.Dot(this.GroundNormal, this.slideLastGroundN);
				this.slideLastGroundN = this.GroundNormal;
				if (num < 0.2f)
				{
					float num2 = 0.7f;
					if (num < -0.2f)
					{
						num2 = 0.2f;
					}
					this.m_SlopeSlideSpeed *= num2;
					this.m_ExternalForce *= num2;
				}
			}
			this.AddForce(Vector3.Cross(Vector3.Cross(this.GroundNormal, Vector3.down), this.GroundNormal) * this.m_SlopeSlideSpeed * vp_TimeUtility.AdjustedTimeScale);
		}
		else
		{
			this.m_OnSteepGroundSince = 0f;
			this.m_SlideFast = false;
			this.m_SlopeSlideSpeed = 0f;
		}
		if (this.m_MotorThrottle != Vector3.zero)
		{
			this.m_Slide = false;
		}
		if (this.m_SlideFast)
		{
			this.m_SlideFallSpeed = base.Transform.position.y;
		}
		else if (slideFast && !this.Grounded)
		{
			this.m_FallSpeed = Mathf.Min(0f, base.Transform.position.y - this.m_SlideFallSpeed);
		}
		if (slide != this.m_Slide)
		{
			this.Player.SetState("Slide", this.m_Slide, true, false);
		}
	}

	// Token: 0x0600B056 RID: 45142 RVA: 0x0041C58C File Offset: 0x0041A78C
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateOutOfControl()
	{
		if (this.m_ExternalForce.magnitude > 0.2f || this.m_FallSpeed < -0.2f || this.m_SlideFast)
		{
			this.Player.OutOfControl.Start(0f);
			return;
		}
		if (this.Player.OutOfControl.Active)
		{
			this.Player.OutOfControl.Stop(0f);
		}
	}

	// Token: 0x0600B057 RID: 45143 RVA: 0x0041C600 File Offset: 0x0041A800
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void FixedMove()
	{
		Physics.SyncTransforms();
		this.m_MoveDirection = Vector3.zero;
		this.m_MoveDirection += this.m_ExternalForce;
		this.m_MoveDirection += this.m_MotorThrottle;
		this.m_MoveDirection.x = this.m_MoveDirection.x * this.m_SpeedModifier;
		this.m_MoveDirection.z = this.m_MoveDirection.z * this.m_SpeedModifier;
		this.m_MoveDirection.y = this.m_MoveDirection.y + this.m_FallSpeed;
		if (this.MotorFreeFly && this.m_MoveDirection.y < 0f)
		{
			this.m_MotorThrottle.y = this.m_MotorThrottle.y + this.m_FallSpeed;
			this.m_FallSpeed = 0f;
		}
		if (this.MotorFreeFly)
		{
			this.m_MaxHeight = float.MinValue;
			this.m_MaxHeightInitialFallSpeed = 0f;
		}
		else
		{
			float num = base.Transform.position.y + Origin.position.y;
			if (this.m_Grounded || num > this.m_MaxHeight)
			{
				this.m_MaxHeight = num;
			}
		}
		bool flag = this.m_Grounded;
		if (flag)
		{
			Vector3 vector = this.m_MoveDirection;
			vector.y = 0f;
			if (vector != Vector3.zero)
			{
				vector = vector.normalized;
				vector.y = 0.15f;
				vector = vector.normalized;
				Vector3 vector2 = this.m_Transform.position - vector * this.m_CharacterController.radius;
				vector2.y += this.m_CharacterController.height + 0.18f;
				float num2 = this.m_CharacterController.radius * 2f + 0.5f;
				RaycastHit raycastHit;
				if (Physics.SphereCast(vector2, 0.05f, vector, out raycastHit, num2, 1073807360))
				{
					flag = false;
				}
			}
		}
		if (flag)
		{
			this.m_CharacterController.stepOffset = this.m_StepHeight;
		}
		else
		{
			this.m_CharacterController.stepOffset = 0.1f;
		}
		this.m_CurrentAntiBumpOffset = 0f;
		if (this.m_Grounded && this.m_MotorThrottle.y <= 0.001f)
		{
			this.m_CurrentAntiBumpOffset = Mathf.Max(0.1f, Vector3.Scale(this.m_MoveDirection, Vector3.one - Vector3.up).magnitude);
			this.m_MoveDirection.y = this.m_MoveDirection.y - this.m_CurrentAntiBumpOffset;
		}
		this.m_PredictedPos = base.Transform.position + vp_MathUtility.NaNSafeVector3(this.m_MoveDirection * base.Delta * Time.timeScale, default(Vector3));
		if (this.m_Platform != null && this.m_PositionOnPlatform != Vector3.zero)
		{
			this.Player.Move.Send(vp_MathUtility.NaNSafeVector3(this.m_Platform.TransformPoint(this.m_PositionOnPlatform) - this.m_Transform.position, default(Vector3)));
		}
		this.Player.Move.Send(vp_MathUtility.NaNSafeVector3(this.m_MoveDirection * base.Delta * Time.timeScale, default(Vector3)));
		if (this.Player.Dead.Active)
		{
			this.Player.InputMoveVector.Set(Vector2.zero);
		}
		Physics.SphereCast(new Ray(base.Transform.position + Vector3.up * this.m_CharacterController.radius, Vector3.down), this.m_CharacterController.radius, out this.m_GroundHit, this.m_SkinWidth + 0.001f, 1084850184);
		this.m_Grounded = this.m_GroundHit.collider != null;
		RaycastHit raycastHit2;
		if (!this.m_Grounded && Physics.SphereCast(new Ray(base.Transform.position + Vector3.up * this.m_CharacterController.radius, Vector3.down), this.m_CharacterController.radius, out raycastHit2, (this.m_CharacterController.skinWidth + 0.001f) * 4f, 1084850184) && raycastHit2.collider is CharacterController)
		{
			this.m_Grounded = true;
			this.m_GroundHit = raycastHit2;
		}
		if (!this.m_Grounded && this.Player.Velocity.Get().y > 0f)
		{
			Physics.SphereCast(new Ray(base.Transform.position, Vector3.up), this.m_CharacterController.radius, out this.m_CeilingHit, this.m_CharacterController.height - (this.m_CharacterController.radius - this.m_CharacterController.skinWidth) + 0.01f, 1084850184);
			this.m_HeadContact = this.m_CeilingHit.collider != null;
		}
		else
		{
			this.m_HeadContact = false;
		}
		if (this.m_GroundHit.transform == null && this.m_LastGroundHit.transform != null)
		{
			if (this.m_Platform != null && this.m_PositionOnPlatform != Vector3.zero)
			{
				this.AddForce(this.m_Platform.position - this.m_LastPlatformPos);
				this.m_Platform = null;
			}
			if (this.m_CurrentAntiBumpOffset != 0f)
			{
				this.Player.Move.Send(vp_MathUtility.NaNSafeVector3(this.m_CurrentAntiBumpOffset * Vector3.up, default(Vector3)) * base.Delta * Time.timeScale);
				this.m_PredictedPos += vp_MathUtility.NaNSafeVector3(this.m_CurrentAntiBumpOffset * Vector3.up, default(Vector3)) * base.Delta * Time.timeScale;
				this.m_MoveDirection.y = this.m_MoveDirection.y + this.m_CurrentAntiBumpOffset;
			}
		}
	}

	// Token: 0x0600B058 RID: 45144 RVA: 0x0041CC34 File Offset: 0x0041AE34
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SmoothMove()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}
		if (this.Player.Driving.Active)
		{
			this.m_FixedPosition = base.Transform.position;
			this.m_SmoothPosition = base.Transform.position;
			return;
		}
		this.m_FixedPosition = base.Transform.position;
		base.Transform.position = this.m_SmoothPosition;
		Physics.SyncTransforms();
		this.Player.Move.Send(vp_MathUtility.NaNSafeVector3(this.m_MoveDirection * base.Delta * Time.timeScale, default(Vector3)));
		this.m_SmoothPosition = base.Transform.position;
		if (Vector3.Distance(base.Transform.position, this.m_SmoothPosition) > this.Player.Radius.Get())
		{
			this.m_SmoothPosition = this.m_FixedPosition;
		}
		if (this.m_Platform != null && (this.m_LastPlatformPos.y < this.m_Platform.position.y || this.m_LastPlatformPos.y > this.m_Platform.position.y))
		{
			this.m_SmoothPosition.y = base.Transform.position.y;
		}
		this.m_SmoothPosition = Vector3.Lerp(this.m_SmoothPosition, this.m_FixedPosition, Time.deltaTime);
	}

	// Token: 0x0600B059 RID: 45145 RVA: 0x0041CDB4 File Offset: 0x0041AFB4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateCollisions()
	{
		if (this.m_GroundHit.transform != null && this.m_GroundHit.transform != this.m_LastGroundHit.transform)
		{
			if (this.m_LastGroundHit.transform == null)
			{
				float num = -(this.CharacterController.velocity.y * 0.01f) * Time.timeScale;
				float num2 = base.Transform.position.y + Origin.position.y;
				float num3 = Mathf.Round((this.m_MaxHeight - num2) * 2f) / 2f;
				if (!this.MotorFreeFly && num3 >= 0f)
				{
					float num4 = Math.Abs(this.m_GravityForce);
					this.m_FallImpact = (float)Math.Sqrt(Math.Pow((double)this.m_MaxHeightInitialFallSpeed, 2.0) + (double)(2f * num4 * num3)) * 0.8677499f;
				}
				else
				{
					this.m_FallImpact = num;
				}
				this.m_MaxHeight = float.MinValue;
				this.m_MaxHeightInitialFallSpeed = 0f;
				this.m_SmoothPosition.y = base.Transform.position.y;
				this.DeflectDownForce();
				this.Player.FallImpact.Send(this.m_FallImpact);
				this.Player.FallImpact2.Send(this.m_FallImpact);
				this.m_MotorThrottle.y = 0f;
				this.m_MotorJumpForceAcc = 0f;
				this.m_MotorJumpForceHoldSkipFrames = 0;
			}
			if (this.m_GroundHit.collider.gameObject.layer == 28)
			{
				this.m_Platform = this.m_GroundHit.transform;
				this.m_LastPlatformAngle = this.m_Platform.eulerAngles.y;
			}
			else
			{
				this.m_Platform = null;
			}
			Terrain component = this.m_GroundHit.transform.GetComponent<Terrain>();
			if (component != null)
			{
				this.m_CurrentTerrain = component;
			}
			else
			{
				this.m_CurrentTerrain = null;
			}
			vp_SurfaceIdentifier component2 = this.m_GroundHit.transform.GetComponent<vp_SurfaceIdentifier>();
			if (component2 != null)
			{
				this.m_CurrentSurface = component2;
			}
			else
			{
				this.m_CurrentSurface = null;
			}
		}
		else
		{
			this.m_FallImpact = 0f;
		}
		this.m_LastGroundHit = this.m_GroundHit;
		if (this.m_PredictedPos.y > base.Transform.position.y && (this.m_ExternalForce.y > 0f || this.m_MotorThrottle.y > 0f))
		{
			this.DeflectUpForce();
		}
		if (this.m_PredictedPos.x != base.Transform.position.x || this.m_PredictedPos.z != base.Transform.position.z)
		{
			this.DeflectHorizontalForce();
		}
	}

	// Token: 0x0600B05A RID: 45146 RVA: 0x0041D08C File Offset: 0x0041B28C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateSlopeFactor()
	{
		if (!this.m_Grounded)
		{
			this.m_SlopeFactor = 1f;
			return;
		}
		Vector3 motorThrottle = this.m_MotorThrottle;
		motorThrottle.y = 0f;
		float num = Vector3.Angle(this.m_GroundHit.normal, motorThrottle);
		this.m_SlopeFactor = 1f + (1f - num / 90f);
		if (Mathf.Abs(1f - this.m_SlopeFactor) < 0.25f)
		{
			this.m_SlopeFactor = 1f;
			return;
		}
		if (this.m_SlopeFactor <= 1f)
		{
			if (this.MotorSlopeSpeedUp == 1f)
			{
				this.m_SlopeFactor *= 1.2f;
			}
			else
			{
				this.m_SlopeFactor *= this.MotorSlopeSpeedUp;
			}
			this.m_SlopeFactor = ((this.GroundAngle > this.Player.SlopeLimit.Get()) ? 0f : this.m_SlopeFactor);
			return;
		}
		if (this.MotorSlopeSpeedDown == 1f)
		{
			this.m_SlopeFactor = 1f / this.m_SlopeFactor;
			this.m_SlopeFactor *= 1.2f;
			return;
		}
		this.m_SlopeFactor *= this.MotorSlopeSpeedDown;
	}

	// Token: 0x0600B05B RID: 45147 RVA: 0x0041D1C8 File Offset: 0x0041B3C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdatePlatformMove()
	{
		if (this.m_Platform == null)
		{
			return;
		}
		this.m_PositionOnPlatform = this.m_Platform.InverseTransformPoint(this.m_Transform.position);
		this.Player.Rotation.Set(new Vector2(this.Player.Rotation.Get().x, this.Player.Rotation.Get().y - Mathf.DeltaAngle(this.m_Platform.eulerAngles.y, this.m_LastPlatformAngle)));
		this.m_LastPlatformAngle = this.m_Platform.eulerAngles.y;
		this.m_LastPlatformPos = this.m_Platform.position;
		this.m_SmoothPosition = base.Transform.position;
	}

	// Token: 0x0600B05C RID: 45148 RVA: 0x0041D2A2 File Offset: 0x0041B4A2
	public virtual void SetPosition(Vector3 position)
	{
		base.Transform.position = position;
		this.m_PrevPos = position;
		this.m_SmoothPosition = position;
		this.m_FixedPosition = position;
		Physics.SyncTransforms();
	}

	// Token: 0x0600B05D RID: 45149 RVA: 0x0041D2CA File Offset: 0x0041B4CA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void AddForceInternal(Vector3 force)
	{
		this.m_ExternalForce += force;
	}

	// Token: 0x0600B05E RID: 45150 RVA: 0x0041D2DE File Offset: 0x0041B4DE
	public virtual void AddForce(float x, float y, float z)
	{
		this.AddForce(new Vector3(x, y, z));
	}

	// Token: 0x0600B05F RID: 45151 RVA: 0x0041D2EE File Offset: 0x0041B4EE
	public virtual void AddForce(Vector3 force)
	{
		if (Time.timeScale >= 1f)
		{
			this.AddForceInternal(force);
			return;
		}
		this.AddSoftForce(force, 1f);
	}

	// Token: 0x0600B060 RID: 45152 RVA: 0x0041D310 File Offset: 0x0041B510
	public virtual void AddSoftForce(Vector3 force, float frames)
	{
		force /= Time.timeScale;
		frames = Mathf.Clamp(frames, 1f, 120f);
		this.AddForceInternal(force / frames);
		for (int i = 0; i < Mathf.RoundToInt(frames) - 1; i++)
		{
			this.m_SmoothForceFrame[i] += force / frames;
		}
	}

	// Token: 0x0600B061 RID: 45153 RVA: 0x0041D380 File Offset: 0x0041B580
	public virtual void StopSoftForce()
	{
		int num = 0;
		while (num < 120 && !(this.m_SmoothForceFrame[num] == Vector3.zero))
		{
			this.m_SmoothForceFrame[num] = Vector3.zero;
			num++;
		}
	}

	// Token: 0x0600B062 RID: 45154 RVA: 0x0041D3C4 File Offset: 0x0041B5C4
	public virtual void Stop()
	{
		this.Player.Move.Send(Vector3.zero);
		this.m_MotorThrottle = Vector3.zero;
		this.m_MotorJumpDone = true;
		this.m_MotorJumpForceAcc = 0f;
		this.m_ExternalForce = Vector3.zero;
		this.StopSoftForce();
		this.Player.InputMoveVector.Set(Vector2.zero);
		this.m_FallSpeed = 0f;
		this.m_SmoothPosition = base.Transform.position;
		this.m_MaxHeight = float.MinValue;
		this.m_MaxHeightInitialFallSpeed = 0f;
	}

	// Token: 0x0600B063 RID: 45155 RVA: 0x0041D468 File Offset: 0x0041B668
	public virtual void DeflectDownForce()
	{
		if (this.GroundAngle > this.PhysicsSlopeSlideLimit)
		{
			this.m_SlopeSlideSpeed = this.m_FallImpact * (0.25f * Time.timeScale);
		}
		if (this.GroundAngle > 85f)
		{
			this.m_MotorThrottle += vp_3DUtility.HorizontalVector(this.GroundNormal * this.m_FallImpact);
			this.m_Grounded = false;
		}
	}

	// Token: 0x0600B064 RID: 45156 RVA: 0x0041D4D8 File Offset: 0x0041B6D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void DeflectUpForce()
	{
		if (!this.m_HeadContact)
		{
			return;
		}
		this.m_NewDir = Vector3.Cross(Vector3.Cross(this.m_CeilingHit.normal, Vector3.up), this.m_CeilingHit.normal);
		this.m_ForceImpact = this.m_MotorThrottle.y + this.m_ExternalForce.y;
		Vector3 vector = this.m_NewDir * (this.m_MotorThrottle.y + this.m_ExternalForce.y) * (1f - this.PhysicsWallFriction);
		this.m_ForceImpact -= vector.magnitude;
		this.AddForce(vector * Time.timeScale);
		this.m_MotorThrottle.y = 0f;
		this.m_ExternalForce.y = 0f;
		this.m_FallSpeed = 0f;
		this.m_NewDir.x = base.Transform.InverseTransformDirection(this.m_NewDir).x;
		this.Player.HeadImpact.Send((this.m_NewDir.x < 0f || (this.m_NewDir.x == 0f && global::UnityEngine.Random.value < 0.5f)) ? (-this.m_ForceImpact) : this.m_ForceImpact);
	}

	// Token: 0x0600B065 RID: 45157 RVA: 0x0041D630 File Offset: 0x0041B830
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void DeflectHorizontalForce()
	{
		this.m_PredictedPos.y = base.Transform.position.y;
		this.m_PrevPos.y = base.Transform.position.y;
		this.m_PrevDir = (this.m_PredictedPos - this.m_PrevPos).normalized;
		this.CapsuleBottom = this.m_PrevPos + Vector3.up * this.Player.Radius.Get();
		this.CapsuleTop = this.CapsuleBottom + Vector3.up * (this.Player.Height.Get() - this.Player.Radius.Get() * 2f);
		if (!Physics.CapsuleCast(this.CapsuleBottom, this.CapsuleTop, this.Player.Radius.Get(), this.m_PrevDir, out this.m_WallHit, Vector3.Distance(this.m_PrevPos, this.m_PredictedPos) + 0.07f, 1084850184))
		{
			return;
		}
		this.m_NewDir = Vector3.Cross(this.m_WallHit.normal, Vector3.up).normalized;
		if (Vector3.Dot(Vector3.Cross(this.m_WallHit.point - base.Transform.position, this.m_PrevPos - base.Transform.position), Vector3.up) > 0f)
		{
			this.m_NewDir = -this.m_NewDir;
		}
		this.m_ForceMultiplier = Mathf.Abs(Vector3.Dot(this.m_PrevDir, this.m_NewDir)) * (1f - this.PhysicsWallFriction);
		if (this.PhysicsWallBounce > 0f)
		{
			this.m_NewDir = Vector3.Lerp(this.m_NewDir, Vector3.Reflect(this.m_PrevDir, this.m_WallHit.normal), this.PhysicsWallBounce);
			this.m_ForceMultiplier = Mathf.Lerp(this.m_ForceMultiplier, 1f, this.PhysicsWallBounce * (1f - this.PhysicsWallFriction));
		}
		if (this.m_ExternalForce != Vector3.zero)
		{
			this.m_ForceImpact = 0f;
			float y = this.m_ExternalForce.y;
			this.m_ExternalForce.y = 0f;
			this.m_ForceImpact = this.m_ExternalForce.magnitude;
			this.m_ExternalForce = this.m_NewDir * this.m_ExternalForce.magnitude * this.m_ForceMultiplier;
			this.m_ForceImpact -= this.m_ExternalForce.magnitude;
			int num = 0;
			while (num < 120 && !(this.m_SmoothForceFrame[num] == Vector3.zero))
			{
				this.m_SmoothForceFrame[num] = this.m_SmoothForceFrame[num].magnitude * this.m_NewDir * this.m_ForceMultiplier;
				num++;
			}
			this.m_ExternalForce.y = y;
		}
	}

	// Token: 0x0600B066 RID: 45158 RVA: 0x0041D958 File Offset: 0x0041BB58
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void RefreshColliders()
	{
		if (this.Player.Crouch.Active && (!this.MotorFreeFly || this.Grounded))
		{
			this.CharacterController.height = this.m_CrouchHeight;
			this.CharacterController.center = this.m_CrouchCenter;
		}
		else
		{
			this.CharacterController.height = this.m_NormalHeight;
			this.CharacterController.center = this.m_NormalCenter;
		}
		if (this.m_TriggerCollider != null)
		{
			this.m_TriggerCollider.radius = this.CharacterController.radius + this.m_SkinWidth;
			this.m_TriggerCollider.height = this.CharacterController.height + this.m_SkinWidth * 2f;
			this.m_TriggerCollider.center = this.CharacterController.center;
		}
	}

	// Token: 0x0600B067 RID: 45159 RVA: 0x0041DA34 File Offset: 0x0041BC34
	public float CalculateMaxSpeed(string stateName = "Default", float accelDuration = 5f)
	{
		if (stateName != "Default")
		{
			bool flag = false;
			using (List<vp_State>.Enumerator enumerator = this.States.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Name == stateName)
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Error (",
					(this != null) ? this.ToString() : null,
					") Controller has no such state: '",
					stateName,
					"'."
				}));
				return 0f;
			}
		}
		Dictionary<vp_State, bool> dictionary = new Dictionary<vp_State, bool>();
		foreach (vp_State vp_State in this.States)
		{
			dictionary.Add(vp_State, vp_State.Enabled);
			vp_State.Enabled = false;
		}
		base.StateManager.Reset();
		if (stateName != "Default")
		{
			base.SetState(stateName, true, false, false);
		}
		float num = 0f;
		float num2 = 5f;
		int num3 = 0;
		while ((float)num3 < 60f * num2)
		{
			num += this.MotorAcceleration * 0.1f * 60f;
			num /= 1f + this.MotorDamping;
			num3++;
		}
		foreach (vp_State vp_State2 in this.States)
		{
			bool flag2;
			dictionary.TryGetValue(vp_State2, out flag2);
			vp_State2.Enabled = flag2;
		}
		return num;
	}

	// Token: 0x0600B068 RID: 45160 RVA: 0x0041DBFC File Offset: 0x0041BDFC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
		if (attachedRigidbody == null || attachedRigidbody.isKinematic)
		{
			return;
		}
		if (hit.moveDirection.y < -0.3f)
		{
			return;
		}
		Vector3 vector = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
		attachedRigidbody.velocity = vector * (this.PhysicsPushForce / attachedRigidbody.mass);
	}

	// Token: 0x0600B069 RID: 45161 RVA: 0x0041DC75 File Offset: 0x0041BE75
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Jump()
	{
		return this.MotorFreeFly || (this.m_Grounded && this.m_MotorJumpDone);
	}

	// Token: 0x0600B06A RID: 45162 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Swim()
	{
		return true;
	}

	// Token: 0x0600B06B RID: 45163 RVA: 0x0041DC96 File Offset: 0x0041BE96
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Swim()
	{
		this.m_ExternalForce.y = this.m_ExternalForce.y + this.m_FallSpeed * 0.2f;
		this.m_FallSpeed = 0f;
	}

	// Token: 0x0600B06C RID: 45164 RVA: 0x0041DCBE File Offset: 0x0041BEBE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Run()
	{
		return !this.Player.Crouch.Active;
	}

	// Token: 0x0600B06D RID: 45165 RVA: 0x0041DCD8 File Offset: 0x0041BED8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Jump()
	{
		this.m_SlopeSlideSpeed *= 0.2f;
		this.m_MotorJumpDone = false;
		if (this.MotorFreeFly && !this.Grounded)
		{
			return;
		}
		this.m_MotorThrottle.y = this.MotorJumpForce / Time.timeScale;
		this.m_SmoothPosition.y = base.Transform.position.y;
	}

	// Token: 0x0600B06E RID: 45166 RVA: 0x0041DD41 File Offset: 0x0041BF41
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Jump()
	{
		this.m_MotorJumpDone = true;
	}

	// Token: 0x0600B06F RID: 45167 RVA: 0x0041DD4C File Offset: 0x0041BF4C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStop_Crouch()
	{
		if (Physics.SphereCast(new Ray(base.Transform.position, Vector3.up), this.Player.Radius.Get(), this.m_NormalHeight - this.Player.Radius.Get() + 0.01f, 1084850184))
		{
			this.Player.Crouch.NextAllowedStopTime = Time.time + 0.1f;
			return false;
		}
		return true;
	}

	// Token: 0x0600B070 RID: 45168 RVA: 0x0041DDCF File Offset: 0x0041BFCF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Crouch()
	{
		this.Player.Run.Stop(0f);
		this.RefreshColliders();
	}

	// Token: 0x0600B071 RID: 45169 RVA: 0x0041DDEC File Offset: 0x0041BFEC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Crouch()
	{
		this.RefreshColliders();
	}

	// Token: 0x0600B072 RID: 45170 RVA: 0x0041DDF4 File Offset: 0x0041BFF4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_ForceImpact(Vector3 force)
	{
		this.AddForce(force);
	}

	// Token: 0x0600B073 RID: 45171 RVA: 0x0041DDFD File Offset: 0x0041BFFD
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_Stop()
	{
		this.Stop();
	}

	// Token: 0x170014A3 RID: 5283
	// (get) Token: 0x0600B074 RID: 45172 RVA: 0x0041DE05 File Offset: 0x0041C005
	// (set) Token: 0x0600B075 RID: 45173 RVA: 0x0041DE12 File Offset: 0x0041C012
	public virtual Vector3 OnValue_Position
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return base.Transform.position;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.SetPosition(value);
		}
	}

	// Token: 0x170014A4 RID: 5284
	// (get) Token: 0x0600B076 RID: 45174 RVA: 0x0041DE1B File Offset: 0x0041C01B
	public virtual Vector3 OnValue_Velocity
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.velocity;
		}
	}

	// Token: 0x170014A5 RID: 5285
	// (get) Token: 0x0600B077 RID: 45175 RVA: 0x0041DE28 File Offset: 0x0041C028
	public virtual float OnValue_StepOffset
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.stepOffset;
		}
	}

	// Token: 0x170014A6 RID: 5286
	// (get) Token: 0x0600B078 RID: 45176 RVA: 0x0041DE35 File Offset: 0x0041C035
	public virtual float OnValue_SlopeLimit
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.slopeLimit;
		}
	}

	// Token: 0x170014A7 RID: 5287
	// (get) Token: 0x0600B079 RID: 45177 RVA: 0x0041DE42 File Offset: 0x0041C042
	public virtual float OnValue_Radius
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.radius;
		}
	}

	// Token: 0x170014A8 RID: 5288
	// (get) Token: 0x0600B07A RID: 45178 RVA: 0x0041DE4F File Offset: 0x0041C04F
	public virtual float OnValue_Height
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.height;
		}
	}

	// Token: 0x0600B07B RID: 45179 RVA: 0x0041DE5C File Offset: 0x0041C05C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_Move(Vector3 direction)
	{
		if (this.CharacterController.enabled)
		{
			this.LastMoveCollisionFlags = this.CharacterController.Move(direction);
		}
	}

	// Token: 0x170014A9 RID: 5289
	// (get) Token: 0x0600B07C RID: 45180 RVA: 0x0041DE7D File Offset: 0x0041C07D
	// (set) Token: 0x0600B07D RID: 45181 RVA: 0x0041DE85 File Offset: 0x0041C085
	public virtual Vector3 OnValue_MotorThrottle
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_MotorThrottle;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.m_MotorThrottle = value;
		}
	}

	// Token: 0x170014AA RID: 5290
	// (get) Token: 0x0600B07E RID: 45182 RVA: 0x0041DE8E File Offset: 0x0041C08E
	public virtual bool OnValue_MotorJumpDone
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_MotorJumpDone;
		}
	}

	// Token: 0x170014AB RID: 5291
	// (get) Token: 0x0600B07F RID: 45183 RVA: 0x0041DE96 File Offset: 0x0041C096
	// (set) Token: 0x0600B080 RID: 45184 RVA: 0x0041DE9E File Offset: 0x0041C09E
	public virtual float OnValue_FallSpeed
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_FallSpeed;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.m_FallSpeed = value;
		}
	}

	// Token: 0x170014AC RID: 5292
	// (get) Token: 0x0600B081 RID: 45185 RVA: 0x0041DEA7 File Offset: 0x0041C0A7
	// (set) Token: 0x0600B082 RID: 45186 RVA: 0x0041DEAF File Offset: 0x0041C0AF
	public virtual Transform OnValue_Platform
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_Platform;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.m_Platform = value;
		}
	}

	// Token: 0x170014AD RID: 5293
	// (get) Token: 0x0600B083 RID: 45187 RVA: 0x0041DEB8 File Offset: 0x0041C0B8
	public virtual Texture OnValue_GroundTexture
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.GroundTransform == null)
			{
				return null;
			}
			if (this.GroundTransform.GetComponent<Renderer>() == null && this.m_CurrentTerrain == null)
			{
				return null;
			}
			int num = -1;
			if (this.m_CurrentTerrain != null)
			{
				num = vp_FootstepManager.GetMainTerrainTexture(this.Player.Position.Get(), this.m_CurrentTerrain);
				if (num > this.m_CurrentTerrain.terrainData.terrainLayers.Length - 1)
				{
					return null;
				}
			}
			if (!(this.m_CurrentTerrain == null))
			{
				return this.m_CurrentTerrain.terrainData.terrainLayers[num].diffuseTexture;
			}
			return this.GroundTransform.GetComponent<Renderer>().material.mainTexture;
		}
	}

	// Token: 0x170014AE RID: 5294
	// (get) Token: 0x0600B084 RID: 45188 RVA: 0x0041DF7D File Offset: 0x0041C17D
	public virtual vp_SurfaceIdentifier OnValue_SurfaceType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_CurrentSurface;
		}
	}

	// Token: 0x0600B085 RID: 45189 RVA: 0x0041DF85 File Offset: 0x0041C185
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		this.Player.OutOfControl.Stop(0f);
	}

	// Token: 0x0600B086 RID: 45190 RVA: 0x0041DF9C File Offset: 0x0041C19C
	public void ScaleFallSpeed(float scale)
	{
		this.m_FallSpeed *= scale;
		this.m_MaxHeightInitialFallSpeed *= scale;
		float num = base.Transform.position.y + Origin.position.y;
		this.m_MaxHeight = num + scale * scale * (this.m_MaxHeight - num);
	}

	// Token: 0x040083A2 RID: 33698
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x040083A3 RID: 33699
	public EntityPlayerLocal localPlayer;

	// Token: 0x040083A4 RID: 33700
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CharacterController m_CharacterController;

	// Token: 0x040083A5 RID: 33701
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_FixedPosition = Vector3.zero;

	// Token: 0x040083A6 RID: 33702
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_SmoothPosition = Vector3.zero;

	// Token: 0x040083A7 RID: 33703
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SpeedModifier = 1f;

	// Token: 0x040083A8 RID: 33704
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Grounded;

	// Token: 0x040083A9 RID: 33705
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_HeadContact;

	// Token: 0x040083AA RID: 33706
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_GroundHit;

	// Token: 0x040083AB RID: 33707
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_LastGroundHit;

	// Token: 0x040083AC RID: 33708
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_CeilingHit;

	// Token: 0x040083AD RID: 33709
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_WallHit;

	// Token: 0x040083AE RID: 33710
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FallImpact;

	// Token: 0x040083AF RID: 33711
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Terrain m_CurrentTerrain;

	// Token: 0x040083B0 RID: 33712
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_SurfaceIdentifier m_CurrentSurface;

	// Token: 0x040083B1 RID: 33713
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CollisionFlags LastMoveCollisionFlags;

	// Token: 0x040083B2 RID: 33714
	public float MotorAcceleration = 0.18f;

	// Token: 0x040083B3 RID: 33715
	public float MotorDamping = 0.17f;

	// Token: 0x040083B4 RID: 33716
	public float MotorBackwardsSpeed = 0.65f;

	// Token: 0x040083B5 RID: 33717
	public float MotorSidewaysSpeed = 0.65f;

	// Token: 0x040083B6 RID: 33718
	public float MotorAirSpeed = 0.35f;

	// Token: 0x040083B7 RID: 33719
	public float MotorSlopeSpeedUp = 1f;

	// Token: 0x040083B8 RID: 33720
	public float MotorSlopeSpeedDown = 1f;

	// Token: 0x040083B9 RID: 33721
	public bool MotorFreeFly;

	// Token: 0x040083BA RID: 33722
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_MoveDirection = Vector3.zero;

	// Token: 0x040083BB RID: 33723
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SlopeFactor = 1f;

	// Token: 0x040083BC RID: 33724
	public Vector3 m_MotorThrottle = Vector3.zero;

	// Token: 0x040083BD RID: 33725
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MotorAirSpeedModifier = 1f;

	// Token: 0x040083BE RID: 33726
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentAntiBumpOffset;

	// Token: 0x040083BF RID: 33727
	public float originalMotorJumpForce = -1f;

	// Token: 0x040083C0 RID: 33728
	public float MotorJumpForce = 0.18f;

	// Token: 0x040083C1 RID: 33729
	public float MotorJumpForceDamping = 0.08f;

	// Token: 0x040083C2 RID: 33730
	public float MotorJumpForceHold = 0.003f;

	// Token: 0x040083C3 RID: 33731
	public float MotorJumpForceHoldDamping = 0.5f;

	// Token: 0x040083C4 RID: 33732
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_MotorJumpForceHoldSkipFrames;

	// Token: 0x040083C5 RID: 33733
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MotorJumpForceAcc;

	// Token: 0x040083C6 RID: 33734
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_MotorJumpDone = true;

	// Token: 0x040083C7 RID: 33735
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FallSpeed;

	// Token: 0x040083C8 RID: 33736
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MaxHeight = float.MinValue;

	// Token: 0x040083C9 RID: 33737
	public float m_MaxHeightInitialFallSpeed;

	// Token: 0x040083CA RID: 33738
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_GravityForce;

	// Token: 0x040083CB RID: 33739
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 slideLastGroundN;

	// Token: 0x040083CC RID: 33740
	public float PhysicsForceDamping = 0.05f;

	// Token: 0x040083CD RID: 33741
	public float PhysicsPushForce = 5f;

	// Token: 0x040083CE RID: 33742
	public float PhysicsGravityModifier = 0.2f;

	// Token: 0x040083CF RID: 33743
	public float PhysicsSlopeSlideLimit = 30f;

	// Token: 0x040083D0 RID: 33744
	public float PhysicsSlopeSlidiness = 0.15f;

	// Token: 0x040083D1 RID: 33745
	public float PhysicsWallBounce;

	// Token: 0x040083D2 RID: 33746
	public float PhysicsWallFriction;

	// Token: 0x040083D3 RID: 33747
	public float PhysicsCrouchHeightModifier = 0.5f;

	// Token: 0x040083D4 RID: 33748
	public bool PhysicsHasCollisionTrigger = true;

	// Token: 0x040083D5 RID: 33749
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_Trigger;

	// Token: 0x040083D6 RID: 33750
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CapsuleCollider m_TriggerCollider;

	// Token: 0x040083D7 RID: 33751
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_ExternalForce = Vector3.zero;

	// Token: 0x040083D8 RID: 33752
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3[] m_SmoothForceFrame = new Vector3[120];

	// Token: 0x040083D9 RID: 33753
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Slide;

	// Token: 0x040083DA RID: 33754
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_SlideFast;

	// Token: 0x040083DB RID: 33755
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SlideFallSpeed;

	// Token: 0x040083DC RID: 33756
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_OnSteepGroundSince;

	// Token: 0x040083DD RID: 33757
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SlopeSlideSpeed;

	// Token: 0x040083DE RID: 33758
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PredictedPos = Vector3.zero;

	// Token: 0x040083DF RID: 33759
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PrevPos = Vector3.zero;

	// Token: 0x040083E0 RID: 33760
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PrevDir = Vector3.zero;

	// Token: 0x040083E1 RID: 33761
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_NewDir = Vector3.zero;

	// Token: 0x040083E2 RID: 33762
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_ForceImpact;

	// Token: 0x040083E3 RID: 33763
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_ForceMultiplier;

	// Token: 0x040083E4 RID: 33764
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 CapsuleBottom = Vector3.zero;

	// Token: 0x040083E5 RID: 33765
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 CapsuleTop = Vector3.zero;

	// Token: 0x040083E6 RID: 33766
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_StepHeight = 0.7f;

	// Token: 0x040083E7 RID: 33767
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SkinWidth = 0.08f;

	// Token: 0x040083E8 RID: 33768
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Platform;

	// Token: 0x040083E9 RID: 33769
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PositionOnPlatform = Vector3.zero;

	// Token: 0x040083EA RID: 33770
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastPlatformAngle;

	// Token: 0x040083EB RID: 33771
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_LastPlatformPos = Vector3.zero;

	// Token: 0x040083EC RID: 33772
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NormalHeight;

	// Token: 0x040083ED RID: 33773
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_NormalCenter = Vector3.zero;

	// Token: 0x040083EE RID: 33774
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CrouchHeight;

	// Token: 0x040083EF RID: 33775
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CrouchCenter = Vector3.zero;
}
