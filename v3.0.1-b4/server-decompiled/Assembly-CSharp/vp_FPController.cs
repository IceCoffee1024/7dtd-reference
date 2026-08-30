using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020015DA RID: 5594
[RequireComponent(typeof(vp_FPPlayerEventHandler))]
[RequireComponent(typeof(CharacterController))]
[Preserve]
public class vp_FPController : vp_Component
{
	// Token: 0x17001419 RID: 5145
	// (get) Token: 0x0600AEA8 RID: 44712 RVA: 0x00412A35 File Offset: 0x00410C35
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

	// Token: 0x1700141A RID: 5146
	// (get) Token: 0x0600AEA9 RID: 44713 RVA: 0x00412A6A File Offset: 0x00410C6A
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

	// Token: 0x1700141B RID: 5147
	// (get) Token: 0x0600AEAA RID: 44714 RVA: 0x00412A91 File Offset: 0x00410C91
	public Vector3 SmoothPosition
	{
		get
		{
			return this.m_SmoothPosition;
		}
	}

	// Token: 0x1700141C RID: 5148
	// (get) Token: 0x0600AEAB RID: 44715 RVA: 0x00412A99 File Offset: 0x00410C99
	public Vector3 Velocity
	{
		get
		{
			return this.m_CharacterController.velocity;
		}
	}

	// Token: 0x1700141D RID: 5149
	// (get) Token: 0x0600AEAC RID: 44716 RVA: 0x00412AA6 File Offset: 0x00410CA6
	// (set) Token: 0x0600AEAD RID: 44717 RVA: 0x00412AAE File Offset: 0x00410CAE
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

	// Token: 0x0600AEAE RID: 44718 RVA: 0x00412AB7 File Offset: 0x00410CB7
	public void Reposition(Vector3 _deltaVec)
	{
		this.m_SmoothPosition += _deltaVec;
		this.SetPosition(this.m_SmoothPosition);
	}

	// Token: 0x1700141E RID: 5150
	// (get) Token: 0x0600AEAF RID: 44719 RVA: 0x00412AD7 File Offset: 0x00410CD7
	public bool Grounded
	{
		get
		{
			return this.m_Grounded;
		}
	}

	// Token: 0x1700141F RID: 5151
	// (get) Token: 0x0600AEB0 RID: 44720 RVA: 0x00412ADF File Offset: 0x00410CDF
	public bool HeadContact
	{
		get
		{
			return this.m_HeadContact;
		}
	}

	// Token: 0x17001420 RID: 5152
	// (get) Token: 0x0600AEB1 RID: 44721 RVA: 0x00412AE7 File Offset: 0x00410CE7
	public Vector3 GroundNormal
	{
		get
		{
			return this.m_GroundHit.normal;
		}
	}

	// Token: 0x17001421 RID: 5153
	// (get) Token: 0x0600AEB2 RID: 44722 RVA: 0x00412AF4 File Offset: 0x00410CF4
	public float GroundAngle
	{
		get
		{
			return Vector3.Angle(this.m_GroundHit.normal, Vector3.up);
		}
	}

	// Token: 0x17001422 RID: 5154
	// (get) Token: 0x0600AEB3 RID: 44723 RVA: 0x00412B0B File Offset: 0x00410D0B
	public Transform GroundTransform
	{
		get
		{
			return this.m_GroundHit.transform;
		}
	}

	// Token: 0x17001423 RID: 5155
	// (get) Token: 0x0600AEB4 RID: 44724 RVA: 0x00412B18 File Offset: 0x00410D18
	public bool IsCollidingWall
	{
		get
		{
			return this.m_WallHit.collider != null;
		}
	}

	// Token: 0x17001424 RID: 5156
	// (get) Token: 0x0600AEB5 RID: 44725 RVA: 0x00412B2B File Offset: 0x00410D2B
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

