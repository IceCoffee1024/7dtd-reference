using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020015D8 RID: 5592
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(AudioListener))]
[Preserve]
public class vp_FPCamera : vp_Component
{
	// Token: 0x17001407 RID: 5127
	// (get) Token: 0x0600AE5A RID: 44634 RVA: 0x004104E9 File Offset: 0x0040E6E9
	// (set) Token: 0x0600AE5B RID: 44635 RVA: 0x004104F1 File Offset: 0x0040E6F1
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

	// Token: 0x17001408 RID: 5128
	// (get) Token: 0x0600AE5C RID: 44636 RVA: 0x004104FA File Offset: 0x0040E6FA
	public Vector3 CollisionVector
	{
		get
		{
			return this.m_CollisionVector;
		}
	}

	// Token: 0x17001409 RID: 5129
	// (get) Token: 0x0600AE5D RID: 44637 RVA: 0x00410502 File Offset: 0x0040E702
	// (set) Token: 0x0600AE5E RID: 44638 RVA: 0x0041050A File Offset: 0x0040E70A
	public bool HasOverheadSpace
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700140A RID: 5130
	// (get) Token: 0x0600AE5F RID: 44639 RVA: 0x00410513 File Offset: 0x0040E713
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

	// Token: 0x1700140B RID: 5131
	// (get) Token: 0x0600AE60 RID: 44640 RVA: 0x00410548 File Offset: 0x0040E748
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

	// Token: 0x1700140C RID: 5132
	// (get) Token: 0x0600AE61 RID: 44641 RVA: 0x00410574 File Offset: 0x0040E774
	// (set) Token: 0x0600AE62 RID: 44642 RVA: 0x00410587 File Offset: 0x0040E787
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

	// Token: 0x1700140D RID: 5133
	// (get) Token: 0x0600AE63 RID: 44643 RVA: 0x004105A1 File Offset: 0x0040E7A1
	public Vector3 Forward
	{
		get
		{
			return this.m_Transform.forward;
		}
	}

	// Token: 0x1700140E RID: 5134
	// (get) Token: 0x0600AE64 RID: 44644 RVA: 0x004105AE File Offset: 0x0040E7AE
	// (set) Token: 0x0600AE65 RID: 44645 RVA: 0x004105B6 File Offset: 0x0040E7B6
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

	// Token: 0x1700140F RID: 5135
	// (get) Token: 0x0600AE66 RID: 44646 RVA: 0x004105D0 File Offset: 0x0040E7D0
	// (set) Token: 0x0600AE67 RID: 44647 RVA: 0x004105D8 File Offset: 0x0040E7D8
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

	// Token: 0x17001410 RID: 5136
	// (get) Token: 0x0600AE68 RID: 44648 RVA: 0x004105E1 File Offset: 0x0040E7E1
	// (set) Token: 0x0600AE69 RID: 44649 RVA: 0x004105E9 File Offset: 0x0040E7E9
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

	// Token: 0x17001411 RID: 5137
	// (get) Token: 0x0600AE6A RID: 44650 RVA: 0x004105F2 File Offset: 0x0040E7F2
	public float CurrentCameraDistance
	{
		get
		{
			return Mathf.Min(this.current3rdPersonOffset.z, this.CameraCollisionDistance);
		}
	}

	// Token: 0x17001412 RID: 5138
	// (get) Token: 0x0600AE6B RID: 44651 RVA: 0x0041060A File Offset: 0x0040E80A
	// (set) Token: 0x0600AE6C RID: 44652 RVA: 0x00410612 File Offset: 0x0040E812
	public float CameraCollisionDistance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17001413 RID: 5139
	// (get) Token: 0x0600AE6D RID: 44653 RVA: 0x0041061B File Offset: 0x0040E81B
	// (set) Token: 0x0600AE6E RID: 44654 RVA: 0x00410624 File Offset: 0x0040E824
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

