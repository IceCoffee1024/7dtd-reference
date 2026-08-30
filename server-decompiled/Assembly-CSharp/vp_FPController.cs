using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020015F5 RID: 5621
[RequireComponent(typeof(vp_FPPlayerEventHandler))]
[RequireComponent(typeof(CharacterController))]
[Preserve]
public class vp_FPController : vp_Component
{
	// Token: 0x17001452 RID: 5202
	// (get) Token: 0x0600AFA5 RID: 44965 RVA: 0x00417D75 File Offset: 0x00415F75
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

	// Token: 0x17001453 RID: 5203
	// (get) Token: 0x0600AFA6 RID: 44966 RVA: 0x00417DAA File Offset: 0x00415FAA
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

	// Token: 0x17001454 RID: 5204
	// (get) Token: 0x0600AFA7 RID: 44967 RVA: 0x00417DD1 File Offset: 0x00415FD1
	public Vector3 SmoothPosition
	{
		get
		{
			return this.m_SmoothPosition;
		}
	}

	// Token: 0x17001455 RID: 5205
	// (get) Token: 0x0600AFA8 RID: 44968 RVA: 0x00417DD9 File Offset: 0x00415FD9
	public Vector3 Velocity
	{
		get
		{
			return this.m_CharacterController.velocity;
		}
	}

	// Token: 0x17001456 RID: 5206
	// (get) Token: 0x0600AFA9 RID: 44969 RVA: 0x00417DE6 File Offset: 0x00415FE6
	// (set) Token: 0x0600AFAA RID: 44970 RVA: 0x00417DEE File Offset: 0x00415FEE
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

	// Token: 0x0600AFAB RID: 44971 RVA: 0x00417DF7 File Offset: 0x00415FF7
	public void Reposition(Vector3 _deltaVec)
	{
		this.m_SmoothPosition += _deltaVec;
		this.SetPosition(this.m_SmoothPosition);
	}

	// Token: 0x17001457 RID: 5207
	// (get) Token: 0x0600AFAC RID: 44972 RVA: 0x00417E17 File Offset: 0x00416017
	public bool Grounded
	{
		get
		{
			return this.m_Grounded;
		}
	}

	// Token: 0x17001458 RID: 5208
	// (get) Token: 0x0600AFAD RID: 44973 RVA: 0x00417E1F File Offset: 0x0041601F
	public bool HeadContact
	{
		get
		{
			return this.m_HeadContact;
		}
	}

	// Token: 0x17001459 RID: 5209
	// (get) Token: 0x0600AFAE RID: 44974 RVA: 0x00417E27 File Offset: 0x00416027
	public Vector3 GroundNormal
	{
		get
		{
			return this.m_GroundHit.normal;
		}
	}

	// Token: 0x1700145A RID: 5210
	// (get) Token: 0x0600AFAF RID: 44975 RVA: 0x00417E34 File Offset: 0x00416034
	public float GroundAngle
	{
		get
		{
			return Vector3.Angle(this.m_GroundHit.normal, Vector3.up);
		}
	}

	// Token: 0x1700145B RID: 5211
	// (get) Token: 0x0600AFB0 RID: 44976 RVA: 0x00417E4B File Offset: 0x0041604B
	public Transform GroundTransform
	{
		get
		{
			return this.m_GroundHit.transform;
		}
	}

	// Token: 0x1700145C RID: 5212
	// (get) Token: 0x0600AFB1 RID: 44977 RVA: 0x00417E58 File Offset: 0x00416058
	public bool IsCollidingWall
	{
		get
		{
			return this.m_WallHit.collider != null;
		}
	}

	// Token: 0x1700145D RID: 5213
	// (get) Token: 0x0600AFB2 RID: 44978 RVA: 0x00417E6B File Offset: 0x0041606B
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

