using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015EC RID: 5612
[RequireComponent(typeof(Animator))]
public class vp_BodyAnimator : MonoBehaviour
{
	// Token: 0x17001462 RID: 5218
	// (get) Token: 0x0600AFEC RID: 45036 RVA: 0x0041B4C4 File Offset: 0x004196C4
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

	// Token: 0x17001463 RID: 5219
	// (get) Token: 0x0600AFED RID: 45037 RVA: 0x0041B4FF File Offset: 0x004196FF
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

	// Token: 0x17001464 RID: 5220
	// (get) Token: 0x0600AFEE RID: 45038 RVA: 0x0041B521 File Offset: 0x00419721
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

	// Token: 0x17001465 RID: 5221
	// (get) Token: 0x0600AFEF RID: 45039 RVA: 0x0041B55C File Offset: 0x0041975C
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

	// Token: 0x17001466 RID: 5222
	// (get) Token: 0x0600AFF0 RID: 45040 RVA: 0x0041B588 File Offset: 0x00419788
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

	// Token: 0x17001467 RID: 5223
	// (get) Token: 0x0600AFF1 RID: 45041 RVA: 0x0041B5AA File Offset: 0x004197AA
	public Vector3 m_LocalVelocity
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return vp_MathUtility.SnapToZero(this.Transform.root.InverseTransformDirection(this.Player.Velocity.Get()) / this.m_MaxSpeed, 0.0001f);
		}
	}

	// Token: 0x17001468 RID: 5224
	// (get) Token: 0x0600AFF2 RID: 45042 RVA: 0x0041B5E6 File Offset: 0x004197E6
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

	// Token: 0x17001469 RID: 5225
	// (get) Token: 0x0600AFF3 RID: 45043 RVA: 0x0041B620 File Offset: 0x00419820
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

	// Token: 0x1700146A RID: 5226
	// (get) Token: 0x0600AFF4 RID: 45044 RVA: 0x0041B6B1 File Offset: 0x004198B1
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

	// Token: 0x1700146B RID: 5227
	// (get) Token: 0x0600AFF5 RID: 45045 RVA: 0x0041B6D4 File Offset: 0x004198D4
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

	// Token: 0x0600AFF6 RID: 45046 RVA: 0x0041B74D File Offset: 0x0041994D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
	}

	// Token: 0x0600AFF7 RID: 45047 RVA: 0x0041B769 File Offset: 0x00419969
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600AFF8 RID: 45048 RVA: 0x0041B785 File Offset: 0x00419985
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

	// Token: 0x0600AFF9 RID: 45049 RVA: 0x0041B7A4 File Offset: 0x004199A4
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

	// Token: 0x0600AFFA RID: 45050 RVA: 0x0041B800 File Offset: 0x00419A00
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

	// Token: 0x0600AFFB RID: 45051 RVA: 0x0041B9EC File Offset: 0x00419BEC
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

	// Token: 0x0600AFFC RID: 45052 RVA: 0x0041BA4C File Offset: 0x00419C4C
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

	// Token: 0x0600AFFD RID: 45053 RVA: 0x0041BCE8 File Offset: 0x00419EE8
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

	// Token: 0x0600AFFE RID: 45054 RVA: 0x0041BDCC File Offset: 0x00419FCC
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

	// Token: 0x0600AFFF RID: 45055 RVA: 0x0041BE08 File Offset: 0x0041A008
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

	// Token: 0x0600B000 RID: 45056 RVA: 0x0041BE5C File Offset: 0x0041A05C
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

	// Token: 0x0600B001 RID: 45057 RVA: 0x0041C14C File Offset: 0x0041A34C
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

	// Token: 0x0600B002 RID: 45058 RVA: 0x0041C52C File Offset: 0x0041A72C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool GetIsMoving()
	{
		return Vector3.Scale(this.Player.Velocity.Get(), Vector3.right + Vector3.forward).magnitude > 0.01f;
	}

	// Token: 0x0600B003 RID: 45059 RVA: 0x0041C574 File Offset: 0x0041A774
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual Vector3 GetLookPoint()
	{
		this.m_HeadLookBackup = this.HeadPoint.transform.eulerAngles;
		this.HeadPoint.transform.eulerAngles = this.Player.Rotation.Get();
		this.m_LookPoint = this.HeadPoint.transform.position + this.HeadPoint.transform.forward * 1000f;
		this.HeadPoint.transform.eulerAngles = this.m_HeadLookBackup;
		return this.m_LookPoint;
	}

	// Token: 0x0600B004 RID: 45060 RVA: 0x0041C614 File Offset: 0x0041A814
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

	// Token: 0x0600B005 RID: 45061 RVA: 0x0041C6C8 File Offset: 0x0041A8C8
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

	// Token: 0x0600B006 RID: 45062 RVA: 0x0041C768 File Offset: 0x0041A968
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateGrounding()
	{
		Physics.SphereCast(new Ray(this.Transform.position + Vector3.up * 0.5f, Vector3.down), 0.4f, out this.m_GroundHit, 1f, 1084850184);
		this.m_Grounded = this.m_GroundHit.collider != null;
	}

	// Token: 0x0600B007 RID: 45063 RVA: 0x0041C7D0 File Offset: 0x0041A9D0
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

	// Token: 0x0600B008 RID: 45064 RVA: 0x0041C850 File Offset: 0x0041AA50
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

	// Token: 0x0600B009 RID: 45065 RVA: 0x0041C8D0 File Offset: 0x0041AAD0
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

	// Token: 0x0600B00A RID: 45066 RVA: 0x0041CA20 File Offset: 0x0041AC20
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

	// Token: 0x0600B00B RID: 45067 RVA: 0x0041CAD4 File Offset: 0x0041ACD4
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

	// Token: 0x0600B00C RID: 45068 RVA: 0x0041CBF0 File Offset: 0x0041ADF0
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

	// Token: 0x1700146C RID: 5228
	// (get) Token: 0x0600B00D RID: 45069 RVA: 0x0041CC45 File Offset: 0x0041AE45
	public virtual float OnValue_BodyYaw
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.Transform.eulerAngles.y;
		}
	}

	// Token: 0x0600B00E RID: 45070 RVA: 0x0041CC57 File Offset: 0x0041AE57
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStart_Attack()
	{
		this.m_AttackDoneTimer.Cancel();
		this.Animator.SetBool(this.IsAttacking, true);
	}

	// Token: 0x0600B00F RID: 45071 RVA: 0x0041CC76 File Offset: 0x0041AE76
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Attack()
	{
		vp_Timer.In(0.5f, delegate
		{
			this.Animator.SetBool(this.IsAttacking, false);
			this.RefreshWeaponStates();
		}, this.m_AttackDoneTimer);
	}

	// Token: 0x0600B010 RID: 45072 RVA: 0x0041CC94 File Offset: 0x0041AE94
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

	// Token: 0x0600B011 RID: 45073 RVA: 0x0041CCB2 File Offset: 0x0041AEB2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Reload()
	{
		this.Animator.SetTrigger(this.StartReload);
	}

	// Token: 0x0600B012 RID: 45074 RVA: 0x0041CCC5 File Offset: 0x0041AEC5
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_OutOfControl()
	{
		this.Animator.SetTrigger(this.StartOutOfControl);
	}

	// Token: 0x0600B013 RID: 45075 RVA: 0x0041CCD8 File Offset: 0x0041AED8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Climb()
	{
		this.Animator.SetTrigger(this.StartClimb);
	}

	// Token: 0x0600B014 RID: 45076 RVA: 0x0041CCEB File Offset: 0x0041AEEB
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Dead()
	{
		if (this.m_AttackDoneTimer.Active)
		{
			this.m_AttackDoneTimer.Execute();
		}
	}

	// Token: 0x0600B015 RID: 45077 RVA: 0x0041CD05 File Offset: 0x0041AF05
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		this.HeadPointDirty = true;
	}

	// Token: 0x0600B016 RID: 45078 RVA: 0x0041CD0E File Offset: 0x0041AF0E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraToggle3rdPerson()
	{
		this.m_WasMoving = !this.m_WasMoving;
		this.HeadPointDirty = true;
	}

	// Token: 0x1700146D RID: 5229
	// (get) Token: 0x0600B017 RID: 45079 RVA: 0x0041CD28 File Offset: 0x0041AF28
	public virtual Vector3 OnValue_HeadLookDirection
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return (this.Player.LookPoint.Get() - this.HeadPoint.transform.position).normalized;
		}
	}

	// Token: 0x1700146E RID: 5230
	// (get) Token: 0x0600B018 RID: 45080 RVA: 0x00410402 File Offset: 0x0040E602
	public virtual Vector3 OnValue_LookPoint
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.GetLookPoint();
		}
	}

	// Token: 0x040083C4 RID: 33732
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsValid = true;

	// Token: 0x040083C5 RID: 33733
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_ValidLookPoint = Vector3.zero;

	// Token: 0x040083C6 RID: 33734
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_ValidLookPointForward;

	// Token: 0x040083C7 RID: 33735
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool HeadPointDirty = true;

	// Token: 0x040083C8 RID: 33736
	public GameObject HeadBone;

	// Token: 0x040083C9 RID: 33737
	public GameObject LowestSpineBone;

	// Token: 0x040083CA RID: 33738
	[Range(0f, 90f)]
	public float HeadPitchCap = 45f;

	// Token: 0x040083CB RID: 33739
	[Range(2f, 20f)]
	public float HeadPitchSpeed = 7f;

	// Token: 0x040083CC RID: 33740
	[Range(0.2f, 20f)]
	public float HeadYawSpeed = 2f;

	// Token: 0x040083CD RID: 33741
	[Range(0f, 1f)]
	public float LeaningFactor = 0.25f;

	// Token: 0x040083CE RID: 33742
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<GameObject> m_HeadLookBones = new List<GameObject>();

	// Token: 0x040083CF RID: 33743
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Vector3> m_ReferenceUpDirs;

	// Token: 0x040083D0 RID: 33744
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Vector3> m_ReferenceLookDirs;

	// Token: 0x040083D1 RID: 33745
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentHeadLookYaw;

	// Token: 0x040083D2 RID: 33746
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentHeadLookPitch;

	// Token: 0x040083D3 RID: 33747
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<float> m_HeadLookFalloffs = new List<float>();

	// Token: 0x040083D4 RID: 33748
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<float> m_HeadLookCurrentFalloffs;

	// Token: 0x040083D5 RID: 33749
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<float> m_HeadLookTargetFalloffs;

	// Token: 0x040083D6 RID: 33750
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_HeadLookTargetWorldDir;

	// Token: 0x040083D7 RID: 33751
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_HeadLookCurrentWorldDir;

	// Token: 0x040083D8 RID: 33752
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_HeadLookBackup = Vector3.zero;

	// Token: 0x040083D9 RID: 33753
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_LookPoint = Vector3.zero;

	// Token: 0x040083DA RID: 33754
	public float FeetAdjustAngle = 80f;

	// Token: 0x040083DB RID: 33755
	public float FeetAdjustSpeedStanding = 10f;

	// Token: 0x040083DC RID: 33756
	public float FeetAdjustSpeedMoving = 12f;

	// Token: 0x040083DD RID: 33757
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_PrevBodyYaw;

	// Token: 0x040083DE RID: 33758
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_BodyYaw;

	// Token: 0x040083DF RID: 33759
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentBodyYawTarget;

	// Token: 0x040083E0 RID: 33760
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastYaw;

	// Token: 0x040083E1 RID: 33761
	public Vector3 ClimbOffset = Vector3.forward * 0.6f;

	// Token: 0x040083E2 RID: 33762
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentForward;

	// Token: 0x040083E3 RID: 33763
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentStrafe;

	// Token: 0x040083E4 RID: 33764
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentTurn;

	// Token: 0x040083E5 RID: 33765
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentTurnTarget;

	// Token: 0x040083E6 RID: 33766
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MaxWalkSpeed = 1f;

	// Token: 0x040083E7 RID: 33767
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MaxRunSpeed = 1f;

	// Token: 0x040083E8 RID: 33768
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MaxCrouchSpeed = 1f;

	// Token: 0x040083E9 RID: 33769
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_WasMoving;

	// Token: 0x040083EA RID: 33770
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_GroundHit;

	// Token: 0x040083EB RID: 33771
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Grounded = true;

	// Token: 0x040083EC RID: 33772
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_AttackDoneTimer = new vp_Timer.Handle();

	// Token: 0x040083ED RID: 33773
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedUpdateTurnTargetTime;

	// Token: 0x040083EE RID: 33774
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float TURNMODIFIER = 0.2f;

	// Token: 0x040083EF RID: 33775
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float CROUCHTURNMODIFIER = 100f;

	// Token: 0x040083F0 RID: 33776
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float MOVEMODIFIER = 100f;

	// Token: 0x040083F1 RID: 33777
	public bool ShowDebugObjects;

	// Token: 0x040083F2 RID: 33778
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int ForwardAmount;

	// Token: 0x040083F3 RID: 33779
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int PitchAmount;

	// Token: 0x040083F4 RID: 33780
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int StrafeAmount;

	// Token: 0x040083F5 RID: 33781
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int TurnAmount;

	// Token: 0x040083F6 RID: 33782
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int VerticalMoveAmount;

	// Token: 0x040083F7 RID: 33783
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsAttacking;

	// Token: 0x040083F8 RID: 33784
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsClimbing;

	// Token: 0x040083F9 RID: 33785
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsCrouching;

	// Token: 0x040083FA RID: 33786
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsGrounded;

	// Token: 0x040083FB RID: 33787
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsMoving;

	// Token: 0x040083FC RID: 33788
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsOutOfControl;

	// Token: 0x040083FD RID: 33789
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsReloading;

	// Token: 0x040083FE RID: 33790
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsRunning;

	// Token: 0x040083FF RID: 33791
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsSettingWeapon;

	// Token: 0x04008400 RID: 33792
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsZooming;

	// Token: 0x04008401 RID: 33793
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int StartClimb;

	// Token: 0x04008402 RID: 33794
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int StartOutOfControl;

	// Token: 0x04008403 RID: 33795
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int StartReload;

	// Token: 0x04008404 RID: 33796
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int WeaponGripIndex;

	// Token: 0x04008405 RID: 33797
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int WeaponTypeIndex;

	// Token: 0x04008406 RID: 33798
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_WeaponHandler m_WeaponHandler;

	// Token: 0x04008407 RID: 33799
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008408 RID: 33800
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x04008409 RID: 33801
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public SkinnedMeshRenderer m_Renderer;

	// Token: 0x0400840A RID: 33802
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Animator m_Animator;

	// Token: 0x0400840B RID: 33803
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_HeadPoint;

	// Token: 0x0400840C RID: 33804
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_DebugLookTarget;

	// Token: 0x0400840D RID: 33805
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_DebugLookArrow;
}
