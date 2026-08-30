using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001600 RID: 5632
[Preserve]
public class vp_FPWeapon : vp_Weapon
{
	// Token: 0x17001487 RID: 5255
	// (get) Token: 0x0600B07A RID: 45178 RVA: 0x0041CAD7 File Offset: 0x0041ACD7
	public CameraMatrixOverride CameraMatrixOverride
	{
		get
		{
			return this.m_CameraMatrixOverride;
		}
	}

	// Token: 0x17001488 RID: 5256
	// (get) Token: 0x0600B07B RID: 45179 RVA: 0x0041CADF File Offset: 0x0041ACDF
	public GameObject WeaponModel
	{
		get
		{
			return this.m_WeaponModel;
		}
	}

	// Token: 0x17001489 RID: 5257
	// (get) Token: 0x0600B07C RID: 45180 RVA: 0x0041CAE7 File Offset: 0x0041ACE7
	public Vector3 DefaultPosition
	{
		get
		{
			return (Vector3)base.DefaultState.Preset.GetFieldValue("PositionOffset");
		}
	}

	// Token: 0x1700148A RID: 5258
	// (get) Token: 0x0600B07D RID: 45181 RVA: 0x0041CB03 File Offset: 0x0041AD03
	public Vector3 DefaultRotation
	{
		get
		{
			return (Vector3)base.DefaultState.Preset.GetFieldValue("RotationOffset");
		}
	}

	// Token: 0x1700148B RID: 5259
	// (get) Token: 0x0600B07E RID: 45182 RVA: 0x0041CB1F File Offset: 0x0041AD1F
	// (set) Token: 0x0600B07F RID: 45183 RVA: 0x0041CB27 File Offset: 0x0041AD27
	public bool DrawRetractionDebugLine
	{
		get
		{
			return this.m_DrawRetractionDebugLine;
		}
		set
		{
			this.m_DrawRetractionDebugLine = value;
		}
	}

