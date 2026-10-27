using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001638 RID: 5688
[RequireComponent(typeof(Animator))]
public class vp_BodyAnimator : MonoBehaviour
{
	// Token: 0x170014EA RID: 5354
	// (get) Token: 0x0600B1D3 RID: 45523 RVA: 0x004268E4 File Offset: 0x00424AE4
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

	// Token: 0x170014EB RID: 5355
	// (get) Token: 0x0600B1D4 RID: 45524 RVA: 0x0042691F File Offset: 0x00424B1F
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

	// Token: 0x170014EC RID: 5356
	// (get) Token: 0x0600B1D5 RID: 45525 RVA: 0x00426941 File Offset: 0x00424B41
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

	// Token: 0x170014ED RID: 5357
	// (get) Token: 0x0600B1D6 RID: 45526 RVA: 0x0042697C File Offset: 0x00424B7C
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

	// Token: 0x170014EE RID: 5358
	// (get) Token: 0x0600B1D7 RID: 45527 RVA: 0x004269A8 File Offset: 0x00424BA8
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

	// Token: 0x170014EF RID: 5359
	// (get) Token: 0x0600B1D8 RID: 45528 RVA: 0x004269CA File Offset: 0x00424BCA
	public Vector3 m_LocalVelocity
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return vp_MathUtility.SnapToZero(this.Transform.root.InverseTransformDirection(this.Player.Velocity.Get()) / this.m_MaxSpeed, 0.0001f);
		}
	}

	// Token: 0x170014F0 RID: 5360
	// (get) Token: 0x0600B1D9 RID: 45529 RVA: 0x00426A06 File Offset: 0x00424C06
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

	// Token: 0x170014F1 RID: 5361
	// (get) Token: 0x0600B1DA RID: 45530 RVA: 0x00426A40 File Offset: 0x00424C40
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

	// Token: 0x170014F2 RID: 5362
	// (get) Token: 0x0600B1DB RID: 45531 RVA: 0x00426AD1 File Offset: 0x00424CD1
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

	// Token: 0x170014F3 RID: 5363
	// (get) Token: 0x0600B1DC RID: 45532 RVA: 0x00426AF4 File Offset: 0x00424CF4
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

	// Token: 0x0600B1DD RID: 45533 RVA: 0x00426B6D File Offset: 0x00424D6D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
	}

	// Token: 0x0600B1DE RID: 45534 RVA: 0x00426B89 File Offset: 0x00424D89
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B1DF RID: 45535 RVA: 0x00426BA5 File Offset: 0x00424DA5
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

	// Token: 0x0600B1E0 RID: 45536 RVA: 0x00426BC4 File Offset: 0x00424DC4
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

	// Token: 0x0600B1E1 RID: 45537 RVA: 0x00426C20 File Offset: 0x00424E20
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

	// Token: 0x0600B1E2 RID: 45538 RVA: 0x00426E0C File Offset: 0x0042500C
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

	// Token: 0x0600B1E3 RID: 45539 RVA: 0x00426E6C File Offset: 0x0042506C
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

	// Token: 0x0600B1E4 RID: 45540 RVA: 0x00427108 File Offset: 0x00425308
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

	// Token: 0x0600B1E5 RID: 45541 RVA: 0x004271EC File Offset: 0x004253EC
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

	// Token: 0x0600B1E6 RID: 45542 RVA: 0x00427228 File Offset: 0x00425428
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

	// Token: 0x0600B1E7 RID: 45543 RVA: 0x0042727C File Offset: 0x0042547C
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

	// Token: 0x0600B1E8 RID: 45544 RVA: 0x0042756C File Offset: 0x0042576C
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

	// Token: 0x0600B1E9 RID: 45545 RVA: 0x0042794C File Offset: 0x00425B4C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool GetIsMoving()
	{
		return Vector3.Scale(this.Player.Velocity.Get(), Vector3.right + Vector3.forward).magnitude > 0.01f;
	}

	// Token: 0x0600B1EA RID: 45546 RVA: 0x00427994 File Offset: 0x00425B94
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual Vector3 GetLookPoint()
	{
		this.m_HeadLookBackup = this.HeadPoint.transform.eulerAngles;
		this.HeadPoint.transform.eulerAngles = this.Player.Rotation.Get();
		this.m_LookPoint = this.HeadPoint.transform.position + this.HeadPoint.transform.forward * 1000f;
		this.HeadPoint.transform.eulerAngles = this.m_HeadLookBackup;
		return this.m_LookPoint;
	}

	// Token: 0x0600B1EB RID: 45547 RVA: 0x00427A34 File Offset: 0x00425C34
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

	// Token: 0x0600B1EC RID: 45548 RVA: 0x00427AE8 File Offset: 0x00425CE8
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

	// Token: 0x0600B1ED RID: 45549 RVA: 0x00427B88 File Offset: 0x00425D88
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateGrounding()
	{
		Physics.SphereCast(new Ray(this.Transform.position + Vector3.up * 0.5f, Vector3.down), 0.4f, out this.m_GroundHit, 1f, 1084850184);
		this.m_Grounded = this.m_GroundHit.collider != null;
	}

	// Token: 0x0600B1EE RID: 45550 RVA: 0x00427BF0 File Offset: 0x00425DF0
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

	// Token: 0x0600B1EF RID: 45551 RVA: 0x00427C70 File Offset: 0x00425E70
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

	// Token: 0x0600B1F0 RID: 45552 RVA: 0x00427CF0 File Offset: 0x00425EF0
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

	// Token: 0x0600B1F1 RID: 45553 RVA: 0x00427E40 File Offset: 0x00426040
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

	// Token: 0x0600B1F2 RID: 45554 RVA: 0x00427EF4 File Offset: 0x004260F4
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

	// Token: 0x0600B1F3 RID: 45555 RVA: 0x00428010 File Offset: 0x00426210
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

	// Token: 0x170014F4 RID: 5364
	// (get) Token: 0x0600B1F4 RID: 45556 RVA: 0x00428065 File Offset: 0x00426265
	public virtual float OnValue_BodyYaw
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.Transform.eulerAngles.y;
		}
	}

	// Token: 0x0600B1F5 RID: 45557 RVA: 0x00428077 File Offset: 0x00426277
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStart_Attack()
	{
		this.m_AttackDoneTimer.Cancel();
		this.Animator.SetBool(this.IsAttacking, true);
	}

	// Token: 0x0600B1F6 RID: 45558 RVA: 0x00428096 File Offset: 0x00426296
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Attack()
	{
		vp_Timer.In(0.5f, delegate
		{
			this.Animator.SetBool(this.IsAttacking, false);
			this.RefreshWeaponStates();
		}, this.m_AttackDoneTimer);
	}

	// Token: 0x0600B1F7 RID: 45559 RVA: 0x004280B4 File Offset: 0x004262B4
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

	// Token: 0x0600B1F8 RID: 45560 RVA: 0x004280D2 File Offset: 0x004262D2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Reload()
	{
		this.Animator.SetTrigger(this.StartReload);
	}

	// Token: 0x0600B1F9 RID: 45561 RVA: 0x004280E5 File Offset: 0x004262E5
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_OutOfControl()
	{
		this.Animator.SetTrigger(this.StartOutOfControl);
	}

	// Token: 0x0600B1FA RID: 45562 RVA: 0x004280F8 File Offset: 0x004262F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Climb()
	{
		this.Animator.SetTrigger(this.StartClimb);
	}

	// Token: 0x0600B1FB RID: 45563 RVA: 0x0042810B File Offset: 0x0042630B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Dead()
	{
		if (this.m_AttackDoneTimer.Active)
		{
			this.m_AttackDoneTimer.Execute();
		}
	}

	// Token: 0x0600B1FC RID: 45564 RVA: 0x00428125 File Offset: 0x00426325
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		this.HeadPointDirty = true;
	}

	// Token: 0x0600B1FD RID: 45565 RVA: 0x0042812E File Offset: 0x0042632E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraToggle3rdPerson()
	{
		this.m_WasMoving = !this.m_WasMoving;
		this.HeadPointDirty = true;
	}

	// Token: 0x170014F5 RID: 5365
	// (get) Token: 0x0600B1FE RID: 45566 RVA: 0x00428148 File Offset: 0x00426348
	public virtual Vector3 OnValue_HeadLookDirection
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return (this.Player.LookPoint.Get() - this.HeadPoint.transform.position).normalized;
		}
	}

	// Token: 0x170014F6 RID: 5366
	// (get) Token: 0x0600B1FF RID: 45567 RVA: 0x0041B822 File Offset: 0x00419A22
	public virtual Vector3 OnValue_LookPoint
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.GetLookPoint();
		}
	}

	// Token: 0x04008534 RID: 34100
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsValid = true;

	// Token: 0x04008535 RID: 34101
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_ValidLookPoint = Vector3.zero;

	// Token: 0x04008536 RID: 34102
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_ValidLookPointForward;

	// Token: 0x04008537 RID: 34103
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool HeadPointDirty = true;

	// Token: 0x04008538 RID: 34104
	public GameObject HeadBone;

	// Token: 0x04008539 RID: 34105
	public GameObject LowestSpineBone;

	// Token: 0x0400853A RID: 34106
	[Range(0f, 90f)]
	public float HeadPitchCap = 45f;

	// Token: 0x0400853B RID: 34107
	[Range(2f, 20f)]
	public float HeadPitchSpeed = 7f;

	// Token: 0x0400853C RID: 34108
	[Range(0.2f, 20f)]
	public float HeadYawSpeed = 2f;

	// Token: 0x0400853D RID: 34109
	[Range(0f, 1f)]
	public float LeaningFactor = 0.25f;

	// Token: 0x0400853E RID: 34110
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<GameObject> m_HeadLookBones = new List<GameObject>();

	// Token: 0x0400853F RID: 34111
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Vector3> m_ReferenceUpDirs;

	// Token: 0x04008540 RID: 34112
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Vector3> m_ReferenceLookDirs;

	// Token: 0x04008541 RID: 34113
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentHeadLookYaw;

	// Token: 0x04008542 RID: 34114
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentHeadLookPitch;

	// Token: 0x04008543 RID: 34115
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<float> m_HeadLookFalloffs = new List<float>();

	// Token: 0x04008544 RID: 34116
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<float> m_HeadLookCurrentFalloffs;

	// Token: 0x04008545 RID: 34117
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<float> m_HeadLookTargetFalloffs;

	// Token: 0x04008546 RID: 34118
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_HeadLookTargetWorldDir;

	// Token: 0x04008547 RID: 34119
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_HeadLookCurrentWorldDir;

	// Token: 0x04008548 RID: 34120
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_HeadLookBackup = Vector3.zero;

	// Token: 0x04008549 RID: 34121
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_LookPoint = Vector3.zero;

	// Token: 0x0400854A RID: 34122
	public float FeetAdjustAngle = 80f;

	// Token: 0x0400854B RID: 34123
	public float FeetAdjustSpeedStanding = 10f;

	// Token: 0x0400854C RID: 34124
	public float FeetAdjustSpeedMoving = 12f;

	// Token: 0x0400854D RID: 34125
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_PrevBodyYaw;

	// Token: 0x0400854E RID: 34126
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_BodyYaw;

	// Token: 0x0400854F RID: 34127
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentBodyYawTarget;

	// Token: 0x04008550 RID: 34128
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastYaw;

	// Token: 0x04008551 RID: 34129
	public Vector3 ClimbOffset = Vector3.forward * 0.6f;

	// Token: 0x04008552 RID: 34130
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentForward;

	// Token: 0x04008553 RID: 34131
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentStrafe;

	// Token: 0x04008554 RID: 34132
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentTurn;

	// Token: 0x04008555 RID: 34133
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentTurnTarget;

	// Token: 0x04008556 RID: 34134
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MaxWalkSpeed = 1f;

	// Token: 0x04008557 RID: 34135
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MaxRunSpeed = 1f;

	// Token: 0x04008558 RID: 34136
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MaxCrouchSpeed = 1f;

	// Token: 0x04008559 RID: 34137
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_WasMoving;

	// Token: 0x0400855A RID: 34138
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_GroundHit;

	// Token: 0x0400855B RID: 34139
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Grounded = true;

	// Token: 0x0400855C RID: 34140
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_AttackDoneTimer = new vp_Timer.Handle();

	// Token: 0x0400855D RID: 34141
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedUpdateTurnTargetTime;

	// Token: 0x0400855E RID: 34142
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float TURNMODIFIER = 0.2f;

	// Token: 0x0400855F RID: 34143
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float CROUCHTURNMODIFIER = 100f;

	// Token: 0x04008560 RID: 34144
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float MOVEMODIFIER = 100f;

	// Token: 0x04008561 RID: 34145
	public bool ShowDebugObjects;

	// Token: 0x04008562 RID: 34146
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int ForwardAmount;

	// Token: 0x04008563 RID: 34147
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int PitchAmount;

	// Token: 0x04008564 RID: 34148
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int StrafeAmount;

	// Token: 0x04008565 RID: 34149
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int TurnAmount;

	// Token: 0x04008566 RID: 34150
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int VerticalMoveAmount;

	// Token: 0x04008567 RID: 34151
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsAttacking;

	// Token: 0x04008568 RID: 34152
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsClimbing;

	// Token: 0x04008569 RID: 34153
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsCrouching;

	// Token: 0x0400856A RID: 34154
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsGrounded;

	// Token: 0x0400856B RID: 34155
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsMoving;

	// Token: 0x0400856C RID: 34156
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsOutOfControl;

	// Token: 0x0400856D RID: 34157
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsReloading;

	// Token: 0x0400856E RID: 34158
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsRunning;

	// Token: 0x0400856F RID: 34159
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsSettingWeapon;

	// Token: 0x04008570 RID: 34160
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsZooming;

	// Token: 0x04008571 RID: 34161
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int StartClimb;

	// Token: 0x04008572 RID: 34162
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int StartOutOfControl;

	// Token: 0x04008573 RID: 34163
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int StartReload;

	// Token: 0x04008574 RID: 34164
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int WeaponGripIndex;

	// Token: 0x04008575 RID: 34165
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int WeaponTypeIndex;

	// Token: 0x04008576 RID: 34166
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_WeaponHandler m_WeaponHandler;

	// Token: 0x04008577 RID: 34167
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008578 RID: 34168
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x04008579 RID: 34169
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public SkinnedMeshRenderer m_Renderer;

	// Token: 0x0400857A RID: 34170
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Animator m_Animator;

	// Token: 0x0400857B RID: 34171
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_HeadPoint;

	// Token: 0x0400857C RID: 34172
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_DebugLookTarget;

	// Token: 0x0400857D RID: 34173
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_DebugLookArrow;
}