	// Token: 0x17001425 RID: 5157
	// (get) Token: 0x0600AEB6 RID: 44726 RVA: 0x00412B4C File Offset: 0x00410D4C
	public Vector3 FixedPosition
	{
		get
		{
			return this.m_FixedPosition;
		}
	}

	// Token: 0x17001426 RID: 5158
	// (get) Token: 0x0600AEB7 RID: 44727 RVA: 0x00412B54 File Offset: 0x00410D54
	public float SkinWidth
	{
		get
		{
			return this.m_SkinWidth;
		}
	}

	// Token: 0x0600AEB8 RID: 44728 RVA: 0x00412B5C File Offset: 0x00410D5C
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

	// Token: 0x0600AEB9 RID: 44729 RVA: 0x00412B84 File Offset: 0x00410D84
	public void SyncCharacterController()
	{
		this.m_NormalHeight = this.CharacterController.height;
		this.CharacterController.center = (this.m_NormalCenter = new Vector3(0f, this.m_NormalHeight * 0.5f, 0f));
		this.CharacterController.radius = this.m_NormalHeight * 0.16666f;
		this.m_CrouchHeight = this.m_NormalHeight * this.PhysicsCrouchHeightModifier;
		this.m_CrouchCenter = this.m_NormalCenter * this.PhysicsCrouchHeightModifier;
		this.m_StepHeight = this.CharacterController.stepOffset;
	}

