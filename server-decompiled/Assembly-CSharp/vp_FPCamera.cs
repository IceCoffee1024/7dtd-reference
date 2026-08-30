using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020015F3 RID: 5619
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(AudioListener))]
[Preserve]
public class vp_FPCamera : vp_Component
{
	// Token: 0x17001440 RID: 5184
	// (get) Token: 0x0600AF57 RID: 44887 RVA: 0x00415829 File Offset: 0x00413A29
	// (set) Token: 0x0600AF58 RID: 44888 RVA: 0x00415831 File Offset: 0x00413A31
	public bool DrawCameraCollisionDebugLine
	{
		get
		{
			return this.m_DrawCameraCollisionDebugLine;
		}
		set
		{
			this.m_DrawCameraCollisionDebugLine = value;
		}
	}

	// Token: 0x17001441 RID: 5185
	// (get) Token: 0x0600AF59 RID: 44889 RVA: 0x0041583A File Offset: 0x00413A3A
	public Vector3 CollisionVector
	{
		get
		{
			return this.m_CollisionVector;
		}
	}

	// Token: 0x17001442 RID: 5186
	// (get) Token: 0x0600AF5A RID: 44890 RVA: 0x00415842 File Offset: 0x00413A42
	// (set) Token: 0x0600AF5B RID: 44891 RVA: 0x0041584A File Offset: 0x00413A4A
	public bool HasOverheadSpace
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17001443 RID: 5187
	// (get) Token: 0x0600AF5C RID: 44892 RVA: 0x00415853 File Offset: 0x00413A53
	public vp_FPPlayerEventHandler Player
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.m_Player == null && base.EventHandler != null)
			{
				this.m_Player = (vp_FPPlayerEventHandler)base.EventHandler;
			}
			return this.m_Player;
		}
	}

	// Token: 0x17001444 RID: 5188
	// (get) Token: 0x0600AF5D RID: 44893 RVA: 0x00415888 File Offset: 0x00413A88
	public Rigidbody FirstRigidBody
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.m_FirstRigidbody == null)
			{
				this.m_FirstRigidbody = base.Transform.root.GetComponentInChildren<Rigidbody>();
			}
			return this.m_FirstRigidbody;
		}
	}

	// Token: 0x17001445 RID: 5189
	// (get) Token: 0x0600AF5E RID: 44894 RVA: 0x004158B4 File Offset: 0x00413AB4
	// (set) Token: 0x0600AF5F RID: 44895 RVA: 0x004158C7 File Offset: 0x00413AC7
	public Vector2 Angle
	{
		get
		{
			return new Vector2(this.m_Pitch, this.m_Yaw);
		}
		set
		{
			this.Pitch = value.x;
			this.Yaw = value.y;
		}
	}

	// Token: 0x17001446 RID: 5190
	// (get) Token: 0x0600AF60 RID: 44896 RVA: 0x004158E1 File Offset: 0x00413AE1
	public Vector3 Forward
	{
		get
		{
			return this.m_Transform.forward;
		}
	}

	// Token: 0x17001447 RID: 5191
	// (get) Token: 0x0600AF61 RID: 44897 RVA: 0x004158EE File Offset: 0x00413AEE
	// (set) Token: 0x0600AF62 RID: 44898 RVA: 0x004158F6 File Offset: 0x00413AF6
	public float Pitch
	{
		get
		{
			return this.m_Pitch;
		}
		set
		{
			if (value > 90f)
			{
				value -= 360f;
			}
			this.m_Pitch = value;
		}
	}

	// Token: 0x17001448 RID: 5192
	// (get) Token: 0x0600AF63 RID: 44899 RVA: 0x00415910 File Offset: 0x00413B10
	// (set) Token: 0x0600AF64 RID: 44900 RVA: 0x00415918 File Offset: 0x00413B18
	public float Yaw
	{
		get
		{
			return this.m_Yaw;
		}
		set
		{
			this.m_Yaw = value;
		}
	}

	// Token: 0x17001449 RID: 5193
	// (get) Token: 0x0600AF65 RID: 44901 RVA: 0x00415921 File Offset: 0x00413B21
	// (set) Token: 0x0600AF66 RID: 44902 RVA: 0x00415929 File Offset: 0x00413B29
	public virtual bool OnValue_CameraRelativeMovement3P
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this._cameraRelative3PMovement;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this._cameraRelative3PMovement = value;
		}
	}

	// Token: 0x1700144A RID: 5194
	// (get) Token: 0x0600AF67 RID: 44903 RVA: 0x00415932 File Offset: 0x00413B32
	public float CurrentCameraDistance
	{
		get
		{
			return Mathf.Min(this.current3rdPersonOffset.z, this.CameraCollisionDistance);
		}
	}

	// Token: 0x1700144B RID: 5195
	// (get) Token: 0x0600AF68 RID: 44904 RVA: 0x0041594A File Offset: 0x00413B4A
	// (set) Token: 0x0600AF69 RID: 44905 RVA: 0x00415952 File Offset: 0x00413B52
	public float CameraCollisionDistance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700144C RID: 5196
	// (get) Token: 0x0600AF6A RID: 44906 RVA: 0x0041595B File Offset: 0x00413B5B
	// (set) Token: 0x0600AF6B RID: 44907 RVA: 0x00415964 File Offset: 0x00413B64
	public virtual bool OnValue_IsFirstPerson
	{
		get
		{
			return this.m_IsFirstPerson;
		}
		set
		{
			if (this.m_IsFirstPerson != value)
			{
				this.m_IsFirstPerson = value;
				if (this.m_IsFirstPerson)
				{
					this.m_Yaw = this.yaw3P - 180f;
					Quaternion quaternion = Quaternion.AngleAxis(this.m_Yaw, Vector3.up);
					Quaternion quaternion2 = Quaternion.AngleAxis(0f, Vector3.left);
					base.Parent.rotation = vp_MathUtility.NaNSafeQuaternion(quaternion * quaternion2, base.Parent.rotation);
					this.SnapSprings();
				}
				if (!this.m_IsFirstPerson)
				{
					this.current3rdPersonOffset = Vector3.Lerp(new Vector3(0f, this.Position3rdPersonOffset.y, 0f), this.Position3rdPersonOffset, 0.5f);
					this.pitch3P = -base.transform.forward.y * 57.29578f;
					if (this.Player.Driving.Active)
					{
						this.yaw3P = Mathf.Atan2(-base.transform.forward.x, -base.transform.forward.z) * 57.29578f;
					}
					else
					{
						this.yaw3P = Mathf.Atan2(-base.Parent.transform.forward.x, -base.Parent.transform.forward.z) * 57.29578f;
						this.targetRotation3P = base.transform.eulerAngles.y;
						this.Player.Rotation.Set(new Vector2(this.Pitch, this.targetRotation3P));
					}
					this.SnapSprings();
					this.shake3P = Vector2.zero;
				}
			}
		}
	}

	// Token: 0x1700144D RID: 5197
	// (get) Token: 0x0600AF6C RID: 44908 RVA: 0x0010C95A File Offset: 0x0010AB5A
	public virtual Transform OnValue_CameraTransform
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return base.transform;
		}
	}

	// Token: 0x0600AF6D RID: 44909 RVA: 0x00415B10 File Offset: 0x00413D10
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.FPController = base.Transform.parent.GetComponent<vp_FPController>();
		this.SetRotation(new Vector2(base.Transform.eulerAngles.x, base.Transform.eulerAngles.y));
		this.thisCamera = base.GetComponent<Camera>();
		this.thisCamera.fieldOfView = (float)Constants.cDefaultCameraFieldOfView;
		this.m_PositionSpring = new vp_Spring(base.Transform, vp_Spring.UpdateMode.Position, false);
		this.m_PositionSpring.MinVelocity = 1E-05f;
		this.m_PositionSpring.RestState = this.PositionOffset + this.AimingPositionOffset;
		this.m_PositionSpring2 = new vp_Spring(base.Transform, vp_Spring.UpdateMode.PositionAdditiveLocal, false);
		this.m_PositionSpring2.MinVelocity = 1E-05f;
		this.m_RotationSpring = new vp_Spring(base.Transform, vp_Spring.UpdateMode.RotationAdditiveLocal, false);
		this.m_RotationSpring.MinVelocity = 1E-05f;
		this.m_RotationSpring.SdtdStopping = true;
		this.HasOverheadSpace = true;
	}

	// Token: 0x0600AF6E RID: 44910 RVA: 0x00415C1C File Offset: 0x00413E1C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		base.OnEnable();
		vp_TargetEvent<float>.Register(base.Parent, "CameraBombShake", new Action<float>(this.OnMessage_CameraBombShake));
		vp_TargetEvent<float>.Register(base.Parent, "CameraGroundStomp", new Action<float>(this.OnMessage_CameraGroundStomp));
	}

	// Token: 0x0600AF6F RID: 44911 RVA: 0x00415C6C File Offset: 0x00413E6C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
		vp_TargetEvent<float>.Unregister(base.Parent, "CameraBombShake", new Action<float>(this.OnMessage_CameraBombShake));
		vp_TargetEvent<float>.Unregister(base.Parent, "CameraGroundStomp", new Action<float>(this.OnMessage_CameraGroundStomp));
	}

	// Token: 0x0600AF70 RID: 44912 RVA: 0x00415CB9 File Offset: 0x00413EB9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.Refresh();
		this.SnapSprings();
		this.SnapZoom();
	}

	// Token: 0x0600AF71 RID: 44913 RVA: 0x00415CD3 File Offset: 0x00413ED3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x0600AF72 RID: 44914 RVA: 0x00415CDC File Offset: 0x00413EDC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		if (Time.timeScale == 0f)
		{
			return;
		}
		this.UpdateInput();
		if (!this.Player.IsFirstPerson.Get())
		{
			if (!this.Player.Driving.Active)
			{
				new Vector2(this.m_Pitch, base.Parent.transform.eulerAngles.y);
				float num = this.targetRotation3P;
				if (this.Player.CameraRelativeMovement3P.Get() || Vector3.Dot(base.Parent.transform.forward, new Vector3(base.transform.forward.x, 0f, base.transform.forward.z)) < 0.95f)
				{
					num = Mathf.LerpAngle(base.Parent.transform.eulerAngles.y, this.targetRotation3P, Time.deltaTime * 16f);
				}
				Vector2 vector = new Vector2(this.m_Pitch, num);
				this.Player.Rotation.Set(vector);
				base.Parent.rotation = Quaternion.Euler(new Vector2(0f, vector.y));
			}
			this.current3rdPersonOffset = Vector3.Lerp(this.current3rdPersonOffset, this.Position3rdPersonOffset, Time.deltaTime * 10f);
		}
	}

	// Token: 0x0600AF73 RID: 44915 RVA: 0x00415E50 File Offset: 0x00414050
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void FixedUpdate()
	{
		if (!this.Player.IsFirstPerson.Get())
		{
			if (!this.hasLateUpdateRan)
			{
				return;
			}
			this.hasLateUpdateRan = false;
			if (Time.timeScale > 0f && !this.Locked3rdPerson)
			{
				this.UpdateShakes();
				this.UpdateSprings();
			}
			return;
		}
		else
		{
			if (!this.hasLateUpdateRan)
			{
				return;
			}
			this.hasLateUpdateRan = false;
			base.FixedUpdate();
			if (Time.timeScale == 0f)
			{
				return;
			}
			if (this.Locked3rdPerson)
			{
				return;
			}
			this.UpdateZoom();
			this.UpdateSwaying();
			this.UpdateBob();
			this.UpdateEarthQuake();
			this.UpdateShakes();
			this.UpdateSprings();
			return;
		}
	}

	// Token: 0x0600AF74 RID: 44916 RVA: 0x00415EF8 File Offset: 0x004140F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void LateUpdate()
	{
		if (!this.Player.IsFirstPerson.Get())
		{
			this.hasLateUpdateRan = true;
			this.Update3rdPerson();
			return;
		}
		this.hasLateUpdateRan = true;
		base.LateUpdate();
		bool active = this.Player.Driving.Active;
		if (this.Locked3rdPerson)
		{
			if (!active)
			{
				Quaternion quaternion = Quaternion.AngleAxis(this.m_Yaw, Vector3.up);
				Quaternion quaternion2 = Quaternion.AngleAxis(0f, Vector3.left);
				base.Parent.rotation = vp_MathUtility.NaNSafeQuaternion(quaternion * quaternion2, base.Parent.rotation);
			}
			return;
		}
		if (this.FPController.enabled)
		{
			if (this.Player.Driving.Active)
			{
				this.m_Transform.position = this.DrivingPosition;
			}
			else if (this.Player.IsFirstPerson.Get())
			{
				this.m_Transform.position = this.FPController.SmoothPosition;
			}
			else
			{
				this.m_Transform.position = this.FPController.transform.position;
			}
			if (this.Player.IsFirstPerson.Get())
			{
				this.m_Transform.localPosition += this.m_PositionSpring.State + this.m_PositionSpring2.State;
			}
			else if (!this.Player.Driving.Active)
			{
				this.m_Transform.localPosition += this.m_PositionSpring.State + Vector3.Scale(this.m_PositionSpring2.State, Vector3.up);
			}
			if (this.HasCollision)
			{
				this.DoCameraCollision();
			}
		}
		Quaternion quaternion3 = Quaternion.AngleAxis(this.m_Yaw, Vector3.up);
		if (this.Player.Driving.Active)
		{
			Quaternion quaternion4 = Quaternion.AngleAxis(-this.m_Pitch, Vector3.left);
			base.Transform.rotation = vp_MathUtility.NaNSafeQuaternion(quaternion3 * quaternion4, base.Transform.rotation);
		}
		else
		{
			Quaternion quaternion5 = Quaternion.AngleAxis(0f, Vector3.left);
			base.Parent.rotation = vp_MathUtility.NaNSafeQuaternion(quaternion3 * quaternion5, base.Parent.rotation);
			quaternion5 = Quaternion.AngleAxis(-this.m_Pitch, Vector3.left);
			base.Transform.rotation = vp_MathUtility.NaNSafeQuaternion(quaternion3 * quaternion5, base.Transform.rotation);
		}
		base.Transform.localEulerAngles += vp_MathUtility.NaNSafeVector3(Vector3.right * this.m_RotationSpring.State.x, default(Vector3));
		base.Transform.localEulerAngles += vp_MathUtility.NaNSafeVector3(Vector3.forward * this.m_RotationSpring.State.z, default(Vector3));
	}

	// Token: 0x0600AF75 RID: 44917 RVA: 0x00416200 File Offset: 0x00414400
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update3rdPerson()
	{
		if (this.Position3rdPersonOffset == Vector3.zero)
		{
			return;
		}
		if (this.PositionOnDeath != Vector3.zero)
		{
			base.Transform.position = this.PositionOnDeath;
			if (this.FirstRigidBody != null)
			{
				base.Transform.LookAt(this.FirstRigidBody.transform.position + Vector3.up);
				return;
			}
			base.Transform.LookAt(base.Root.position + Vector3.up);
			return;
		}
		else
		{
			bool active = this.Player.Driving.Active;
			bool flag = this.Player.CameraRelativeMovement3P.Get();
			this.pitch3P = Mathf.Clamp(this.pitch3P + this.Player.InputSmoothLook.Get().y, -89.5f, 89.5f);
			this.m_Pitch = this.pitch3P;
			this.yaw3P += this.Player.InputSmoothLook.Get().x;
			if (this.Locked3rdPerson)
			{
				this.targetRotation3P += this.Player.InputSmoothLook.Get().x;
				this.yaw3P = this.targetRotation3P;
				return;
			}
			if (flag)
			{
				Vector2 vector = this.Player.InputMoveVector.Get();
				if (vector != Vector2.zero)
				{
					this.targetRotation3P = Mathf.Atan2(vector.x, vector.y) * 57.29578f;
				}
			}
			else
			{
				this.targetRotation3P = base.transform.eulerAngles.y;
			}
			this.m_Final3rdPersonCameraOffset = base.Transform.position;
			this.pitch3P += this.shake3P.y;
			this.yaw3P += this.shake3P.x;
			this.shake3P = Vector2.zero;
			Vector3 normalized = (Quaternion.Euler(-this.pitch3P, this.yaw3P, 0f) * Vector3.forward).normalized;
			if (active)
			{
				this.FPController.localPlayer.RefreshDrivingCameraPositions();
			}
			Vector3 vector2 = (active ? this.DrivingPosition : this.FPController.SmoothPosition);
			Vector3 vector3 = vector2 + normalized * this.current3rdPersonOffset.z;
			vector3 += Vector3.up * this.current3rdPersonOffset.y;
			base.Transform.position = vector3;
			this.m_Final3rdPersonCameraOffset -= base.Transform.position;
			Vector3 vector4 = vector2 + Vector3.up * this.current3rdPersonOffset.y;
			base.Transform.LookAt(vector4);
			base.Transform.position += base.Transform.TransformDirection(new Vector3(this.current3rdPersonOffset.x, 0f, 0f));
			float num = this.Pitch / 90f;
			float num2 = this.current3rdPersonOffset.z * num * 0.6f;
			num2 = Utils.FastMin(num2, 0.7f);
			base.Transform.position += new Vector3(base.Transform.forward.x, 0f, base.Transform.forward.z).normalized * num2;
			this.DoCameraCollision();
			this.LookPoint = this.GetLookPoint();
			return;
		}
	}

	// Token: 0x0600AF76 RID: 44918 RVA: 0x004165B7 File Offset: 0x004147B7
	public void AlignCharacterToCamera()
	{
		this.targetRotation3P = base.Transform.eulerAngles.y;
	}

	// Token: 0x0600AF77 RID: 44919 RVA: 0x004165D0 File Offset: 0x004147D0
	public virtual void DoCameraCollision()
	{
		this.HasOverheadSpace = true;
		this.CameraCollisionDistance = float.MaxValue;
		this.m_CameraCollisionStartPos = this.FPController.Transform.TransformPoint(0f, this.PositionOffset.y + this.AimingPositionOffset.y, 0f) - (this.m_Player.IsFirstPerson.Get() ? Vector3.zero : (this.FPController.Transform.position - this.FPController.SmoothPosition));
		if (this.m_Player.IsFirstPerson.Get())
		{
			Vector3 vector = this.m_CameraCollisionStartPos - Vector3.up * (this.FPController.CharacterController.height * 0.5f + 0.05f);
			this.m_CameraCollisionEndPos = vector + Vector3.up * this.FPController.CharacterController.radius * 2.1f;
			if (Physics.SphereCast(vector, this.FPController.CharacterController.radius, Vector3.up, out this.m_CameraHit, this.FPController.CharacterController.radius * 2.1f, 1082195968) && !this.m_CameraHit.collider.isTrigger)
			{
				this.m_CollisionVector = this.m_CameraCollisionEndPos - this.m_CameraHit.point;
				base.Transform.position = vector + Vector3.up * this.FPController.CharacterController.radius + Vector3.down * this.m_CollisionVector.y;
				this.CameraCollisionDistance = this.m_CameraHit.distance;
				this.HasOverheadSpace = false;
			}
			return;
		}
		this.m_CameraCollisionEndPos = base.Transform.position + (base.Transform.position - this.m_CameraCollisionStartPos).normalized * this.FPController.CharacterController.radius;
		this.m_CollisionVector = Vector3.zero;
		if (Physics.Linecast(this.m_CameraCollisionStartPos, this.m_CameraCollisionEndPos, out this.m_CameraHit, 1082195968) && !this.m_CameraHit.collider.isTrigger)
		{
			base.Transform.position = this.m_CameraHit.point - (this.m_CameraHit.point - this.m_CameraCollisionStartPos).normalized * this.FPController.CharacterController.radius;
			this.m_CollisionVector = this.m_CameraHit.point - this.m_CameraCollisionEndPos;
			this.CameraCollisionDistance = this.m_CameraHit.distance;
			return;
		}
		Camera playerCamera = GameManager.Instance.World.GetPrimaryPlayer().playerCamera;
		Vector3[] array = new Vector3[4];
		playerCamera.CalculateFrustumCorners(new Rect(0f, 0f, 1f, 1f), playerCamera.nearClipPlane, Camera.MonoOrStereoscopicEye.Mono, array);
		this.cameraCollisionEndPosList.Clear();
		for (int i = 0; i < array.Length; i++)
		{
			Vector3 vector2 = array[i] * 1.5f;
			this.m_CameraCollisionEndPos = base.Transform.position + base.Transform.TransformDirection(vector2);
			this.m_CollisionVector = Vector3.zero;
			this.cameraCollisionEndPosList.Add(this.m_CameraCollisionEndPos);
			if (Physics.Linecast(this.m_CameraCollisionStartPos, this.m_CameraCollisionEndPos, out this.m_CameraHit, 1082195968) && !this.m_CameraHit.collider.isTrigger)
			{
				base.Transform.position = this.m_CameraHit.point - (this.m_CameraHit.point - this.m_CameraCollisionStartPos).normalized * this.FPController.CharacterController.radius;
				this.m_CollisionVector = this.m_CameraHit.point - this.m_CameraCollisionEndPos;
				this.CameraCollisionDistance = this.m_CameraHit.distance;
				return;
			}
		}
	}

	// Token: 0x0600AF78 RID: 44920 RVA: 0x00416A1C File Offset: 0x00414C1C
	public virtual void AddForce(Vector3 force)
	{
		this.m_PositionSpring.AddForce(force);
	}

	// Token: 0x0600AF79 RID: 44921 RVA: 0x00416A2A File Offset: 0x00414C2A
	public virtual void AddForce(float x, float y, float z)
	{
		this.AddForce(new Vector3(x, y, z));
	}

	// Token: 0x0600AF7A RID: 44922 RVA: 0x00416A3A File Offset: 0x00414C3A
	public virtual void AddForce2(Vector3 force)
	{
		this.m_PositionSpring2.AddForce(force);
	}

	// Token: 0x0600AF7B RID: 44923 RVA: 0x00416A48 File Offset: 0x00414C48
	public void AddForce2(float x, float y, float z)
	{
		this.AddForce2(new Vector3(x, y, z));
	}

	// Token: 0x0600AF7C RID: 44924 RVA: 0x00416A58 File Offset: 0x00414C58
	public virtual void AddRollForce(float force)
	{
		this.m_RotationSpring.AddForce(Vector3.forward * force);
	}

	// Token: 0x0600AF7D RID: 44925 RVA: 0x00416A70 File Offset: 0x00414C70
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateInput()
	{
		if (this.Player.Dead.Active)
		{
			return;
		}
		if (this.Player.InputSmoothLook.Get() == Vector2.zero)
		{
			return;
		}
		this.m_Yaw += this.Player.InputSmoothLook.Get().x;
		this.m_Pitch += this.Player.InputSmoothLook.Get().y;
		this.m_Yaw = ((this.m_Yaw < -360f) ? (this.m_Yaw += 360f) : this.m_Yaw);
		this.m_Yaw = ((this.m_Yaw > 360f) ? (this.m_Yaw -= 360f) : this.m_Yaw);
		this.m_Yaw = Mathf.Clamp(this.m_Yaw, this.RotationYawLimit.x, this.RotationYawLimit.y);
		this.m_Pitch = ((this.m_Pitch < -360f) ? (this.m_Pitch += 360f) : this.m_Pitch);
		this.m_Pitch = ((this.m_Pitch > 360f) ? (this.m_Pitch -= 360f) : this.m_Pitch);
		this.m_Pitch = Mathf.Clamp(this.m_Pitch, -this.RotationPitchLimit.x, -this.RotationPitchLimit.y);
	}

	// Token: 0x0600AF7E RID: 44926 RVA: 0x00416C10 File Offset: 0x00414E10
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateZoom()
	{
		if (this.m_FinalZoomTime <= Time.time)
		{
			return;
		}
		this.RenderingZoomDamping = Mathf.Max(this.RenderingZoomDamping, 0.01f);
		float num = 1f - (this.m_FinalZoomTime - Time.time) / this.RenderingZoomDamping;
		base.gameObject.GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(base.gameObject.GetComponent<Camera>().fieldOfView, (float)Constants.cDefaultCameraFieldOfView + this.ZoomOffset, num);
	}

	// Token: 0x0600AF7F RID: 44927 RVA: 0x00416C90 File Offset: 0x00414E90
	public void RefreshZoom()
	{
		float num = 1f - (this.m_FinalZoomTime - Time.time) / this.RenderingZoomDamping;
		base.gameObject.GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(base.gameObject.GetComponent<Camera>().fieldOfView, (float)Constants.cDefaultCameraFieldOfView + this.ZoomOffset, num);
	}

	// Token: 0x0600AF80 RID: 44928 RVA: 0x00416CEA File Offset: 0x00414EEA
	public virtual void Zoom()
	{
		this.m_FinalZoomTime = Time.time + this.RenderingZoomDamping;
	}

	// Token: 0x0600AF81 RID: 44929 RVA: 0x00416CFE File Offset: 0x00414EFE
	public virtual void SnapZoom()
	{
		base.gameObject.GetComponent<Camera>().fieldOfView = (float)Constants.cDefaultCameraFieldOfView + this.ZoomOffset;
	}

	// Token: 0x0600AF82 RID: 44930 RVA: 0x00416D20 File Offset: 0x00414F20
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateShakes()
	{
		if (this.ShakeSpeed != 0f)
		{
			this.m_Yaw -= this.m_Shake.y;
			this.m_Pitch -= this.m_Shake.x;
			this.m_Shake = Vector3.Scale(vp_SmoothRandom.GetVector3Centered(this.ShakeSpeed), this.ShakeAmplitude);
			if (float.IsNaN(this.m_Shake.x) || float.IsNaN(this.m_Shake.y) || float.IsNaN(this.m_Shake.z))
			{
				Log.Warning("Shake NaN {0}, time {1}, speed {2}, amp {3}", new object[]
				{
					this.m_Shake,
					Time.time,
					this.ShakeSpeed,
					this.ShakeAmplitude
				});
				this.ShakeSpeed = 0f;
				this.m_Shake = Vector3.zero;
				this.m_Pitch += -1f;
			}
			this.m_Yaw += this.m_Shake.y;
			this.m_Pitch += this.m_Shake.x;
			this.m_RotationSpring.AddForce(Vector3.forward * this.m_Shake.z * Time.timeScale);
		}
		if (this.ShakeSpeed2 != 0f)
		{
			this.m_Yaw -= this.m_Shake2.y;
			this.m_Pitch -= this.m_Shake2.x;
			this.shake3P.x = this.shake3P.x - this.m_Shake2.x;
			this.shake3P.y = this.shake3P.y - this.m_Shake2.y;
			this.m_Shake2 = Vector3.Scale(vp_SmoothRandom.GetVector3Centered(this.ShakeSpeed2), this.ShakeAmplitude2);
			if (float.IsNaN(this.m_Shake2.x) || float.IsNaN(this.m_Shake2.y) || float.IsNaN(this.m_Shake2.z))
			{
				Log.Warning("Shake2 NaN {0}, time {1}, speed {2}, amp {3}", new object[]
				{
					this.m_Shake2,
					Time.time,
					this.ShakeSpeed2,
					this.ShakeAmplitude2
				});
				this.ShakeSpeed2 = 0f;
				this.m_Shake2 = Vector3.zero;
				this.m_Pitch += -1f;
			}
			this.m_Yaw += this.m_Shake2.y;
			this.m_Pitch += this.m_Shake2.x;
			this.m_RotationSpring.AddForce(Vector3.forward * this.m_Shake2.z * Time.timeScale);
			this.shake3P.x = this.shake3P.x + this.m_Shake2.x;
			this.shake3P.y = this.shake3P.y + this.m_Shake2.y;
		}
	}

	// Token: 0x0600AF83 RID: 44931 RVA: 0x0041704C File Offset: 0x0041524C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateBob()
	{
		if (this.BobAmplitude == Vector4.zero || this.BobRate == Vector4.zero)
		{
			return;
		}
		if (!this.Player.IsFirstPerson.Get())
		{
			return;
		}
		this.m_BobSpeed = ((this.BobRequireGroundContact && !this.FPController.Grounded) ? 0f : this.FPController.CharacterController.velocity.sqrMagnitude);
		this.m_BobSpeed = Mathf.Min(this.m_BobSpeed * this.BobInputVelocityScale, this.BobMaxInputVelocity);
		this.m_BobSpeed = Mathf.Round(this.m_BobSpeed * 1000f) / 1000f;
		if (this.m_BobSpeed == 0f)
		{
			this.m_BobSpeed = Mathf.Min(this.m_LastBobSpeed * 0.93f, this.BobMaxInputVelocity);
		}
		this.m_CurrentBobAmp.y = this.m_BobSpeed * (this.BobAmplitude.y * -0.0001f);
		this.m_CurrentBobVal.y = Mathf.Cos(Time.time * (this.BobRate.y * 10f)) * this.m_CurrentBobAmp.y;
		this.m_CurrentBobAmp.x = this.m_BobSpeed * (this.BobAmplitude.x * 0.0001f);
		this.m_CurrentBobVal.x = Mathf.Cos(Time.time * (this.BobRate.x * 10f)) * this.m_CurrentBobAmp.x;
		this.m_CurrentBobAmp.z = this.m_BobSpeed * (this.BobAmplitude.z * 0.0001f);
		this.m_CurrentBobVal.z = Mathf.Cos(Time.time * (this.BobRate.z * 10f)) * this.m_CurrentBobAmp.z;
		this.m_CurrentBobAmp.w = this.m_BobSpeed * (this.BobAmplitude.w * 0.0001f);
		this.m_CurrentBobVal.w = Mathf.Cos(Time.time * (this.BobRate.w * 10f)) * this.m_CurrentBobAmp.w;
		this.m_PositionSpring.AddForce(this.m_CurrentBobVal * Time.timeScale);
		this.AddRollForce(this.m_CurrentBobVal.w * Time.timeScale);
		this.m_LastBobSpeed = this.m_BobSpeed;
		this.DetectBobStep(this.m_BobSpeed, this.m_CurrentBobVal.y);
	}

	// Token: 0x0600AF84 RID: 44932 RVA: 0x004172E8 File Offset: 0x004154E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void DetectBobStep(float speed, float upBob)
	{
		if (this.BobStepCallback == null)
		{
			return;
		}
		if (speed < this.BobStepThreshold)
		{
			return;
		}
		bool flag = this.m_LastUpBob < upBob;
		this.m_LastUpBob = upBob;
		if (flag && !this.m_BobWasElevating)
		{
			this.BobStepCallback();
		}
		this.m_BobWasElevating = flag;
	}

	// Token: 0x0600AF85 RID: 44933 RVA: 0x0041733C File Offset: 0x0041553C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateSwaying()
	{
		Vector3 vector = base.Transform.InverseTransformDirection(this.FPController.CharacterController.velocity * 0.016f) * Time.timeScale;
		this.AddRollForce(vector.x * this.RotationStrafeRoll);
	}

	// Token: 0x0600AF86 RID: 44934 RVA: 0x0041738C File Offset: 0x0041558C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateEarthQuake()
	{
		if (this.Player == null)
		{
			return;
		}
		if (!this.Player.CameraEarthQuake.Active)
		{
			return;
		}
		if (this.m_PositionSpring.State.y >= this.m_PositionSpring.RestState.y)
		{
			Vector3 vector = this.Player.CameraEarthQuakeForce.Get();
			vector.y = -vector.y;
			this.Player.CameraEarthQuakeForce.Set(vector);
		}
		this.m_PositionSpring.AddForce(this.Player.CameraEarthQuakeForce.Get() * this.PositionEarthQuakeFactor);
		this.m_RotationSpring.AddForce(Vector3.forward * (-this.Player.CameraEarthQuakeForce.Get().x * 2f) * this.RotationEarthQuakeFactor);
	}

	// Token: 0x0600AF87 RID: 44935 RVA: 0x00417484 File Offset: 0x00415684
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateSprings()
	{
		Vector3 position = base.Transform.position;
		this.m_PositionSpring.FixedUpdate();
		this.m_PositionSpring2.FixedUpdate();
		this.m_RotationSpring.FixedUpdate();
		base.Transform.position = position;
	}

	// Token: 0x0600AF88 RID: 44936 RVA: 0x004174CC File Offset: 0x004156CC
	public virtual void DoBomb(Vector3 positionForce, float minRollForce, float maxRollForce)
	{
		this.AddForce2(positionForce);
		float num = global::UnityEngine.Random.Range(minRollForce, maxRollForce);
		if (global::UnityEngine.Random.value > 0.5f)
		{
			num = -num;
		}
		this.AddRollForce(num);
	}

	// Token: 0x0600AF89 RID: 44937 RVA: 0x00417500 File Offset: 0x00415700
	public override void Refresh()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.m_PositionSpring != null)
		{
			this.m_PositionSpring.Stiffness = new Vector3(this.PositionSpringStiffness, this.PositionSpringStiffness, this.PositionSpringStiffness);
			this.m_PositionSpring.Damping = Vector3.one - new Vector3(this.PositionSpringDamping, this.PositionSpringDamping, this.PositionSpringDamping);
			this.m_PositionSpring.MinState.y = this.PositionGroundLimit;
			this.m_PositionSpring.RestState = this.PositionOffset + this.AimingPositionOffset;
		}
		if (this.m_PositionSpring2 != null)
		{
			this.m_PositionSpring2.Stiffness = new Vector3(this.PositionSpring2Stiffness, this.PositionSpring2Stiffness, this.PositionSpring2Stiffness);
			this.m_PositionSpring2.Damping = Vector3.one - new Vector3(this.PositionSpring2Damping, this.PositionSpring2Damping, this.PositionSpring2Damping);
			this.m_PositionSpring2.MinState.y = -this.PositionOffset.y - this.AimingPositionOffset.y + this.PositionGroundLimit;
		}
		if (this.m_RotationSpring != null)
		{
			this.m_RotationSpring.Stiffness = new Vector3(this.RotationSpringStiffness, this.RotationSpringStiffness, this.RotationSpringStiffness);
			this.m_RotationSpring.Damping = Vector3.one - new Vector3(this.RotationSpringDamping, this.RotationSpringDamping, this.RotationSpringDamping);
		}
	}

	// Token: 0x0600AF8A RID: 44938 RVA: 0x0041767C File Offset: 0x0041587C
	public virtual void SnapSprings()
	{
		if (this.m_PositionSpring != null)
		{
			this.m_PositionSpring.RestState = this.PositionOffset + this.AimingPositionOffset;
			this.m_PositionSpring.State = this.PositionOffset + this.AimingPositionOffset;
			this.m_PositionSpring.Stop(true);
		}
		if (this.m_PositionSpring2 != null)
		{
			this.m_PositionSpring2.RestState = Vector3.zero;
			this.m_PositionSpring2.State = Vector3.zero;
			this.m_PositionSpring2.Stop(true);
		}
		if (this.m_RotationSpring != null)
		{
			this.m_RotationSpring.RestState = Vector3.zero;
			this.m_RotationSpring.State = Vector3.zero;
			this.m_RotationSpring.Stop(true);
		}
	}

	// Token: 0x0600AF8B RID: 44939 RVA: 0x00417740 File Offset: 0x00415940
	public virtual void StopSprings()
	{
		if (this.m_PositionSpring != null)
		{
			this.m_PositionSpring.Stop(true);
		}
		if (this.m_PositionSpring2 != null)
		{
			this.m_PositionSpring2.Stop(true);
		}
		if (this.m_RotationSpring != null)
		{
			this.m_RotationSpring.Stop(true);
		}
		this.m_BobSpeed = 0f;
		this.m_LastBobSpeed = 0f;
	}

	// Token: 0x0600AF8C RID: 44940 RVA: 0x0041779F File Offset: 0x0041599F
	public virtual void Stop()
	{
		this.SnapSprings();
		this.SnapZoom();
		this.Refresh();
	}

	// Token: 0x0600AF8D RID: 44941 RVA: 0x004177B3 File Offset: 0x004159B3
	public virtual void SetRotation(Vector2 eulerAngles, bool stopZoomAndSprings)
	{
		this.Angle = eulerAngles;
		if (stopZoomAndSprings)
		{
			this.Stop();
		}
	}

	// Token: 0x0600AF8E RID: 44942 RVA: 0x004177C5 File Offset: 0x004159C5
	public virtual void SetRotation(Vector2 eulerAngles)
	{
		this.Angle = eulerAngles;
		this.Stop();
	}

	// Token: 0x0600AF8F RID: 44943 RVA: 0x004177D4 File Offset: 0x004159D4
	public virtual void SetRotation(Vector2 eulerAngles, bool stopZoomAndSprings, bool obsolete)
	{
		this.SetRotation(eulerAngles, stopZoomAndSprings);
	}

	// Token: 0x0600AF90 RID: 44944 RVA: 0x004177E0 File Offset: 0x004159E0
	public Vector3 GetLookPoint()
	{
		if (!this.Player.IsFirstPerson.Get() && Physics.Linecast(base.Transform.position, base.Transform.position + base.Transform.forward * 1000f, out this.m_LookPointHit, 1082195968) && !this.m_LookPointHit.collider.isTrigger && base.Root.InverseTransformPoint(this.m_LookPointHit.point).z > 0f)
		{
			return this.m_LookPointHit.point;
		}
		return base.Transform.position + base.Transform.forward * 1000f;
	}

	// Token: 0x1700144E RID: 5198
	// (get) Token: 0x0600AF91 RID: 44945 RVA: 0x004178AE File Offset: 0x00415AAE
	public virtual Vector3 OnValue_LookPoint
	{
		get
		{
			return this.LookPoint;
		}
	}

	// Token: 0x1700144F RID: 5199
	// (get) Token: 0x0600AF92 RID: 44946 RVA: 0x004178B8 File Offset: 0x00415AB8
	public virtual Vector3 OnValue_CameraLookDirection
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return (this.Player.LookPoint.Get() - base.Transform.position).normalized;
		}
	}

	// Token: 0x0600AF93 RID: 44947 RVA: 0x004178F4 File Offset: 0x00415AF4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_FallImpact(float impact)
	{
		impact = Mathf.Abs(impact * 55f);
		float num = impact * this.PositionKneeling;
		float num2 = impact * this.RotationKneeling;
		num = Mathf.SmoothStep(0f, 1f, num);
		num2 = Mathf.SmoothStep(0f, 1f, num2);
		num2 = Mathf.SmoothStep(0f, 1f, num2);
		if (this.m_PositionSpring != null)
		{
			this.m_PositionSpring.AddSoftForce(Vector3.down * num, (float)this.PositionKneelingSoftness);
		}
		if (this.m_RotationSpring != null)
		{
			this.m_RotationSpring.AddSoftForce(Vector3.right * (num2 * 2f), (float)this.RotationKneelingSoftness);
		}
	}

	// Token: 0x0600AF94 RID: 44948 RVA: 0x004179A8 File Offset: 0x00415BA8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_HeadImpact(float impact)
	{
		if (this.m_RotationSpring != null && Mathf.Abs(this.m_RotationSpring.State.z) < 30f)
		{
			this.m_RotationSpring.AddForce(Vector3.forward * (impact * 20f) * Time.timeScale);
		}
	}

	// Token: 0x0600AF95 RID: 44949 RVA: 0x004179FF File Offset: 0x00415BFF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraGroundStomp(float impact)
	{
		this.AddForce2(new Vector3(0f, -1f, 0f) * impact);
	}

	// Token: 0x0600AF96 RID: 44950 RVA: 0x00417A21 File Offset: 0x00415C21
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraBombShake(float impact)
	{
		this.DoBomb(new Vector3(1f, -10f, 1f) * impact, 1f, 2f);
	}

	// Token: 0x0600AF97 RID: 44951 RVA: 0x00417A4D File Offset: 0x00415C4D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Zoom()
	{
		if (this.Player == null)
		{
			return;
		}
		this.Player.Run.Stop(0f);
	}

	// Token: 0x0600AF98 RID: 44952 RVA: 0x00417A73 File Offset: 0x00415C73
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Run()
	{
		return this.Player == null || !this.Player.Zoom.Active;
	}

	// Token: 0x17001450 RID: 5200
	// (get) Token: 0x0600AF99 RID: 44953 RVA: 0x00417A9A File Offset: 0x00415C9A
	// (set) Token: 0x0600AF9A RID: 44954 RVA: 0x00417AA2 File Offset: 0x00415CA2
	public virtual Vector2 OnValue_Rotation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.Angle;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.Angle = value;
		}
	}

	// Token: 0x0600AF9B RID: 44955 RVA: 0x00417AAB File Offset: 0x00415CAB
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_Stop()
	{
		this.Stop();
	}

	// Token: 0x0600AF9C RID: 44956 RVA: 0x00417AB3 File Offset: 0x00415CB3
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Dead()
	{
		if (this.Player.IsFirstPerson.Get())
		{
			return;
		}
		this.PositionOnDeath = base.Transform.position - this.m_Final3rdPersonCameraOffset;
	}

	// Token: 0x0600AF9D RID: 44957 RVA: 0x00417AE9 File Offset: 0x00415CE9
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		this.PositionOnDeath = Vector3.zero;
		this.m_Current3rdPersonBlend = 0f;
	}

	// Token: 0x17001451 RID: 5201
	// (get) Token: 0x0600AF9E RID: 44958 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool OnValue_IsLocal
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x0600AF9F RID: 44959 RVA: 0x00417B01 File Offset: 0x00415D01
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraToggle3rdPerson()
	{
		this.m_Player.IsFirstPerson.Set(!this.m_Player.IsFirstPerson.Get());
	}

	// Token: 0x040082FE RID: 33534
	public Vector3 DrivingPosition;

	// Token: 0x040082FF RID: 33535
	public vp_FPController FPController;

	// Token: 0x04008300 RID: 33536
	public float RenderingFieldOfView = 60f;

	// Token: 0x04008301 RID: 33537
	public float RenderingZoomDamping = 0.2f;

	// Token: 0x04008302 RID: 33538
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FinalZoomTime;

	// Token: 0x04008303 RID: 33539
	public float ZoomOffset;

	// Token: 0x04008304 RID: 33540
	public Vector3 PositionOffset = new Vector3(0f, 1.75f, 0.1f);

	// Token: 0x04008305 RID: 33541
	public Vector3 AimingPositionOffset = Vector3.zero;

	// Token: 0x04008306 RID: 33542
	public float PositionGroundLimit = 0.1f;

	// Token: 0x04008307 RID: 33543
	public float PositionSpringStiffness = 0.01f;

	// Token: 0x04008308 RID: 33544
	public float PositionSpringDamping = 0.25f;

	// Token: 0x04008309 RID: 33545
	public float PositionSpring2Stiffness = 0.95f;

	// Token: 0x0400830A RID: 33546
	public float PositionSpring2Damping = 0.25f;

	// Token: 0x0400830B RID: 33547
	public float PositionKneeling = 0.025f;

	// Token: 0x0400830C RID: 33548
	public int PositionKneelingSoftness = 1;

	// Token: 0x0400830D RID: 33549
	public float PositionEarthQuakeFactor = 1f;

	// Token: 0x0400830E RID: 33550
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_PositionSpring;

	// Token: 0x0400830F RID: 33551
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_PositionSpring2;

	// Token: 0x04008310 RID: 33552
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_DrawCameraCollisionDebugLine;

	// Token: 0x04008311 RID: 33553
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 PositionOnDeath = Vector3.zero;

	// Token: 0x04008312 RID: 33554
	public Vector2 RotationPitchLimit = new Vector2(90f, -90f);

	// Token: 0x04008313 RID: 33555
	public Vector2 RotationYawLimit = new Vector2(-360f, 360f);

	// Token: 0x04008314 RID: 33556
	public float RotationSpringStiffness = 0.01f;

	// Token: 0x04008315 RID: 33557
	public float RotationSpringDamping = 0.25f;

	// Token: 0x04008316 RID: 33558
	public float RotationKneeling = 0.025f;

	// Token: 0x04008317 RID: 33559
	public int RotationKneelingSoftness = 1;

	// Token: 0x04008318 RID: 33560
	public float RotationStrafeRoll = 0.01f;

	// Token: 0x04008319 RID: 33561
	public float RotationEarthQuakeFactor;

	// Token: 0x0400831A RID: 33562
	public Vector3 LookPoint = Vector3.zero;

	// Token: 0x0400831B RID: 33563
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_Pitch;

	// Token: 0x0400831C RID: 33564
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_Yaw;

	// Token: 0x0400831D RID: 33565
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_RotationSpring;

	// Token: 0x0400831E RID: 33566
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_LookPointHit;

	// Token: 0x0400831F RID: 33567
	public Vector3 Position3rdPersonOffset = new Vector3(0.5f, 0.1f, 0.75f);

	// Token: 0x04008320 RID: 33568
	public bool Locked3rdPerson;

	// Token: 0x04008321 RID: 33569
	public float m_Current3rdPersonBlend;

	// Token: 0x04008322 RID: 33570
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Final3rdPersonCameraOffset = Vector3.zero;

	// Token: 0x04008323 RID: 33571
	public float ShakeSpeed;

	// Token: 0x04008324 RID: 33572
	public Vector3 ShakeAmplitude = new Vector3(10f, 10f, 0f);

	// Token: 0x04008325 RID: 33573
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Shake = Vector3.zero;

	// Token: 0x04008326 RID: 33574
	public float ShakeSpeed2;

	// Token: 0x04008327 RID: 33575
	public Vector3 ShakeAmplitude2 = new Vector3(10f, 10f, 0f);

	// Token: 0x04008328 RID: 33576
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Shake2 = Vector3.zero;

	// Token: 0x04008329 RID: 33577
	public Vector4 BobRate = new Vector4(0f, 1.4f, 0f, 0.7f);

	// Token: 0x0400832A RID: 33578
	public Vector4 BobAmplitude = new Vector4(0f, 0.25f, 0f, 0.5f);

	// Token: 0x0400832B RID: 33579
	public float BobInputVelocityScale = 1f;

	// Token: 0x0400832C RID: 33580
	public float BobMaxInputVelocity = 100f;

	// Token: 0x0400832D RID: 33581
	public bool BobRequireGroundContact = true;

	// Token: 0x0400832E RID: 33582
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastBobSpeed;

	// Token: 0x0400832F RID: 33583
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector4 m_CurrentBobAmp = Vector4.zero;

	// Token: 0x04008330 RID: 33584
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector4 m_CurrentBobVal = Vector4.zero;

	// Token: 0x04008331 RID: 33585
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_BobSpeed;

	// Token: 0x04008332 RID: 33586
	public vp_FPCamera.BobStepDelegate BobStepCallback;

	// Token: 0x04008333 RID: 33587
	public float BobStepThreshold = 10f;

	// Token: 0x04008334 RID: 33588
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastUpBob;

	// Token: 0x04008335 RID: 33589
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_BobWasElevating;

	// Token: 0x04008336 RID: 33590
	public bool HasCollision = true;

	// Token: 0x04008337 RID: 33591
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CollisionVector = Vector3.zero;

	// Token: 0x04008338 RID: 33592
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CameraCollisionStartPos = Vector3.zero;

	// Token: 0x04008339 RID: 33593
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CameraCollisionEndPos = Vector3.zero;

	// Token: 0x0400833A RID: 33594
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_CameraHit;

	// Token: 0x0400833B RID: 33595
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> cameraCollisionEndPosList = new List<Vector3>();

	// Token: 0x0400833D RID: 33597
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x0400833E RID: 33598
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Rigidbody m_FirstRigidbody;

	// Token: 0x0400833F RID: 33599
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cPlayerRotationSpeed = 16f;

	// Token: 0x04008340 RID: 33600
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cCameraOffsetSpeed = 10f;

	// Token: 0x04008341 RID: 33601
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 current3rdPersonOffset = Vector3.zero;

	// Token: 0x04008342 RID: 33602
	public float yaw3P;

	// Token: 0x04008343 RID: 33603
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float pitch3P;

	// Token: 0x04008344 RID: 33604
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetRotation3P;

	// Token: 0x04008345 RID: 33605
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool _cameraRelative3PMovement;

	// Token: 0x04008346 RID: 33606
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 shake3P;

	// Token: 0x04008348 RID: 33608
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Camera thisCamera;

	// Token: 0x04008349 RID: 33609
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsFirstPerson = true;

	// Token: 0x0400834A RID: 33610
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool hasLateUpdateRan;

	// Token: 0x0400834B RID: 33611
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector3 lastPos;

	// Token: 0x020015F4 RID: 5620
	// (Invoke) Token: 0x0600AFA2 RID: 44962
	public delegate void BobStepDelegate();
}
