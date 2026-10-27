using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001624 RID: 5668
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(AudioListener))]
[Preserve]
public class vp_FPCamera : vp_Component
{
	// Token: 0x1700148F RID: 5263
	// (get) Token: 0x0600B041 RID: 45121 RVA: 0x0041B909 File Offset: 0x00419B09
	// (set) Token: 0x0600B042 RID: 45122 RVA: 0x0041B911 File Offset: 0x00419B11
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

	// Token: 0x17001490 RID: 5264
	// (get) Token: 0x0600B043 RID: 45123 RVA: 0x0041B91A File Offset: 0x00419B1A
	public Vector3 CollisionVector
	{
		get
		{
			return this.m_CollisionVector;
		}
	}

	// Token: 0x17001491 RID: 5265
	// (get) Token: 0x0600B044 RID: 45124 RVA: 0x0041B922 File Offset: 0x00419B22
	// (set) Token: 0x0600B045 RID: 45125 RVA: 0x0041B92A File Offset: 0x00419B2A
	public bool HasOverheadSpace
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17001492 RID: 5266
	// (get) Token: 0x0600B046 RID: 45126 RVA: 0x0041B933 File Offset: 0x00419B33
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

	// Token: 0x17001493 RID: 5267
	// (get) Token: 0x0600B047 RID: 45127 RVA: 0x0041B968 File Offset: 0x00419B68
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

	// Token: 0x17001494 RID: 5268
	// (get) Token: 0x0600B048 RID: 45128 RVA: 0x0041B994 File Offset: 0x00419B94
	// (set) Token: 0x0600B049 RID: 45129 RVA: 0x0041B9A7 File Offset: 0x00419BA7
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

	// Token: 0x17001495 RID: 5269
	// (get) Token: 0x0600B04A RID: 45130 RVA: 0x0041B9C1 File Offset: 0x00419BC1
	public Vector3 Forward
	{
		get
		{
			return this.m_Transform.forward;
		}
	}

	// Token: 0x17001496 RID: 5270
	// (get) Token: 0x0600B04B RID: 45131 RVA: 0x0041B9CE File Offset: 0x00419BCE
	// (set) Token: 0x0600B04C RID: 45132 RVA: 0x0041B9D6 File Offset: 0x00419BD6
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

	// Token: 0x17001497 RID: 5271
	// (get) Token: 0x0600B04D RID: 45133 RVA: 0x0041B9F0 File Offset: 0x00419BF0
	// (set) Token: 0x0600B04E RID: 45134 RVA: 0x0041B9F8 File Offset: 0x00419BF8
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

	// Token: 0x17001498 RID: 5272
	// (get) Token: 0x0600B04F RID: 45135 RVA: 0x0041BA01 File Offset: 0x00419C01
	// (set) Token: 0x0600B050 RID: 45136 RVA: 0x0041BA09 File Offset: 0x00419C09
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

	// Token: 0x17001499 RID: 5273
	// (get) Token: 0x0600B051 RID: 45137 RVA: 0x0041BA12 File Offset: 0x00419C12
	public float CurrentCameraDistance
	{
		get
		{
			return Mathf.Min(this.current3rdPersonOffset.z, this.CameraCollisionDistance);
		}
	}

	// Token: 0x1700149A RID: 5274
	// (get) Token: 0x0600B052 RID: 45138 RVA: 0x0041BA2A File Offset: 0x00419C2A
	// (set) Token: 0x0600B053 RID: 45139 RVA: 0x0041BA32 File Offset: 0x00419C32
	public float CameraCollisionDistance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700149B RID: 5275
	// (get) Token: 0x0600B054 RID: 45140 RVA: 0x0041BA3B File Offset: 0x00419C3B
	// (set) Token: 0x0600B055 RID: 45141 RVA: 0x0041BA44 File Offset: 0x00419C44
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

