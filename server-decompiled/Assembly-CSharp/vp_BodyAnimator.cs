using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001626 RID: 5670
[RequireComponent(typeof(Animator))]
public class vp_BodyAnimator : MonoBehaviour
{
	// Token: 0x170014DE RID: 5342
	// (get) Token: 0x0600B17B RID: 45435 RVA: 0x004244E0 File Offset: 0x004226E0
	public vp_WeaponHandler WeaponHandler
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_WeaponHandler == null)
			{
				this.m_WeaponHandler = (vp_WeaponHandler)base.transform.root.GetComponentInChildren(typeof(vp_WeaponHandler));
			}
			return this.m_WeaponHandler;
		}
	}

	// Token: 0x170014DF RID: 5343
	// (get) Token: 0x0600B17C RID: 45436 RVA: 0x0042451B File Offset: 0x0042271B
	public Transform Transform
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Transform == null)
			{
				this.m_Transform = base.transform;
			}
			return this.m_Transform;
		}
	}

	// Token: 0x170014E0 RID: 5344
	// (get) Token: 0x0600B17D RID: 45437 RVA: 0x0042453D File Offset: 0x0042273D
	public vp_PlayerEventHandler Player
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Player == null)
			{
				this.m_Player = (vp_PlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_PlayerEventHandler));
			}
			return this.m_Player;
		}
	}

	// Token: 0x170014E1 RID: 5345
	// (get) Token: 0x0600B17E RID: 45438 RVA: 0x00424578 File Offset: 0x00422778
	public SkinnedMeshRenderer Renderer
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Renderer == null)
			{
				this.m_Renderer = base.transform.root.GetComponentInChildren<SkinnedMeshRenderer>();
			}
			return this.m_Renderer;
		}
	}

	// Token: 0x170014E2 RID: 5346
	// (get) Token: 0x0600B17F RID: 45439 RVA: 0x004245A4 File Offset: 0x004227A4
	public Animator Animator
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Animator == null)
			{
				this.m_Animator = base.GetComponent<Animator>();
			}
			return this.m_Animator;
		}
	}

	// Token: 0x170014E3 RID: 5347
	// (get) Token: 0x0600B180 RID: 45440 RVA: 0x004245C6 File Offset: 0x004227C6
	public Vector3 m_LocalVelocity
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return vp_MathUtility.SnapToZero(this.Transform.root.InverseTransformDirection(this.Player.Velocity.Get()) / this.m_MaxSpeed, 0.0001f);
		}
	}

	// Token: 0x170014E4 RID: 5348
	// (get) Token: 0x0600B181 RID: 45441 RVA: 0x00424602 File Offset: 0x00422802
	public float m_MaxSpeed
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.Player.Run.Active)
			{
				return this.m_MaxRunSpeed;
			}
			if (this.Player.Crouch.Active)
			{
				return this.m_MaxCrouchSpeed;
			}
			return this.m_MaxWalkSpeed;
		}
	}

	// Token: 0x170014E5 RID: 5349
	// (get) Token: 0x0600B182 RID: 45442 RVA: 0x0042463C File Offset: 0x0042283C
	public GameObject HeadPoint
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_HeadPoint == null)
			{
				this.m_HeadPoint = new GameObject("HeadPoint");
				this.m_HeadPoint.transform.parent = this.m_HeadLookBones[0].transform;
				this.m_HeadPoint.transform.localPosition = Vector3.zero;
				this.HeadPoint.transform.eulerAngles = this.Player.Rotation.Get();
			}
			return this.m_HeadPoint;
		}
	}

	// Token: 0x170014E6 RID: 5350
	// (get) Token: 0x0600B183 RID: 45443 RVA: 0x004246CD File Offset: 0x004228CD
	public GameObject DebugLookTarget
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_DebugLookTarget == null)
			{
				this.m_DebugLookTarget = vp_3DUtility.DebugBall(null);
			}
			return this.m_DebugLookTarget;
		}
	}

	// Token: 0x170014E7 RID: 5351
	// (get) Token: 0x0600B184 RID: 45444 RVA: 0x004246F0 File Offset: 0x004228F0
	public GameObject DebugLookArrow
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_DebugLookArrow == null)
			{
				this.m_DebugLookArrow = vp_3DUtility.DebugPointer(null);
				this.m_DebugLookArrow.transform.parent = this.HeadPoint.transform;
				this.m_DebugLookArrow.transform.localPosition = Vector3.zero;
				this.m_DebugLookArrow.transform.localRotation = Quaternion.identity;
				return this.m_DebugLookArrow;
			}
			return this.m_DebugLookArrow;
		}
	}

	// Token: 0x0600B185 RID: 45445 RVA: 0x00424769 File Offset: 0x00422969
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
	}

	// Token: 0x0600B186 RID: 45446 RVA: 0x00424785 File Offset: 0x00422985
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B187 RID: 45447 RVA: 0x004247A1 File Offset: 0x004229A1
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		if (!this.IsValidSetup())
		{
			return;
		}
		this.InitHashIDs();
		this.InitHeadLook();
		this.InitMaxSpeeds();
	}

	// Token: 0x0600B188 RID: 45448 RVA: 0x004247C0 File Offset: 0x004229C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void LateUpdate()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}
		if (!this.m_IsValid)
		{
			base.enabled = false;
			return;
		}
		this.UpdatePosition();
		this.UpdateGrounding();
		this.UpdateBody();
		this.UpdateSpine();
		this.UpdateAnimationSpeeds();
		this.UpdateAnimator();
		this.UpdateDebugInfo();
		this.UpdateHeadPoint();
	}

	// Token: 0x0600B189 RID: 45449 RVA: 0x0042481C File Offset: 0x00422A1C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateAnimationSpeeds()
	{
		if (Time.time > this.m_NextAllowedUpdateTurnTargetTime)
		{
			this.m_CurrentTurnTarget = Mathf.DeltaAngle(this.m_PrevBodyYaw, this.m_BodyYaw) * (this.Player.Crouch.Active ? 100f : 0.2f);
			this.m_NextAllowedUpdateTurnTargetTime = Time.time + 0.1f;
		}
		this.m_CurrentTurn = Mathf.Lerp(this.m_CurrentTurn, this.m_CurrentTurnTarget, Time.deltaTime);
		if (Mathf.Round(this.Transform.root.eulerAngles.y) == Mathf.Round(this.m_LastYaw))
		{
			this.m_CurrentTurn *= 0.6f;
		}
		this.m_LastYaw = this.Transform.root.eulerAngles.y;
		this.m_CurrentTurn = vp_MathUtility.SnapToZero(this.m_CurrentTurn, 0.0001f);
		this.m_CurrentForward = Mathf.Lerp(this.m_CurrentForward, this.m_LocalVelocity.z, Time.deltaTime * 100f);
		this.m_CurrentForward = ((Mathf.Abs(this.m_CurrentForward) > 0.03f) ? this.m_CurrentForward : 0f);
		if (this.Player.Crouch.Active)
		{
			if (Mathf.Abs(this.GetStrafeDirection()) < Mathf.Abs(this.m_CurrentTurn))
			{
				this.m_CurrentStrafe = Mathf.Lerp(this.m_CurrentStrafe, this.m_CurrentTurn, Time.deltaTime * 5f);
			}
			else
			{
				this.m_CurrentStrafe = Mathf.Lerp(this.m_CurrentStrafe, this.GetStrafeDirection(), Time.deltaTime * 5f);
			}
		}
		else
		{
			this.m_CurrentStrafe = Mathf.Lerp(this.m_CurrentStrafe, this.GetStrafeDirection(), Time.deltaTime * 5f);
		}
		this.m_CurrentStrafe = ((Mathf.Abs(this.m_CurrentStrafe) > 0.03f) ? this.m_CurrentStrafe : 0f);
	}

	// Token: 0x0600B18A RID: 45450 RVA: 0x00424A08 File Offset: 0x00422C08
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual float GetStrafeDirection()
	{
		if (this.Player.InputMoveVector.Get().x < 0f)
		{
			return -1f;
		}
		if (this.Player.InputMoveVector.Get().x > 0f)
		{
			return 1f;
		}
		return 0f;
	}

	// Token: 0x0600B18B RID: 45451 RVA: 0x00424A68 File Offset: 0x00422C68
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateAnimator()
	{
		this.Animator.SetBool(this.IsRunning, this.Player.Run.Active && this.GetIsMoving());
		this.Animator.SetBool(this.IsCrouching, this.Player.Crouch.Active);
		this.Animator.SetInteger(this.WeaponTypeIndex, this.Player.CurrentWeaponType.Get());
		this.Animator.SetInteger(this.WeaponGripIndex, this.Player.CurrentWeaponGrip.Get());
		this.Animator.SetBool(this.IsSettingWeapon, this.Player.SetWeapon.Active);
		this.Animator.SetBool(this.IsReloading, this.Player.Reload.Active);
		this.Animator.SetBool(this.IsOutOfControl, this.Player.OutOfControl.Active);
		this.Animator.SetBool(this.IsClimbing, this.Player.Climb.Active);
		this.Animator.SetBool(this.IsZooming, this.Player.Zoom.Active);
		this.Animator.SetBool(this.IsGrounded, this.m_Grounded);
		this.Animator.SetBool(this.IsMoving, this.GetIsMoving());
		this.Animator.SetFloat(this.TurnAmount, this.m_CurrentTurn);
		this.Animator.SetFloat(this.ForwardAmount, this.m_CurrentForward);
		this.Animator.SetFloat(this.StrafeAmount, this.m_CurrentStrafe);
		this.Animator.SetFloat(this.PitchAmount, -this.Player.Rotation.Get().x / 90f);
		if (this.m_Grounded)
		{
			this.Animator.SetFloat(this.VerticalMoveAmount, 0f);
			return;
		}
		if (this.Player.Velocity.Get().y < 0f)
		{
			this.Animator.SetFloat(this.VerticalMoveAmount, Mathf.Lerp(this.Animator.GetFloat(this.VerticalMoveAmount), -1f, Time.deltaTime * 3f));
			return;
		}
		this.Animator.SetFloat(this.VerticalMoveAmount, this.Player.MotorThrottle.Get().y * 10f);
	}

	// Token: 0x0600B18C RID: 45452 RVA: 0x00424D04 File Offset: 0x00422F04
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateDebugInfo()
	{
		if (this.ShowDebugObjects)
		{
			this.DebugLookTarget.transform.position = this.m_HeadLookBones[0].transform.position + this.HeadPoint.transform.forward * 1000f;
			this.DebugLookArrow.transform.LookAt(this.DebugLookTarget.transform.position);
			if (!vp_Utility.IsActive(this.m_DebugLookTarget))
			{
				vp_Utility.Activate(this.m_DebugLookTarget, true);
			}
			if (!vp_Utility.IsActive(this.m_DebugLookArrow))
			{
				vp_Utility.Activate(this.m_DebugLookArrow, true);
				return;
			}
		}
		else
		{
			if (this.m_DebugLookTarget != null)
			{
				vp_Utility.Activate(this.m_DebugLookTarget, false);
			}
			if (this.m_DebugLookArrow != null)
			{
				vp_Utility.Activate(this.m_DebugLookArrow, false);
			}
		}
	}

	// Token: 0x0600B18D RID: 45453 RVA: 0x00424DE8 File Offset: 0x00422FE8
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateHeadPoint()
	{
		if (!this.HeadPointDirty)
		{
			return;
		}
		this.HeadPoint.transform.eulerAngles = this.Player.Rotation.Get();
		this.HeadPointDirty = false;
	}

	// Token: 0x0600B18E RID: 45454 RVA: 0x00424E24 File Offset: 0x00423024
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdatePosition()
	{
		if (this.Player.IsFirstPerson.Get())
		{
			return;
		}
		if (this.Player.Climb.Active)
		{
			this.Transform.localPosition += this.ClimbOffset;
		}
	}

	// Token: 0x0600B18F RID: 45455 RVA: 0x00424E78 File Offset: 0x00423078
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateBody()
	{
		this.m_PrevBodyYaw = this.m_BodyYaw;
		this.m_BodyYaw = Mathf.LerpAngle(this.m_BodyYaw, this.m_CurrentBodyYawTarget, Time.deltaTime * ((this.Player.Velocity.Get().magnitude > 0.1f) ? this.FeetAdjustSpeedMoving : this.FeetAdjustSpeedStanding));
		this.m_BodyYaw = ((this.m_BodyYaw < -360f) ? (this.m_BodyYaw += 360f) : this.m_BodyYaw);
		this.m_BodyYaw = ((this.m_BodyYaw > 360f) ? (this.m_BodyYaw -= 360f) : this.m_BodyYaw);
		this.Transform.eulerAngles = this.m_BodyYaw * Vector3.up;
		this.m_CurrentHeadLookYaw = Mathf.DeltaAngle(this.Player.Rotation.Get().y, this.Transform.eulerAngles.y);
		if (Mathf.Max(0f, this.m_CurrentHeadLookYaw - 90f) > 0f)
		{
			this.Transform.eulerAngles = Vector3.up * (this.Transform.root.eulerAngles.y + 90f);
			this.m_BodyYaw = (this.m_CurrentBodyYawTarget = this.Transform.eulerAngles.y);
		}
		else if (Mathf.Min(0f, this.m_CurrentHeadLookYaw - -90f) < 0f)
		{
			this.Transform.eulerAngles = Vector3.up * (this.Transform.root.eulerAngles.y - 90f);
			this.m_BodyYaw = (this.m_CurrentBodyYawTarget = this.Transform.eulerAngles.y);
		}
		if (Mathf.Abs(this.Player.Rotation.Get().y - this.m_BodyYaw) > 180f)
		{
			if (this.m_BodyYaw > 0f)
			{
				this.m_BodyYaw -= 360f;
				this.m_PrevBodyYaw -= 360f;
			}
			else if (this.m_BodyYaw < 0f)
			{
				this.m_BodyYaw += 360f;
				this.m_PrevBodyYaw += 360f;
			}
		}
		if (this.m_CurrentHeadLookYaw > this.FeetAdjustAngle || this.m_CurrentHeadLookYaw < -this.FeetAdjustAngle || this.Player.Velocity.Get().magnitude > 0.1f)
		{
			this.m_CurrentBodyYawTarget = Mathf.LerpAngle(this.m_CurrentBodyYawTarget, this.Transform.root.eulerAngles.y, 0.1f);
		}
	}

	// Token: 0x0600B190 RID: 45456 RVA: 0x00425168 File Offset: 0x00423368
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateSpine()
	{
		for (int i = 0; i < this.m_HeadLookBones.Count; i++)
		{
			if (this.Player.IsFirstPerson.Get() || this.Animator.GetBool(this.IsAttacking) || this.Animator.GetBool(this.IsZooming))
			{
				this.m_HeadLookTargetFalloffs[i] = this.m_HeadLookFalloffs[this.m_HeadLookFalloffs.Count - 1 - i];
			}
			else
			{
				this.m_HeadLookTargetFalloffs[i] = this.m_HeadLookFalloffs[i];
			}
			if (this.m_WasMoving && !this.Animator.GetBool(this.IsMoving))
			{
				this.m_HeadLookCurrentFalloffs[i] = this.m_HeadLookTargetFalloffs[i];
			}
			this.m_HeadLookCurrentFalloffs[i] = Mathf.SmoothStep(this.m_HeadLookCurrentFalloffs[i], Mathf.LerpAngle(this.m_HeadLookCurrentFalloffs[i], this.m_HeadLookTargetFalloffs[i], Time.deltaTime * 10f), Time.deltaTime * 20f);
			if (this.Player.IsFirstPerson.Get())
			{
				this.m_HeadLookTargetWorldDir = this.GetLookPoint() - this.m_HeadLookBones[0].transform.position;
				this.m_HeadLookCurrentWorldDir = Vector3.Slerp(this.m_HeadLookTargetWorldDir, vp_3DUtility.HorizontalVector(this.m_HeadLookTargetWorldDir), this.m_HeadLookCurrentFalloffs[i] / this.m_HeadLookFalloffs[0]);
			}
			else
			{
				this.m_ValidLookPoint = this.GetLookPoint();
				this.m_ValidLookPointForward = this.Transform.InverseTransformDirection(this.m_ValidLookPoint - this.m_HeadLookBones[0].transform.position).z;
				if (this.m_ValidLookPointForward < 0f)
				{
					this.m_ValidLookPoint += this.Transform.forward * -this.m_ValidLookPointForward;
				}
				this.m_HeadLookTargetWorldDir = Vector3.Slerp(this.m_HeadLookTargetWorldDir, this.m_ValidLookPoint - this.m_HeadLookBones[0].transform.position, Time.deltaTime * this.HeadYawSpeed);
				this.m_HeadLookCurrentWorldDir = Vector3.Slerp(this.m_HeadLookCurrentWorldDir, vp_3DUtility.HorizontalVector(this.m_HeadLookTargetWorldDir), this.m_HeadLookCurrentFalloffs[i] / this.m_HeadLookFalloffs[0]);
			}
			this.m_HeadLookBones[i].transform.rotation = vp_3DUtility.GetBoneLookRotationInWorldSpace(this.m_HeadLookBones[i].transform.rotation, this.m_HeadLookBones[this.m_HeadLookBones.Count - 1].transform.parent.rotation, this.m_HeadLookCurrentWorldDir, this.m_HeadLookCurrentFalloffs[i], this.m_ReferenceLookDirs[i], this.m_ReferenceUpDirs[i], Quaternion.identity);
			if (!this.Player.IsFirstPerson.Get())
			{
				this.m_CurrentHeadLookPitch = Mathf.SmoothStep(this.m_CurrentHeadLookPitch, Mathf.Clamp(this.Player.Rotation.Get().x, -this.HeadPitchCap, this.HeadPitchCap), Time.deltaTime * this.HeadPitchSpeed);
				this.m_HeadLookBones[i].transform.Rotate(this.HeadPoint.transform.right, this.m_CurrentHeadLookPitch * Mathf.Lerp(this.m_HeadLookFalloffs[i], this.m_HeadLookCurrentFalloffs[i], this.LeaningFactor), Space.World);
			}
		}
		this.m_WasMoving = this.Animator.GetBool(this.IsMoving);
	}

	// Token: 0x0600B191 RID: 45457 RVA: 0x00425548 File Offset: 0x00423748
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool GetIsMoving()
	{
		return Vector3.Scale(this.Player.Velocity.Get(), Vector3.right + Vector3.forward).magnitude > 0.01f;
	}

	// Token: 0x0600B192 RID: 45458 RVA: 0x00425590 File Offset: 0x00423790
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual Vector3 GetLookPoint()
	{
		this.m_HeadLookBackup = this.HeadPoint.transform.eulerAngles;
		this.HeadPoint.transform.eulerAngles = this.Player.Rotation.Get();
		this.m_LookPoint = this.HeadPoint.transform.position + this.HeadPoint.transform.forward * 1000f;
		this.HeadPoint.transform.eulerAngles = this.m_HeadLookBackup;
		return this.m_LookPoint;
	}

	// Token: 0x0600B193 RID: 45459 RVA: 0x00425630 File Offset: 0x00423830
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual List<float> CalculateBoneFalloffs(List<GameObject> boneList)
	{
		List<float> list = new List<float>();
		float num = 0f;
		for (int i = boneList.Count - 1; i > -1; i--)
		{
			if (boneList[i] == null)
			{
				boneList.RemoveAt(i);
			}
			else
			{
				float num2 = Mathf.Lerp(0f, 1f, (float)(i + 1) / (float)boneList.Count);
				list.Add(num2 * num2 * num2);
				num += num2 * num2 * num2;
			}
		}
		if (boneList.Count == 0)
		{
			return list;
		}
		for (int j = 0; j < list.Count; j++)
		{
			List<float> list2 = list;
			int num3 = j;
			list2[num3] *= 1f / num;
		}
		return list;
	}

	// Token: 0x0600B194 RID: 45460 RVA: 0x004256E4 File Offset: 0x004238E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void StoreReferenceDirections()
	{
		for (int i = 0; i < this.m_HeadLookBones.Count; i++)
		{
			Quaternion quaternion = Quaternion.Inverse(this.m_HeadLookBones[this.m_HeadLookBones.Count - 1].transform.parent.rotation);
			this.m_ReferenceLookDirs.Add(quaternion * this.Transform.rotation * Vector3.forward);
			this.m_ReferenceUpDirs.Add(quaternion * this.Transform.rotation * Vector3.up);
		}
	}

	// Token: 0x0600B195 RID: 45461 RVA: 0x00425784 File Offset: 0x00423984
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateGrounding()
	{
		Physics.SphereCast(new Ray(this.Transform.position + Vector3.up * 0.5f, Vector3.down), 0.4f, out this.m_GroundHit, 1f, 1084850184);
		this.m_Grounded = this.m_GroundHit.collider != null;
	}

	// Token: 0x0600B196 RID: 45462 RVA: 0x004257EC File Offset: 0x004239EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshWeaponStates()
	{
		if (this.WeaponHandler == null)
		{
			return;
		}
		if (this.WeaponHandler.CurrentWeapon == null)
		{
			return;
		}
		this.WeaponHandler.CurrentWeapon.SetState("Attack", this.Player.Attack.Active, false, false);
		this.WeaponHandler.CurrentWeapon.SetState("Zoom", this.Player.Zoom.Active, false, false);
	}

	// Token: 0x0600B197 RID: 45463 RVA: 0x0042586C File Offset: 0x00423A6C
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitMaxSpeeds()
	{
		vp_FPController vp_FPController = global::UnityEngine.Object.FindAnyObjectByType<vp_FPController>();
		if (vp_FPController != null)
		{
			this.m_MaxWalkSpeed = vp_FPController.CalculateMaxSpeed("Default", 5f);
			this.m_MaxRunSpeed = vp_FPController.CalculateMaxSpeed("Run", 5f);
			this.m_MaxCrouchSpeed = vp_FPController.CalculateMaxSpeed("Crouch", 5f);
			return;
		}
		this.m_MaxWalkSpeed = 3.999999f;
		this.m_MaxRunSpeed = 10.08f;
		this.m_MaxCrouchSpeed = 1.44f;
	}

	// Token: 0x0600B198 RID: 45464 RVA: 0x004258EC File Offset: 0x00423AEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitHashIDs()
	{
		this.ForwardAmount = Animator.StringToHash("Forward");
		this.PitchAmount = Animator.StringToHash("Pitch");
		this.StrafeAmount = Animator.StringToHash("Strafe");
		this.TurnAmount = Animator.StringToHash("Turn");
		this.VerticalMoveAmount = Animator.StringToHash("VerticalMove");
		this.IsAttacking = Animator.StringToHash("IsAttacking");
		this.IsClimbing = Animator.StringToHash("IsClimbing");
		this.IsCrouching = Animator.StringToHash("IsCrouching");
		this.IsGrounded = Animator.StringToHash("IsGrounded");
		this.IsMoving = Animator.StringToHash("IsMoving");
		this.IsOutOfControl = Animator.StringToHash("IsOutOfControl");
		this.IsReloading = Animator.StringToHash("IsReloading");
		this.IsRunning = Animator.StringToHash("IsRunning");
		this.IsSettingWeapon = Animator.StringToHash("IsSettingWeapon");
		this.IsZooming = Animator.StringToHash("IsZooming");
		this.StartClimb = Animator.StringToHash("StartClimb");
		this.StartOutOfControl = Animator.StringToHash("StartOutOfControl");
		this.StartReload = Animator.StringToHash("StartReload");
		this.WeaponGripIndex = Animator.StringToHash("WeaponGrip");
		this.WeaponTypeIndex = Animator.StringToHash("WeaponType");
	}

	// Token: 0x0600B199 RID: 45465 RVA: 0x00425A3C File Offset: 0x00423C3C
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitHeadLook()
	{
		if (!this.m_IsValid)
		{
			return;
		}
		this.m_HeadLookBones.Clear();
		GameObject gameObject = this.HeadBone;
		while (gameObject != this.LowestSpineBone.transform.parent.gameObject)
		{
			this.m_HeadLookBones.Add(gameObject);
			gameObject = gameObject.transform.parent.gameObject;
		}
		this.m_ReferenceUpDirs = new List<Vector3>();
		this.m_ReferenceLookDirs = new List<Vector3>();
		this.m_HeadLookFalloffs = this.CalculateBoneFalloffs(this.m_HeadLookBones);
		this.m_HeadLookCurrentFalloffs = new List<float>(this.m_HeadLookFalloffs);
		this.m_HeadLookTargetFalloffs = new List<float>(this.m_HeadLookFalloffs);
		this.StoreReferenceDirections();
	}

	// Token: 0x0600B19A RID: 45466 RVA: 0x00425AF0 File Offset: 0x00423CF0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsValidSetup()
	{
		if (this.HeadBone == null)
		{
			Debug.LogError("Error (" + ((this != null) ? this.ToString() : null) + ") No gameobject has been assigned for 'HeadBone'.");
		}
		else if (this.LowestSpineBone == null)
		{
			Debug.LogError("Error (" + ((this != null) ? this.ToString() : null) + ") No gameobject has been assigned for 'LowestSpineBone'.");
		}
		else if (!vp_Utility.IsDescendant(this.HeadBone.transform, base.transform.root))
		{
			this.NotInSameHierarchyError(this.HeadBone);
		}
		else if (!vp_Utility.IsDescendant(this.LowestSpineBone.transform, base.transform.root))
		{
			this.NotInSameHierarchyError(this.LowestSpineBone);
		}
		else
		{
			if (vp_Utility.IsDescendant(this.HeadBone.transform, this.LowestSpineBone.transform))
			{
				return true;
			}
			Debug.LogError("Error (" + ((this != null) ? this.ToString() : null) + ") 'HeadBone' must be a child or descendant of 'LowestSpineBone'.");
		}
		this.m_IsValid = false;
		base.enabled = false;
		return false;
	}

	// Token: 0x0600B19B RID: 45467 RVA: 0x00425C0C File Offset: 0x00423E0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void NotInSameHierarchyError(GameObject o)
	{
		Debug.LogError(string.Concat(new string[]
		{
			"Error '",
			(o != null) ? o.ToString() : null,
			"' can not be used as a bone for  ",
			(this != null) ? this.ToString() : null,
			" because it is not part of the same hierarchy."
		}));
	}

	// Token: 0x170014E8 RID: 5352
	// (get) Token: 0x0600B19C RID: 45468 RVA: 0x00425C61 File Offset: 0x00423E61
	public virtual float OnValue_BodyYaw
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.Transform.eulerAngles.y;
		}
	}

	// Token: 0x0600B19D RID: 45469 RVA: 0x00425C73 File Offset: 0x00423E73
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStart_Attack()
	{
		this.m_AttackDoneTimer.Cancel();
		this.Animator.SetBool(this.IsAttacking, true);
	}

	// Token: 0x0600B19E RID: 45470 RVA: 0x00425C92 File Offset: 0x00423E92
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Attack()
	{
		vp_Timer.In(0.5f, delegate
		{
			this.Animator.SetBool(this.IsAttacking, false);
			this.RefreshWeaponStates();
		}, this.m_AttackDoneTimer);
	}

	// Token: 0x0600B19F RID: 45471 RVA: 0x00425CB0 File Offset: 0x00423EB0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Zoom()
	{
		vp_Timer.In(0.5f, delegate
		{
			if (!this.Player.Attack.Active)
			{
				this.Animator.SetBool(this.IsAttacking, false);
			}
			this.RefreshWeaponStates();
		}, this.m_AttackDoneTimer);
	}

	// Token: 0x0600B1A0 RID: 45472 RVA: 0x00425CCE File Offset: 0x00423ECE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Reload()
	{
		this.Animator.SetTrigger(this.StartReload);
	}

	// Token: 0x0600B1A1 RID: 45473 RVA: 0x00425CE1 File Offset: 0x00423EE1
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_OutOfControl()
	{
		this.Animator.SetTrigger(this.StartOutOfControl);
	}

	// Token: 0x0600B1A2 RID: 45474 RVA: 0x00425CF4 File Offset: 0x00423EF4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Climb()
	{
		this.Animator.SetTrigger(this.StartClimb);
	}

	// Token: 0x0600B1A3 RID: 45475 RVA: 0x00425D07 File Offset: 0x00423F07
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Dead()
	{
		if (this.m_AttackDoneTimer.Active)
		{
			this.m_AttackDoneTimer.Execute();
		}
	}

	// Token: 0x0600B1A4 RID: 45476 RVA: 0x00425D21 File Offset: 0x00423F21
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		this.HeadPointDirty = true;
	}

	// Token: 0x0600B1A5 RID: 45477 RVA: 0x00425D2A File Offset: 0x00423F2A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraToggle3rdPerson()
	{
		this.m_WasMoving = !this.m_WasMoving;
		this.HeadPointDirty = true;
	}

	// Token: 0x170014E9 RID: 5353
	// (get) Token: 0x0600B1A6 RID: 45478 RVA: 0x00425D44 File Offset: 0x00423F44
	public virtual Vector3 OnValue_HeadLookDirection
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return (this.Player.LookPoint.Get() - this.HeadPoint.transform.position).normalized;
		}
	}

	// Token: 0x170014EA RID: 5354
	// (get) Token: 0x0600B1A7 RID: 45479 RVA: 0x0041941E File Offset: 0x0041761E
	public virtual Vector3 OnValue_LookPoint
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.GetLookPoint();
		}
	}

	// Token: 0x040084F3 RID: 34035
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsValid = true;

	// Token: 0x040084F4 RID: 34036
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_ValidLookPoint = Vector3.zero;

	// Token: 0x040084F5 RID: 34037
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_ValidLookPointForward;

	// Token: 0x040084F6 RID: 34038
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool HeadPointDirty = true;

	// Token: 0x040084F7 RID: 34039
	public GameObject HeadBone;

	// Token: 0x040084F8 RID: 34040
	public GameObject LowestSpineBone;

	// Token: 0x040084F9 RID: 34041
	[Range(0f, 90f)]
	public float HeadPitchCap = 45f;

	// Token: 0x040084FA RID: 34042
	[Range(2f, 20f)]
	public float HeadPitchSpeed = 7f;

	// Token: 0x040084FB RID: 34043
	[Range(0.2f, 20f)]
	public float HeadYawSpeed = 2f;

	// Token: 0x040084FC RID: 34044
	[Range(0f, 1f)]
	public float LeaningFactor = 0.25f;

	// Token: 0x040084FD RID: 34045
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<GameObject> m_HeadLookBones = new List<GameObject>();

	// Token: 0x040084FE RID: 34046
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Vector3> m_ReferenceUpDirs;

	// Token: 0x040084FF RID: 34047
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Vector3> m_ReferenceLookDirs;

	// Token: 0x04008500 RID: 34048
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentHeadLookYaw;

	// Token: 0x04008501 RID: 34049
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentHeadLookPitch;

	// Token: 0x04008502 RID: 34050
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<float> m_HeadLookFalloffs = new List<float>();

	// Token: 0x04008503 RID: 34051
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<float> m_HeadLookCurrentFalloffs;

	// Token: 0x04008504 RID: 34052
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<float> m_HeadLookTargetFalloffs;

	// Token: 0x04008505 RID: 34053
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_HeadLookTargetWorldDir;

	// Token: 0x04008506 RID: 34054
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_HeadLookCurrentWorldDir;

	// Token: 0x04008507 RID: 34055
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_HeadLookBackup = Vector3.zero;

	// Token: 0x04008508 RID: 34056
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_LookPoint = Vector3.zero;

	// Token: 0x04008509 RID: 34057
	public float FeetAdjustAngle = 80f;

	// Token: 0x0400850A RID: 34058
	public float FeetAdjustSpeedStanding = 10f;

	// Token: 0x0400850B RID: 34059
	public float FeetAdjustSpeedMoving = 12f;

	// Token: 0x0400850C RID: 34060
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_PrevBodyYaw;

	// Token: 0x0400850D RID: 34061
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_BodyYaw;

	// Token: 0x0400850E RID: 34062
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentBodyYawTarget;

	// Token: 0x0400850F RID: 34063
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastYaw;

	// Token: 0x04008510 RID: 34064
	public Vector3 ClimbOffset = Vector3.forward * 0.6f;

	// Token: 0x04008511 RID: 34065
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentForward;

	// Token: 0x04008512 RID: 34066
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentStrafe;

	// Token: 0x04008513 RID: 34067
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentTurn;

	// Token: 0x04008514 RID: 34068
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentTurnTarget;

	// Token: 0x04008515 RID: 34069
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MaxWalkSpeed = 1f;

	// Token: 0x04008516 RID: 34070
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MaxRunSpeed = 1f;

	// Token: 0x04008517 RID: 34071
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MaxCrouchSpeed = 1f;

	// Token: 0x04008518 RID: 34072
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_WasMoving;

	// Token: 0x04008519 RID: 34073
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_GroundHit;

	// Token: 0x0400851A RID: 34074
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Grounded = true;

	// Token: 0x0400851B RID: 34075
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_AttackDoneTimer = new vp_Timer.Handle();

	// Token: 0x0400851C RID: 34076
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedUpdateTurnTargetTime;

	// Token: 0x0400851D RID: 34077
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float TURNMODIFIER = 0.2f;

	// Token: 0x0400851E RID: 34078
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float CROUCHTURNMODIFIER = 100f;

	// Token: 0x0400851F RID: 34079
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float MOVEMODIFIER = 100f;

	// Token: 0x04008520 RID: 34080
	public bool ShowDebugObjects;

	// Token: 0x04008521 RID: 34081
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int ForwardAmount;

	// Token: 0x04008522 RID: 34082
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int PitchAmount;

	// Token: 0x04008523 RID: 34083
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int StrafeAmount;

	// Token: 0x04008524 RID: 34084
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int TurnAmount;

	// Token: 0x04008525 RID: 34085
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int VerticalMoveAmount;

	// Token: 0x04008526 RID: 34086
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsAttacking;

	// Token: 0x04008527 RID: 34087
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsClimbing;

	// Token: 0x04008528 RID: 34088
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsCrouching;

	// Token: 0x04008529 RID: 34089
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsGrounded;

	// Token: 0x0400852A RID: 34090
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsMoving;

	// Token: 0x0400852B RID: 34091
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsOutOfControl;

	// Token: 0x0400852C RID: 34092
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsReloading;

	// Token: 0x0400852D RID: 34093
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsRunning;

	// Token: 0x0400852E RID: 34094
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsSettingWeapon;

	// Token: 0x0400852F RID: 34095
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsZooming;

	// Token: 0x04008530 RID: 34096
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int StartClimb;

	// Token: 0x04008531 RID: 34097
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int StartOutOfControl;

	// Token: 0x04008532 RID: 34098
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int StartReload;

	// Token: 0x04008533 RID: 34099
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int WeaponGripIndex;

	// Token: 0x04008534 RID: 34100
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int WeaponTypeIndex;

	// Token: 0x04008535 RID: 34101
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_WeaponHandler m_WeaponHandler;

	// Token: 0x04008536 RID: 34102
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008537 RID: 34103
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x04008538 RID: 34104
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public SkinnedMeshRenderer m_Renderer;

	// Token: 0x04008539 RID: 34105
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Animator m_Animator;

	// Token: 0x0400853A RID: 34106
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_HeadPoint;

	// Token: 0x0400853B RID: 34107
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_DebugLookTarget;

	// Token: 0x0400853C RID: 34108
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_DebugLookArrow;
}