	// Token: 0x17001414 RID: 5140
	// (get) Token: 0x0600AE6F RID: 44655 RVA: 0x0010B91E File Offset: 0x00109B1E
	public virtual Transform OnValue_CameraTransform
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return base.transform;
		}
	}

	// Token: 0x0600AE70 RID: 44656 RVA: 0x004107D0 File Offset: 0x0040E9D0
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

	// Token: 0x0600AE71 RID: 44657 RVA: 0x004108DC File Offset: 0x0040EADC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		base.OnEnable();
		vp_TargetEvent<float>.Register(base.Parent, "CameraBombShake", new Action<float>(this.OnMessage_CameraBombShake));
		vp_TargetEvent<float>.Register(base.Parent, "CameraGroundStomp", new Action<float>(this.OnMessage_CameraGroundStomp));
	}

	// Token: 0x0600AE72 RID: 44658 RVA: 0x0041092C File Offset: 0x0040EB2C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
		vp_TargetEvent<float>.Unregister(base.Parent, "CameraBombShake", new Action<float>(this.OnMessage_CameraBombShake));
		vp_TargetEvent<float>.Unregister(base.Parent, "CameraGroundStomp", new Action<float>(this.OnMessage_CameraGroundStomp));
	}

	// Token: 0x0600AE73 RID: 44659 RVA: 0x00410979 File Offset: 0x0040EB79
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.Refresh();
		this.SnapSprings();
		this.SnapZoom();
	}

	// Token: 0x0600AE74 RID: 44660 RVA: 0x00410993 File Offset: 0x0040EB93
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x0600AE75 RID: 44661 RVA: 0x0041099C File Offset: 0x0040EB9C
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

	// Token: 0x0600AE76 RID: 44662 RVA: 0x00410B10 File Offset: 0x0040ED10
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

	// Token: 0x0600AE77 RID: 44663 RVA: 0x00410BB8 File Offset: 0x0040EDB8
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

	// Token: 0x0600AE78 RID: 44664 RVA: 0x00410EC0 File Offset: 0x0040F0C0
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

	// Token: 0x0600AE79 RID: 44665 RVA: 0x00411277 File Offset: 0x0040F477
	public void AlignCharacterToCamera()
	{
		this.targetRotation3P = base.Transform.eulerAngles.y;
	}

	// Token: 0x0600AE7A RID: 44666 RVA: 0x00411290 File Offset: 0x0040F490
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

	// Token: 0x0600AE7B RID: 44667 RVA: 0x004116DC File Offset: 0x0040F8DC
	public virtual void AddForce(Vector3 force)
	{
		this.m_PositionSpring.AddForce(force);
	}

	// Token: 0x0600AE7C RID: 44668 RVA: 0x004116EA File Offset: 0x0040F8EA
	public virtual void AddForce(float x, float y, float z)
	{
		this.AddForce(new Vector3(x, y, z));
	}

	// Token: 0x0600AE7D RID: 44669 RVA: 0x004116FA File Offset: 0x0040F8FA
	public virtual void AddForce2(Vector3 force)
	{
		this.m_PositionSpring2.AddForce(force);
	}

	// Token: 0x0600AE7E RID: 44670 RVA: 0x00411708 File Offset: 0x0040F908
	public void AddForce2(float x, float y, float z)
	{
		this.AddForce2(new Vector3(x, y, z));
	}

	// Token: 0x0600AE7F RID: 44671 RVA: 0x00411718 File Offset: 0x0040F918
	public virtual void AddRollForce(float force)
	{
		this.m_RotationSpring.AddForce(Vector3.forward * force);
	}

	// Token: 0x0600AE80 RID: 44672 RVA: 0x00411730 File Offset: 0x0040F930
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

	// Token: 0x0600AE81 RID: 44673 RVA: 0x004118D0 File Offset: 0x0040FAD0
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

	// Token: 0x0600AE82 RID: 44674 RVA: 0x00411950 File Offset: 0x0040FB50
	public void RefreshZoom()
	{
		float num = 1f - (this.m_FinalZoomTime - Time.time) / this.RenderingZoomDamping;
		base.gameObject.GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(base.gameObject.GetComponent<Camera>().fieldOfView, (float)Constants.cDefaultCameraFieldOfView + this.ZoomOffset, num);
	}

	// Token: 0x0600AE83 RID: 44675 RVA: 0x004119AA File Offset: 0x0040FBAA
	public virtual void Zoom()
	{
		this.m_FinalZoomTime = Time.time + this.RenderingZoomDamping;
	}

	// Token: 0x0600AE84 RID: 44676 RVA: 0x004119BE File Offset: 0x0040FBBE
	public virtual void SnapZoom()
	{
		base.gameObject.GetComponent<Camera>().fieldOfView = (float)Constants.cDefaultCameraFieldOfView + this.ZoomOffset;
	}

	// Token: 0x0600AE85 RID: 44677 RVA: 0x004119E0 File Offset: 0x0040FBE0
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

	// Token: 0x0600AE86 RID: 44678 RVA: 0x00411D0C File Offset: 0x0040FF0C
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

	// Token: 0x0600AE87 RID: 44679 RVA: 0x00411FA8 File Offset: 0x004101A8
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

	// Token: 0x0600AE88 RID: 44680 RVA: 0x00411FFC File Offset: 0x004101FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateSwaying()
	{
		Vector3 vector = base.Transform.InverseTransformDirection(this.FPController.CharacterController.velocity * 0.016f) * Time.timeScale;
		this.AddRollForce(vector.x * this.RotationStrafeRoll);
	}

	// Token: 0x0600AE89 RID: 44681 RVA: 0x0041204C File Offset: 0x0041024C
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

	// Token: 0x0600AE8A RID: 44682 RVA: 0x00412144 File Offset: 0x00410344
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateSprings()
	{
		Vector3 position = base.Transform.position;
		this.m_PositionSpring.FixedUpdate();
		this.m_PositionSpring2.FixedUpdate();
		this.m_RotationSpring.FixedUpdate();
		base.Transform.position = position;
	}

	// Token: 0x0600AE8B RID: 44683 RVA: 0x0041218C File Offset: 0x0041038C
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

	// Token: 0x0600AE8C RID: 44684 RVA: 0x004121C0 File Offset: 0x004103C0
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

	// Token: 0x0600AE8D RID: 44685 RVA: 0x0041233C File Offset: 0x0041053C
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

	// Token: 0x0600AE8E RID: 44686 RVA: 0x00412400 File Offset: 0x00410600
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

	// Token: 0x0600AE8F RID: 44687 RVA: 0x0041245F File Offset: 0x0041065F
	public virtual void Stop()
	{
		this.SnapSprings();
		this.SnapZoom();
		this.Refresh();
	}

	// Token: 0x0600AE90 RID: 44688 RVA: 0x00412473 File Offset: 0x00410673
	public virtual void SetRotation(Vector2 eulerAngles, bool stopZoomAndSprings)
	{
		this.Angle = eulerAngles;
		if (stopZoomAndSprings)
		{
			this.Stop();
		}
	}

	// Token: 0x0600AE91 RID: 44689 RVA: 0x00412485 File Offset: 0x00410685
	public virtual void SetRotation(Vector2 eulerAngles)
	{
		this.Angle = eulerAngles;
		this.Stop();
	}

	// Token: 0x0600AE92 RID: 44690 RVA: 0x00412494 File Offset: 0x00410694
	public virtual void SetRotation(Vector2 eulerAngles, bool stopZoomAndSprings, bool obsolete)
	{
		this.SetRotation(eulerAngles, stopZoomAndSprings);
	}

	// Token: 0x0600AE93 RID: 44691 RVA: 0x004124A0 File Offset: 0x004106A0
	public Vector3 GetLookPoint()
	{
		if (!this.Player.IsFirstPerson.Get() && Physics.Linecast(base.Transform.position, base.Transform.position + base.Transform.forward * 1000f, out this.m_LookPointHit, 1082195968) && !this.m_LookPointHit.collider.isTrigger && base.Root.InverseTransformPoint(this.m_LookPointHit.point).z > 0f)
		{
			return this.m_LookPointHit.point;
		}
		return base.Transform.position + base.Transform.forward * 1000f;
	}

	// Token: 0x17001415 RID: 5141
	// (get) Token: 0x0600AE94 RID: 44692 RVA: 0x0041256E File Offset: 0x0041076E
	public virtual Vector3 OnValue_LookPoint
	{
		get
		{
			return this.LookPoint;
		}
	}

	// Token: 0x17001416 RID: 5142
	// (get) Token: 0x0600AE95 RID: 44693 RVA: 0x00412578 File Offset: 0x00410778
	public virtual Vector3 OnValue_CameraLookDirection
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return (this.Player.LookPoint.Get() - base.Transform.position).normalized;
		}
	}

	// Token: 0x0600AE96 RID: 44694 RVA: 0x004125B4 File Offset: 0x004107B4
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

	// Token: 0x0600AE97 RID: 44695 RVA: 0x00412668 File Offset: 0x00410868
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_HeadImpact(float impact)
	{
		if (this.m_RotationSpring != null && Mathf.Abs(this.m_RotationSpring.State.z) < 30f)
		{
			this.m_RotationSpring.AddForce(Vector3.forward * (impact * 20f) * Time.timeScale);
		}
	}

	// Token: 0x0600AE98 RID: 44696 RVA: 0x004126BF File Offset: 0x004108BF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraGroundStomp(float impact)
	{
		this.AddForce2(new Vector3(0f, -1f, 0f) * impact);
	}

	// Token: 0x0600AE99 RID: 44697 RVA: 0x004126E1 File Offset: 0x004108E1
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraBombShake(float impact)
	{
		this.DoBomb(new Vector3(1f, -10f, 1f) * impact, 1f, 2f);
	}

	// Token: 0x0600AE9A RID: 44698 RVA: 0x0041270D File Offset: 0x0041090D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Zoom()
	{
		if (this.Player == null)
		{
			return;
		}
		this.Player.Run.Stop(0f);
	}

	// Token: 0x0600AE9B RID: 44699 RVA: 0x00412733 File Offset: 0x00410933
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Run()
	{
		return this.Player == null || !this.Player.Zoom.Active;
	}

	// Token: 0x17001417 RID: 5143
	// (get) Token: 0x0600AE9C RID: 44700 RVA: 0x0041275A File Offset: 0x0041095A
	// (set) Token: 0x0600AE9D RID: 44701 RVA: 0x00412762 File Offset: 0x00410962
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

	// Token: 0x0600AE9E RID: 44702 RVA: 0x0041276B File Offset: 0x0041096B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_Stop()
	{
		this.Stop();
	}

	// Token: 0x0600AE9F RID: 44703 RVA: 0x00412773 File Offset: 0x00410973
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Dead()
	{
		if (this.Player.IsFirstPerson.Get())
		{
			return;
		}
		this.PositionOnDeath = base.Transform.position - this.m_Final3rdPersonCameraOffset;
	}

	// Token: 0x0600AEA0 RID: 44704 RVA: 0x004127A9 File Offset: 0x004109A9
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		this.PositionOnDeath = Vector3.zero;
		this.m_Current3rdPersonBlend = 0f;
	}

	// Token: 0x17001418 RID: 5144
	// (get) Token: 0x0600AEA1 RID: 44705 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool OnValue_IsLocal
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x0600AEA2 RID: 44706 RVA: 0x004127C1 File Offset: 0x004109C1
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraToggle3rdPerson()
	{
		this.m_Player.IsFirstPerson.Set(!this.m_Player.IsFirstPerson.Get());
	}

	// Token: 0x04008225 RID: 33317
	public Vector3 DrivingPosition;

	// Token: 0x04008226 RID: 33318
	public vp_FPController FPController;

	// Token: 0x04008227 RID: 33319
	public float RenderingFieldOfView = 60f;

	// Token: 0x04008228 RID: 33320
	public float RenderingZoomDamping = 0.2f;

	// Token: 0x04008229 RID: 33321
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FinalZoomTime;

	// Token: 0x0400822A RID: 33322
	public float ZoomOffset;

	// Token: 0x0400822B RID: 33323
	public Vector3 PositionOffset = new Vector3(0f, 1.75f, 0.1f);

	// Token: 0x0400822C RID: 33324
	public Vector3 AimingPositionOffset = Vector3.zero;

	// Token: 0x0400822D RID: 33325
	public float PositionGroundLimit = 0.1f;

	// Token: 0x0400822E RID: 33326
	public float PositionSpringStiffness = 0.01f;

	// Token: 0x0400822F RID: 33327
	public float PositionSpringDamping = 0.25f;

	// Token: 0x04008230 RID: 33328
	public float PositionSpring2Stiffness = 0.95f;

	// Token: 0x04008231 RID: 33329
	public float PositionSpring2Damping = 0.25f;

	// Token: 0x04008232 RID: 33330
	public float PositionKneeling = 0.025f;

	// Token: 0x04008233 RID: 33331
	public int PositionKneelingSoftness = 1;

	// Token: 0x04008234 RID: 33332
	public float PositionEarthQuakeFactor = 1f;

	// Token: 0x04008235 RID: 33333
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_PositionSpring;

	// Token: 0x04008236 RID: 33334
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_PositionSpring2;

	// Token: 0x04008237 RID: 33335
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_DrawCameraCollisionDebugLine;

	// Token: 0x04008238 RID: 33336
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 PositionOnDeath = Vector3.zero;

	// Token: 0x04008239 RID: 33337
	public Vector2 RotationPitchLimit = new Vector2(90f, -90f);

	// Token: 0x0400823A RID: 33338
	public Vector2 RotationYawLimit = new Vector2(-360f, 360f);

	// Token: 0x0400823B RID: 33339
	public float RotationSpringStiffness = 0.01f;

	// Token: 0x0400823C RID: 33340
	public float RotationSpringDamping = 0.25f;

	// Token: 0x0400823D RID: 33341
	public float RotationKneeling = 0.025f;

	// Token: 0x0400823E RID: 33342
	public int RotationKneelingSoftness = 1;

	// Token: 0x0400823F RID: 33343
	public float RotationStrafeRoll = 0.01f;

	// Token: 0x04008240 RID: 33344
	public float RotationEarthQuakeFactor;

	// Token: 0x04008241 RID: 33345
	public Vector3 LookPoint = Vector3.zero;

	// Token: 0x04008242 RID: 33346
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_Pitch;

	// Token: 0x04008243 RID: 33347
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_Yaw;

	// Token: 0x04008244 RID: 33348
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_RotationSpring;

	// Token: 0x04008245 RID: 33349
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_LookPointHit;

	// Token: 0x04008246 RID: 33350
	public Vector3 Position3rdPersonOffset = new Vector3(0.5f, 0.1f, 0.75f);

	// Token: 0x04008247 RID: 33351
	public bool Locked3rdPerson;

	// Token: 0x04008248 RID: 33352
	public float m_Current3rdPersonBlend;

	// Token: 0x04008249 RID: 33353
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Final3rdPersonCameraOffset = Vector3.zero;

	// Token: 0x0400824A RID: 33354
	public float ShakeSpeed;

	// Token: 0x0400824B RID: 33355
	public Vector3 ShakeAmplitude = new Vector3(10f, 10f, 0f);

	// Token: 0x0400824C RID: 33356
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Shake = Vector3.zero;

	// Token: 0x0400824D RID: 33357
	public float ShakeSpeed2;

	// Token: 0x0400824E RID: 33358
	public Vector3 ShakeAmplitude2 = new Vector3(10f, 10f, 0f);

	// Token: 0x0400824F RID: 33359
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Shake2 = Vector3.zero;

	// Token: 0x04008250 RID: 33360
	public Vector4 BobRate = new Vector4(0f, 1.4f, 0f, 0.7f);

	// Token: 0x04008251 RID: 33361
	public Vector4 BobAmplitude = new Vector4(0f, 0.25f, 0f, 0.5f);

	// Token: 0x04008252 RID: 33362
	public float BobInputVelocityScale = 1f;

	// Token: 0x04008253 RID: 33363
	public float BobMaxInputVelocity = 100f;

	// Token: 0x04008254 RID: 33364
	public bool BobRequireGroundContact = true;

	// Token: 0x04008255 RID: 33365
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastBobSpeed;

	// Token: 0x04008256 RID: 33366
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector4 m_CurrentBobAmp = Vector4.zero;

	// Token: 0x04008257 RID: 33367
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector4 m_CurrentBobVal = Vector4.zero;

	// Token: 0x04008258 RID: 33368
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_BobSpeed;

	// Token: 0x04008259 RID: 33369
	public vp_FPCamera.BobStepDelegate BobStepCallback;

	// Token: 0x0400825A RID: 33370
	public float BobStepThreshold = 10f;

	// Token: 0x0400825B RID: 33371
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastUpBob;

	// Token: 0x0400825C RID: 33372
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_BobWasElevating;

	// Token: 0x0400825D RID: 33373
	public bool HasCollision = true;

	// Token: 0x0400825E RID: 33374
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CollisionVector = Vector3.zero;

	// Token: 0x0400825F RID: 33375
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CameraCollisionStartPos = Vector3.zero;

	// Token: 0x04008260 RID: 33376
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CameraCollisionEndPos = Vector3.zero;

	// Token: 0x04008261 RID: 33377
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_CameraHit;

	// Token: 0x04008262 RID: 33378
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> cameraCollisionEndPosList = new List<Vector3>();

	// Token: 0x04008264 RID: 33380
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x04008265 RID: 33381
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Rigidbody m_FirstRigidbody;

	// Token: 0x04008266 RID: 33382
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cPlayerRotationSpeed = 16f;

	// Token: 0x04008267 RID: 33383
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cCameraOffsetSpeed = 10f;

	// Token: 0x04008268 RID: 33384
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 current3rdPersonOffset = Vector3.zero;

	// Token: 0x04008269 RID: 33385
	public float yaw3P;

	// Token: 0x0400826A RID: 33386
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float pitch3P;

	// Token: 0x0400826B RID: 33387
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetRotation3P;

	// Token: 0x0400826C RID: 33388
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool _cameraRelative3PMovement;

	// Token: 0x0400826D RID: 33389
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 shake3P;

	// Token: 0x0400826F RID: 33391
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Camera thisCamera;

	// Token: 0x04008270 RID: 33392
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsFirstPerson = true;

	// Token: 0x04008271 RID: 33393
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool hasLateUpdateRan;

	// Token: 0x04008272 RID: 33394
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector3 lastPos;

	// Token: 0x020015D9 RID: 5593
	// (Invoke) Token: 0x0600AEA5 RID: 44709
	public delegate void BobStepDelegate();
}