	// Token: 0x1700145E RID: 5214
	// (get) Token: 0x0600AFB3 RID: 44979 RVA: 0x00417E8C File Offset: 0x0041608C
	public Vector3 FixedPosition
	{
		get
		{
			return this.m_FixedPosition;
		}
	}

	// Token: 0x1700145F RID: 5215
	// (get) Token: 0x0600AFB4 RID: 44980 RVA: 0x00417E94 File Offset: 0x00416094
	public float SkinWidth
	{
		get
		{
			return this.m_SkinWidth;
		}
	}

	// Token: 0x0600AFB5 RID: 44981 RVA: 0x00417E9C File Offset: 0x0041609C
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

	// Token: 0x0600AFB6 RID: 44982 RVA: 0x00417EC4 File Offset: 0x004160C4
	public void SyncCharacterController()
	{
		this.m_NormalHeight = this.CharacterController.height;
		this.CharacterController.center = (this.m_NormalCenter = new Vector3(0f, this.m_NormalHeight * 0.5f, 0f));
		this.CharacterController.radius = this.m_NormalHeight * 0.16666f;
		this.m_CrouchHeight = this.m_NormalHeight * this.PhysicsCrouchHeightModifier;
		this.m_CrouchCenter = this.m_NormalCenter * this.PhysicsCrouchHeightModifier;
		this.m_StepHeight = this.CharacterController.stepOffset;
	}