	// Token: 0x1700148C RID: 5260
	// (get) Token: 0x0600B080 RID: 45184 RVA: 0x0041CB30 File Offset: 0x0041AD30
	public vp_FPPlayerEventHandler FPPlayer
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.m_FPPlayer == null && base.EventHandler != null)
			{
				this.m_FPPlayer = base.EventHandler as vp_FPPlayerEventHandler;
			}
			return this.m_FPPlayer;
		}
	}

	// Token: 0x0600B081 RID: 45185 RVA: 0x0041CB68 File Offset: 0x0041AD68
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		if (base.transform.parent == null)
		{
			Debug.LogError("Error (" + ((this != null) ? this.ToString() : null) + ") Must not be placed in scene root. Disabling self.");
			vp_Utility.Activate(base.gameObject, false);
			return;
		}
		this.Controller = base.Transform.gameObject.GetComponentInParent<CharacterController>();
		if (this.Controller == null)
		{
			Debug.LogError("Error (" + ((this != null) ? this.ToString() : null) + ") Could not find CharacterController. Disabling self.");
			vp_Utility.Activate(base.gameObject, false);
			return;
		}
		base.Transform.eulerAngles = Vector3.zero;
		Transform transform = base.transform;
		while (transform != null && !transform.TryGetComponent<CameraMatrixOverride>(out this.m_CameraMatrixOverride))
		{
			this.m_CameraMatrixOverride = null;
			transform = transform.parent;
		}
		if (this.m_CameraMatrixOverride == null)
		{
			Debug.LogWarning("vp_FPWeapon could not find CameraMatrixOverride in hierarchy, some functionality will be unavailable");
		}
		if (base.GetComponent<Collider>() != null)
		{
			base.GetComponent<Collider>().enabled = false;
		}
	}

	// Token: 0x0600B082 RID: 45186 RVA: 0x0041CC80 File Offset: 0x0041AE80
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		this.InstantiateWeaponModel();
		base.Start();
		this.originalRenderingFieldOfView = this.RenderingFieldOfView;
		Transform transform = ((base.Transform.parent != null && base.Transform.parent.gameObject.name == "UFPSRoot") ? base.Transform.parent : base.Transform.FindParent("UFPSRoot"));
		if (transform == null)
		{
			transform = new GameObject("UFPSRoot").transform;
			transform.parent = base.Transform.parent;
		}
		this.m_WeaponGroup = ((transform != null) ? transform.gameObject : null);
		this.m_WeaponGroupTransform = this.m_WeaponGroup.transform;
		this.m_WeaponGroupTransform.localPosition = this.PositionOffset;
		vp_Layer.Set(this.m_WeaponGroup, 10, false);
		base.Transform.parent = this.m_WeaponGroupTransform;
		base.Transform.localPosition = Vector3.zero;
		this.m_WeaponGroupTransform.localEulerAngles = this.RotationOffset;
		this.m_Pivot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		this.m_Pivot.name = "Pivot";
		this.m_Pivot.GetComponent<Collider>().enabled = false;
		this.m_Pivot.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		this.m_Pivot.transform.parent = this.m_WeaponGroupTransform;
		this.m_Pivot.transform.localPosition = Vector3.zero;
		this.m_Pivot.layer = 10;
		vp_Utility.Activate(this.m_Pivot.gameObject, false);
		Material material = new Material(Shader.Find("Transparent/Diffuse"));
		material.color = new Color(0f, 0f, 1f, 0.5f);
		this.m_Pivot.GetComponent<Renderer>().material = material;
		this.m_PositionSpring = new vp_Spring(this.m_WeaponGroup.gameObject.transform, vp_Spring.UpdateMode.Position, true);
		this.m_PositionSpring.RestState = this.PositionOffset;
		this.m_PositionPivotSpring = new vp_Spring(base.Transform, vp_Spring.UpdateMode.Position, true);
		this.m_PositionPivotSpring.RestState = this.PositionPivot;
		this.m_PositionSpring2 = new vp_Spring(base.Transform, vp_Spring.UpdateMode.PositionAdditiveLocal, true);
		this.m_PositionSpring2.MinVelocity = 1E-05f;
		this.m_RotationSpring = new vp_Spring(this.m_WeaponGroup.gameObject.transform, vp_Spring.UpdateMode.Rotation, true);
		this.m_RotationSpring.RestState = this.RotationOffset;
		this.m_RotationPivotSpring = new vp_Spring(base.Transform, vp_Spring.UpdateMode.Rotation, true);
		this.m_RotationPivotSpring.RestState = this.RotationPivot;
		this.m_RotationSpring2 = new vp_Spring(this.m_WeaponGroup.gameObject.transform, vp_Spring.UpdateMode.RotationAdditiveLocal, true);
		this.m_RotationSpring2.MinVelocity = 1E-05f;
		this.SnapSprings();
		this.Refresh();
	}

	// Token: 0x0600B083 RID: 45187 RVA: 0x0041CF74 File Offset: 0x0041B174
	public virtual void InstantiateWeaponModel()
	{
		if (this.WeaponPrefab != null)
		{
			if (this.m_WeaponModel != null && this.m_WeaponModel != base.gameObject)
			{
				global::UnityEngine.Object.Destroy(this.m_WeaponModel);
			}
			this.m_WeaponModel = global::UnityEngine.Object.Instantiate<GameObject>(this.WeaponPrefab);
			this.m_WeaponModel.transform.parent = base.transform;
			this.m_WeaponModel.transform.localPosition = Vector3.zero;
			this.m_WeaponModel.transform.localScale = new Vector3(1f, 1f, this.RenderingZScale);
			this.m_WeaponModel.transform.localEulerAngles = Vector3.zero;
		}
		else
		{
			this.m_WeaponModel = base.gameObject;
		}
		base.CacheRenderers();
	}

	// Token: 0x0600B084 RID: 45188 RVA: 0x0041D048 File Offset: 0x0041B248
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Init()
	{
		base.Init();
		this.ScheduleAmbientAnimation();
	}

	// Token: 0x0600B085 RID: 45189 RVA: 0x0041D056 File Offset: 0x0041B256
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		if (Time.timeScale != 0f)
		{
			this.UpdateInput();
		}
	}

	// Token: 0x0600B086 RID: 45190 RVA: 0x0041D070 File Offset: 0x0041B270
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void FixedUpdate()
	{
		if (Time.timeScale != 0f)
		{
			this.UpdateZoom();
			this.UpdateSwaying();
			this.UpdateBob();
			this.UpdateEarthQuake();
			this.UpdateStep();
			this.UpdateShakes();
			this.UpdateRetraction(true);
			this.UpdateSprings();
			this.UpdateLookDown();
		}
	}

	// Token: 0x0600B087 RID: 45191 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void LateUpdate()
	{
	}

	// Token: 0x0600B088 RID: 45192 RVA: 0x0041D0C0 File Offset: 0x0041B2C0
	public virtual void AddForce(Vector3 force)
	{
		this.m_PositionSpring.AddForce(force);
	}

	// Token: 0x0600B089 RID: 45193 RVA: 0x0041D0CE File Offset: 0x0041B2CE
	public virtual void AddForce(float x, float y, float z)
	{
		this.AddForce(new Vector3(x, y, z));
	}

	// Token: 0x0600B08A RID: 45194 RVA: 0x0041D0DE File Offset: 0x0041B2DE
	public virtual void AddForce(Vector3 positional, Vector3 angular)
	{
		this.m_PositionSpring.AddForce(positional);
		this.m_RotationSpring.AddForce(angular);
	}

	// Token: 0x0600B08B RID: 45195 RVA: 0x0041D0F8 File Offset: 0x0041B2F8
	public virtual void AddForce(float xPos, float yPos, float zPos, float xRot, float yRot, float zRot)
	{
		this.AddForce(new Vector3(xPos, yPos, zPos), new Vector3(xRot, yRot, zRot));
	}

	// Token: 0x0600B08C RID: 45196 RVA: 0x0041D113 File Offset: 0x0041B313
	public virtual void AddSoftForce(Vector3 force, int frames)
	{
		this.m_PositionSpring.AddSoftForce(force, (float)frames);
	}

	// Token: 0x0600B08D RID: 45197 RVA: 0x0041D123 File Offset: 0x0041B323
	public virtual void AddSoftForce(float x, float y, float z, int frames)
	{
		this.AddSoftForce(new Vector3(x, y, z), frames);
	}

	// Token: 0x0600B08E RID: 45198 RVA: 0x0041D135 File Offset: 0x0041B335
	public virtual void AddSoftForce(Vector3 positional, Vector3 angular, int frames)
	{
		this.m_PositionSpring.AddSoftForce(positional, (float)frames);
		this.m_RotationSpring.AddSoftForce(angular, (float)frames);
	}

	// Token: 0x0600B08F RID: 45199 RVA: 0x0041D153 File Offset: 0x0041B353
	public virtual void AddSoftForce(float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, int frames)
	{
		this.AddSoftForce(new Vector3(xPos, yPos, zPos), new Vector3(xRot, yRot, zRot), frames);
	}

	// Token: 0x0600B090 RID: 45200 RVA: 0x0041D170 File Offset: 0x0041B370
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateInput()
	{
		if (base.Player.Dead.Active)
		{
			return;
		}
		this.m_LookInput = this.FPPlayer.InputRawLook.Get() / base.Delta * Time.timeScale * Time.timeScale;
		this.m_LookInput *= this.RotationInputVelocityScale;
		this.m_LookInput = Vector3.Min(this.m_LookInput, Vector3.one * this.RotationMaxInputVelocity);
		this.m_LookInput = Vector3.Max(this.m_LookInput, Vector3.one * -this.RotationMaxInputVelocity);
	}

	// Token: 0x0600B091 RID: 45201 RVA: 0x0041D238 File Offset: 0x0041B438
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateZoom()
	{
		if (this.m_FinalZoomTime <= Time.time)
		{
			return;
		}
		if (!this.m_Wielded)
		{
			return;
		}
		this.RenderingZoomDamping = Mathf.Max(this.RenderingZoomDamping, 0.01f);
		float num = 1f - (this.m_FinalZoomTime - Time.time) / this.RenderingZoomDamping;
		if (this.m_CameraMatrixOverride != null)
		{
			this.m_CameraMatrixOverride.fov = Mathf.SmoothStep(this.m_CameraMatrixOverride.fov, this.RenderingFieldOfView, num * 15f);
		}
	}

	// Token: 0x0600B092 RID: 45202 RVA: 0x0041D2C2 File Offset: 0x0041B4C2
	public virtual void Zoom()
	{
		this.m_FinalZoomTime = Time.time + this.RenderingZoomDamping;
	}

	// Token: 0x0600B093 RID: 45203 RVA: 0x0041D2D6 File Offset: 0x0041B4D6
	public virtual void SnapZoom()
	{
		if (this.m_CameraMatrixOverride != null)
		{
			this.m_CameraMatrixOverride.fov = this.RenderingFieldOfView;
		}
	}

	// Token: 0x0600B094 RID: 45204 RVA: 0x0041D2F8 File Offset: 0x0041B4F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateShakes()
	{
		if (this.ShakeSpeed != 0f)
		{
			this.m_Shake = Vector3.Scale(vp_SmoothRandom.GetVector3Centered(this.ShakeSpeed), this.ShakeAmplitude);
			this.m_RotationSpring.AddForce(this.m_Shake * Time.timeScale);
		}
	}

	// Token: 0x0600B095 RID: 45205 RVA: 0x0041D34C File Offset: 0x0041B54C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateRetraction(bool firstIteration = true)
	{
		if (this.RetractionDistance == 0f)
		{
			return;
		}
		Vector3 vector = this.WeaponModel.transform.TransformPoint(this.RetractionOffset);
		Vector3 vector2 = vector + this.WeaponModel.transform.forward * this.RetractionDistance;
		RaycastHit raycastHit;
		if (Physics.Linecast(vector, vector2, out raycastHit, 1084850184) && !raycastHit.collider.isTrigger)
		{
			this.WeaponModel.transform.position = raycastHit.point - (raycastHit.point - vector).normalized * (this.RetractionDistance * 0.99f);
			this.WeaponModel.transform.localPosition = Vector3.forward * Mathf.Min(this.WeaponModel.transform.localPosition.z, 0f);
			return;
		}
		if (firstIteration && this.WeaponModel.transform.localPosition != Vector3.zero && this.WeaponModel != base.gameObject)
		{
			this.WeaponModel.transform.localPosition = Vector3.forward * Mathf.SmoothStep(this.WeaponModel.transform.localPosition.z, 0f, this.RetractionRelaxSpeed * Time.timeScale);
			this.UpdateRetraction(false);
		}
	}

	// Token: 0x0600B096 RID: 45206 RVA: 0x0041D4C0 File Offset: 0x0041B6C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateBob()
	{
		if (this.BobAmplitude == Vector4.zero || this.BobRate == Vector4.zero)
		{
			return;
		}
		this.m_BobSpeed = ((this.BobRequireGroundContact && !this.Controller.isGrounded) ? 0f : this.Controller.velocity.sqrMagnitude);
		this.m_BobSpeed = Mathf.Min(this.m_BobSpeed * this.BobInputVelocityScale, this.BobMaxInputVelocity);
		this.m_BobSpeed = Mathf.Round(this.m_BobSpeed * 1000f) / 1000f;
		if (this.m_BobSpeed == 0f)
		{
			this.m_BobSpeed = Mathf.Min(this.m_LastBobSpeed * 0.93f, this.BobMaxInputVelocity);
		}
		this.m_CurrentBobAmp.x = this.m_BobSpeed * (this.BobAmplitude.x * -0.0001f);
		this.m_CurrentBobVal.x = Mathf.Cos(Time.time * (this.BobRate.x * 10f)) * this.m_CurrentBobAmp.x;
		this.m_CurrentBobAmp.y = this.m_BobSpeed * (this.BobAmplitude.y * 0.0001f);
		this.m_CurrentBobVal.y = Mathf.Cos(Time.time * (this.BobRate.y * 10f)) * this.m_CurrentBobAmp.y;
		this.m_CurrentBobAmp.z = this.m_BobSpeed * (this.BobAmplitude.z * 0.0001f);
		this.m_CurrentBobVal.z = Mathf.Cos(Time.time * (this.BobRate.z * 10f)) * this.m_CurrentBobAmp.z;
		this.m_CurrentBobAmp.w = this.m_BobSpeed * (this.BobAmplitude.w * 0.0001f);
		this.m_CurrentBobVal.w = Mathf.Cos(Time.time * (this.BobRate.w * 10f)) * this.m_CurrentBobAmp.w;
		this.m_RotationSpring.AddForce(this.m_CurrentBobVal * Time.timeScale);
		this.m_PositionSpring.AddForce(Vector3.forward * this.m_CurrentBobVal.w * Time.timeScale);
		this.m_LastBobSpeed = this.m_BobSpeed;
	}

	// Token: 0x0600B097 RID: 45207 RVA: 0x0041D73C File Offset: 0x0041B93C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateEarthQuake()
	{
		if (this.FPPlayer == null)
		{
			return;
		}
		if (!this.FPPlayer.CameraEarthQuake.Active)
		{
			return;
		}
		if (!this.Controller.isGrounded)
		{
			return;
		}
		Vector3 vector = this.FPPlayer.CameraEarthQuakeForce.Get();
		this.AddForce(new Vector3(0f, 0f, -vector.z * 0.015f), new Vector3(vector.y * 2f, -vector.x, vector.x * 2f));
	}

	// Token: 0x0600B098 RID: 45208 RVA: 0x0041D7D8 File Offset: 0x0041B9D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateSprings()
	{
		this.m_PositionSpring.FixedUpdate();
		this.m_PositionPivotSpring.FixedUpdate();
		this.m_RotationPivotSpring.FixedUpdate();
		this.m_RotationSpring.FixedUpdate();
		this.m_PositionSpring2.FixedUpdate();
		this.m_RotationSpring2.FixedUpdate();
	}

	// Token: 0x0600B099 RID: 45209 RVA: 0x0041D828 File Offset: 0x0041BA28
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateLookDown()
	{
		if (!this.LookDownActive)
		{
			return;
		}
		if (this.FPPlayer.Rotation.Get().x < 0f && this.m_LookDownPitch == 0f && this.m_LookDownYaw == 0f)
		{
			return;
		}
		if (this.FPPlayer.Rotation.Get().x > 0f)
		{
			this.m_LookDownPitch = Mathf.Lerp(this.m_LookDownPitch, vp_MathUtility.SnapToZero(Mathf.Max(0f, this.FPPlayer.Rotation.Get().x / 90f), 0.0001f), Time.deltaTime * 2f);
			this.m_LookDownYaw = Mathf.Lerp(this.m_LookDownYaw, vp_MathUtility.SnapToZero(Mathf.DeltaAngle(this.FPPlayer.Rotation.Get().y, this.FPPlayer.BodyYaw.Get()), 0.0001f) / 90f * vp_MathUtility.SnapToZero(Mathf.Max(0f, (this.FPPlayer.Rotation.Get().x - this.LookDownYawLimit) / (90f - this.LookDownYawLimit)), 0.0001f), Time.deltaTime * 2f);
		}
		else
		{
			this.m_LookDownPitch *= 0.9f;
			this.m_LookDownYaw *= 0.9f;
			if (this.m_LookDownPitch < 0.01f)
			{
				this.m_LookDownPitch = 0f;
			}
			if (this.m_LookDownYaw < 0.01f)
			{
				this.m_LookDownYaw = 0f;
			}
		}
		this.m_WeaponGroupTransform.localPosition = vp_MathUtility.NaNSafeVector3(Vector3.Lerp(this.m_WeaponGroupTransform.localPosition, this.LookDownPositionOffsetMiddle, this.m_LookDownCurve.Evaluate(this.m_LookDownPitch)), default(Vector3));
		this.m_WeaponGroupTransform.localRotation = vp_MathUtility.NaNSafeQuaternion(Quaternion.Slerp(this.m_WeaponGroupTransform.localRotation, Quaternion.Euler(this.LookDownRotationOffsetMiddle), this.m_LookDownPitch), default(Quaternion));
		if (this.m_LookDownYaw > 0f)
		{
			this.m_WeaponGroupTransform.localPosition = vp_MathUtility.NaNSafeVector3(Vector3.Lerp(this.m_WeaponGroupTransform.localPosition, this.LookDownPositionOffsetLeft, Mathf.SmoothStep(0f, 1f, this.m_LookDownYaw)), default(Vector3));
			this.m_WeaponGroupTransform.localRotation = vp_MathUtility.NaNSafeQuaternion(Quaternion.Slerp(this.m_WeaponGroupTransform.localRotation, Quaternion.Euler(this.LookDownRotationOffsetLeft), this.m_LookDownYaw), default(Quaternion));
		}
		else
		{
			this.m_WeaponGroupTransform.localPosition = vp_MathUtility.NaNSafeVector3(Vector3.Lerp(this.m_WeaponGroupTransform.localPosition, this.LookDownPositionOffsetRight, Mathf.SmoothStep(0f, 1f, -this.m_LookDownYaw)), default(Vector3));
			this.m_WeaponGroupTransform.localRotation = vp_MathUtility.NaNSafeQuaternion(Quaternion.Slerp(this.m_WeaponGroupTransform.localRotation, Quaternion.Euler(this.LookDownRotationOffsetRight), -this.m_LookDownYaw), default(Quaternion));
		}
		this.m_CurrentPosRestState = Vector3.Lerp(this.m_CurrentPosRestState, this.m_PositionSpring.RestState, Time.fixedDeltaTime);
		this.m_CurrentRotRestState = Vector3.Lerp(this.m_CurrentRotRestState, this.m_RotationSpring.RestState, Time.fixedDeltaTime);
		this.m_WeaponGroupTransform.localPosition += vp_MathUtility.NaNSafeVector3((this.m_PositionSpring.State - this.m_CurrentPosRestState) * (this.m_LookDownPitch * this.LookDownPositionSpringPower), default(Vector3));
		this.m_WeaponGroupTransform.localEulerAngles -= vp_MathUtility.NaNSafeVector3(new Vector3(Mathf.DeltaAngle(this.m_RotationSpring.State.x, this.m_CurrentRotRestState.x), Mathf.DeltaAngle(this.m_RotationSpring.State.y, this.m_CurrentRotRestState.y), Mathf.DeltaAngle(this.m_RotationSpring.State.z, this.m_CurrentRotRestState.z)) * (this.m_LookDownPitch * this.LookDownRotationSpringPower), default(Vector3));
	}

	// Token: 0x0600B09A RID: 45210 RVA: 0x0041DCA0 File Offset: 0x0041BEA0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateStep()
	{
		if (this.StepMinVelocity <= 0f || (this.BobRequireGroundContact && !this.Controller.isGrounded) || this.Controller.velocity.sqrMagnitude < this.StepMinVelocity)
		{
			return;
		}
		bool flag = this.m_LastUpBob < this.m_CurrentBobVal.x;
		this.m_LastUpBob = this.m_CurrentBobVal.x;
		if (flag && !this.m_BobWasElevating)
		{
			if (Mathf.Cos(Time.time * (this.BobRate.x * 5f)) > 0f)
			{
				this.m_PosStep = this.StepPositionForce - this.StepPositionForce * this.StepPositionBalance;
				this.m_RotStep = this.StepRotationForce - this.StepPositionForce * this.StepRotationBalance;
			}
			else
			{
				this.m_PosStep = this.StepPositionForce + this.StepPositionForce * this.StepPositionBalance;
				this.m_RotStep = Vector3.Scale(this.StepRotationForce - this.StepPositionForce * this.StepRotationBalance, -Vector3.one + Vector3.right * 2f);
			}
			this.AddSoftForce(this.m_PosStep * this.StepForceScale, this.m_RotStep * this.StepForceScale, this.StepSoftness);
		}
		this.m_BobWasElevating = flag;
	}

	// Token: 0x0600B09B RID: 45211 RVA: 0x0041DE2C File Offset: 0x0041C02C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateSwaying()
	{
		this.m_SwayVel = this.Controller.velocity * this.PositionInputVelocityScale;
		this.m_SwayVel = Vector3.Min(this.m_SwayVel, Vector3.one * this.PositionMaxInputVelocity);
		this.m_SwayVel = Vector3.Max(this.m_SwayVel, Vector3.one * -this.PositionMaxInputVelocity);
		this.m_SwayVel *= Time.timeScale;
		Vector3 vector = base.Transform.InverseTransformDirection(this.m_SwayVel / 60f);
		this.m_RotationSpring.AddForce(new Vector3(this.m_LookInput.y * (this.RotationLookSway.x * 0.025f), this.m_LookInput.x * (this.RotationLookSway.y * -0.025f), this.m_LookInput.x * (this.RotationLookSway.z * -0.025f)));
		this.m_FallSway = this.RotationFallSway * (this.m_SwayVel.y * 0.005f);
		if (this.Controller.isGrounded)
		{
			this.m_FallSway *= this.RotationSlopeSway;
		}
		this.m_FallSway.z = Mathf.Max(0f, this.m_FallSway.z);
		this.m_RotationSpring.AddForce(this.m_FallSway);
		this.m_FallSwaySpringLimit.x = Mathf.Abs(this.RotationFallSwayAbsoluteSpringLimit.x);
		this.m_FallSwaySpringLimit.y = Mathf.Abs(this.RotationFallSwayAbsoluteSpringLimit.y);
		this.m_FallSwaySpringLimit.z = Mathf.Abs(this.RotationFallSwayAbsoluteSpringLimit.z);
		if (this.m_FallSwaySpringLimit.x > 0f)
		{
			this.m_RotationSpring.State.x = Utils.FastClamp(this.m_RotationSpring.State.x, -this.m_FallSwaySpringLimit.x, this.m_FallSwaySpringLimit.x);
		}
		if (this.m_FallSwaySpringLimit.y > 0f)
		{
			this.m_RotationSpring.State.y = Utils.FastClamp(this.m_RotationSpring.State.y, -this.m_FallSwaySpringLimit.y, this.m_FallSwaySpringLimit.y);
		}
		if (this.m_FallSwaySpringLimit.z > 0f)
		{
			this.m_RotationSpring.State.z = Utils.FastClamp(this.m_RotationSpring.State.z, -this.m_FallSwaySpringLimit.z, this.m_FallSwaySpringLimit.z);
		}
		this.m_PositionSpring.AddForce(Vector3.forward * -Mathf.Abs(this.m_SwayVel.y * (this.PositionFallRetract * 2.5E-05f)));
		this.m_PositionSpring.AddForce(new Vector3(vector.x * (this.PositionWalkSlide.x * 0.0016f), -Mathf.Abs(vector.x * (this.PositionWalkSlide.y * 0.0016f)), -vector.z * (this.PositionWalkSlide.z * 0.0016f)));
		this.m_RotationSpring.AddForce(new Vector3(-Mathf.Abs(vector.x * (this.RotationStrafeSway.x * 0.16f)), -(vector.x * (this.RotationStrafeSway.y * 0.16f)), vector.x * (this.RotationStrafeSway.z * 0.16f)));
	}

	// Token: 0x0600B09C RID: 45212 RVA: 0x0041E1D8 File Offset: 0x0041C3D8
	public virtual void ResetSprings(float positionReset, float rotationReset, float positionPauseTime = 0f, float rotationPauseTime = 0f)
	{
		this.m_PositionSpring.State = Vector3.Lerp(this.m_PositionSpring.State, this.m_PositionSpring.RestState, positionReset);
		this.m_RotationSpring.State = Vector3.Lerp(this.m_RotationSpring.State, this.m_RotationSpring.RestState, rotationReset);
		this.m_PositionPivotSpring.State = Vector3.Lerp(this.m_PositionPivotSpring.State, this.m_PositionPivotSpring.RestState, positionReset);
		this.m_RotationPivotSpring.State = Vector3.Lerp(this.m_RotationPivotSpring.State, this.m_RotationPivotSpring.RestState, rotationReset);
		if (positionPauseTime != 0f)
		{
			this.m_PositionSpring.ForceVelocityFadeIn(positionPauseTime);
		}
		if (rotationPauseTime != 0f)
		{
			this.m_RotationSpring.ForceVelocityFadeIn(rotationPauseTime);
		}
		if (positionPauseTime != 0f)
		{
			this.m_PositionPivotSpring.ForceVelocityFadeIn(positionPauseTime);
		}
		if (rotationPauseTime != 0f)
		{
			this.m_RotationPivotSpring.ForceVelocityFadeIn(rotationPauseTime);
		}
	}

	// Token: 0x0600B09D RID: 45213 RVA: 0x0041E2D5 File Offset: 0x0041C4D5
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3 SmoothStep(Vector3 from, Vector3 to, float amount)
	{
		return new Vector3(Mathf.SmoothStep(from.x, to.x, amount), Mathf.SmoothStep(from.y, to.y, amount), Mathf.SmoothStep(from.z, to.z, amount));
	}

	// Token: 0x0600B09E RID: 45214 RVA: 0x0041E314 File Offset: 0x0041C514
	public override void Refresh()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		float num = 1f - (this.m_FinalZoomTime - Time.time) / Mathf.Max(this.RenderingZoomDamping, 0.01f);
		if (this.m_Wielded)
		{
			this.PositionOffset = vp_FPWeapon.SmoothStep(this.PositionOffset, this.DefaultPosition + this.AimingPositionOffset, num * 15f);
		}
		else
		{
			this.PositionOffset = vp_FPWeapon.SmoothStep(this.PositionOffset, this.PositionExitOffset, num * 15f);
		}
		if (this.m_PositionSpring != null)
		{
			this.m_PositionSpring.Stiffness = new Vector3(this.PositionSpringStiffness, this.PositionSpringStiffness, this.PositionSpringStiffness);
			this.m_PositionSpring.Damping = Vector3.one - new Vector3(this.PositionSpringDamping, this.PositionSpringDamping, this.PositionSpringDamping);
			this.m_PositionSpring.RestState = this.PositionOffset - this.PositionPivot;
		}
		if (this.m_PositionPivotSpring != null)
		{
			this.m_PositionPivotSpring.Stiffness = new Vector3(this.PositionPivotSpringStiffness, this.PositionPivotSpringStiffness, this.PositionPivotSpringStiffness);
			this.m_PositionPivotSpring.Damping = Vector3.one - new Vector3(this.PositionPivotSpringDamping, this.PositionPivotSpringDamping, this.PositionPivotSpringDamping);
			this.m_PositionPivotSpring.RestState = this.PositionPivot;
		}
		if (this.m_RotationPivotSpring != null)
		{
			this.m_RotationPivotSpring.Stiffness = new Vector3(this.RotationPivotSpringStiffness, this.RotationPivotSpringStiffness, this.RotationPivotSpringStiffness);
			this.m_RotationPivotSpring.Damping = Vector3.one - new Vector3(this.RotationPivotSpringDamping, this.RotationPivotSpringDamping, this.RotationPivotSpringDamping);
			this.m_RotationPivotSpring.RestState = this.RotationPivot;
		}
		if (this.m_PositionSpring2 != null)
		{
			this.m_PositionSpring2.Stiffness = new Vector3(this.PositionSpring2Stiffness, this.PositionSpring2Stiffness, this.PositionSpring2Stiffness);
			this.m_PositionSpring2.Damping = Vector3.one - new Vector3(this.PositionSpring2Damping, this.PositionSpring2Damping, this.PositionSpring2Damping);
			this.m_PositionSpring2.RestState = Vector3.zero;
		}
		if (this.m_RotationSpring != null)
		{
			this.m_RotationSpring.Stiffness = new Vector3(this.RotationSpringStiffness, this.RotationSpringStiffness, this.RotationSpringStiffness);
			this.m_RotationSpring.Damping = Vector3.one - new Vector3(this.RotationSpringDamping, this.RotationSpringDamping, this.RotationSpringDamping);
			this.m_RotationSpring.RestState = this.RotationOffset;
		}
		if (this.m_RotationSpring2 != null)
		{
			this.m_RotationSpring2.Stiffness = new Vector3(this.RotationSpring2Stiffness, this.RotationSpring2Stiffness, this.RotationSpring2Stiffness);
			this.m_RotationSpring2.Damping = Vector3.one - new Vector3(this.RotationSpring2Damping, this.RotationSpring2Damping, this.RotationSpring2Damping);
			this.m_RotationSpring2.RestState = Vector3.zero;
		}
		if (base.Rendering)
		{
			this.Zoom();
		}
	}

	// Token: 0x0600B09F RID: 45215 RVA: 0x0041E61F File Offset: 0x0041C81F
	public override void Activate()
	{
		base.Activate();
		this.SnapZoom();
		if (this.m_WeaponGroup != null && !vp_Utility.IsActive(this.m_WeaponGroup))
		{
			vp_Utility.Activate(this.m_WeaponGroup, true);
		}
		this.SetPivotVisible(false);
	}

	// Token: 0x0600B0A0 RID: 45216 RVA: 0x0041E65B File Offset: 0x0041C85B
	public override void Deactivate()
	{
		this.m_Wielded = false;
		if (this.m_WeaponGroup != null && vp_Utility.IsActive(this.m_WeaponGroup))
		{
			vp_Utility.Activate(this.m_WeaponGroup, false);
		}
	}

	// Token: 0x0600B0A1 RID: 45217 RVA: 0x0041E68C File Offset: 0x0041C88C
	public virtual void SnapPivot()
	{
		if (this.m_PositionSpring != null)
		{
			this.m_PositionSpring.RestState = this.PositionOffset - this.PositionPivot;
			this.m_PositionSpring.State = this.PositionOffset - this.PositionPivot;
		}
		if (this.m_WeaponGroup != null)
		{
			this.m_WeaponGroupTransform.localPosition = this.PositionOffset - this.PositionPivot;
		}
		if (this.m_PositionPivotSpring != null)
		{
			this.m_PositionPivotSpring.RestState = this.PositionPivot;
			this.m_PositionPivotSpring.State = this.PositionPivot;
		}
		if (this.m_RotationPivotSpring != null)
		{
			this.m_RotationPivotSpring.RestState = this.RotationPivot;
			this.m_RotationPivotSpring.State = this.RotationPivot;
		}
		base.Transform.localPosition = this.PositionPivot;
		base.Transform.localEulerAngles = this.RotationPivot;
	}

	// Token: 0x0600B0A2 RID: 45218 RVA: 0x0041E779 File Offset: 0x0041C979
	public virtual void SetPivotVisible(bool visible)
	{
		if (this.m_Pivot == null)
		{
			return;
		}
		vp_Utility.Activate(this.m_Pivot.gameObject, visible);
	}

	// Token: 0x0600B0A3 RID: 45219 RVA: 0x0041E79B File Offset: 0x0041C99B
	public virtual void SnapToExit()
	{
		this.RotationOffset = this.RotationExitOffset;
		this.PositionOffset = this.PositionExitOffset;
		this.SnapSprings();
		this.SnapPivot();
	}

	// Token: 0x0600B0A4 RID: 45220 RVA: 0x0041E7C4 File Offset: 0x0041C9C4
	public override void SnapSprings()
	{
		base.SnapSprings();
		if (this.m_PositionSpring != null)
		{
			this.m_PositionSpring.RestState = this.PositionOffset - this.PositionPivot;
			this.m_PositionSpring.State = this.PositionOffset - this.PositionPivot;
			this.m_PositionSpring.Stop(true);
		}
		if (this.m_WeaponGroup != null)
		{
			this.m_WeaponGroupTransform.localPosition = this.PositionOffset - this.PositionPivot;
		}
		if (this.m_PositionPivotSpring != null)
		{
			this.m_PositionPivotSpring.RestState = this.PositionPivot;
			this.m_PositionPivotSpring.State = this.PositionPivot;
			this.m_PositionPivotSpring.Stop(true);
		}
		base.Transform.localPosition = this.PositionPivot;
		if (this.m_RotationPivotSpring != null)
		{
			this.m_RotationPivotSpring.RestState = this.RotationPivot;
			this.m_RotationPivotSpring.State = this.RotationPivot;
			this.m_RotationPivotSpring.Stop(true);
		}
		base.Transform.localEulerAngles = this.RotationPivot;
		if (this.m_RotationSpring != null)
		{
			this.m_RotationSpring.RestState = this.RotationOffset;
			this.m_RotationSpring.State = this.RotationOffset;
			this.m_RotationSpring.Stop(true);
		}
	}

	// Token: 0x0600B0A5 RID: 45221 RVA: 0x0041E914 File Offset: 0x0041CB14
	public override void StopSprings()
	{
		if (this.m_PositionSpring != null)
		{
			this.m_PositionSpring.Stop(true);
		}
		if (this.m_PositionPivotSpring != null)
		{
			this.m_PositionPivotSpring.Stop(true);
		}
		if (this.m_RotationSpring != null)
		{
			this.m_RotationSpring.Stop(true);
		}
		if (this.m_RotationPivotSpring != null)
		{
			this.m_RotationPivotSpring.Stop(true);
		}
	}

	// Token: 0x0600B0A6 RID: 45222 RVA: 0x0041E974 File Offset: 0x0041CB74
	public override void Wield(bool isWielding = true)
	{
		if (isWielding)
		{
			this.SnapToExit();
		}
		this.PositionOffset = (isWielding ? (this.DefaultPosition + this.AimingPositionOffset) : this.PositionExitOffset);
		this.RotationOffset = (isWielding ? this.DefaultRotation : this.RotationExitOffset);
		this.m_Wielded = isWielding;
		this.Refresh();
		base.StateManager.CombineStates();
		if (base.Audio != null && (isWielding ? this.SoundWield : this.SoundUnWield) != null && vp_Utility.IsActive(base.gameObject))
		{
			base.Audio.pitch = Time.timeScale;
			base.Audio.PlayOneShot(isWielding ? this.SoundWield : this.SoundUnWield);
		}
		if ((isWielding ? this.AnimationWield : this.AnimationUnWield) != null && vp_Utility.IsActive(base.gameObject))
		{
			if (isWielding)
			{
				this.m_WeaponModel.GetComponent<Animation>().CrossFade(this.AnimationWield.name);
				return;
			}
			this.m_WeaponModel.GetComponent<Animation>().CrossFade(this.AnimationUnWield.name);
		}
	}

	// Token: 0x0600B0A7 RID: 45223 RVA: 0x0041EA9C File Offset: 0x0041CC9C
	public virtual void ScheduleAmbientAnimation()
	{
		if (this.AnimationAmbient.Count == 0 || !vp_Utility.IsActive(base.gameObject))
		{
			return;
		}
		vp_Timer.In(global::UnityEngine.Random.Range(this.AmbientInterval.x, this.AmbientInterval.y), delegate
		{
			if (vp_Utility.IsActive(base.gameObject))
			{
				this.m_CurrentAmbientAnimation = global::UnityEngine.Random.Range(0, this.AnimationAmbient.Count);
				if (this.AnimationAmbient[this.m_CurrentAmbientAnimation] != null)
				{
					this.m_WeaponModel.GetComponent<Animation>().CrossFadeQueued(this.AnimationAmbient[this.m_CurrentAmbientAnimation].name);
					this.ScheduleAmbientAnimation();
				}
			}
		}, this.m_AnimationAmbientTimer);
	}

	// Token: 0x0600B0A8 RID: 45224 RVA: 0x0041EAF8 File Offset: 0x0041CCF8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_FallImpact(float impact)
	{
		if (this.m_PositionSpring != null)
		{
			this.m_PositionSpring.AddSoftForce(Vector3.down * impact * this.PositionKneeling, (float)this.PositionKneelingSoftness);
		}
		if (this.m_RotationSpring != null)
		{
			this.m_RotationSpring.AddSoftForce(Vector3.right * impact * this.RotationKneeling, (float)this.RotationKneelingSoftness);
		}
	}

	// Token: 0x0600B0A9 RID: 45225 RVA: 0x0041EB65 File Offset: 0x0041CD65
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_HeadImpact(float impact)
	{
		this.AddForce(Vector3.zero, Vector3.forward * (impact * 20f) * Time.timeScale);
	}

	// Token: 0x0600B0AA RID: 45226 RVA: 0x0041EB8D File Offset: 0x0041CD8D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraGroundStomp(float impact)
	{
		this.AddForce(Vector3.zero, new Vector3(-0.25f, 0f, 0f) * impact);
	}

	// Token: 0x0600B0AB RID: 45227 RVA: 0x0041EBB4 File Offset: 0x0041CDB4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraBombShake(float impact)
	{
		this.AddForce(Vector3.zero, new Vector3(-0.3f, 0.1f, 0.5f) * impact);
	}

	// Token: 0x0600B0AC RID: 45228 RVA: 0x0041EBDB File Offset: 0x0041CDDB
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraToggle3rdPerson()
	{
		this.RefreshWeaponModel();
	}

	// Token: 0x1700148D RID: 5261
	// (get) Token: 0x0600B0AD RID: 45229 RVA: 0x0041EBE4 File Offset: 0x0041CDE4
	public override Vector3 OnValue_AimDirection
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.FPPlayer.IsFirstPerson.Get())
			{
				return this.FPPlayer.HeadLookDirection.Get();
			}
			if (this.Weapon3rdPersonModel == null)
			{
				return this.FPPlayer.HeadLookDirection.Get();
			}
			return (this.Weapon3rdPersonModel.transform.position - this.FPPlayer.LookPoint.Get()).normalized;
		}
	}

	// Token: 0x040083EE RID: 33774
	public GameObject WeaponPrefab;

	// Token: 0x040083EF RID: 33775
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CharacterController Controller;

	// Token: 0x040083F0 RID: 33776
	public float RenderingZoomDamping = 0.5f;

	// Token: 0x040083F1 RID: 33777
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FinalZoomTime;

	// Token: 0x040083F2 RID: 33778
	public float RenderingFieldOfView = 75f;

	// Token: 0x040083F3 RID: 33779
	public float originalRenderingFieldOfView;

	// Token: 0x040083F4 RID: 33780
	public Vector2 RenderingClippingPlanes = new Vector2(0.01f, 10f);

	// Token: 0x040083F5 RID: 33781
	public float RenderingZScale = 1f;

	// Token: 0x040083F6 RID: 33782
	public float PositionSpringStiffness = 0.01f;

	// Token: 0x040083F7 RID: 33783
	public float PositionSpringDamping = 0.25f;

	// Token: 0x040083F8 RID: 33784
	public float PositionFallRetract = 1f;

	// Token: 0x040083F9 RID: 33785
	public float PositionPivotSpringStiffness = 0.01f;

	// Token: 0x040083FA RID: 33786
	public float PositionPivotSpringDamping = 0.25f;

	// Token: 0x040083FB RID: 33787
	public float PositionKneeling = 0.06f;

	// Token: 0x040083FC RID: 33788
	public int PositionKneelingSoftness = 1;

	// Token: 0x040083FD RID: 33789
	public Vector3 PositionWalkSlide = new Vector3(0.5f, 0.75f, 0.5f);

	// Token: 0x040083FE RID: 33790
	public Vector3 PositionPivot = Vector3.zero;

	// Token: 0x040083FF RID: 33791
	public Vector3 RotationPivot = Vector3.zero;

	// Token: 0x04008400 RID: 33792
	public float PositionInputVelocityScale = 1f;

	// Token: 0x04008401 RID: 33793
	public float PositionMaxInputVelocity = 25f;

	// Token: 0x04008402 RID: 33794
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_PositionSpring;

	// Token: 0x04008403 RID: 33795
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_PositionPivotSpring;

	// Token: 0x04008404 RID: 33796
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_RotationPivotSpring;

	// Token: 0x04008405 RID: 33797
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CameraMatrixOverride m_CameraMatrixOverride;

	// Token: 0x04008406 RID: 33798
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_WeaponGroup;

	// Token: 0x04008407 RID: 33799
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_Pivot;

	// Token: 0x04008408 RID: 33800
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_WeaponGroupTransform;

	// Token: 0x04008409 RID: 33801
	public float RotationSpringStiffness = 0.01f;

	// Token: 0x0400840A RID: 33802
	public float RotationSpringDamping = 0.25f;

	// Token: 0x0400840B RID: 33803
	public float RotationPivotSpringStiffness = 0.01f;

	// Token: 0x0400840C RID: 33804
	public float RotationPivotSpringDamping = 0.25f;

	// Token: 0x0400840D RID: 33805
	public float RotationKneeling;

	// Token: 0x0400840E RID: 33806
	public int RotationKneelingSoftness = 1;

	// Token: 0x0400840F RID: 33807
	public Vector3 RotationLookSway = new Vector3(1f, 0.7f, 0f);

	// Token: 0x04008410 RID: 33808
	public Vector3 RotationStrafeSway = new Vector3(0.3f, 1f, 1.5f);

	// Token: 0x04008411 RID: 33809
	public Vector3 RotationFallSway = new Vector3(1f, -0.5f, -3f);

	// Token: 0x04008412 RID: 33810
	public float RotationSlopeSway = 0.5f;

	// Token: 0x04008413 RID: 33811
	public float RotationInputVelocityScale = 1f;

	// Token: 0x04008414 RID: 33812
	public float RotationMaxInputVelocity = 15f;

	// Token: 0x04008415 RID: 33813
	public Vector3 RotationFallSwayAbsoluteSpringLimit = new Vector3(0f, 0f, 3f);

	// Token: 0x04008416 RID: 33814
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_RotationSpring;

	// Token: 0x04008417 RID: 33815
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_SwayVel = Vector3.zero;

	// Token: 0x04008418 RID: 33816
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_FallSway = Vector3.zero;

	// Token: 0x04008419 RID: 33817
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_FallSwaySpringLimit = Vector3.zero;

	// Token: 0x0400841A RID: 33818
	public float RetractionDistance;

	// Token: 0x0400841B RID: 33819
	public Vector2 RetractionOffset = new Vector2(0f, 0f);

	// Token: 0x0400841C RID: 33820
	public float RetractionRelaxSpeed = 0.25f;

	// Token: 0x0400841D RID: 33821
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_DrawRetractionDebugLine;

	// Token: 0x0400841E RID: 33822
	public float ShakeSpeed = 0.05f;

	// Token: 0x0400841F RID: 33823
	public Vector3 ShakeAmplitude = new Vector3(0.25f, 0f, 2f);

	// Token: 0x04008420 RID: 33824
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Shake = Vector3.zero;

	// Token: 0x04008421 RID: 33825
	public Vector4 BobRate = new Vector4(0.9f, 0.45f, 0f, 0f);

	// Token: 0x04008422 RID: 33826
	public Vector4 BobAmplitude = new Vector4(0.35f, 0.5f, 0f, 0f);

	// Token: 0x04008423 RID: 33827
	public float BobInputVelocityScale = 1f;

	// Token: 0x04008424 RID: 33828
	public float BobMaxInputVelocity = 100f;

	// Token: 0x04008425 RID: 33829
	public bool BobRequireGroundContact = true;

	// Token: 0x04008426 RID: 33830
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastBobSpeed;

	// Token: 0x04008427 RID: 33831
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector4 m_CurrentBobAmp = Vector4.zero;

	// Token: 0x04008428 RID: 33832
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector4 m_CurrentBobVal = Vector4.zero;

	// Token: 0x04008429 RID: 33833
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_BobSpeed;

	// Token: 0x0400842A RID: 33834
	public Vector3 StepPositionForce = new Vector3(0f, -0.0012f, -0.0012f);

	// Token: 0x0400842B RID: 33835
	public Vector3 StepRotationForce = new Vector3(0f, 0f, 0f);

	// Token: 0x0400842C RID: 33836
	public int StepSoftness = 4;

	// Token: 0x0400842D RID: 33837
	public float StepMinVelocity;

	// Token: 0x0400842E RID: 33838
	public float StepPositionBalance;

	// Token: 0x0400842F RID: 33839
	public float StepRotationBalance;

	// Token: 0x04008430 RID: 33840
	public float StepForceScale = 1f;

	// Token: 0x04008431 RID: 33841
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastUpBob;

	// Token: 0x04008432 RID: 33842
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_BobWasElevating;

	// Token: 0x04008433 RID: 33843
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PosStep = Vector3.zero;

	// Token: 0x04008434 RID: 33844
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_RotStep = Vector3.zero;

	// Token: 0x04008435 RID: 33845
	public bool LookDownActive;

	// Token: 0x04008436 RID: 33846
	public float LookDownYawLimit = 60f;

	// Token: 0x04008437 RID: 33847
	public Vector3 LookDownPositionOffsetMiddle = new Vector3(0.32f, -0.37f, 0.78f);

	// Token: 0x04008438 RID: 33848
	public Vector3 LookDownPositionOffsetLeft = new Vector3(0.27f, -0.31f, 0.7f);

	// Token: 0x04008439 RID: 33849
	public Vector3 LookDownPositionOffsetRight = new Vector3(0.6f, -0.41f, 0.86f);

	// Token: 0x0400843A RID: 33850
	public float LookDownPositionSpringPower = 1f;

	// Token: 0x0400843B RID: 33851
	public Vector3 LookDownRotationOffsetMiddle = new Vector3(-3.9f, 2.24f, 4.69f);

	// Token: 0x0400843C RID: 33852
	public Vector3 LookDownRotationOffsetLeft = new Vector3(-7f, -10.5f, 15.6f);

	// Token: 0x0400843D RID: 33853
	public Vector3 LookDownRotationOffsetRight = new Vector3(-9.2f, -9.8f, 48.84f);

	// Token: 0x0400843E RID: 33854
	public float LookDownRotationSpringPower = 1f;

	// Token: 0x0400843F RID: 33855
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentPosRestState = Vector3.zero;

	// Token: 0x04008440 RID: 33856
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentRotRestState = Vector3.zero;

	// Token: 0x04008441 RID: 33857
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AnimationCurve m_LookDownCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.8f, 0.2f, 0.9f, 1.5f),
		new Keyframe(1f, 1f)
	});

	// Token: 0x04008442 RID: 33858
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LookDownPitch;

	// Token: 0x04008443 RID: 33859
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LookDownYaw;

	// Token: 0x04008444 RID: 33860
	public AudioClip SoundWield;

	// Token: 0x04008445 RID: 33861
	public AudioClip SoundUnWield;

	// Token: 0x04008446 RID: 33862
	public AnimationClip AnimationWield;

	// Token: 0x04008447 RID: 33863
	public AnimationClip AnimationUnWield;

	// Token: 0x04008448 RID: 33864
	public List<global::UnityEngine.Object> AnimationAmbient = new List<global::UnityEngine.Object>();

	// Token: 0x04008449 RID: 33865
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<bool> m_AmbAnimPlayed = new List<bool>();

	// Token: 0x0400844A RID: 33866
	public Vector2 AmbientInterval = new Vector2(2.5f, 7.5f);

	// Token: 0x0400844B RID: 33867
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_CurrentAmbientAnimation;

	// Token: 0x0400844C RID: 33868
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_AnimationAmbientTimer = new vp_Timer.Handle();

	// Token: 0x0400844D RID: 33869
	public Vector3 PositionExitOffset = new Vector3(0f, -1f, 0f);

	// Token: 0x0400844E RID: 33870
	public Vector3 RotationExitOffset = new Vector3(40f, 0f, 0f);

	// Token: 0x0400844F RID: 33871
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_LookInput = Vector2.zero;

	// Token: 0x04008450 RID: 33872
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float LOOKDOWNSPEED = 2f;

	// Token: 0x04008451 RID: 33873
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_FPPlayer;
}