	// Token: 0x1700149C RID: 5276
	// (get) Token: 0x0600B056 RID: 45142 RVA: 0x0010D7E6 File Offset: 0x0010B9E6
	public virtual Transform OnValue_CameraTransform
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return base.transform;
		}
	}

	// Token: 0x0600B057 RID: 45143 RVA: 0x0041BBF0 File Offset: 0x00419DF0
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

	// Token: 0x0600B058 RID: 45144 RVA: 0x0041BCFC File Offset: 0x00419EFC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		base.OnEnable();
		vp_TargetEvent<float>.Register(base.Parent, "CameraBombShake", new Action<float>(this.OnMessage_CameraBombShake));
		vp_TargetEvent<float>.Register(base.Parent, "CameraGroundStomp", new Action<float>(this.OnMessage_CameraGroundStomp));
	}

	// Token: 0x0600B059 RID: 45145 RVA: 0x0041BD4C File Offset: 0x00419F4C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
		vp_TargetEvent<float>.Unregister(base.Parent, "CameraBombShake", new Action<float>(this.OnMessage_CameraBombShake));
		vp_TargetEvent<float>.Unregister(base.Parent, "CameraGroundStomp", new Action<float>(this.OnMessage_CameraGroundStomp));
	}

	// Token: 0x0600B05A RID: 45146 RVA: 0x0041BD99 File Offset: 0x00419F99
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.Refresh();
		this.SnapSprings();
		this.SnapZoom();
	}

	// Token: 0x0600B05B RID: 45147 RVA: 0x0041BDB3 File Offset: 0x00419FB3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x0600B05C RID: 45148 RVA: 0x0041BDBC File Offset: 0x00419FBC
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

	// Token: 0x0600B05D RID: 45149 RVA: 0x0041BF30 File Offset: 0x0041A130
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

	// Token: 0x0600B05E RID: 45150 RVA: 0x0041BFD8 File Offset: 0x0041A1D8
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

	// Token: 0x0600B05F RID: 45151 RVA: 0x0041C2E0 File Offset: 0x0041A4E0
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

	// Token: 0x0600B060 RID: 45152 RVA: 0x0041C697 File Offset: 0x0041A897
	public void AlignCharacterToCamera()
	{
		this.targetRotation3P = base.Transform.eulerAngles.y;
	}

	// Token: 0x0600B061 RID: 45153 RVA: 0x0041C6B0 File Offset: 0x0041A8B0
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

	// Token: 0x0600B062 RID: 45154 RVA: 0x0041CAFC File Offset: 0x0041ACFC
	public virtual void AddForce(Vector3 force)
	{
		this.m_PositionSpring.AddForce(force);
	}

	// Token: 0x0600B063 RID: 45155 RVA: 0x0041CB0A File Offset: 0x0041AD0A
	public virtual void AddForce(float x, float y, float z)
	{
		this.AddForce(new Vector3(x, y, z));
	}

	// Token: 0x0600B064 RID: 45156 RVA: 0x0041CB1A File Offset: 0x0041AD1A
	public virtual void AddForce2(Vector3 force)
	{
		this.m_PositionSpring2.AddForce(force);
	}

	// Token: 0x0600B065 RID: 45157 RVA: 0x0041CB28 File Offset: 0x0041AD28
	public void AddForce2(float x, float y, float z)
	{
		this.AddForce2(new Vector3(x, y, z));
	}

	// Token: 0x0600B066 RID: 45158 RVA: 0x0041CB38 File Offset: 0x0041AD38
	public virtual void AddRollForce(float force)
	{
		this.m_RotationSpring.AddForce(Vector3.forward * force);
	}

	// Token: 0x0600B067 RID: 45159 RVA: 0x0041CB50 File Offset: 0x0041AD50
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

	// Token: 0x0600B068 RID: 45160 RVA: 0x0041CCF0 File Offset: 0x0041AEF0
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

	// Token: 0x0600B069 RID: 45161 RVA: 0x0041CD70 File Offset: 0x0041AF70
	public void RefreshZoom()
	{
		float num = 1f - (this.m_FinalZoomTime - Time.time) / this.RenderingZoomDamping;
		base.gameObject.GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(base.gameObject.GetComponent<Camera>().fieldOfView, (float)Constants.cDefaultCameraFieldOfView + this.ZoomOffset, num);
	}

	// Token: 0x0600B06A RID: 45162 RVA: 0x0041CDCA File Offset: 0x0041AFCA
	public virtual void Zoom()
	{
		this.m_FinalZoomTime = Time.time + this.RenderingZoomDamping;
	}

	// Token: 0x0600B06B RID: 45163 RVA: 0x0041CDDE File Offset: 0x0041AFDE
	public virtual void SnapZoom()
	{
		base.gameObject.GetComponent<Camera>().fieldOfView = (float)Constants.cDefaultCameraFieldOfView + this.ZoomOffset;
	}

	// Token: 0x0600B06C RID: 45164 RVA: 0x0041CE00 File Offset: 0x0041B000
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

	// Token: 0x0600B06D RID: 45165 RVA: 0x0041D12C File Offset: 0x0041B32C
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

	// Token: 0x0600B06E RID: 45166 RVA: 0x0041D3C8 File Offset: 0x0041B5C8
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

	// Token: 0x0600B06F RID: 45167 RVA: 0x0041D41C File Offset: 0x0041B61C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateSwaying()
	{
		Vector3 vector = base.Transform.InverseTransformDirection(this.FPController.CharacterController.velocity * 0.016f) * Time.timeScale;
		this.AddRollForce(vector.x * this.RotationStrafeRoll);
	}

	// Token: 0x0600B070 RID: 45168 RVA: 0x0041D46C File Offset: 0x0041B66C
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

	// Token: 0x0600B071 RID: 45169 RVA: 0x0041D564 File Offset: 0x0041B764
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateSprings()
	{
		Vector3 position = base.Transform.position;
		this.m_PositionSpring.FixedUpdate();
		this.m_PositionSpring2.FixedUpdate();
		this.m_RotationSpring.FixedUpdate();
		base.Transform.position = position;
	}

	// Token: 0x0600B072 RID: 45170 RVA: 0x0041D5AC File Offset: 0x0041B7AC
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

	// Token: 0x0600B073 RID: 45171 RVA: 0x0041D5E0 File Offset: 0x0041B7E0
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

	// Token: 0x0600B074 RID: 45172 RVA: 0x0041D75C File Offset: 0x0041B95C
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

	// Token: 0x0600B075 RID: 45173 RVA: 0x0041D820 File Offset: 0x0041BA20
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

	// Token: 0x0600B076 RID: 45174 RVA: 0x0041D87F File Offset: 0x0041BA7F
	public virtual void Stop()
	{
		this.SnapSprings();
		this.SnapZoom();
		this.Refresh();
	}

	// Token: 0x0600B077 RID: 45175 RVA: 0x0041D893 File Offset: 0x0041BA93
	public virtual void SetRotation(Vector2 eulerAngles, bool stopZoomAndSprings)
	{
		this.Angle = eulerAngles;
		if (stopZoomAndSprings)
		{
			this.Stop();
		}
	}

	// Token: 0x0600B078 RID: 45176 RVA: 0x0041D8A5 File Offset: 0x0041BAA5
	public virtual void SetRotation(Vector2 eulerAngles)
	{
		this.Angle = eulerAngles;
		this.Stop();
	}

	// Token: 0x0600B079 RID: 45177 RVA: 0x0041D8B4 File Offset: 0x0041BAB4
	public virtual void SetRotation(Vector2 eulerAngles, bool stopZoomAndSprings, bool obsolete)
	{
		this.SetRotation(eulerAngles, stopZoomAndSprings);
	}

	// Token: 0x0600B07A RID: 45178 RVA: 0x0041D8C0 File Offset: 0x0041BAC0
	public Vector3 GetLookPoint()
	{
		if (!this.Player.IsFirstPerson.Get() && Physics.Linecast(base.Transform.position, base.Transform.position + base.Transform.forward * 1000f, out this.m_LookPointHit, 1082195968) && !this.m_LookPointHit.collider.isTrigger && base.Root.InverseTransformPoint(this.m_LookPointHit.point).z > 0f)
		{
			return this.m_LookPointHit.point;
		}
		return base.Transform.position + base.Transform.forward * 1000f;
	}

	// Token: 0x1700149D RID: 5277
	// (get) Token: 0x0600B07B RID: 45179 RVA: 0x0041D98E File Offset: 0x0041BB8E
	public virtual Vector3 OnValue_LookPoint
	{
		get
		{
			return this.LookPoint;
		}
	}

	// Token: 0x1700149E RID: 5278
	// (get) Token: 0x0600B07C RID: 45180 RVA: 0x0041D998 File Offset: 0x0041BB98
	public virtual Vector3 OnValue_CameraLookDirection
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return (this.Player.LookPoint.Get() - base.Transform.position).normalized;
		}
	}

	// Token: 0x0600B07D RID: 45181 RVA: 0x0041D9D4 File Offset: 0x0041BBD4
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

	// Token: 0x0600B07E RID: 45182 RVA: 0x0041DA88 File Offset: 0x0041BC88
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_HeadImpact(float impact)
	{
		if (this.m_RotationSpring != null && Mathf.Abs(this.m_RotationSpring.State.z) < 30f)
		{
			this.m_RotationSpring.AddForce(Vector3.forward * (impact * 20f) * Time.timeScale);
		}
	}

	// Token: 0x0600B07F RID: 45183 RVA: 0x0041DADF File Offset: 0x0041BCDF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraGroundStomp(float impact)
	{
		this.AddForce2(new Vector3(0f, -1f, 0f) * impact);
	}

	// Token: 0x0600B080 RID: 45184 RVA: 0x0041DB01 File Offset: 0x0041BD01
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraBombShake(float impact)
	{
		this.DoBomb(new Vector3(1f, -10f, 1f) * impact, 1f, 2f);
	}

	// Token: 0x0600B081 RID: 45185 RVA: 0x0041DB2D File Offset: 0x0041BD2D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Zoom()
	{
		if (this.Player == null)
		{
			return;
		}
		this.Player.Run.Stop(0f);
	}

	// Token: 0x0600B082 RID: 45186 RVA: 0x0041DB53 File Offset: 0x0041BD53
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Run()
	{
		return this.Player == null || !this.Player.Zoom.Active;
	}

	// Token: 0x1700149F RID: 5279
	// (get) Token: 0x0600B083 RID: 45187 RVA: 0x0041DB7A File Offset: 0x0041BD7A
	// (set) Token: 0x0600B084 RID: 45188 RVA: 0x0041DB82 File Offset: 0x0041BD82
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

	// Token: 0x0600B085 RID: 45189 RVA: 0x0041DB8B File Offset: 0x0041BD8B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_Stop()
	{
		this.Stop();
	}

	// Token: 0x0600B086 RID: 45190 RVA: 0x0041DB93 File Offset: 0x0041BD93
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Dead()
	{
		if (this.Player.IsFirstPerson.Get())
		{
			return;
		}
		this.PositionOnDeath = base.Transform.position - this.m_Final3rdPersonCameraOffset;
	}

	// Token: 0x0600B087 RID: 45191 RVA: 0x0041DBC9 File Offset: 0x0041BDC9
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		this.PositionOnDeath = Vector3.zero;
		this.m_Current3rdPersonBlend = 0f;
	}

	// Token: 0x170014A0 RID: 5280
	// (get) Token: 0x0600B088 RID: 45192 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool OnValue_IsLocal
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x0600B089 RID: 45193 RVA: 0x0041DBE1 File Offset: 0x0041BDE1
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraToggle3rdPerson()
	{
		this.m_Player.IsFirstPerson.Set(!this.m_Player.IsFirstPerson.Get());
	}

	// Token: 0x04008395 RID: 33685
	public Vector3 DrivingPosition;

	// Token: 0x04008396 RID: 33686
	public vp_FPController FPController;

	// Token: 0x04008397 RID: 33687
	public float RenderingFieldOfView = 60f;

	// Token: 0x04008398 RID: 33688
	public float RenderingZoomDamping = 0.2f;

	// Token: 0x04008399 RID: 33689
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FinalZoomTime;

	// Token: 0x0400839A RID: 33690
	public float ZoomOffset;

	// Token: 0x0400839B RID: 33691
	public Vector3 PositionOffset = new Vector3(0f, 1.75f, 0.1f);

	// Token: 0x0400839C RID: 33692
	public Vector3 AimingPositionOffset = Vector3.zero;

	// Token: 0x0400839D RID: 33693
	public float PositionGroundLimit = 0.1f;

	// Token: 0x0400839E RID: 33694
	public float PositionSpringStiffness = 0.01f;

	// Token: 0x0400839F RID: 33695
	public float PositionSpringDamping = 0.25f;

	// Token: 0x040083A0 RID: 33696
	public float PositionSpring2Stiffness = 0.95f;

	// Token: 0x040083A1 RID: 33697
	public float PositionSpring2Damping = 0.25f;

	// Token: 0x040083A2 RID: 33698
	public float PositionKneeling = 0.025f;

	// Token: 0x040083A3 RID: 33699
	public int PositionKneelingSoftness = 1;

	// Token: 0x040083A4 RID: 33700
	public float PositionEarthQuakeFactor = 1f;

	// Token: 0x040083A5 RID: 33701
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_PositionSpring;

	// Token: 0x040083A6 RID: 33702
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_PositionSpring2;

	// Token: 0x040083A7 RID: 33703
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_DrawCameraCollisionDebugLine;

	// Token: 0x040083A8 RID: 33704
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 PositionOnDeath = Vector3.zero;

	// Token: 0x040083A9 RID: 33705
	public Vector2 RotationPitchLimit = new Vector2(90f, -90f);

	// Token: 0x040083AA RID: 33706
	public Vector2 RotationYawLimit = new Vector2(-360f, 360f);

	// Token: 0x040083AB RID: 33707
	public float RotationSpringStiffness = 0.01f;

	// Token: 0x040083AC RID: 33708
	public float RotationSpringDamping = 0.25f;

	// Token: 0x040083AD RID: 33709
	public float RotationKneeling = 0.025f;

	// Token: 0x040083AE RID: 33710
	public int RotationKneelingSoftness = 1;

	// Token: 0x040083AF RID: 33711
	public float RotationStrafeRoll = 0.01f;

	// Token: 0x040083B0 RID: 33712
	public float RotationEarthQuakeFactor;

	// Token: 0x040083B1 RID: 33713
	public Vector3 LookPoint = Vector3.zero;

	// Token: 0x040083B2 RID: 33714
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_Pitch;

	// Token: 0x040083B3 RID: 33715
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_Yaw;

	// Token: 0x040083B4 RID: 33716
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_RotationSpring;

	// Token: 0x040083B5 RID: 33717
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_LookPointHit;

	// Token: 0x040083B6 RID: 33718
	public Vector3 Position3rdPersonOffset = new Vector3(0.5f, 0.1f, 0.75f);

	// Token: 0x040083B7 RID: 33719
	public bool Locked3rdPerson;

	// Token: 0x040083B8 RID: 33720
	public float m_Current3rdPersonBlend;

	// Token: 0x040083B9 RID: 33721
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Final3rdPersonCameraOffset = Vector3.zero;

	// Token: 0x040083BA RID: 33722
	public float ShakeSpeed;

	// Token: 0x040083BB RID: 33723
	public Vector3 ShakeAmplitude = new Vector3(10f, 10f, 0f);

	// Token: 0x040083BC RID: 33724
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Shake = Vector3.zero;

	// Token: 0x040083BD RID: 33725
	public float ShakeSpeed2;

	// Token: 0x040083BE RID: 33726
	public Vector3 ShakeAmplitude2 = new Vector3(10f, 10f, 0f);

	// Token: 0x040083BF RID: 33727
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Shake2 = Vector3.zero;

	// Token: 0x040083C0 RID: 33728
	public Vector4 BobRate = new Vector4(0f, 1.4f, 0f, 0.7f);

	// Token: 0x040083C1 RID: 33729
	public Vector4 BobAmplitude = new Vector4(0f, 0.25f, 0f, 0.5f);

	// Token: 0x040083C2 RID: 33730
	public float BobInputVelocityScale = 1f;

	// Token: 0x040083C3 RID: 33731
	public float BobMaxInputVelocity = 100f;

	// Token: 0x040083C4 RID: 33732
	public bool BobRequireGroundContact = true;

	// Token: 0x040083C5 RID: 33733
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastBobSpeed;

	// Token: 0x040083C6 RID: 33734
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector4 m_CurrentBobAmp = Vector4.zero;

	// Token: 0x040083C7 RID: 33735
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector4 m_CurrentBobVal = Vector4.zero;

	// Token: 0x040083C8 RID: 33736
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_BobSpeed;

	// Token: 0x040083C9 RID: 33737
	public vp_FPCamera.BobStepDelegate BobStepCallback;

	// Token: 0x040083CA RID: 33738
	public float BobStepThreshold = 10f;

	// Token: 0x040083CB RID: 33739
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastUpBob;

	// Token: 0x040083CC RID: 33740
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_BobWasElevating;

	// Token: 0x040083CD RID: 33741
	public bool HasCollision = true;

	// Token: 0x040083CE RID: 33742
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CollisionVector = Vector3.zero;

	// Token: 0x040083CF RID: 33743
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CameraCollisionStartPos = Vector3.zero;

	// Token: 0x040083D0 RID: 33744
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CameraCollisionEndPos = Vector3.zero;

	// Token: 0x040083D1 RID: 33745
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_CameraHit;

	// Token: 0x040083D2 RID: 33746
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> cameraCollisionEndPosList = new List<Vector3>();

	// Token: 0x040083D4 RID: 33748
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x040083D5 RID: 33749
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Rigidbody m_FirstRigidbody;

	// Token: 0x040083D6 RID: 33750
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cPlayerRotationSpeed = 16f;

	// Token: 0x040083D7 RID: 33751
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cCameraOffsetSpeed = 10f;

	// Token: 0x040083D8 RID: 33752
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 current3rdPersonOffset = Vector3.zero;

	// Token: 0x040083D9 RID: 33753
	public float yaw3P;

	// Token: 0x040083DA RID: 33754
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float pitch3P;

	// Token: 0x040083DB RID: 33755
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetRotation3P;

	// Token: 0x040083DC RID: 33756
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool _cameraRelative3PMovement;

	// Token: 0x040083DD RID: 33757
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 shake3P;

	// Token: 0x040083DF RID: 33759
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Camera thisCamera;

	// Token: 0x040083E0 RID: 33760
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsFirstPerson = true;

	// Token: 0x040083E1 RID: 33761
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool hasLateUpdateRan;

	// Token: 0x040083E2 RID: 33762
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector3 lastPos;

	// Token: 0x02001625 RID: 5669
	// (Invoke) Token: 0x0600B08C RID: 45196
	public delegate void BobStepDelegate();
}
