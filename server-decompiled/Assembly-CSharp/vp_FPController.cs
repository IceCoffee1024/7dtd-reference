using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001626 RID: 5670
[RequireComponent(typeof(vp_FPPlayerEventHandler))]
[RequireComponent(typeof(CharacterController))]
[Preserve]
public class vp_FPController : vp_Component
{
	// Token: 0x170014A1 RID: 5281
	// (get) Token: 0x0600B08F RID: 45199 RVA: 0x0041DE55 File Offset: 0x0041C055
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

	// Token: 0x170014A2 RID: 5282
	// (get) Token: 0x0600B090 RID: 45200 RVA: 0x0041DE8A File Offset: 0x0041C08A
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

	// Token: 0x170014A3 RID: 5283
	// (get) Token: 0x0600B091 RID: 45201 RVA: 0x0041DEB1 File Offset: 0x0041C0B1
	public Vector3 SmoothPosition
	{
		get
		{
			return this.m_SmoothPosition;
		}
	}

	// Token: 0x170014A4 RID: 5284
	// (get) Token: 0x0600B092 RID: 45202 RVA: 0x0041DEB9 File Offset: 0x0041C0B9
	public Vector3 Velocity
	{
		get
		{
			return this.m_CharacterController.velocity;
		}
	}

	// Token: 0x170014A5 RID: 5285
	// (get) Token: 0x0600B093 RID: 45203 RVA: 0x0041DEC6 File Offset: 0x0041C0C6
	// (set) Token: 0x0600B094 RID: 45204 RVA: 0x0041DECE File Offset: 0x0041C0CE
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

	// Token: 0x0600B095 RID: 45205 RVA: 0x0041DED7 File Offset: 0x0041C0D7
	public void Reposition(Vector3 _deltaVec)
	{
		this.m_SmoothPosition += _deltaVec;
		this.SetPosition(this.m_SmoothPosition);
	}

	// Token: 0x170014A6 RID: 5286
	// (get) Token: 0x0600B096 RID: 45206 RVA: 0x0041DEF7 File Offset: 0x0041C0F7
	public bool Grounded
	{
		get
		{
			return this.m_Grounded;
		}
	}

	// Token: 0x170014A7 RID: 5287
	// (get) Token: 0x0600B097 RID: 45207 RVA: 0x0041DEFF File Offset: 0x0041C0FF
	public bool HeadContact
	{
		get
		{
			return this.m_HeadContact;
		}
	}

	// Token: 0x170014A8 RID: 5288
	// (get) Token: 0x0600B098 RID: 45208 RVA: 0x0041DF07 File Offset: 0x0041C107
	public Vector3 GroundNormal
	{
		get
		{
			return this.m_GroundHit.normal;
		}
	}

	// Token: 0x170014A9 RID: 5289
	// (get) Token: 0x0600B099 RID: 45209 RVA: 0x0041DF14 File Offset: 0x0041C114
	public float GroundAngle
	{
		get
		{
			return Vector3.Angle(this.m_GroundHit.normal, Vector3.up);
		}
	}

	// Token: 0x170014AA RID: 5290
	// (get) Token: 0x0600B09A RID: 45210 RVA: 0x0041DF2B File Offset: 0x0041C12B
	public Transform GroundTransform
	{
		get
		{
			return this.m_GroundHit.transform;
		}
	}

	// Token: 0x170014AB RID: 5291
	// (get) Token: 0x0600B09B RID: 45211 RVA: 0x0041DF38 File Offset: 0x0041C138
	public bool IsCollidingWall
	{
		get
		{
			return this.m_WallHit.collider != null;
		}
	}

	// Token: 0x170014AC RID: 5292
	// (get) Token: 0x0600B09C RID: 45212 RVA: 0x0041DF4B File Offset: 0x0041C14B
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

	// Token: 0x170014AD RID: 5293
	// (get) Token: 0x0600B09D RID: 45213 RVA: 0x0041DF6C File Offset: 0x0041C16C
	public Vector3 FixedPosition
	{
		get
		{
			return this.m_FixedPosition;
		}
	}

