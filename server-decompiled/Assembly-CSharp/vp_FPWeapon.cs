using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200161F RID: 5663
[Preserve]
public class vp_FPWeapon : vp_Weapon
{
	// Token: 0x170014CA RID: 5322
	// (get) Token: 0x0600B10C RID: 45324 RVA: 0x004207B3 File Offset: 0x0041E9B3
	public CameraMatrixOverride CameraMatrixOverride
	{
		get
		{
			return this.m_CameraMatrixOverride;
		}
	}

	// Token: 0x170014CB RID: 5323
	// (get) Token: 0x0600B10D RID: 45325 RVA: 0x004207BB File Offset: 0x0041E9BB
	public GameObject WeaponModel
	{
		get
		{
			return this.m_WeaponModel;
		}
	}

	// Token: 0x170014CC RID: 5324
	// (get) Token: 0x0600B10E RID: 45326 RVA: 0x004207C3 File Offset: 0x0041E9C3
	public Vector3 DefaultPosition
	{
		get
		{
			return (Vector3)base.DefaultState.Preset.GetFieldValue("PositionOffset");
		}
	}

	// Token: 0x170014CD RID: 5325
	// (get) Token: 0x0600B10F RID: 45327 RVA: 0x004207DF File Offset: 0x0041E9DF
	public Vector3 DefaultRotation
	{
		get
		{
			return (Vector3)base.DefaultState.Preset.GetFieldValue("RotationOffset");
		}
	}

	// Token: 0x170014CE RID: 5326
	// (get) Token: 0x0600B110 RID: 45328 RVA: 0x004207FB File Offset: 0x0041E9FB
	// (set) Token: 0x0600B111 RID: 45329 RVA: 0x00420803 File Offset: 0x0041EA03
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

	// Token: 0x170014CF RID: 5327
	// (get) Token: 0x0600B112 RID: 45330 RVA: 0x0042080C File Offset: 0x0041EA0C
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

	// Token: 0x0600B113 RID: 45331 RVA: 0x00420844 File Offset: 0x0041EA44
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

	// Token: 0x0600B114 RID: 45332 RVA: 0x0042095C File Offset: 0x0041EB5C
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

	// Token: 0x0600B115 RID: 45333 RVA: 0x00420C50 File Offset: 0x0041EE50
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

	// Token: 0x0600B116 RID: 45334 RVA: 0x00420D24 File Offset: 0x0041EF24
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Init()
	{
		base.Init();
		this.ScheduleAmbientAnimation();
	}