	// Token: 0x0600AEBA RID: 44730 RVA: 0x00412C23 File Offset: 0x00410E23
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		base.OnEnable();
		vp_TargetEvent<Vector3>.Register(this.m_Transform, "ForceImpact", new Action<Vector3>(this.AddForce));
	}

	// Token: 0x0600AEBB RID: 44731 RVA: 0x00412C48 File Offset: 0x00410E48
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
		vp_TargetEvent<Vector3>.Unregister(this.m_Root, "ForceImpact", new Action<Vector3>(this.AddForce));
	}

	// Token: 0x0600AEBC RID: 44732 RVA: 0x00412C6D File Offset: 0x00410E6D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.SetPosition(base.Transform.position);
	}

	// Token: 0x0600AEBD RID: 44733 RVA: 0x00412C86 File Offset: 0x00410E86
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		this.SmoothMove();
	}

	// Token: 0x0600AEBE RID: 44734 RVA: 0x00412C94 File Offset: 0x00410E94
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

	// Token: 0x0600AEBF RID: 44735 RVA: 0x00412D41 File Offset: 0x00410F41
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

	// Token: 0x0600AEC0 RID: 44736 RVA: 0x00412D74 File Offset: 0x00410F74
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

	// Token: 0x0600AEC1 RID: 44737 RVA: 0x00412F18 File Offset: 0x00411118
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

	// Token: 0x0600AEC2 RID: 44738 RVA: 0x00413028 File Offset: 0x00411228
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

	// Token: 0x0600AEC3 RID: 44739 RVA: 0x004130C4 File Offset: 0x004112C4
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

	// Token: 0x0600AEC4 RID: 44740 RVA: 0x00413138 File Offset: 0x00411338
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

	// Token: 0x0600AEC5 RID: 44741 RVA: 0x004131E0 File Offset: 0x004113E0
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

	// Token: 0x0600AEC6 RID: 44742 RVA: 0x004132E0 File Offset: 0x004114E0
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

	// Token: 0x0600AEC7 RID: 44743 RVA: 0x00413570 File Offset: 0x00411770
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

	// Token: 0x0600AEC8 RID: 44744 RVA: 0x004135E4 File Offset: 0x004117E4
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

	// Token: 0x0600AEC9 RID: 44745 RVA: 0x00413C18 File Offset: 0x00411E18
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

	// Token: 0x0600AECA RID: 44746 RVA: 0x00413D98 File Offset: 0x00411F98
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

	// Token: 0x0600AECB RID: 44747 RVA: 0x00414070 File Offset: 0x00412270
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

	// Token: 0x0600AECC RID: 44748 RVA: 0x004141AC File Offset: 0x004123AC
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

	// Token: 0x0600AECD RID: 44749 RVA: 0x00414286 File Offset: 0x00412486
	public virtual void SetPosition(Vector3 position)
	{
		base.Transform.position = position;
		this.m_PrevPos = position;
		this.m_SmoothPosition = position;
		this.m_FixedPosition = position;
		Physics.SyncTransforms();
	}

	// Token: 0x0600AECE RID: 44750 RVA: 0x004142AE File Offset: 0x004124AE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void AddForceInternal(Vector3 force)
	{
		this.m_ExternalForce += force;
	}

	// Token: 0x0600AECF RID: 44751 RVA: 0x004142C2 File Offset: 0x004124C2
	public virtual void AddForce(float x, float y, float z)
	{
		this.AddForce(new Vector3(x, y, z));
	}

	// Token: 0x0600AED0 RID: 44752 RVA: 0x004142D2 File Offset: 0x004124D2
	public virtual void AddForce(Vector3 force)
	{
		if (Time.timeScale >= 1f)
		{
			this.AddForceInternal(force);
			return;
		}
		this.AddSoftForce(force, 1f);
	}

	// Token: 0x0600AED1 RID: 44753 RVA: 0x004142F4 File Offset: 0x004124F4
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

	// Token: 0x0600AED2 RID: 44754 RVA: 0x00414364 File Offset: 0x00412564
	public virtual void StopSoftForce()
	{
		int num = 0;
		while (num < 120 && !(this.m_SmoothForceFrame[num] == Vector3.zero))
		{
			this.m_SmoothForceFrame[num] = Vector3.zero;
			num++;
		}
	}

	// Token: 0x0600AED3 RID: 44755 RVA: 0x004143A8 File Offset: 0x004125A8
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

	// Token: 0x0600AED4 RID: 44756 RVA: 0x0041444C File Offset: 0x0041264C
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

	// Token: 0x0600AED5 RID: 44757 RVA: 0x004144BC File Offset: 0x004126BC
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

	// Token: 0x0600AED6 RID: 44758 RVA: 0x00414614 File Offset: 0x00412814
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

	// Token: 0x0600AED7 RID: 44759 RVA: 0x0041493C File Offset: 0x00412B3C
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

	// Token: 0x0600AED8 RID: 44760 RVA: 0x00414A18 File Offset: 0x00412C18
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

	// Token: 0x0600AED9 RID: 44761 RVA: 0x00414BE0 File Offset: 0x00412DE0
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

	// Token: 0x0600AEDA RID: 44762 RVA: 0x00414C59 File Offset: 0x00412E59
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Jump()
	{
		return this.MotorFreeFly || (this.m_Grounded && this.m_MotorJumpDone);
	}

	// Token: 0x0600AEDB RID: 44763 RVA: 0x0001FF5D File Offset: 0x0001E15D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Swim()
	{
		return true;
	}

	// Token: 0x0600AEDC RID: 44764 RVA: 0x00414C7A File Offset: 0x00412E7A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Swim()
	{
		this.m_ExternalForce.y = this.m_ExternalForce.y + this.m_FallSpeed * 0.2f;
		this.m_FallSpeed = 0f;
	}

	// Token: 0x0600AEDD RID: 44765 RVA: 0x00414CA2 File Offset: 0x00412EA2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Run()
	{
		return !this.Player.Crouch.Active;
	}

	// Token: 0x0600AEDE RID: 44766 RVA: 0x00414CBC File Offset: 0x00412EBC
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

	// Token: 0x0600AEDF RID: 44767 RVA: 0x00414D25 File Offset: 0x00412F25
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Jump()
	{
		this.m_MotorJumpDone = true;
	}

	// Token: 0x0600AEE0 RID: 44768 RVA: 0x00414D30 File Offset: 0x00412F30
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

	// Token: 0x0600AEE1 RID: 44769 RVA: 0x00414DB3 File Offset: 0x00412FB3
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Crouch()
	{
		this.Player.Run.Stop(0f);
		this.RefreshColliders();
	}

	// Token: 0x0600AEE2 RID: 44770 RVA: 0x00414DD0 File Offset: 0x00412FD0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Crouch()
	{
		this.RefreshColliders();
	}

	// Token: 0x0600AEE3 RID: 44771 RVA: 0x00414DD8 File Offset: 0x00412FD8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_ForceImpact(Vector3 force)
	{
		this.AddForce(force);
	}

	// Token: 0x0600AEE4 RID: 44772 RVA: 0x00414DE1 File Offset: 0x00412FE1
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_Stop()
	{
		this.Stop();
	}

	// Token: 0x17001427 RID: 5159
	// (get) Token: 0x0600AEE5 RID: 44773 RVA: 0x00414DE9 File Offset: 0x00412FE9
	// (set) Token: 0x0600AEE6 RID: 44774 RVA: 0x00414DF6 File Offset: 0x00412FF6
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

	// Token: 0x17001428 RID: 5160
	// (get) Token: 0x0600AEE7 RID: 44775 RVA: 0x00414DFF File Offset: 0x00412FFF
	public virtual Vector3 OnValue_Velocity
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.velocity;
		}
	}

	// Token: 0x17001429 RID: 5161
	// (get) Token: 0x0600AEE8 RID: 44776 RVA: 0x00414E0C File Offset: 0x0041300C
	public virtual float OnValue_StepOffset
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.stepOffset;
		}
	}

	// Token: 0x1700142A RID: 5162
	// (get) Token: 0x0600AEE9 RID: 44777 RVA: 0x00414E19 File Offset: 0x00413019
	public virtual float OnValue_SlopeLimit
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.slopeLimit;
		}
	}

	// Token: 0x1700142B RID: 5163
	// (get) Token: 0x0600AEEA RID: 44778 RVA: 0x00414E26 File Offset: 0x00413026
	public virtual float OnValue_Radius
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.radius;
		}
	}

	// Token: 0x1700142C RID: 5164
	// (get) Token: 0x0600AEEB RID: 44779 RVA: 0x00414E33 File Offset: 0x00413033
	public virtual float OnValue_Height
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.height;
		}
	}

	// Token: 0x0600AEEC RID: 44780 RVA: 0x00414E40 File Offset: 0x00413040
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_Move(Vector3 direction)
	{
		if (this.CharacterController.enabled)
		{
			this.LastMoveCollisionFlags = this.CharacterController.Move(direction);
		}
	}

	// Token: 0x1700142D RID: 5165
	// (get) Token: 0x0600AEED RID: 44781 RVA: 0x00414E61 File Offset: 0x00413061
	// (set) Token: 0x0600AEEE RID: 44782 RVA: 0x00414E69 File Offset: 0x00413069
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

	// Token: 0x1700142E RID: 5166
	// (get) Token: 0x0600AEEF RID: 44783 RVA: 0x00414E72 File Offset: 0x00413072
	public virtual bool OnValue_MotorJumpDone
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_MotorJumpDone;
		}
	}

	// Token: 0x1700142F RID: 5167
	// (get) Token: 0x0600AEF0 RID: 44784 RVA: 0x00414E7A File Offset: 0x0041307A
	// (set) Token: 0x0600AEF1 RID: 44785 RVA: 0x00414E82 File Offset: 0x00413082
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

	// Token: 0x17001430 RID: 5168
	// (get) Token: 0x0600AEF2 RID: 44786 RVA: 0x00414E8B File Offset: 0x0041308B
	// (set) Token: 0x0600AEF3 RID: 44787 RVA: 0x00414E93 File Offset: 0x00413093
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

	// Token: 0x17001431 RID: 5169
	// (get) Token: 0x0600AEF4 RID: 44788 RVA: 0x00414E9C File Offset: 0x0041309C
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

	// Token: 0x17001432 RID: 5170
	// (get) Token: 0x0600AEF5 RID: 44789 RVA: 0x00414F61 File Offset: 0x00413161
	public virtual vp_SurfaceIdentifier OnValue_SurfaceType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_CurrentSurface;
		}
	}

	// Token: 0x0600AEF6 RID: 44790 RVA: 0x00414F69 File Offset: 0x00413169
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		this.Player.OutOfControl.Stop(0f);
	}

	// Token: 0x0600AEF7 RID: 44791 RVA: 0x00414F80 File Offset: 0x00413180
	public void ScaleFallSpeed(float scale)
	{
		this.m_FallSpeed *= scale;
		this.m_MaxHeightInitialFallSpeed *= scale;
		float num = base.Transform.position.y + Origin.position.y;
		this.m_MaxHeight = num + scale * scale * (this.m_MaxHeight - num);
	}

	// Token: 0x04008273 RID: 33395
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x04008274 RID: 33396
	public EntityPlayerLocal localPlayer;

	// Token: 0x04008275 RID: 33397
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CharacterController m_CharacterController;

	// Token: 0x04008276 RID: 33398
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_FixedPosition = Vector3.zero;

	// Token: 0x04008277 RID: 33399
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_SmoothPosition = Vector3.zero;

	// Token: 0x04008278 RID: 33400
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SpeedModifier = 1f;

	// Token: 0x04008279 RID: 33401
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Grounded;

	// Token: 0x0400827A RID: 33402
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_HeadContact;

	// Token: 0x0400827B RID: 33403
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_GroundHit;

	// Token: 0x0400827C RID: 33404
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_LastGroundHit;

	// Token: 0x0400827D RID: 33405
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_CeilingHit;

	// Token: 0x0400827E RID: 33406
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_WallHit;

	// Token: 0x0400827F RID: 33407
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FallImpact;

	// Token: 0x04008280 RID: 33408
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Terrain m_CurrentTerrain;

	// Token: 0x04008281 RID: 33409
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_SurfaceIdentifier m_CurrentSurface;

	// Token: 0x04008282 RID: 33410
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CollisionFlags LastMoveCollisionFlags;

	// Token: 0x04008283 RID: 33411
	public float MotorAcceleration = 0.18f;

	// Token: 0x04008284 RID: 33412
	public float MotorDamping = 0.17f;

	// Token: 0x04008285 RID: 33413
	public float MotorBackwardsSpeed = 0.65f;

	// Token: 0x04008286 RID: 33414
	public float MotorSidewaysSpeed = 0.65f;

	// Token: 0x04008287 RID: 33415
	public float MotorAirSpeed = 0.35f;

	// Token: 0x04008288 RID: 33416
	public float MotorSlopeSpeedUp = 1f;

	// Token: 0x04008289 RID: 33417
	public float MotorSlopeSpeedDown = 1f;

	// Token: 0x0400828A RID: 33418
	public bool MotorFreeFly;

	// Token: 0x0400828B RID: 33419
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_MoveDirection = Vector3.zero;

	// Token: 0x0400828C RID: 33420
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SlopeFactor = 1f;

	// Token: 0x0400828D RID: 33421
	public Vector3 m_MotorThrottle = Vector3.zero;

	// Token: 0x0400828E RID: 33422
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MotorAirSpeedModifier = 1f;

	// Token: 0x0400828F RID: 33423
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentAntiBumpOffset;

	// Token: 0x04008290 RID: 33424
	public float originalMotorJumpForce = -1f;

	// Token: 0x04008291 RID: 33425
	public float MotorJumpForce = 0.18f;

	// Token: 0x04008292 RID: 33426
	public float MotorJumpForceDamping = 0.08f;

	// Token: 0x04008293 RID: 33427
	public float MotorJumpForceHold = 0.003f;

	// Token: 0x04008294 RID: 33428
	public float MotorJumpForceHoldDamping = 0.5f;

	// Token: 0x04008295 RID: 33429
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_MotorJumpForceHoldSkipFrames;

	// Token: 0x04008296 RID: 33430
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MotorJumpForceAcc;

	// Token: 0x04008297 RID: 33431
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_MotorJumpDone = true;

	// Token: 0x04008298 RID: 33432
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FallSpeed;

	// Token: 0x04008299 RID: 33433
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MaxHeight = float.MinValue;

	// Token: 0x0400829A RID: 33434
	public float m_MaxHeightInitialFallSpeed;

	// Token: 0x0400829B RID: 33435
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_GravityForce;

	// Token: 0x0400829C RID: 33436
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 slideLastGroundN;

	// Token: 0x0400829D RID: 33437
	public float PhysicsForceDamping = 0.05f;

	// Token: 0x0400829E RID: 33438
	public float PhysicsPushForce = 5f;

	// Token: 0x0400829F RID: 33439
	public float PhysicsGravityModifier = 0.2f;

	// Token: 0x040082A0 RID: 33440
	public float PhysicsSlopeSlideLimit = 30f;

	// Token: 0x040082A1 RID: 33441
	public float PhysicsSlopeSlidiness = 0.15f;

	// Token: 0x040082A2 RID: 33442
	public float PhysicsWallBounce;

	// Token: 0x040082A3 RID: 33443
	public float PhysicsWallFriction;

	// Token: 0x040082A4 RID: 33444
	public float PhysicsCrouchHeightModifier = 0.5f;

	// Token: 0x040082A5 RID: 33445
	public bool PhysicsHasCollisionTrigger = true;

	// Token: 0x040082A6 RID: 33446
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_Trigger;

	// Token: 0x040082A7 RID: 33447
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CapsuleCollider m_TriggerCollider;

	// Token: 0x040082A8 RID: 33448
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_ExternalForce = Vector3.zero;

	// Token: 0x040082A9 RID: 33449
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3[] m_SmoothForceFrame = new Vector3[120];

	// Token: 0x040082AA RID: 33450
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Slide;

	// Token: 0x040082AB RID: 33451
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_SlideFast;

	// Token: 0x040082AC RID: 33452
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SlideFallSpeed;

	// Token: 0x040082AD RID: 33453
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_OnSteepGroundSince;

	// Token: 0x040082AE RID: 33454
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SlopeSlideSpeed;

	// Token: 0x040082AF RID: 33455
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PredictedPos = Vector3.zero;

	// Token: 0x040082B0 RID: 33456
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PrevPos = Vector3.zero;

	// Token: 0x040082B1 RID: 33457
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PrevDir = Vector3.zero;

	// Token: 0x040082B2 RID: 33458
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_NewDir = Vector3.zero;

	// Token: 0x040082B3 RID: 33459
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_ForceImpact;

	// Token: 0x040082B4 RID: 33460
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_ForceMultiplier;

	// Token: 0x040082B5 RID: 33461
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 CapsuleBottom = Vector3.zero;

	// Token: 0x040082B6 RID: 33462
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 CapsuleTop = Vector3.zero;

	// Token: 0x040082B7 RID: 33463
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_StepHeight = 0.7f;

	// Token: 0x040082B8 RID: 33464
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SkinWidth = 0.08f;

	// Token: 0x040082B9 RID: 33465
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Platform;

	// Token: 0x040082BA RID: 33466
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PositionOnPlatform = Vector3.zero;

	// Token: 0x040082BB RID: 33467
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastPlatformAngle;

	// Token: 0x040082BC RID: 33468
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_LastPlatformPos = Vector3.zero;

	// Token: 0x040082BD RID: 33469
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NormalHeight;

	// Token: 0x040082BE RID: 33470
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_NormalCenter = Vector3.zero;

	// Token: 0x040082BF RID: 33471
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CrouchHeight;

	// Token: 0x040082C0 RID: 33472
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CrouchCenter = Vector3.zero;
}