	// Token: 0x170014AE RID: 5294
	// (get) Token: 0x0600B09E RID: 45214 RVA: 0x0041DF74 File Offset: 0x0041C174
	public float SkinWidth
	{
		get
		{
			return this.m_SkinWidth;
		}
	}

	// Token: 0x0600B09F RID: 45215 RVA: 0x0041DF7C File Offset: 0x0041C17C
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

	// Token: 0x0600B0A0 RID: 45216 RVA: 0x0041DFA4 File Offset: 0x0041C1A4
	public void SyncCharacterController()
	{
		this.m_NormalHeight = this.CharacterController.height;
		this.CharacterController.center = (this.m_NormalCenter = new Vector3(0f, this.m_NormalHeight * 0.5f, 0f));
		this.CharacterController.radius = this.m_NormalHeight * 0.16666f;
		this.m_CrouchHeight = this.m_NormalHeight * this.PhysicsCrouchHeightModifier;
		this.m_CrouchCenter = this.m_NormalCenter * this.PhysicsCrouchHeightModifier;
		this.m_StepHeight = this.CharacterController.stepOffset;
	}

	// Token: 0x0600B0A1 RID: 45217 RVA: 0x0041E043 File Offset: 0x0041C243
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		base.OnEnable();
		vp_TargetEvent<Vector3>.Register(this.m_Transform, "ForceImpact", new Action<Vector3>(this.AddForce));
	}

	// Token: 0x0600B0A2 RID: 45218 RVA: 0x0041E068 File Offset: 0x0041C268
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
		vp_TargetEvent<Vector3>.Unregister(this.m_Root, "ForceImpact", new Action<Vector3>(this.AddForce));
	}

	// Token: 0x0600B0A3 RID: 45219 RVA: 0x0041E08D File Offset: 0x0041C28D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.SetPosition(base.Transform.position);
	}

	// Token: 0x0600B0A4 RID: 45220 RVA: 0x0041E0A6 File Offset: 0x0041C2A6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		this.SmoothMove();
	}

	// Token: 0x0600B0A5 RID: 45221 RVA: 0x0041E0B4 File Offset: 0x0041C2B4
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

	// Token: 0x0600B0A6 RID: 45222 RVA: 0x0041E161 File Offset: 0x0041C361
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

	// Token: 0x0600B0A7 RID: 45223 RVA: 0x0041E194 File Offset: 0x0041C394
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

	// Token: 0x0600B0A8 RID: 45224 RVA: 0x0041E338 File Offset: 0x0041C538
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

	// Token: 0x0600B0A9 RID: 45225 RVA: 0x0041E448 File Offset: 0x0041C648
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

	// Token: 0x0600B0AA RID: 45226 RVA: 0x0041E4E4 File Offset: 0x0041C6E4
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

	// Token: 0x0600B0AB RID: 45227 RVA: 0x0041E558 File Offset: 0x0041C758
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

	// Token: 0x0600B0AC RID: 45228 RVA: 0x0041E600 File Offset: 0x0041C800
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

	// Token: 0x0600B0AD RID: 45229 RVA: 0x0041E700 File Offset: 0x0041C900
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

	// Token: 0x0600B0AE RID: 45230 RVA: 0x0041E990 File Offset: 0x0041CB90
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

	// Token: 0x0600B0AF RID: 45231 RVA: 0x0041EA04 File Offset: 0x0041CC04
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

	// Token: 0x0600B0B0 RID: 45232 RVA: 0x0041F038 File Offset: 0x0041D238
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

	// Token: 0x0600B0B1 RID: 45233 RVA: 0x0041F1B8 File Offset: 0x0041D3B8
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

	// Token: 0x0600B0B2 RID: 45234 RVA: 0x0041F490 File Offset: 0x0041D690
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

	// Token: 0x0600B0B3 RID: 45235 RVA: 0x0041F5CC File Offset: 0x0041D7CC
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

	// Token: 0x0600B0B4 RID: 45236 RVA: 0x0041F6A6 File Offset: 0x0041D8A6
	public virtual void SetPosition(Vector3 position)
	{
		base.Transform.position = position;
		this.m_PrevPos = position;
		this.m_SmoothPosition = position;
		this.m_FixedPosition = position;
		Physics.SyncTransforms();
	}

	// Token: 0x0600B0B5 RID: 45237 RVA: 0x0041F6CE File Offset: 0x0041D8CE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void AddForceInternal(Vector3 force)
	{
		this.m_ExternalForce += force;
	}

	// Token: 0x0600B0B6 RID: 45238 RVA: 0x0041F6E2 File Offset: 0x0041D8E2
	public virtual void AddForce(float x, float y, float z)
	{
		this.AddForce(new Vector3(x, y, z));
	}

	// Token: 0x0600B0B7 RID: 45239 RVA: 0x0041F6F2 File Offset: 0x0041D8F2
	public virtual void AddForce(Vector3 force)
	{
		if (Time.timeScale >= 1f)
		{
			this.AddForceInternal(force);
			return;
		}
		this.AddSoftForce(force, 1f);
	}

	// Token: 0x0600B0B8 RID: 45240 RVA: 0x0041F714 File Offset: 0x0041D914
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

	// Token: 0x0600B0B9 RID: 45241 RVA: 0x0041F784 File Offset: 0x0041D984
	public virtual void StopSoftForce()
	{
		int num = 0;
		while (num < 120 && !(this.m_SmoothForceFrame[num] == Vector3.zero))
		{
			this.m_SmoothForceFrame[num] = Vector3.zero;
			num++;
		}
	}

	// Token: 0x0600B0BA RID: 45242 RVA: 0x0041F7C8 File Offset: 0x0041D9C8
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

	// Token: 0x0600B0BB RID: 45243 RVA: 0x0041F86C File Offset: 0x0041DA6C
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

	// Token: 0x0600B0BC RID: 45244 RVA: 0x0041F8DC File Offset: 0x0041DADC
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

	// Token: 0x0600B0BD RID: 45245 RVA: 0x0041FA34 File Offset: 0x0041DC34
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

	// Token: 0x0600B0BE RID: 45246 RVA: 0x0041FD5C File Offset: 0x0041DF5C
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

	// Token: 0x0600B0BF RID: 45247 RVA: 0x0041FE38 File Offset: 0x0041E038
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

	// Token: 0x0600B0C0 RID: 45248 RVA: 0x00420000 File Offset: 0x0041E200
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

	// Token: 0x0600B0C1 RID: 45249 RVA: 0x00420079 File Offset: 0x0041E279
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Jump()
	{
		return this.MotorFreeFly || (this.m_Grounded && this.m_MotorJumpDone);
	}

	// Token: 0x0600B0C2 RID: 45250 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Swim()
	{
		return true;
	}

	// Token: 0x0600B0C3 RID: 45251 RVA: 0x0042009A File Offset: 0x0041E29A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Swim()
	{
		this.m_ExternalForce.y = this.m_ExternalForce.y + this.m_FallSpeed * 0.2f;
		this.m_FallSpeed = 0f;
	}

	// Token: 0x0600B0C4 RID: 45252 RVA: 0x004200C2 File Offset: 0x0041E2C2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Run()
	{
		return !this.Player.Crouch.Active;
	}

	// Token: 0x0600B0C5 RID: 45253 RVA: 0x004200DC File Offset: 0x0041E2DC
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

	// Token: 0x0600B0C6 RID: 45254 RVA: 0x00420145 File Offset: 0x0041E345
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Jump()
	{
		this.m_MotorJumpDone = true;
	}

	// Token: 0x0600B0C7 RID: 45255 RVA: 0x00420150 File Offset: 0x0041E350
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

	// Token: 0x0600B0C8 RID: 45256 RVA: 0x004201D3 File Offset: 0x0041E3D3
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Crouch()
	{
		this.Player.Run.Stop(0f);
		this.RefreshColliders();
	}

	// Token: 0x0600B0C9 RID: 45257 RVA: 0x004201F0 File Offset: 0x0041E3F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Crouch()
	{
		this.RefreshColliders();
	}

	// Token: 0x0600B0CA RID: 45258 RVA: 0x004201F8 File Offset: 0x0041E3F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_ForceImpact(Vector3 force)
	{
		this.AddForce(force);
	}

	// Token: 0x0600B0CB RID: 45259 RVA: 0x00420201 File Offset: 0x0041E401
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_Stop()
	{
		this.Stop();
	}

	// Token: 0x170014AF RID: 5295
	// (get) Token: 0x0600B0CC RID: 45260 RVA: 0x00420209 File Offset: 0x0041E409
	// (set) Token: 0x0600B0CD RID: 45261 RVA: 0x00420216 File Offset: 0x0041E416
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

	// Token: 0x170014B0 RID: 5296
	// (get) Token: 0x0600B0CE RID: 45262 RVA: 0x0042021F File Offset: 0x0041E41F
	public virtual Vector3 OnValue_Velocity
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.velocity;
		}
	}

	// Token: 0x170014B1 RID: 5297
	// (get) Token: 0x0600B0CF RID: 45263 RVA: 0x0042022C File Offset: 0x0041E42C
	public virtual float OnValue_StepOffset
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.stepOffset;
		}
	}

	// Token: 0x170014B2 RID: 5298
	// (get) Token: 0x0600B0D0 RID: 45264 RVA: 0x00420239 File Offset: 0x0041E439
	public virtual float OnValue_SlopeLimit
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.slopeLimit;
		}
	}

	// Token: 0x170014B3 RID: 5299
	// (get) Token: 0x0600B0D1 RID: 45265 RVA: 0x00420246 File Offset: 0x0041E446
	public virtual float OnValue_Radius
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.radius;
		}
	}

	// Token: 0x170014B4 RID: 5300
	// (get) Token: 0x0600B0D2 RID: 45266 RVA: 0x00420253 File Offset: 0x0041E453
	public virtual float OnValue_Height
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CharacterController.height;
		}
	}

	// Token: 0x0600B0D3 RID: 45267 RVA: 0x00420260 File Offset: 0x0041E460
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_Move(Vector3 direction)
	{
		if (this.CharacterController.enabled)
		{
			this.LastMoveCollisionFlags = this.CharacterController.Move(direction);
		}
	}

	// Token: 0x170014B5 RID: 5301
	// (get) Token: 0x0600B0D4 RID: 45268 RVA: 0x00420281 File Offset: 0x0041E481
	// (set) Token: 0x0600B0D5 RID: 45269 RVA: 0x00420289 File Offset: 0x0041E489
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

	// Token: 0x170014B6 RID: 5302
	// (get) Token: 0x0600B0D6 RID: 45270 RVA: 0x00420292 File Offset: 0x0041E492
	public virtual bool OnValue_MotorJumpDone
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_MotorJumpDone;
		}
	}

	// Token: 0x170014B7 RID: 5303
	// (get) Token: 0x0600B0D7 RID: 45271 RVA: 0x0042029A File Offset: 0x0041E49A
	// (set) Token: 0x0600B0D8 RID: 45272 RVA: 0x004202A2 File Offset: 0x0041E4A2
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

	// Token: 0x170014B8 RID: 5304
	// (get) Token: 0x0600B0D9 RID: 45273 RVA: 0x004202AB File Offset: 0x0041E4AB
	// (set) Token: 0x0600B0DA RID: 45274 RVA: 0x004202B3 File Offset: 0x0041E4B3
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

	// Token: 0x170014B9 RID: 5305
	// (get) Token: 0x0600B0DB RID: 45275 RVA: 0x004202BC File Offset: 0x0041E4BC
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

	// Token: 0x170014BA RID: 5306
	// (get) Token: 0x0600B0DC RID: 45276 RVA: 0x00420381 File Offset: 0x0041E581
	public virtual vp_SurfaceIdentifier OnValue_SurfaceType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_CurrentSurface;
		}
	}

	// Token: 0x0600B0DD RID: 45277 RVA: 0x00420389 File Offset: 0x0041E589
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		this.Player.OutOfControl.Stop(0f);
	}

	// Token: 0x0600B0DE RID: 45278 RVA: 0x004203A0 File Offset: 0x0041E5A0
	public void ScaleFallSpeed(float scale)
	{
		this.m_FallSpeed *= scale;
		this.m_MaxHeightInitialFallSpeed *= scale;
		float num = base.Transform.position.y + Origin.position.y;
		this.m_MaxHeight = num + scale * scale * (this.m_MaxHeight - num);
	}

	// Token: 0x040083E3 RID: 33763
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x040083E4 RID: 33764
	public EntityPlayerLocal localPlayer;

	// Token: 0x040083E5 RID: 33765
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CharacterController m_CharacterController;

	// Token: 0x040083E6 RID: 33766
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_FixedPosition = Vector3.zero;

	// Token: 0x040083E7 RID: 33767
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_SmoothPosition = Vector3.zero;

	// Token: 0x040083E8 RID: 33768
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SpeedModifier = 1f;

	// Token: 0x040083E9 RID: 33769
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Grounded;

	// Token: 0x040083EA RID: 33770
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_HeadContact;

	// Token: 0x040083EB RID: 33771
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_GroundHit;

	// Token: 0x040083EC RID: 33772
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_LastGroundHit;

	// Token: 0x040083ED RID: 33773
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_CeilingHit;

	// Token: 0x040083EE RID: 33774
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_WallHit;

	// Token: 0x040083EF RID: 33775
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FallImpact;

	// Token: 0x040083F0 RID: 33776
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Terrain m_CurrentTerrain;

	// Token: 0x040083F1 RID: 33777
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_SurfaceIdentifier m_CurrentSurface;

	// Token: 0x040083F2 RID: 33778
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CollisionFlags LastMoveCollisionFlags;

	// Token: 0x040083F3 RID: 33779
	public float MotorAcceleration = 0.18f;

	// Token: 0x040083F4 RID: 33780
	public float MotorDamping = 0.17f;

	// Token: 0x040083F5 RID: 33781
	public float MotorBackwardsSpeed = 0.65f;

	// Token: 0x040083F6 RID: 33782
	public float MotorSidewaysSpeed = 0.65f;

	// Token: 0x040083F7 RID: 33783
	public float MotorAirSpeed = 0.35f;

	// Token: 0x040083F8 RID: 33784
	public float MotorSlopeSpeedUp = 1f;

	// Token: 0x040083F9 RID: 33785
	public float MotorSlopeSpeedDown = 1f;

	// Token: 0x040083FA RID: 33786
	public bool MotorFreeFly;

	// Token: 0x040083FB RID: 33787
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_MoveDirection = Vector3.zero;

	// Token: 0x040083FC RID: 33788
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SlopeFactor = 1f;

	// Token: 0x040083FD RID: 33789
	public Vector3 m_MotorThrottle = Vector3.zero;

	// Token: 0x040083FE RID: 33790
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MotorAirSpeedModifier = 1f;

	// Token: 0x040083FF RID: 33791
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentAntiBumpOffset;

	// Token: 0x04008400 RID: 33792
	public float originalMotorJumpForce = -1f;

	// Token: 0x04008401 RID: 33793
	public float MotorJumpForce = 0.18f;

	// Token: 0x04008402 RID: 33794
	public float MotorJumpForceDamping = 0.08f;

	// Token: 0x04008403 RID: 33795
	public float MotorJumpForceHold = 0.003f;

	// Token: 0x04008404 RID: 33796
	public float MotorJumpForceHoldDamping = 0.5f;

	// Token: 0x04008405 RID: 33797
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_MotorJumpForceHoldSkipFrames;

	// Token: 0x04008406 RID: 33798
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MotorJumpForceAcc;

	// Token: 0x04008407 RID: 33799
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_MotorJumpDone = true;

	// Token: 0x04008408 RID: 33800
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FallSpeed;

	// Token: 0x04008409 RID: 33801
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MaxHeight = float.MinValue;

	// Token: 0x0400840A RID: 33802
	public float m_MaxHeightInitialFallSpeed;

	// Token: 0x0400840B RID: 33803
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_GravityForce;

	// Token: 0x0400840C RID: 33804
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 slideLastGroundN;

	// Token: 0x0400840D RID: 33805
	public float PhysicsForceDamping = 0.05f;

	// Token: 0x0400840E RID: 33806
	public float PhysicsPushForce = 5f;

	// Token: 0x0400840F RID: 33807
	public float PhysicsGravityModifier = 0.2f;

	// Token: 0x04008410 RID: 33808
	public float PhysicsSlopeSlideLimit = 30f;

	// Token: 0x04008411 RID: 33809
	public float PhysicsSlopeSlidiness = 0.15f;

	// Token: 0x04008412 RID: 33810
	public float PhysicsWallBounce;

	// Token: 0x04008413 RID: 33811
	public float PhysicsWallFriction;

	// Token: 0x04008414 RID: 33812
	public float PhysicsCrouchHeightModifier = 0.5f;

	// Token: 0x04008415 RID: 33813
	public bool PhysicsHasCollisionTrigger = true;

	// Token: 0x04008416 RID: 33814
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_Trigger;

	// Token: 0x04008417 RID: 33815
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CapsuleCollider m_TriggerCollider;

	// Token: 0x04008418 RID: 33816
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_ExternalForce = Vector3.zero;

	// Token: 0x04008419 RID: 33817
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3[] m_SmoothForceFrame = new Vector3[120];

	// Token: 0x0400841A RID: 33818
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Slide;

	// Token: 0x0400841B RID: 33819
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_SlideFast;

	// Token: 0x0400841C RID: 33820
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SlideFallSpeed;

	// Token: 0x0400841D RID: 33821
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_OnSteepGroundSince;

	// Token: 0x0400841E RID: 33822
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SlopeSlideSpeed;

	// Token: 0x0400841F RID: 33823
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PredictedPos = Vector3.zero;

	// Token: 0x04008420 RID: 33824
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PrevPos = Vector3.zero;

	// Token: 0x04008421 RID: 33825
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PrevDir = Vector3.zero;

	// Token: 0x04008422 RID: 33826
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_NewDir = Vector3.zero;

	// Token: 0x04008423 RID: 33827
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_ForceImpact;

	// Token: 0x04008424 RID: 33828
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_ForceMultiplier;

	// Token: 0x04008425 RID: 33829
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 CapsuleBottom = Vector3.zero;

	// Token: 0x04008426 RID: 33830
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 CapsuleTop = Vector3.zero;

	// Token: 0x04008427 RID: 33831
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_StepHeight = 0.7f;

	// Token: 0x04008428 RID: 33832
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_SkinWidth = 0.08f;

	// Token: 0x04008429 RID: 33833
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Platform;

	// Token: 0x0400842A RID: 33834
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PositionOnPlatform = Vector3.zero;

	// Token: 0x0400842B RID: 33835
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastPlatformAngle;

	// Token: 0x0400842C RID: 33836
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_LastPlatformPos = Vector3.zero;

	// Token: 0x0400842D RID: 33837
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NormalHeight;

	// Token: 0x0400842E RID: 33838
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_NormalCenter = Vector3.zero;

	// Token: 0x0400842F RID: 33839
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CrouchHeight;

	// Token: 0x04008430 RID: 33840
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CrouchCenter = Vector3.zero;
}
