using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001612 RID: 5650
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(AudioListener))]
[Preserve]
public class vp_FPCamera : vp_Component
{
	// Token: 0x17001483 RID: 5251
	// (get) Token: 0x0600AFE9 RID: 45033 RVA: 0x00419505 File Offset: 0x00417705
	// (set) Token: 0x0600AFEA RID: 45034 RVA: 0x0041950D File Offset: 0x0041770D
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

	// Token: 0x17001484 RID: 5252
	// (get) Token: 0x0600AFEB RID: 45035 RVA: 0x00419516 File Offset: 0x00417716
	public Vector3 CollisionVector
	{
		get
		{
			return this.m_CollisionVector;
		}
	}

	// Token: 0x17001485 RID: 5253
	// (get) Token: 0x0600AFEC RID: 45036 RVA: 0x0041951E File Offset: 0x0041771E
	// (set) Token: 0x0600AFED RID: 45037 RVA: 0x00419526 File Offset: 0x00417726
	public bool HasOverheadSpace
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17001486 RID: 5254
	// (get) Token: 0x0600AFEE RID: 45038 RVA: 0x0041952F File Offset: 0x0041772F
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

	// Token: 0x17001487 RID: 5255
	// (get) Token: 0x0600AFEF RID: 45039 RVA: 0x00419564 File Offset: 0x00417764
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

	// Token: 0x17001488 RID: 5256
	// (get) Token: 0x0600AFF0 RID: 45040 RVA: 0x00419590 File Offset: 0x00417790
	// (set) Token: 0x0600AFF1 RID: 45041 RVA: 0x004195A3 File Offset: 0x004177A3
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

	// Token: 0x17001489 RID: 5257
	// (get) Token: 0x0600AFF2 RID: 45042 RVA: 0x004195BD File Offset: 0x004177BD
	public Vector3 Forward
	{
		get
		{
			return this.m_Transform.forward;
		}
	}

	// Token: 0x1700148A RID: 5258
	// (get) Token: 0x0600AFF3 RID: 45043 RVA: 0x004195CA File Offset: 0x004177CA
	// (set) Token: 0x0600AFF4 RID: 45044 RVA: 0x004195D2 File Offset: 0x004177D2
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

	// Token: 0x1700148B RID: 5259
	// (get) Token: 0x0600AFF5 RID: 45045 RVA: 0x004195EC File Offset: 0x004177EC
	// (set) Token: 0x0600AFF6 RID: 45046 RVA: 0x004195F4 File Offset: 0x004177F4
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

	// Token: 0x1700148C RID: 5260
	// (get) Token: 0x0600AFF7 RID: 45047 RVA: 0x004195FD File Offset: 0x004177FD
	// (set) Token: 0x0600AFF8 RID: 45048 RVA: 0x00419605 File Offset: 0x00417805
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

	// Token: 0x1700148D RID: 5261
	// (get) Token: 0x0600AFF9 RID: 45049 RVA: 0x0041960E File Offset: 0x0041780E
	public float CurrentCameraDistance
	{
		get
		{
			return Mathf.Min(this.current3rdPersonOffset.z, this.CameraCollisionDistance);
		}
	}

	// Token: 0x1700148E RID: 5262
	// (get) Token: 0x0600AFFA RID: 45050 RVA: 0x00419626 File Offset: 0x00417826
	// (set) Token: 0x0600AFFB RID: 45051 RVA: 0x0041962E File Offset: 0x0041782E
	public float CameraCollisionDistance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700148F RID: 5263
	// (get) Token: 0x0600AFFC RID: 45052 RVA: 0x00419637 File Offset: 0x00417837
	// (set) Token: 0x0600AFFD RID: 45053 RVA: 0x00419640 File Offset: 0x00417840
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

