using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001607 RID: 5639
[RequireComponent(typeof(Animator))]
public class vp_BodyAnimator : MonoBehaviour
{
	// Token: 0x1700149B RID: 5275
	// (get) Token: 0x0600B0E9 RID: 45289 RVA: 0x00420804 File Offset: 0x0041EA04
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

	// Token: 0x1700149C RID: 5276
	// (get) Token: 0x0600B0EA RID: 45290 RVA: 0x0042083F File Offset: 0x0041EA3F
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

	// Token: 0x1700149D RID: 5277
	// (get) Token: 0x0600B0EB RID: 45291 RVA: 0x00420861 File Offset: 0x0041EA61
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

	// Token: 0x1700149E RID: 5278
	// (get) Token: 0x0600B0EC RID: 45292 RVA: 0x0042089C File Offset: 0x0041EA9C
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

	// Token: 0x1700149F RID: 5279
	// (get) Token: 0x0600B0ED RID: 45293 RVA: 0x004208C8 File Offset: 0x0041EAC8
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

	// Token: 0x170014A0 RID: 5280
	// (get) Token: 0x0600B0EE RID: 45294 RVA: 0x004208EA File Offset: 0x0041EAEA
	public Vector3 m_LocalVelocity
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return vp_MathUtility.SnapToZero(this.Transform.root.InverseTransformDirection(this.Player.Velocity.Get()) / this.m_MaxSpeed, 0.0001f);
		}
	}

	// Token: 0x170014A1 RID: 5281
	// (get) Token: 0x0600B0EF RID: 45295 RVA: 0x00420926 File Offset: 0x0041EB26
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

	// Token: 0x170014A2 RID: 5282
	// (get) Token: 0x0600B0F0 RID: 45296 RVA: 0x00420960 File Offset: 0x0041EB60
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

	// Token: 0x170014A3 RID: 5283
	// (get) Token: 0x0600B0F1 RID: 45297 RVA: 0x004209F1 File Offset: 0x0041EBF1
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

	// Token: 0x170014A4 RID: 5284
	// (get) Token: 0x0600B0F2 RID: 45298 RVA: 0x00420A14 File Offset: 0x0041EC14
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

	// Token: 0x0600B0F3 RID: 45299 RVA: 0x00420A8D File Offset: 0x0041EC8D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
	}

	// Token: 0x0600B0F4 RID: 45300 RVA: 0x00420AA9 File Offset: 0x0041ECA9
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B0F5 RID: 45301 RVA: 0x00420AC5 File Offset: 0x0041ECC5
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

	// Token: 0x0600B0F6 RID: 45302 RVA: 0x00420AE4 File Offset: 0x0041ECE4
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

	// Token: 0x0600B0F7 RID: 45303 RVA: 0x00420B40 File Offset: 0x0041ED40
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

	// Token: 0x0600B0F8 RID: 45304 RVA: 0x00420D2C File Offset: 0x0041EF2C
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

	// Token: 0x0600B0F9 RID: 45305 RVA: 0x00420D8C File Offset: 0x0041EF8C
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

	// Token: 0x0600B0FA RID: 45306 RVA: 0x00421028 File Offset: 0x0041F228
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

	// Token: 0x0600B0FB RID: 45307 RVA: 0x0042110C File Offset: 0x0041F30C
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

	// Token: 0x0600B0FC RID: 45308 RVA: 0x00421148 File Offset: 0x0041F348
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

	// Token: 0x0600B0FD RID: 45309 RVA: 0x0042119C File Offset: 0x0041F39C
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

	// Token: 0x0600B0FE RID: 45310 RVA: 0x0042148C File Offset: 0x0041F68C
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

	// Token: 0x0600B0FF RID: 45311 RVA: 0x0042186C File Offset: 0x0041FA6C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool GetIsMoving()
	{
		return Vector3.Scale(this.Player.Velocity.Get(), Vector3.right + Vector3.forward).magnitude > 0.01f;
	}

	// Token: 0x0600B100 RID: 45312 RVA: 0x004218B4 File Offset: 0x0041FAB4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual Vector3 GetLookPoint()
	{
		this.m_HeadLookBackup = this.HeadPoint.transform.eulerAngles;
		this.HeadPoint.transform.eulerAngles = this.Player.Rotation.Get();
		this.m_LookPoint = this.HeadPoint.transform.position + this.HeadPoint.transform.forward * 1000f;
		this.HeadPoint.transform.eulerAngles = this.m_HeadLookBackup;
		return this.m_LookPoint;
	}

	// Token: 0x0600B101 RID: 45313 RVA: 0x00421954 File Offset: 0x0041FB54
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

	// Token: 0x0600B102 RID: 45314 RVA: 0x00421A08 File Offset: 0x0041FC08
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

	// Token: 0x0600B103 RID: 45315 RVA: 0x00421AA8 File Offset: 0x0041FCA8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateGrounding()
	{
		Physics.SphereCast(new Ray(this.Transform.position + Vector3.up * 0.5f, Vector3.down), 0.4f, out this.m_GroundHit, 1f, 1084850184);
		this.m_Grounded = this.m_GroundHit.collider != null;
	}

	// Token: 0x0600B104 RID: 45316 RVA: 0x00421B10 File Offset: 0x0041FD10
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

	// Token: 0x0600B105 RID: 45317 RVA: 0x00421B90 File Offset: 0x0041FD90
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

	// Token: 0x0600B106 RID: 45318 RVA: 0x00421C10 File Offset: 0x0041FE10
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

	// Token: 0x0600B107 RID: 45319 RVA: 0x00421D60 File Offset: 0x0041FF60
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

	// Token: 0x0600B108 RID: 45320 RVA: 0x00421E14 File Offset: 0x00420014
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

	// Token: 0x0600B109 RID: 45321 RVA: 0x00421F30 File Offset: 0x00420130
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

	// Token: 0x170014A5 RID: 5285
	// (get) Token: 0x0600B10A RID: 45322 RVA: 0x00421F85 File Offset: 0x00420185
	public virtual float OnValue_BodyYaw
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.Transform.eulerAngles.y;
		}
	}

	// Token: 0x0600B10B RID: 45323 RVA: 0x00421F97 File Offset: 0x00420197
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStart_Attack()
	{
		this.m_AttackDoneTimer.Cancel();
		this.Animator.SetBool(this.IsAttacking, true);
	}

	// Token: 0x0600B10C RID: 45324 RVA: 0x00421FB6 File Offset: 0x004201B6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Attack()
	{
		vp_Timer.In(0.5f, delegate
		{
			this.Animator.SetBool(this.IsAttacking, false);
			this.RefreshWeaponStates();
		}, this.m_AttackDoneTimer);
	}

	// Token: 0x0600B10D RID: 45325 RVA: 0x00421FD4 File Offset: 0x004201D4
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

	// Token: 0x0600B10E RID: 45326 RVA: 0x00421FF2 File Offset: 0x004201F2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Reload()
	{
		this.Animator.SetTrigger(this.StartReload);
	}

	// Token: 0x0600B10F RID: 45327 RVA: 0x00422005 File Offset: 0x00420205
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_OutOfControl()
	{
		this.Animator.SetTrigger(this.StartOutOfControl);
	}

	// Token: 0x0600B110 RID: 45328 RVA: 0x00422018 File Offset: 0x00420218
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Climb()
	{
		this.Animator.SetTrigger(this.StartClimb);
	}

	// Token: 0x0600B111 RID: 45329 RVA: 0x0042202B File Offset: 0x0042022B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Dead()
	{
		if (this.m_AttackDoneTimer.Active)
		{
			this.m_AttackDoneTimer.Execute();
		}
	}

	// Token: 0x0600B112 RID: 45330 RVA: 0x00422045 File Offset: 0x00420245
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		this.HeadPointDirty = true;
	}

	// Token: 0x0600B113 RID: 45331 RVA: 0x0042204E File Offset: 0x0042024E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraToggle3rdPerson()
	{
		this.m_WasMoving = !this.m_WasMoving;
		this.HeadPointDirty = true;
	}

	// Token: 0x170014A6 RID: 5286
	// (get) Token: 0x0600B114 RID: 45332 RVA: 0x00422068 File Offset: 0x00420268
	public virtual Vector3 OnValue_HeadLookDirection
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return (this.Player.LookPoint.Get() - this.HeadPoint.transform.position).normalized;
		}
	}

	// Token: 0x170014A7 RID: 5287
	// (get) Token: 0x0600B115 RID: 45333 RVA: 0x00415742 File Offset: 0x00413942
	public virtual Vector3 OnValue_LookPoint
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.GetLookPoint();
		}
	}

	// Token: 0x0400849D RID: 33949
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsValid = true;

	// Token: 0x0400849E RID: 33950
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_ValidLookPoint = Vector3.zero;

	// Token: 0x0400849F RID: 33951
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_ValidLookPointForward;

	// Token: 0x040084A0 RID: 33952
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool HeadPointDirty = true;

	// Token: 0x040084A1 RID: 33953
	public GameObject HeadBone;

	// Token: 0x040084A2 RID: 33954
	public GameObject LowestSpineBone;

	// Token: 0x040084A3 RID: 33955
	[Range(0f, 90f)]
	public float HeadPitchCap = 45f;

	// Token: 0x040084A4 RID: 33956
	[Range(2f, 20f)]
	public float HeadPitchSpeed = 7f;

	// Token: 0x040084A5 RID: 33957
	[Range(0.2f, 20f)]
	public float HeadYawSpeed = 2f;

	// Token: 0x040084A6 RID: 33958
	[Range(0f, 1f)]
	public float LeaningFactor = 0.25f;

	// Token: 0x040084A7 RID: 33959
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<GameObject> m_HeadLookBones = new List<GameObject>();

	// Token: 0x040084A8 RID: 33960
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Vector3> m_ReferenceUpDirs;

	// Token: 0x040084A9 RID: 33961
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Vector3> m_ReferenceLookDirs;

	// Token: 0x040084AA RID: 33962
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentHeadLookYaw;

	// Token: 0x040084AB RID: 33963
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentHeadLookPitch;

	// Token: 0x040084AC RID: 33964
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<float> m_HeadLookFalloffs = new List<float>();

	// Token: 0x040084AD RID: 33965
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<float> m_HeadLookCurrentFalloffs;

	// Token: 0x040084AE RID: 33966
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<float> m_HeadLookTargetFalloffs;

	// Token: 0x040084AF RID: 33967
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_HeadLookTargetWorldDir;

	// Token: 0x040084B0 RID: 33968
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_HeadLookCurrentWorldDir;

	// Token: 0x040084B1 RID: 33969
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_HeadLookBackup = Vector3.zero;

	// Token: 0x040084B2 RID: 33970
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_LookPoint = Vector3.zero;

	// Token: 0x040084B3 RID: 33971
	public float FeetAdjustAngle = 80f;

	// Token: 0x040084B4 RID: 33972
	public float FeetAdjustSpeedStanding = 10f;

	// Token: 0x040084B5 RID: 33973
	public float FeetAdjustSpeedMoving = 12f;

	// Token: 0x040084B6 RID: 33974
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_PrevBodyYaw;

	// Token: 0x040084B7 RID: 33975
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_BodyYaw;

	// Token: 0x040084B8 RID: 33976
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentBodyYawTarget;

	// Token: 0x040084B9 RID: 33977
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastYaw;

	// Token: 0x040084BA RID: 33978
	public Vector3 ClimbOffset = Vector3.forward * 0.6f;

	// Token: 0x040084BB RID: 33979
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentForward;

	// Token: 0x040084BC RID: 33980
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentStrafe;

	// Token: 0x040084BD RID: 33981
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentTurn;

	// Token: 0x040084BE RID: 33982
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentTurnTarget;

	// Token: 0x040084BF RID: 33983
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MaxWalkSpeed = 1f;

	// Token: 0x040084C0 RID: 33984
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MaxRunSpeed = 1f;

	// Token: 0x040084C1 RID: 33985
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MaxCrouchSpeed = 1f;

	// Token: 0x040084C2 RID: 33986
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_WasMoving;

	// Token: 0x040084C3 RID: 33987
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_GroundHit;

	// Token: 0x040084C4 RID: 33988
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Grounded = true;

	// Token: 0x040084C5 RID: 33989
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_AttackDoneTimer = new vp_Timer.Handle();

	// Token: 0x040084C6 RID: 33990
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedUpdateTurnTargetTime;

	// Token: 0x040084C7 RID: 33991
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float TURNMODIFIER = 0.2f;

	// Token: 0x040084C8 RID: 33992
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float CROUCHTURNMODIFIER = 100f;

	// Token: 0x040084C9 RID: 33993
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float MOVEMODIFIER = 100f;

	// Token: 0x040084CA RID: 33994
	public bool ShowDebugObjects;

	// Token: 0x040084CB RID: 33995
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int ForwardAmount;

	// Token: 0x040084CC RID: 33996
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int PitchAmount;

	// Token: 0x040084CD RID: 33997
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int StrafeAmount;

	// Token: 0x040084CE RID: 33998
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int TurnAmount;

	// Token: 0x040084CF RID: 33999
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int VerticalMoveAmount;

	// Token: 0x040084D0 RID: 34000
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsAttacking;

	// Token: 0x040084D1 RID: 34001
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsClimbing;

	// Token: 0x040084D2 RID: 34002
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsCrouching;

	// Token: 0x040084D3 RID: 34003
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsGrounded;

	// Token: 0x040084D4 RID: 34004
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsMoving;

	// Token: 0x040084D5 RID: 34005
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsOutOfControl;

	// Token: 0x040084D6 RID: 34006
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsReloading;

	// Token: 0x040084D7 RID: 34007
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsRunning;

	// Token: 0x040084D8 RID: 34008
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsSettingWeapon;

	// Token: 0x040084D9 RID: 34009
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int IsZooming;

	// Token: 0x040084DA RID: 34010
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int StartClimb;

	// Token: 0x040084DB RID: 34011
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int StartOutOfControl;

	// Token: 0x040084DC RID: 34012
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int StartReload;

	// Token: 0x040084DD RID: 34013
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int WeaponGripIndex;

	// Token: 0x040084DE RID: 34014
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int WeaponTypeIndex;

	// Token: 0x040084DF RID: 34015
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_WeaponHandler m_WeaponHandler;

	// Token: 0x040084E0 RID: 34016
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040084E1 RID: 34017
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x040084E2 RID: 34018
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public SkinnedMeshRenderer m_Renderer;

	// Token: 0x040084E3 RID: 34019
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Animator m_Animator;

	// Token: 0x040084E4 RID: 34020
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_HeadPoint;

	// Token: 0x040084E5 RID: 34021
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_DebugLookTarget;

	// Token: 0x040084E6 RID: 34022
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_DebugLookArrow;
}