	// Token: 0x0600B117 RID: 45335 RVA: 0x00420D32 File Offset: 0x0041EF32
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		if (Time.timeScale != 0f)
		{
			this.UpdateInput();
		}
	}

	// Token: 0x0600B118 RID: 45336 RVA: 0x00420D4C File Offset: 0x0041EF4C
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

	// Token: 0x0600B119 RID: 45337 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void LateUpdate()
	{
	}

	// Token: 0x0600B11A RID: 45338 RVA: 0x00420D9C File Offset: 0x0041EF9C
	public virtual void AddForce(Vector3 force)
	{
		this.m_PositionSpring.AddForce(force);
	}

	// Token: 0x0600B11B RID: 45339 RVA: 0x00420DAA File Offset: 0x0041EFAA
	public virtual void AddForce(float x, float y, float z)
	{
		this.AddForce(new Vector3(x, y, z));
	}

	// Token: 0x0600B11C RID: 45340 RVA: 0x00420DBA File Offset: 0x0041EFBA
	public virtual void AddForce(Vector3 positional, Vector3 angular)
	{
		this.m_PositionSpring.AddForce(positional);
		this.m_RotationSpring.AddForce(angular);
	}

	// Token: 0x0600B11D RID: 45341 RVA: 0x00420DD4 File Offset: 0x0041EFD4
	public virtual void AddForce(float xPos, float yPos, float zPos, float xRot, float yRot, float zRot)
	{
		this.AddForce(new Vector3(xPos, yPos, zPos), new Vector3(xRot, yRot, zRot));
	}

	// Token: 0x0600B11E RID: 45342 RVA: 0x00420DEF File Offset: 0x0041EFEF
	public virtual void AddSoftForce(Vector3 force, int frames)
	{
		this.m_PositionSpring.AddSoftForce(force, (float)frames);
	}

	// Token: 0x0600B11F RID: 45343 RVA: 0x00420DFF File Offset: 0x0041EFFF
	public virtual void AddSoftForce(float x, float y, float z, int frames)
	{
		this.AddSoftForce(new Vector3(x, y, z), frames);
	}

	// Token: 0x0600B120 RID: 45344 RVA: 0x00420E11 File Offset: 0x0041F011
	public virtual void AddSoftForce(Vector3 positional, Vector3 angular, int frames)
	{
		this.m_PositionSpring.AddSoftForce(positional, (float)frames);
		this.m_RotationSpring.AddSoftForce(angular, (float)frames);
	}

	// Token: 0x0600B121 RID: 45345 RVA: 0x00420E2F File Offset: 0x0041F02F
	public virtual void AddSoftForce(float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, int frames)
	{
		this.AddSoftForce(new Vector3(xPos, yPos, zPos), new Vector3(xRot, yRot, zRot), frames);
	}

	// Token: 0x0600B122 RID: 45346 RVA: 0x00420E4C File Offset: 0x0041F04C
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

	// Token: 0x0600B123 RID: 45347 RVA: 0x00420F14 File Offset: 0x0041F114
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

	// Token: 0x0600B124 RID: 45348 RVA: 0x00420F9E File Offset: 0x0041F19E
	public virtual void Zoom()
	{
		this.m_FinalZoomTime = Time.time + this.RenderingZoomDamping;
	}

	// Token: 0x0600B125 RID: 45349 RVA: 0x00420FB2 File Offset: 0x0041F1B2
	public virtual void SnapZoom()
	{
		if (this.m_CameraMatrixOverride != null)
		{
			this.m_CameraMatrixOverride.fov = this.RenderingFieldOfView;
		}
	}

	// Token: 0x0600B126 RID: 45350 RVA: 0x00420FD4 File Offset: 0x0041F1D4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateShakes()
	{
		if (this.ShakeSpeed != 0f)
		{
			this.m_Shake = Vector3.Scale(vp_SmoothRandom.GetVector3Centered(this.ShakeSpeed), this.ShakeAmplitude);
			this.m_RotationSpring.AddForce(this.m_Shake * Time.timeScale);
		}
	}

	// Token: 0x0600B127 RID: 45351 RVA: 0x00421028 File Offset: 0x0041F228
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

	// Token: 0x0600B128 RID: 45352 RVA: 0x0042119C File Offset: 0x0041F39C
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

	// Token: 0x0600B129 RID: 45353 RVA: 0x00421418 File Offset: 0x0041F618
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

	// Token: 0x0600B12A RID: 45354 RVA: 0x004214B4 File Offset: 0x0041F6B4
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

	// Token: 0x0600B12B RID: 45355 RVA: 0x00421504 File Offset: 0x0041F704
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

	// Token: 0x0600B12C RID: 45356 RVA: 0x0042197C File Offset: 0x0041FB7C
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

	// Token: 0x0600B12D RID: 45357 RVA: 0x00421B08 File Offset: 0x0041FD08
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

	// Token: 0x0600B12E RID: 45358 RVA: 0x00421EB4 File Offset: 0x004200B4
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

	// Token: 0x0600B12F RID: 45359 RVA: 0x00421FB1 File Offset: 0x004201B1
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3 SmoothStep(Vector3 from, Vector3 to, float amount)
	{
		return new Vector3(Mathf.SmoothStep(from.x, to.x, amount), Mathf.SmoothStep(from.y, to.y, amount), Mathf.SmoothStep(from.z, to.z, amount));
	}

	// Token: 0x0600B130 RID: 45360 RVA: 0x00421FF0 File Offset: 0x004201F0
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

	// Token: 0x0600B131 RID: 45361 RVA: 0x004222FB File Offset: 0x004204FB
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

	// Token: 0x0600B132 RID: 45362 RVA: 0x00422337 File Offset: 0x00420537
	public override void Deactivate()
	{
		this.m_Wielded = false;
		if (this.m_WeaponGroup != null && vp_Utility.IsActive(this.m_WeaponGroup))
		{
			vp_Utility.Activate(this.m_WeaponGroup, false);
		}
	}

	// Token: 0x0600B133 RID: 45363 RVA: 0x00422368 File Offset: 0x00420568
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

	// Token: 0x0600B134 RID: 45364 RVA: 0x00422455 File Offset: 0x00420655
	public virtual void SetPivotVisible(bool visible)
	{
		if (this.m_Pivot == null)
		{
			return;
		}
		vp_Utility.Activate(this.m_Pivot.gameObject, visible);
	}

	// Token: 0x0600B135 RID: 45365 RVA: 0x00422477 File Offset: 0x00420677
	public virtual void SnapToExit()
	{
		this.RotationOffset = this.RotationExitOffset;
		this.PositionOffset = this.PositionExitOffset;
		this.SnapSprings();
		this.SnapPivot();
	}

	// Token: 0x0600B136 RID: 45366 RVA: 0x004224A0 File Offset: 0x004206A0
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

	// Token: 0x0600B137 RID: 45367 RVA: 0x004225F0 File Offset: 0x004207F0
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

	// Token: 0x0600B138 RID: 45368 RVA: 0x00422650 File Offset: 0x00420850
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

	// Token: 0x0600B139 RID: 45369 RVA: 0x00422778 File Offset: 0x00420978
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

	// Token: 0x0600B13A RID: 45370 RVA: 0x004227D4 File Offset: 0x004209D4
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

	// Token: 0x0600B13B RID: 45371 RVA: 0x00422841 File Offset: 0x00420A41
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_HeadImpact(float impact)
	{
		this.AddForce(Vector3.zero, Vector3.forward * (impact * 20f) * Time.timeScale);
	}

	// Token: 0x0600B13C RID: 45372 RVA: 0x00422869 File Offset: 0x00420A69
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraGroundStomp(float impact)
	{
		this.AddForce(Vector3.zero, new Vector3(-0.25f, 0f, 0f) * impact);
	}

	// Token: 0x0600B13D RID: 45373 RVA: 0x00422890 File Offset: 0x00420A90
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraBombShake(float impact)
	{
		this.AddForce(Vector3.zero, new Vector3(-0.3f, 0.1f, 0.5f) * impact);
	}

	// Token: 0x0600B13E RID: 45374 RVA: 0x004228B7 File Offset: 0x00420AB7
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraToggle3rdPerson()
	{
		this.RefreshWeaponModel();
	}

	// Token: 0x170014D0 RID: 5328
	// (get) Token: 0x0600B13F RID: 45375 RVA: 0x004228C0 File Offset: 0x00420AC0
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

	// Token: 0x04008444 RID: 33860
	public GameObject WeaponPrefab;

	// Token: 0x04008445 RID: 33861
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CharacterController Controller;

	// Token: 0x04008446 RID: 33862
	public float RenderingZoomDamping = 0.5f;

	// Token: 0x04008447 RID: 33863
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FinalZoomTime;

	// Token: 0x04008448 RID: 33864
	public float RenderingFieldOfView = 75f;

	// Token: 0x04008449 RID: 33865
	public float originalRenderingFieldOfView;

	// Token: 0x0400844A RID: 33866
	public Vector2 RenderingClippingPlanes = new Vector2(0.01f, 10f);

	// Token: 0x0400844B RID: 33867
	public float RenderingZScale = 1f;

	// Token: 0x0400844C RID: 33868
	public float PositionSpringStiffness = 0.01f;

	// Token: 0x0400844D RID: 33869
	public float PositionSpringDamping = 0.25f;

	// Token: 0x0400844E RID: 33870
	public float PositionFallRetract = 1f;

	// Token: 0x0400844F RID: 33871
	public float PositionPivotSpringStiffness = 0.01f;

	// Token: 0x04008450 RID: 33872
	public float PositionPivotSpringDamping = 0.25f;

	// Token: 0x04008451 RID: 33873
	public float PositionKneeling = 0.06f;

	// Token: 0x04008452 RID: 33874
	public int PositionKneelingSoftness = 1;

	// Token: 0x04008453 RID: 33875
	public Vector3 PositionWalkSlide = new Vector3(0.5f, 0.75f, 0.5f);

	// Token: 0x04008454 RID: 33876
	public Vector3 PositionPivot = Vector3.zero;

	// Token: 0x04008455 RID: 33877
	public Vector3 RotationPivot = Vector3.zero;

	// Token: 0x04008456 RID: 33878
	public float PositionInputVelocityScale = 1f;

	// Token: 0x04008457 RID: 33879
	public float PositionMaxInputVelocity = 25f;

	// Token: 0x04008458 RID: 33880
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_PositionSpring;

	// Token: 0x04008459 RID: 33881
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_PositionPivotSpring;

	// Token: 0x0400845A RID: 33882
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_RotationPivotSpring;

	// Token: 0x0400845B RID: 33883
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CameraMatrixOverride m_CameraMatrixOverride;

	// Token: 0x0400845C RID: 33884
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_WeaponGroup;

	// Token: 0x0400845D RID: 33885
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_Pivot;

	// Token: 0x0400845E RID: 33886
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_WeaponGroupTransform;

	// Token: 0x0400845F RID: 33887
	public float RotationSpringStiffness = 0.01f;

	// Token: 0x04008460 RID: 33888
	public float RotationSpringDamping = 0.25f;

	// Token: 0x04008461 RID: 33889
	public float RotationPivotSpringStiffness = 0.01f;

	// Token: 0x04008462 RID: 33890
	public float RotationPivotSpringDamping = 0.25f;

	// Token: 0x04008463 RID: 33891
	public float RotationKneeling;

	// Token: 0x04008464 RID: 33892
	public int RotationKneelingSoftness = 1;

	// Token: 0x04008465 RID: 33893
	public Vector3 RotationLookSway = new Vector3(1f, 0.7f, 0f);

	// Token: 0x04008466 RID: 33894
	public Vector3 RotationStrafeSway = new Vector3(0.3f, 1f, 1.5f);

	// Token: 0x04008467 RID: 33895
	public Vector3 RotationFallSway = new Vector3(1f, -0.5f, -3f);

	// Token: 0x04008468 RID: 33896
	public float RotationSlopeSway = 0.5f;

	// Token: 0x04008469 RID: 33897
	public float RotationInputVelocityScale = 1f;

	// Token: 0x0400846A RID: 33898
	public float RotationMaxInputVelocity = 15f;

	// Token: 0x0400846B RID: 33899
	public Vector3 RotationFallSwayAbsoluteSpringLimit = new Vector3(0f, 0f, 3f);

	// Token: 0x0400846C RID: 33900
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_RotationSpring;

	// Token: 0x0400846D RID: 33901
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_SwayVel = Vector3.zero;

	// Token: 0x0400846E RID: 33902
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_FallSway = Vector3.zero;

	// Token: 0x0400846F RID: 33903
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_FallSwaySpringLimit = Vector3.zero;

	// Token: 0x04008470 RID: 33904
	public float RetractionDistance;

	// Token: 0x04008471 RID: 33905
	public Vector2 RetractionOffset = new Vector2(0f, 0f);

	// Token: 0x04008472 RID: 33906
	public float RetractionRelaxSpeed = 0.25f;

	// Token: 0x04008473 RID: 33907
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_DrawRetractionDebugLine;

	// Token: 0x04008474 RID: 33908
	public float ShakeSpeed = 0.05f;

	// Token: 0x04008475 RID: 33909
	public Vector3 ShakeAmplitude = new Vector3(0.25f, 0f, 2f);

	// Token: 0x04008476 RID: 33910
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Shake = Vector3.zero;

	// Token: 0x04008477 RID: 33911
	public Vector4 BobRate = new Vector4(0.9f, 0.45f, 0f, 0f);

	// Token: 0x04008478 RID: 33912
	public Vector4 BobAmplitude = new Vector4(0.35f, 0.5f, 0f, 0f);

	// Token: 0x04008479 RID: 33913
	public float BobInputVelocityScale = 1f;

	// Token: 0x0400847A RID: 33914
	public float BobMaxInputVelocity = 100f;

	// Token: 0x0400847B RID: 33915
	public bool BobRequireGroundContact = true;

	// Token: 0x0400847C RID: 33916
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastBobSpeed;

	// Token: 0x0400847D RID: 33917
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector4 m_CurrentBobAmp = Vector4.zero;

	// Token: 0x0400847E RID: 33918
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector4 m_CurrentBobVal = Vector4.zero;

	// Token: 0x0400847F RID: 33919
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_BobSpeed;

	// Token: 0x04008480 RID: 33920
	public Vector3 StepPositionForce = new Vector3(0f, -0.0012f, -0.0012f);

	// Token: 0x04008481 RID: 33921
	public Vector3 StepRotationForce = new Vector3(0f, 0f, 0f);

	// Token: 0x04008482 RID: 33922
	public int StepSoftness = 4;

	// Token: 0x04008483 RID: 33923
	public float StepMinVelocity;

	// Token: 0x04008484 RID: 33924
	public float StepPositionBalance;

	// Token: 0x04008485 RID: 33925
	public float StepRotationBalance;

	// Token: 0x04008486 RID: 33926
	public float StepForceScale = 1f;

	// Token: 0x04008487 RID: 33927
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastUpBob;

	// Token: 0x04008488 RID: 33928
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_BobWasElevating;

	// Token: 0x04008489 RID: 33929
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PosStep = Vector3.zero;

	// Token: 0x0400848A RID: 33930
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_RotStep = Vector3.zero;

	// Token: 0x0400848B RID: 33931
	public bool LookDownActive;

	// Token: 0x0400848C RID: 33932
	public float LookDownYawLimit = 60f;

	// Token: 0x0400848D RID: 33933
	public Vector3 LookDownPositionOffsetMiddle = new Vector3(0.32f, -0.37f, 0.78f);

	// Token: 0x0400848E RID: 33934
	public Vector3 LookDownPositionOffsetLeft = new Vector3(0.27f, -0.31f, 0.7f);

	// Token: 0x0400848F RID: 33935
	public Vector3 LookDownPositionOffsetRight = new Vector3(0.6f, -0.41f, 0.86f);

	// Token: 0x04008490 RID: 33936
	public float LookDownPositionSpringPower = 1f;

	// Token: 0x04008491 RID: 33937
	public Vector3 LookDownRotationOffsetMiddle = new Vector3(-3.9f, 2.24f, 4.69f);

	// Token: 0x04008492 RID: 33938
	public Vector3 LookDownRotationOffsetLeft = new Vector3(-7f, -10.5f, 15.6f);

	// Token: 0x04008493 RID: 33939
	public Vector3 LookDownRotationOffsetRight = new Vector3(-9.2f, -9.8f, 48.84f);

	// Token: 0x04008494 RID: 33940
	public float LookDownRotationSpringPower = 1f;

	// Token: 0x04008495 RID: 33941
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentPosRestState = Vector3.zero;

	// Token: 0x04008496 RID: 33942
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentRotRestState = Vector3.zero;

	// Token: 0x04008497 RID: 33943
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AnimationCurve m_LookDownCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.8f, 0.2f, 0.9f, 1.5f),
		new Keyframe(1f, 1f)
	});

	// Token: 0x04008498 RID: 33944
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LookDownPitch;

	// Token: 0x04008499 RID: 33945
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LookDownYaw;

	// Token: 0x0400849A RID: 33946
	public AudioClip SoundWield;

	// Token: 0x0400849B RID: 33947
	public AudioClip SoundUnWield;

	// Token: 0x0400849C RID: 33948
	public AnimationClip AnimationWield;

	// Token: 0x0400849D RID: 33949
	public AnimationClip AnimationUnWield;

	// Token: 0x0400849E RID: 33950
	public List<global::UnityEngine.Object> AnimationAmbient = new List<global::UnityEngine.Object>();

	// Token: 0x0400849F RID: 33951
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<bool> m_AmbAnimPlayed = new List<bool>();

	// Token: 0x040084A0 RID: 33952
	public Vector2 AmbientInterval = new Vector2(2.5f, 7.5f);

	// Token: 0x040084A1 RID: 33953
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_CurrentAmbientAnimation;

	// Token: 0x040084A2 RID: 33954
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_AnimationAmbientTimer = new vp_Timer.Handle();

	// Token: 0x040084A3 RID: 33955
	public Vector3 PositionExitOffset = new Vector3(0f, -1f, 0f);

	// Token: 0x040084A4 RID: 33956
	public Vector3 RotationExitOffset = new Vector3(40f, 0f, 0f);

	// Token: 0x040084A5 RID: 33957
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_LookInput = Vector2.zero;

	// Token: 0x040084A6 RID: 33958
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float LOOKDOWNSPEED = 2f;

	// Token: 0x040084A7 RID: 33959
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_FPPlayer;
}