	// Token: 0x0600AFB7 RID: 44983 RVA: 0x00417F63 File Offset: 0x00416163
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		base.OnEnable();
		vp_TargetEvent<Vector3>.Register(this.m_Transform, "ForceImpact", new Action<Vector3>(this.AddForce));
	}

	// Token: 0x0600AFB8 RID: 44984 RVA: 0x00417F88 File Offset: 0x00416188
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
		vp_TargetEvent<Vector3>.Unregister(this.m_Root, "ForceImpact", new Action<Vector3>(this.AddForce));
	}

	// Token: 0x0600AFB9 RID: 44985 RVA: 0x00417FAD File Offset: 0x004161AD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.SetPosition(base.Transform.position);
	}

	// Token: 0x0600AFBA RID: 44986 RVA: 0x00417FC6 File Offset: 0x004161C6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		this.SmoothMove();
	}

	// Token: 0x0600AFBB RID: 44987 RVA: 0x00417FD4 File Offset: 0x004161D4
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

	// Token: 0x0600AFBC RID: 44988 RVA: 0x00418081 File Offset: 0x00416281
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

	// Token: 0x0600AFBD RID: 44989 RVA: 0x004180B4 File Offset: 0x004162B4
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

	// Token: 0x0600AFBE RID: 44990 RVA: 0x00418258 File Offset: 0x00416458
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

	// Token: 0x0600AFBF RID: 44991 RVA: 0x00418368 File Offset: 0x00416568
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

	// Token: 0x0600AFC0 RID: 44992 RVA: 0x00418404 File Offset: 0x00416604
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

	// Token: 0x0600AFC1 RID: 44993 RVA: 0x00418478 File Offset: 0x00416678
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

	// Token: 0x0600AFC2 RID: 44994 RVA: 0x00418520 File Offset: 0x00416720
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

	// Token: 0x0600AFC3 RID: 44995 RVA: 0x00418620 File Offset: 0x00416820
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

	// Token: 0x0600AFC4 RID: 44996 RVA: 0x004188B0 File Offset: 0x00416AB0
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

	// Token: 0x0600AFC5 RID: 44997 RVA: 0x00418924 File Offset: 0x00416B24
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

	// Token: 0x0600AFC6 RID: 44998 RVA: 0x00418F58 File Offset: 0x00417158
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

	// Token: 0x0600AFC7 RID: 44999 RVA: 0x004190D8 File Offset: 0x004172D8
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

	// Token: 0x0600AFC8 RID: 45000 RVA: 0x004193B0 File Offset: 0x004175B0
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

	// Token: 0x0600AFC9 RID: 45001 RVA: 0x004194EC File Offset: 0x004176EC
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

	// Token: 0x0600AFCA RID: 45002 RVA: 0x004195C6 File Offset: 0x004177C6
	public virtual void SetPosition(Vector3 position)
	{
		base.Transform.position = position;
		this.m_PrevPos = position;
		this.m_SmoothPosition = position;
		this.m_FixedPosition = position;
		Physics.SyncTransforms();
	}

	// Token: 0x0600AFCB RID: 45003 RVA: 0x004195EE File Offset: 0x004177EE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void AddForceInternal(Vector3 force)
	{
		this.m_ExternalForce += force;
	}

	// Token: 0x0600AFCC RID: 45004 RVA: 0x00419602 File Offset: 0x00417802
	public virtual void AddForce(float x, float y, float z)
	{
		this.AddForce(new Vector3(x, y, z));
	}

	// Token: 0x0600AFCD RID: 45005 RVA: 0x00419612 File Offset: 0x00417812
	public virtual void AddForce(Vector3 force)
	{
		if (Time.timeScale >= 1f)
		{
			this.AddForceInternal(force);
			return;
		}
		this.AddSoftForce(force, 1f);
	}

	// Token: 0x0600AFCE RID: 45006 RVA: 0x00419634 File Offset: 0x00417834
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

	// Token: 0x0600AFCF RID: 45007 RVA: 0x004196A4 File Offset: 0x004178A4
	public virtual void StopSoftForce()
	{
		int num = 0;
		while (num < 120 && !(this.m_SmoothForceFrame[num] == Vector3.zero))
		{
			this.m_SmoothForceFrame[num] = Vector3.zero;
			num++;
		}
	}

	// Token: 0x0600AFD0 RID: 45008 RVA: 0x004196E8 File Offset: 0x004178E8
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

	// Token: 0x0600AFD1 RID: 45009 RVA: 0x0041978C File Offset: 0x0041798C
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

	// Token: 0x0600AFD2 RID: 45010 RVA: 0x004197FC File Offset: 0x004179FC
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

	// Token: 0x0600AFD3 RID: 45011 RVA: 0x00419954 File Offset: 0x00417B54
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

	// Token: 0x0600AFD4 RID: 45012 RVA: 0x00419C7C File Offset: 0x00417E7C
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

	// Token: 0x0600AFD5 RID: 45013 RVA: 0x00419D58 File Offset: 0x00417F58
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

	// Token: 0x0600AFD6 RID: 45014 RVA: 0x00419F20 File Offset: 0x00418120
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

	// Token: 0x0600AFD7 RID: 45015 RVA: 0x00419F99 File Offset: 0x00418199
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Jump()
	{
		return this.MotorFreeFly || (this.m_Grounded && this.m_MotorJumpDone);
	}

	// Token: 0x0600AFD8 RID: 45016 RVA: 0x00020021 File Offset: 0x0001E221
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Swim()
	{
		return true;
	}

	// Token: 0x0600AFD9 RID: 45017 RVA: 0x00419FBA File Offset: 0x004181BA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Swim()
	{
		this.m_ExternalForce.y = this.m_ExternalForce.y + this.m_FallSpeed * 0.2f;
		this.m_FallSpeed = 0f;
	}

	// Token: 0x0600AFDA RID: 45018 RVA: 0x00419FE2 File Offset: 0x004181E2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Run()
	{
		return !this.Player.Crouch.Active;
	}

	// Token: 0x0600AFDB RID: 45019 RVA: 0x00419FFC File Offset: 0x004181FC
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

	// Token: 0x0600AFDC RID: 45020 RVA: 0x0041A065 File Offset: 0x00418265
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Jump()
	{
		this.m_MotorJumpDone = true;
	}

	// Token: 0x0600AFDD RID: 45021 RVA: 0x0041A070 File Offset: 0x00418270
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

	// Token: 0x0600AFDE RID: 45022 RVA: 0x0041A0F3 File Offset: 0x004182F3
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Crouch()
	{
		this.Player.Run.Stop(0f);
		this.RefreshColliders();
	}

	// Token: 0x0600AFDF RID: 45023 RVA: 0x0041A110 File Offset: 0x00418310
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Crouch()
	{
		this.RefreshColliders();
	}

	// Token: 0x0600AFE0 RID: 45024 RVA: 0x0041A118 File Offset: 0x00418318
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_ForceImpact(Vector3 force)
	{
		this.AddForce(force);
	}

	// Token: 0x0600AFE1 RID: 45025 RVA: 0x0041A121 File Offset: 0x00418321
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_Stop()
	{
		this.Stop();
	}

	// Token: 0x17001460 RID: 5216
	// (get) Token: 0x0600AFE2 RID: 45026 RVA: 0x0041A129 File Offset: 0x00418329
	// (set) Token: 0x0600AFE3 RID: 45027 RVA: 0x0041A136 File Offset: 0x00418336
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

	// Token: 0x17001461 RID: 5217
	// (get) Token: 0x0600AFE4 RID: 45028 RVA: 0x0041A13F File Offset: 0x0041833F
	public virtual Vector3 OnValue_Velocity
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.velocity;
		}
	}

	// Token: 0x17001462 RID: 5218
	// (get) Token: 0x0600AFE5 RID: 45029 RVA: 0x0041A14C File Offset: 0x0041834C
	public virtual float OnValue_StepOffset
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.stepOffset;
		}
	}

	// Token: 0x17001463 RID: 5219
	// (get) Token: 0x0600AFE6 RID: 45030 RVA: 0x0041A159 File Offset: 0x00418359
	public virtual float OnValue_SlopeLimit
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.slopeLimit;
		}
	}

	// Token: 0x17001464 RID: 5220
	// (get) Token: 0x0600AFE7 RID: 45031 RVA: 0x0041A166 File Offset: 0x00418366
	public virtual float OnValue_Radius
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.radius;
		}
	}

	// Token: 0x17001465 RID: 5221
	// (get) Token: 0x0600AFE8 RID: 45032 RVA: 0x0041A173 File Offset: 0x00418373
	public virtual float OnValue_Height
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.height;
		}
	}

	// Token: 0x0600AFE9 RID: 45033 RVA: 0x0041A180 File Offset: 0x00418380
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_Move(Vector3 direction)
	{
		if (this.CharacterController.enabled)
		{
			this.LastMoveCollisionFlags = this.CharacterController.Move(direction);
		}
	}

	// Token: 0x17001466 RID: 5222
	// (get) Token: 0x0600AFEA RID: 45034 RVA: 0x0041A1A1 File Offset: 0x004183A1
	// (set) Token: 0x0600AFEB RID: 45035 RVA: 0x0041A1A9 File Offset: 0x004183A9
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

	// Token: 0x17001467 RID: 5223
	// (get) Token: 0x0600AFEC RID: 45036 RVA: 0x0041A1B2 File Offset: 0x004183B2
	public virtual bool OnValue_MotorJumpDone
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_MotorJumpDone;
		}
	}

	// Token: 0x17001468 RID: 5224
	// (get) Token: 0x0600AFED RID: 45037 RVA: 0x0041A1BA File Offset: 0x004183BA
	// (set) Token: 0x0600AFEE RID: 45038 RVA: 0x0041A1C2 File Offset: 0x004183C2
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

	// Token: 0x17001469 RID: 5225
	// (get) Token: 0x0600AFEF RID: 45039 RVA: 0x0041A1CB File Offset: 0x004183CB
	// (set) Token: 0x0600AFF0 RID: 45040 RVA: 0x0041A1D3 File Offset: 0x004183D3
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

	// Token: 0x1700146A RID: 5226
	// (get) Token: 0x0600AFF1 RID: 45041 RVA: 0x0041A1DC File Offset: 0x004183DC
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

	// Token: 0x1700146B RID: 5227
	// (get) Token: 0x0600AFF2 RID: 45042 RVA: 0x0041A2A1 File Offset: 0x004184A1
	public virtual vp_SurfaceIdentifier OnValue_SurfaceType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_CurrentSurface;
		}
	}

	// Token: 0x0600AFF3 RID: 45043 RVA: 0x0041A2A9 File Offset: 0x004184A9
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		this.Player.OutOfControl.Stop(0f);
	}

	// Token: 0x0600AFF4 RID: 45044 RVA: 0x0041A2C0 File Offset: 0x004184C0
	public void ScaleFallSpeed(float scale)
	{
		this.m_FallSpeed *= scale;
		this.m_MaxHeightInitialFallSpeed *= scale;
		float num = base.Transform.position.y + Origin.position.y;
		this.m_MaxHeight = num + scale * scale * (this.m_MaxHeight - num);
	}

	// Token: 0x0400834C RID: 33612
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x0400834D RID: 33613
	public EntityPlayerLocal localPlayer;

	// Token: 0x0400834E RID: 33614
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CharacterController m_CharacterController;

	// Token: 0x0400834F RID: 33615
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_FixedPosition = Vector3.zero;

	// Token: 0x04008350 RID: 33616
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_SmoothPosition = Vector3.zero;

	// Token: 0x04008351 RID: 33617
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SpeedModifier = 1f;

	// Token: 0x04008352 RID: 33618
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Grounded;

	// Token: 0x04008353 RID: 33619
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_HeadContact;

	// Token: 0x04008354 RID: 33620
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_GroundHit;

	// Token: 0x04008355 RID: 33621
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_LastGroundHit;

	// Token: 0x04008356 RID: 33622
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_CeilingHit;

	// Token: 0x04008357 RID: 33623
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_WallHit;

	// Token: 0x04008358 RID: 33624
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FallImpact;

	// Token: 0x04008359 RID: 33625
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Terrain m_CurrentTerrain;

	// Token: 0x0400835A RID: 33626
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_SurfaceIdentifier m_CurrentSurface;

	// Token: 0x0400835B RID: 33627
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CollisionFlags LastMoveCollisionFlags;

	// Token: 0x0400835C RID: 33628
	public float MotorAcceleration = 0.18f;

	// Token: 0x0400835D RID: 33629
	public float MotorDamping = 0.17f;

	// Token: 0x0400835E RID: 33630
	public float MotorBackwardsSpeed = 0.65f;

	// Token: 0x0400835F RID: 33631
	public float MotorSidewaysSpeed = 0.65f;

	// Token: 0x04008360 RID: 33632
	public float MotorAirSpeed = 0.35f;

	// Token: 0x04008361 RID: 33633
	public float MotorSlopeSpeedUp = 1f;

	// Token: 0x04008362 RID: 33634
	public float MotorSlopeSpeedDown = 1f;

	// Token: 0x04008363 RID: 33635
	public bool MotorFreeFly;

	// Token: 0x04008364 RID: 33636
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_MoveDirection = Vector3.zero;

	// Token: 0x04008365 RID: 33637
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SlopeFactor = 1f;

	// Token: 0x04008366 RID: 33638
	public Vector3 m_MotorThrottle = Vector3.zero;

	// Token: 0x04008367 RID: 33639
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MotorAirSpeedModifier = 1f;

	// Token: 0x04008368 RID: 33640
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentAntiBumpOffset;

	// Token: 0x04008369 RID: 33641
	public float originalMotorJumpForce = -1f;

	// Token: 0x0400836A RID: 33642
	public float MotorJumpForce = 0.18f;

	// Token: 0x0400836B RID: 33643
	public float MotorJumpForceDamping = 0.08f;

	// Token: 0x0400836C RID: 33644
	public float MotorJumpForceHold = 0.003f;

	// Token: 0x0400836D RID: 33645
	public float MotorJumpForceHoldDamping = 0.5f;

	// Token: 0x0400836E RID: 33646
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_MotorJumpForceHoldSkipFrames;

	// Token: 0x0400836F RID: 33647
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MotorJumpForceAcc;

	// Token: 0x04008370 RID: 33648
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_MotorJumpDone = true;

	// Token: 0x04008371 RID: 33649
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FallSpeed;

	// Token: 0x04008372 RID: 33650
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MaxHeight = float.MinValue;

	// Token: 0x04008373 RID: 33651
	public float m_MaxHeightInitialFallSpeed;

	// Token: 0x04008374 RID: 33652
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_GravityForce;

	// Token: 0x04008375 RID: 33653
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 slideLastGroundN;

	// Token: 0x04008376 RID: 33654
	public float PhysicsForceDamping = 0.05f;

	// Token: 0x04008377 RID: 33655
	public float PhysicsPushForce = 5f;

	// Token: 0x04008378 RID: 33656
	public float PhysicsGravityModifier = 0.2f;

	// Token: 0x04008379 RID: 33657
	public float PhysicsSlopeSlideLimit = 30f;

	// Token: 0x0400837A RID: 33658
	public float PhysicsSlopeSlidiness = 0.15f;

	// Token: 0x0400837B RID: 33659
	public float PhysicsWallBounce;

	// Token: 0x0400837C RID: 33660
	public float PhysicsWallFriction;

	// Token: 0x0400837D RID: 33661
	public float PhysicsCrouchHeightModifier = 0.5f;

	// Token: 0x0400837E RID: 33662
	public bool PhysicsHasCollisionTrigger = true;

	// Token: 0x0400837F RID: 33663
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_Trigger;

	// Token: 0x04008380 RID: 33664
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CapsuleCollider m_TriggerCollider;

	// Token: 0x04008381 RID: 33665
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_ExternalForce = Vector3.zero;

	// Token: 0x04008382 RID: 33666
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3[] m_SmoothForceFrame = new Vector3[120];

	// Token: 0x04008383 RID: 33667
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Slide;

	// Token: 0x04008384 RID: 33668
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_SlideFast;

	// Token: 0x04008385 RID: 33669
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SlideFallSpeed;

	// Token: 0x04008386 RID: 33670
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_OnSteepGroundSince;

	// Token: 0x04008387 RID: 33671
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SlopeSlideSpeed;

	// Token: 0x04008388 RID: 33672
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PredictedPos = Vector3.zero;

	// Token: 0x04008389 RID: 33673
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PrevPos = Vector3.zero;

	// Token: 0x0400838A RID: 33674
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PrevDir = Vector3.zero;

	// Token: 0x0400838B RID: 33675
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_NewDir = Vector3.zero;

	// Token: 0x0400838C RID: 33676
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_ForceImpact;

	// Token: 0x0400838D RID: 33677
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_ForceMultiplier;

	// Token: 0x0400838E RID: 33678
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 CapsuleBottom = Vector3.zero;

	// Token: 0x0400838F RID: 33679
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 CapsuleTop = Vector3.zero;

	// Token: 0x04008390 RID: 33680
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_StepHeight = 0.7f;

	// Token: 0x04008391 RID: 33681
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SkinWidth = 0.08f;

	// Token: 0x04008392 RID: 33682
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Platform;

	// Token: 0x04008393 RID: 33683
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PositionOnPlatform = Vector3.zero;

	// Token: 0x04008394 RID: 33684
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastPlatformAngle;

	// Token: 0x04008395 RID: 33685
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_LastPlatformPos = Vector3.zero;

	// Token: 0x04008396 RID: 33686
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NormalHeight;

	// Token: 0x04008397 RID: 33687
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_NormalCenter = Vector3.zero;

	// Token: 0x04008398 RID: 33688
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CrouchHeight;

	// Token: 0x04008399 RID: 33689
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CrouchCenter = Vector3.zero;
}