	// Token: 0x17001490 RID: 5264
	// (get) Token: 0x0600AFFE RID: 45054 RVA: 0x0010D292 File Offset: 0x0010B492
	public virtual Transform OnValue_CameraTransform
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return base.transform;
		}
	}

	// Token: 0x0600AFFF RID: 45055 RVA: 0x004197EC File Offset: 0x004179EC
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

	// Token: 0x0600B000 RID: 45056 RVA: 0x004198F8 File Offset: 0x00417AF8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		base.OnEnable();
		vp_TargetEvent<float>.Register(base.Parent, "CameraBombShake", new Action<float>(this.OnMessage_CameraBombShake));
		vp_TargetEvent<float>.Register(base.Parent, "CameraGroundStomp", new Action<float>(this.OnMessage_CameraGroundStomp));
	}

	// Token: 0x0600B001 RID: 45057 RVA: 0x00419948 File Offset: 0x00417B48
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
		vp_TargetEvent<float>.Unregister(base.Parent, "CameraBombShake", new Action<float>(this.OnMessage_CameraBombShake));
		vp_TargetEvent<float>.Unregister(base.Parent, "CameraGroundStomp", new Action<float>(this.OnMessage_CameraGroundStomp));
	}

	// Token: 0x0600B002 RID: 45058 RVA: 0x00419995 File Offset: 0x00417B95
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.Refresh();
		this.SnapSprings();
		this.SnapZoom();
	}

	// Token: 0x0600B003 RID: 45059 RVA: 0x004199AF File Offset: 0x00417BAF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x0600B004 RID: 45060 RVA: 0x004199B8 File Offset: 0x00417BB8
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

	// Token: 0x0600B005 RID: 45061 RVA: 0x00419B2C File Offset: 0x00417D2C
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

	// Token: 0x0600B006 RID: 45062 RVA: 0x00419BD4 File Offset: 0x00417DD4
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

	// Token: 0x0600B007 RID: 45063 RVA: 0x00419EDC File Offset: 0x004180DC
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

	// Token: 0x0600B008 RID: 45064 RVA: 0x0041A293 File Offset: 0x00418493
	public void AlignCharacterToCamera()
	{
		this.targetRotation3P = base.Transform.eulerAngles.y;
	}

	// Token: 0x0600B009 RID: 45065 RVA: 0x0041A2AC File Offset: 0x004184AC
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

	// Token: 0x0600B00A RID: 45066 RVA: 0x0041A6F8 File Offset: 0x004188F8
	public virtual void AddForce(Vector3 force)
	{
		this.m_PositionSpring.AddForce(force);
	}

	// Token: 0x0600B00B RID: 45067 RVA: 0x0041A706 File Offset: 0x00418906
	public virtual void AddForce(float x, float y, float z)
	{
		this.AddForce(new Vector3(x, y, z));
	}

	// Token: 0x0600B00C RID: 45068 RVA: 0x0041A716 File Offset: 0x00418916
	public virtual void AddForce2(Vector3 force)
	{
		this.m_PositionSpring2.AddForce(force);
	}

	// Token: 0x0600B00D RID: 45069 RVA: 0x0041A724 File Offset: 0x00418924
	public void AddForce2(float x, float y, float z)
	{
		this.AddForce2(new Vector3(x, y, z));
	}

	// Token: 0x0600B00E RID: 45070 RVA: 0x0041A734 File Offset: 0x00418934
	public virtual void AddRollForce(float force)
	{
		this.m_RotationSpring.AddForce(Vector3.forward * force);
	}

	// Token: 0x0600B00F RID: 45071 RVA: 0x0041A74C File Offset: 0x0041894C
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

	// Token: 0x0600B010 RID: 45072 RVA: 0x0041A8EC File Offset: 0x00418AEC
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

	// Token: 0x0600B011 RID: 45073 RVA: 0x0041A96C File Offset: 0x00418B6C
	public void RefreshZoom()
	{
		float num = 1f - (this.m_FinalZoomTime - Time.time) / this.RenderingZoomDamping;
		base.gameObject.GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(base.gameObject.GetComponent<Camera>().fieldOfView, (float)Constants.cDefaultCameraFieldOfView + this.ZoomOffset, num);
	}

	// Token: 0x0600B012 RID: 45074 RVA: 0x0041A9C6 File Offset: 0x00418BC6
	public virtual void Zoom()
	{
		this.m_FinalZoomTime = Time.time + this.RenderingZoomDamping;
	}

	// Token: 0x0600B013 RID: 45075 RVA: 0x0041A9DA File Offset: 0x00418BDA
	public virtual void SnapZoom()
	{
		base.gameObject.GetComponent<Camera>().fieldOfView = (float)Constants.cDefaultCameraFieldOfView + this.ZoomOffset;
	}

	// Token: 0x0600B014 RID: 45076 RVA: 0x0041A9FC File Offset: 0x00418BFC
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

	// Token: 0x0600B015 RID: 45077 RVA: 0x0041AD28 File Offset: 0x00418F28
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

	// Token: 0x0600B016 RID: 45078 RVA: 0x0041AFC4 File Offset: 0x004191C4
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

	// Token: 0x0600B017 RID: 45079 RVA: 0x0041B018 File Offset: 0x00419218
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateSwaying()
	{
		Vector3 vector = base.Transform.InverseTransformDirection(this.FPController.CharacterController.velocity * 0.016f) * Time.timeScale;
		this.AddRollForce(vector.x * this.RotationStrafeRoll);
	}

	// Token: 0x0600B018 RID: 45080 RVA: 0x0041B068 File Offset: 0x00419268
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

	// Token: 0x0600B019 RID: 45081 RVA: 0x0041B160 File Offset: 0x00419360
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateSprings()
	{
		Vector3 position = base.Transform.position;
		this.m_PositionSpring.FixedUpdate();
		this.m_PositionSpring2.FixedUpdate();
		this.m_RotationSpring.FixedUpdate();
		base.Transform.position = position;
	}

	// Token: 0x0600B01A RID: 45082 RVA: 0x0041B1A8 File Offset: 0x004193A8
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

	// Token: 0x0600B01B RID: 45083 RVA: 0x0041B1DC File Offset: 0x004193DC
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

	// Token: 0x0600B01C RID: 45084 RVA: 0x0041B358 File Offset: 0x00419558
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

	// Token: 0x0600B01D RID: 45085 RVA: 0x0041B41C File Offset: 0x0041961C
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

	// Token: 0x0600B01E RID: 45086 RVA: 0x0041B47B File Offset: 0x0041967B
	public virtual void Stop()
	{
		this.SnapSprings();
		this.SnapZoom();
		this.Refresh();
	}

	// Token: 0x0600B01F RID: 45087 RVA: 0x0041B48F File Offset: 0x0041968F
	public virtual void SetRotation(Vector2 eulerAngles, bool stopZoomAndSprings)
	{
		this.Angle = eulerAngles;
		if (stopZoomAndSprings)
		{
			this.Stop();
		}
	}

	// Token: 0x0600B020 RID: 45088 RVA: 0x0041B4A1 File Offset: 0x004196A1
	public virtual void SetRotation(Vector2 eulerAngles)
	{
		this.Angle = eulerAngles;
		this.Stop();
	}

	// Token: 0x0600B021 RID: 45089 RVA: 0x0041B4B0 File Offset: 0x004196B0
	public virtual void SetRotation(Vector2 eulerAngles, bool stopZoomAndSprings, bool obsolete)
	{
		this.SetRotation(eulerAngles, stopZoomAndSprings);
	}

	// Token: 0x0600B022 RID: 45090 RVA: 0x0041B4BC File Offset: 0x004196BC
	public Vector3 GetLookPoint()
	{
		if (!this.Player.IsFirstPerson.Get() && Physics.Linecast(base.Transform.position, base.Transform.position + base.Transform.forward * 1000f, out this.m_LookPointHit, 1082195968) && !this.m_LookPointHit.collider.isTrigger && base.Root.InverseTransformPoint(this.m_LookPointHit.point).z > 0f)
		{
			return this.m_LookPointHit.point;
		}
		return base.Transform.position + base.Transform.forward * 1000f;
	}

	// Token: 0x17001491 RID: 5265
	// (get) Token: 0x0600B023 RID: 45091 RVA: 0x0041B58A File Offset: 0x0041978A
	public virtual Vector3 OnValue_LookPoint
	{
		get
		{
			return this.LookPoint;
		}
	}

	// Token: 0x17001492 RID: 5266
	// (get) Token: 0x0600B024 RID: 45092 RVA: 0x0041B594 File Offset: 0x00419794
	public virtual Vector3 OnValue_CameraLookDirection
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return (this.Player.LookPoint.Get() - base.Transform.position).normalized;
		}
	}

	// Token: 0x0600B025 RID: 45093 RVA: 0x0041B5D0 File Offset: 0x004197D0
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

	// Token: 0x0600B026 RID: 45094 RVA: 0x0041B684 File Offset: 0x00419884
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_HeadImpact(float impact)
	{
		if (this.m_RotationSpring != null && Mathf.Abs(this.m_RotationSpring.State.z) < 30f)
		{
			this.m_RotationSpring.AddForce(Vector3.forward * (impact * 20f) * Time.timeScale);
		}
	}

	// Token: 0x0600B027 RID: 45095 RVA: 0x0041B6DB File Offset: 0x004198DB
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraGroundStomp(float impact)
	{
		this.AddForce2(new Vector3(0f, -1f, 0f) * impact);
	}

	// Token: 0x0600B028 RID: 45096 RVA: 0x0041B6FD File Offset: 0x004198FD
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraBombShake(float impact)
	{
		this.DoBomb(new Vector3(1f, -10f, 1f) * impact, 1f, 2f);
	}

	// Token: 0x0600B029 RID: 45097 RVA: 0x0041B729 File Offset: 0x00419929
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Zoom()
	{
		if (this.Player == null)
		{
			return;
		}
		this.Player.Run.Stop(0f);
	}

	// Token: 0x0600B02A RID: 45098 RVA: 0x0041B74F File Offset: 0x0041994F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Run()
	{
		return this.Player == null || !this.Player.Zoom.Active;
	}

	// Token: 0x17001493 RID: 5267
	// (get) Token: 0x0600B02B RID: 45099 RVA: 0x0041B776 File Offset: 0x00419976
	// (set) Token: 0x0600B02C RID: 45100 RVA: 0x0041B77E File Offset: 0x0041997E
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

	// Token: 0x0600B02D RID: 45101 RVA: 0x0041B787 File Offset: 0x00419987
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_Stop()
	{
		this.Stop();
	}

	// Token: 0x0600B02E RID: 45102 RVA: 0x0041B78F File Offset: 0x0041998F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Dead()
	{
		if (this.Player.IsFirstPerson.Get())
		{
			return;
		}
		this.PositionOnDeath = base.Transform.position - this.m_Final3rdPersonCameraOffset;
	}

	// Token: 0x0600B02F RID: 45103 RVA: 0x0041B7C5 File Offset: 0x004199C5
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		this.PositionOnDeath = Vector3.zero;
		this.m_Current3rdPersonBlend = 0f;
	}

	// Token: 0x17001494 RID: 5268
	// (get) Token: 0x0600B030 RID: 45104 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool OnValue_IsLocal
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x0600B031 RID: 45105 RVA: 0x0041B7DD File Offset: 0x004199DD
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraToggle3rdPerson()
	{
		this.m_Player.IsFirstPerson.Set(!this.m_Player.IsFirstPerson.Get());
	}

	// Token: 0x04008354 RID: 33620
	public Vector3 DrivingPosition;

	// Token: 0x04008355 RID: 33621
	public vp_FPController FPController;

	// Token: 0x04008356 RID: 33622
	public float RenderingFieldOfView = 60f;

	// Token: 0x04008357 RID: 33623
	public float RenderingZoomDamping = 0.2f;

	// Token: 0x04008358 RID: 33624
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FinalZoomTime;

	// Token: 0x04008359 RID: 33625
	public float ZoomOffset;

	// Token: 0x0400835A RID: 33626
	public Vector3 PositionOffset = new Vector3(0f, 1.75f, 0.1f);

	// Token: 0x0400835B RID: 33627
	public Vector3 AimingPositionOffset = Vector3.zero;

	// Token: 0x0400835C RID: 33628
	public float PositionGroundLimit = 0.1f;

	// Token: 0x0400835D RID: 33629
	public float PositionSpringStiffness = 0.01f;

	// Token: 0x0400835E RID: 33630
	public float PositionSpringDamping = 0.25f;

	// Token: 0x0400835F RID: 33631
	public float PositionSpring2Stiffness = 0.95f;

	// Token: 0x04008360 RID: 33632
	public float PositionSpring2Damping = 0.25f;

	// Token: 0x04008361 RID: 33633
	public float PositionKneeling = 0.025f;

	// Token: 0x04008362 RID: 33634
	public int PositionKneelingSoftness = 1;

	// Token: 0x04008363 RID: 33635
	public float PositionEarthQuakeFactor = 1f;

	// Token: 0x04008364 RID: 33636
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_PositionSpring;

	// Token: 0x04008365 RID: 33637
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_PositionSpring2;

	// Token: 0x04008366 RID: 33638
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_DrawCameraCollisionDebugLine;

	// Token: 0x04008367 RID: 33639
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 PositionOnDeath = Vector3.zero;

	// Token: 0x04008368 RID: 33640
	public Vector2 RotationPitchLimit = new Vector2(90f, -90f);

	// Token: 0x04008369 RID: 33641
	public Vector2 RotationYawLimit = new Vector2(-360f, 360f);

	// Token: 0x0400836A RID: 33642
	public float RotationSpringStiffness = 0.01f;

	// Token: 0x0400836B RID: 33643
	public float RotationSpringDamping = 0.25f;

	// Token: 0x0400836C RID: 33644
	public float RotationKneeling = 0.025f;

	// Token: 0x0400836D RID: 33645
	public int RotationKneelingSoftness = 1;

	// Token: 0x0400836E RID: 33646
	public float RotationStrafeRoll = 0.01f;

	// Token: 0x0400836F RID: 33647
	public float RotationEarthQuakeFactor;

	// Token: 0x04008370 RID: 33648
	public Vector3 LookPoint = Vector3.zero;

	// Token: 0x04008371 RID: 33649
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_Pitch;

	// Token: 0x04008372 RID: 33650
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_Yaw;

	// Token: 0x04008373 RID: 33651
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_RotationSpring;

	// Token: 0x04008374 RID: 33652
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_LookPointHit;

	// Token: 0x04008375 RID: 33653
	public Vector3 Position3rdPersonOffset = new Vector3(0.5f, 0.1f, 0.75f);

	// Token: 0x04008376 RID: 33654
	public bool Locked3rdPerson;

	// Token: 0x04008377 RID: 33655
	public float m_Current3rdPersonBlend;

	// Token: 0x04008378 RID: 33656
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Final3rdPersonCameraOffset = Vector3.zero;

	// Token: 0x04008379 RID: 33657
	public float ShakeSpeed;

	// Token: 0x0400837A RID: 33658
	public Vector3 ShakeAmplitude = new Vector3(10f, 10f, 0f);

	// Token: 0x0400837B RID: 33659
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Shake = Vector3.zero;

	// Token: 0x0400837C RID: 33660
	public float ShakeSpeed2;

	// Token: 0x0400837D RID: 33661
	public Vector3 ShakeAmplitude2 = new Vector3(10f, 10f, 0f);

	// Token: 0x0400837E RID: 33662
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Shake2 = Vector3.zero;

	// Token: 0x0400837F RID: 33663
	public Vector4 BobRate = new Vector4(0f, 1.4f, 0f, 0.7f);

	// Token: 0x04008380 RID: 33664
	public Vector4 BobAmplitude = new Vector4(0f, 0.25f, 0f, 0.5f);

	// Token: 0x04008381 RID: 33665
	public float BobInputVelocityScale = 1f;

	// Token: 0x04008382 RID: 33666
	public float BobMaxInputVelocity = 100f;

	// Token: 0x04008383 RID: 33667
	public bool BobRequireGroundContact = true;

	// Token: 0x04008384 RID: 33668
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastBobSpeed;

	// Token: 0x04008385 RID: 33669
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector4 m_CurrentBobAmp = Vector4.zero;

	// Token: 0x04008386 RID: 33670
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector4 m_CurrentBobVal = Vector4.zero;

	// Token: 0x04008387 RID: 33671
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_BobSpeed;

	// Token: 0x04008388 RID: 33672
	public vp_FPCamera.BobStepDelegate BobStepCallback;

	// Token: 0x04008389 RID: 33673
	public float BobStepThreshold = 10f;

	// Token: 0x0400838A RID: 33674
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastUpBob;

	// Token: 0x0400838B RID: 33675
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_BobWasElevating;

	// Token: 0x0400838C RID: 33676
	public bool HasCollision = true;

	// Token: 0x0400838D RID: 33677
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CollisionVector = Vector3.zero;

	// Token: 0x0400838E RID: 33678
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CameraCollisionStartPos = Vector3.zero;

	// Token: 0x0400838F RID: 33679
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CameraCollisionEndPos = Vector3.zero;

	// Token: 0x04008390 RID: 33680
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_CameraHit;

	// Token: 0x04008391 RID: 33681
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> cameraCollisionEndPosList = new List<Vector3>();

	// Token: 0x04008393 RID: 33683
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x04008394 RID: 33684
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Rigidbody m_FirstRigidbody;

	// Token: 0x04008395 RID: 33685
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cPlayerRotationSpeed = 16f;

	// Token: 0x04008396 RID: 33686
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cCameraOffsetSpeed = 10f;

	// Token: 0x04008397 RID: 33687
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 current3rdPersonOffset = Vector3.zero;

	// Token: 0x04008398 RID: 33688
	public float yaw3P;

	// Token: 0x04008399 RID: 33689
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float pitch3P;

	// Token: 0x0400839A RID: 33690
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetRotation3P;

	// Token: 0x0400839B RID: 33691
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool _cameraRelative3PMovement;

	// Token: 0x0400839C RID: 33692
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 shake3P;

	// Token: 0x0400839E RID: 33694
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Camera thisCamera;

	// Token: 0x0400839F RID: 33695
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsFirstPerson = true;

	// Token: 0x040083A0 RID: 33696
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool hasLateUpdateRan;

	// Token: 0x040083A1 RID: 33697
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector3 lastPos;

	// Token: 0x02001613 RID: 5651
	// (Invoke) Token: 0x0600B034 RID: 45108
	public delegate void BobStepDelegate();
}
