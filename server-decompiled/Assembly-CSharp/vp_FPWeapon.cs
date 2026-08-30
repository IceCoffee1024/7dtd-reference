using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020015E5 RID: 5605
[Preserve]
public class vp_FPWeapon : vp_Weapon
{
	// Token: 0x1700144E RID: 5198
	// (get) Token: 0x0600AF7D RID: 44925 RVA: 0x00417797 File Offset: 0x00415997
	public CameraMatrixOverride CameraMatrixOverride
	{
		get
		{
			return this.m_CameraMatrixOverride;
		}
	}

	// Token: 0x1700144F RID: 5199
	// (get) Token: 0x0600AF7E RID: 44926 RVA: 0x0041779F File Offset: 0x0041599F
	public GameObject WeaponModel
	{
		get
		{
			return this.m_WeaponModel;
		}
	}

	// Token: 0x17001450 RID: 5200
	// (get) Token: 0x0600AF7F RID: 44927 RVA: 0x004177A7 File Offset: 0x004159A7
	public Vector3 DefaultPosition
	{
		get
		{
			return (Vector3)base.DefaultState.Preset.GetFieldValue("PositionOffset");
		}
	}

	// Token: 0x17001451 RID: 5201
	// (get) Token: 0x0600AF80 RID: 44928 RVA: 0x004177C3 File Offset: 0x004159C3
	public Vector3 DefaultRotation
	{
		get
		{
			return (Vector3)base.DefaultState.Preset.GetFieldValue("RotationOffset");
		}
	}

	// Token: 0x17001452 RID: 5202
	// (get) Token: 0x0600AF81 RID: 44929 RVA: 0x004177DF File Offset: 0x004159DF
	// (set) Token: 0x0600AF82 RID: 44930 RVA: 0x004177E7 File Offset: 0x004159E7
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

	// Token: 0x17001453 RID: 5203
	// (get) Token: 0x0600AF83 RID: 44931 RVA: 0x004177F0 File Offset: 0x004159F0
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

	// Token: 0x0600AF84 RID: 44932 RVA: 0x00417828 File Offset: 0x00415A28
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

	// Token: 0x0600AF85 RID: 44933 RVA: 0x00417940 File Offset: 0x00415B40
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

	// Token: 0x0600AF86 RID: 44934 RVA: 0x00417C34 File Offset: 0x00415E34
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

	// Token: 0x0600AF87 RID: 44935 RVA: 0x00417D08 File Offset: 0x00415F08
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Init()
	{
		base.Init();
		this.ScheduleAmbientAnimation();
	}

	// Token: 0x0600AF88 RID: 44936 RVA: 0x00417D16 File Offset: 0x00415F16
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		if (Time.timeScale != 0f)
		{
			this.UpdateInput();
		}
	}

	// Token: 0x0600AF89 RID: 44937 RVA: 0x00417D30 File Offset: 0x00415F30
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

	// Token: 0x0600AF8A RID: 44938 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void LateUpdate()
	{
	}

	// Token: 0x0600AF8B RID: 44939 RVA: 0x00417D80 File Offset: 0x00415F80
	public virtual void AddForce(Vector3 force)
	{
		this.m_PositionSpring.AddForce(force);
	}

	// Token: 0x0600AF8C RID: 44940 RVA: 0x00417D8E File Offset: 0x00415F8E
	public virtual void AddForce(float x, float y, float z)
	{
		this.AddForce(new Vector3(x, y, z));
	}

	// Token: 0x0600AF8D RID: 44941 RVA: 0x00417D9E File Offset: 0x00415F9E
	public virtual void AddForce(Vector3 positional, Vector3 angular)
	{
		this.m_PositionSpring.AddForce(positional);
		this.m_RotationSpring.AddForce(angular);
	}

	// Token: 0x0600AF8E RID: 44942 RVA: 0x00417DB8 File Offset: 0x00415FB8
	public virtual void AddForce(float xPos, float yPos, float zPos, float xRot, float yRot, float zRot)
	{
		this.AddForce(new Vector3(xPos, yPos, zPos), new Vector3(xRot, yRot, zRot));
	}

	// Token: 0x0600AF8F RID: 44943 RVA: 0x00417DD3 File Offset: 0x00415FD3
	public virtual void AddSoftForce(Vector3 force, int frames)
	{
		this.m_PositionSpring.AddSoftForce(force, (float)frames);
	}

	// Token: 0x0600AF90 RID: 44944 RVA: 0x00417DE3 File Offset: 0x00415FE3
	public virtual void AddSoftForce(float x, float y, float z, int frames)
	{
		this.AddSoftForce(new Vector3(x, y, z), frames);
	}

	// Token: 0x0600AF91 RID: 44945 RVA: 0x00417DF5 File Offset: 0x00415FF5
	public virtual void AddSoftForce(Vector3 positional, Vector3 angular, int frames)
	{
		this.m_PositionSpring.AddSoftForce(positional, (float)frames);
		this.m_RotationSpring.AddSoftForce(angular, (float)frames);
	}

	// Token: 0x0600AF92 RID: 44946 RVA: 0x00417E13 File Offset: 0x00416013
	public virtual void AddSoftForce(float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, int frames)
	{
		this.AddSoftForce(new Vector3(xPos, yPos, zPos), new Vector3(xRot, yRot, zRot), frames);
	}

	// Token: 0x0600AF93 RID: 44947 RVA: 0x00417E30 File Offset: 0x00416030
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

	// Token: 0x0600AF94 RID: 44948 RVA: 0x00417EF8 File Offset: 0x004160F8
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

	// Token: 0x0600AF95 RID: 44949 RVA: 0x00417F82 File Offset: 0x00416182
	public virtual void Zoom()
	{
		this.m_FinalZoomTime = Time.time + this.RenderingZoomDamping;
	}

	// Token: 0x0600AF96 RID: 44950 RVA: 0x00417F96 File Offset: 0x00416196
	public virtual void SnapZoom()
	{
		if (this.m_CameraMatrixOverride != null)
		{
			this.m_CameraMatrixOverride.fov = this.RenderingFieldOfView;
		}
	}

	// Token: 0x0600AF97 RID: 44951 RVA: 0x00417FB8 File Offset: 0x004161B8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateShakes()
	{
		if (this.ShakeSpeed != 0f)
		{
			this.m_Shake = Vector3.Scale(vp_SmoothRandom.GetVector3Centered(this.ShakeSpeed), this.ShakeAmplitude);
			this.m_RotationSpring.AddForce(this.m_Shake * Time.timeScale);
		}
	}

	// Token: 0x0600AF98 RID: 44952 RVA: 0x0041800C File Offset: 0x0041620C
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

	// Token: 0x0600AF99 RID: 44953 RVA: 0x00418180 File Offset: 0x00416380
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

	// Token: 0x0600AF9A RID: 44954 RVA: 0x004183FC File Offset: 0x004165FC
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

	// Token: 0x0600AF9B RID: 44955 RVA: 0x00418498 File Offset: 0x00416698
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

	// Token: 0x0600AF9C RID: 44956 RVA: 0x004184E8 File Offset: 0x004166E8
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

	// Token: 0x0600AF9D RID: 44957 RVA: 0x00418960 File Offset: 0x00416B60
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

	// Token: 0x0600AF9E RID: 44958 RVA: 0x00418AEC File Offset: 0x00416CEC
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

	// Token: 0x0600AF9F RID: 44959 RVA: 0x00418E98 File Offset: 0x00417098
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

	// Token: 0x0600AFA0 RID: 44960 RVA: 0x00418F95 File Offset: 0x00417195
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3 SmoothStep(Vector3 from, Vector3 to, float amount)
	{
		return new Vector3(Mathf.SmoothStep(from.x, to.x, amount), Mathf.SmoothStep(from.y, to.y, amount), Mathf.SmoothStep(from.z, to.z, amount));
	}

	// Token: 0x0600AFA1 RID: 44961 RVA: 0x00418FD4 File Offset: 0x004171D4
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

	// Token: 0x0600AFA2 RID: 44962 RVA: 0x004192DF File Offset: 0x004174DF
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

	// Token: 0x0600AFA3 RID: 44963 RVA: 0x0041931B File Offset: 0x0041751B
	public override void Deactivate()
	{
		this.m_Wielded = false;
		if (this.m_WeaponGroup != null && vp_Utility.IsActive(this.m_WeaponGroup))
		{
			vp_Utility.Activate(this.m_WeaponGroup, false);
		}
	}

	// Token: 0x0600AFA4 RID: 44964 RVA: 0x0041934C File Offset: 0x0041754C
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

	// Token: 0x0600AFA5 RID: 44965 RVA: 0x00419439 File Offset: 0x00417639
	public virtual void SetPivotVisible(bool visible)
	{
		if (this.m_Pivot == null)
		{
			return;
		}
		vp_Utility.Activate(this.m_Pivot.gameObject, visible);
	}

	// Token: 0x0600AFA6 RID: 44966 RVA: 0x0041945B File Offset: 0x0041765B
	public virtual void SnapToExit()
	{
		this.RotationOffset = this.RotationExitOffset;
		this.PositionOffset = this.PositionExitOffset;
		this.SnapSprings();
		this.SnapPivot();
	}

	// Token: 0x0600AFA7 RID: 44967 RVA: 0x00419484 File Offset: 0x00417684
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

	// Token: 0x0600AFA8 RID: 44968 RVA: 0x004195D4 File Offset: 0x004177D4
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

	// Token: 0x0600AFA9 RID: 44969 RVA: 0x00419634 File Offset: 0x00417834
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

	// Token: 0x0600AFAA RID: 44970 RVA: 0x0041975C File Offset: 0x0041795C
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

	// Token: 0x0600AFAB RID: 44971 RVA: 0x004197B8 File Offset: 0x004179B8
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

	// Token: 0x0600AFAC RID: 44972 RVA: 0x00419825 File Offset: 0x00417A25
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_HeadImpact(float impact)
	{
		this.AddForce(Vector3.zero, Vector3.forward * (impact * 20f) * Time.timeScale);
	}

	// Token: 0x0600AFAD RID: 44973 RVA: 0x0041984D File Offset: 0x00417A4D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraGroundStomp(float impact)
	{
		this.AddForce(Vector3.zero, new Vector3(-0.25f, 0f, 0f) * impact);
	}

	// Token: 0x0600AFAE RID: 44974 RVA: 0x00419874 File Offset: 0x00417A74
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraBombShake(float impact)
	{
		this.AddForce(Vector3.zero, new Vector3(-0.3f, 0.1f, 0.5f) * impact);
	}

	// Token: 0x0600AFAF RID: 44975 RVA: 0x0041989B File Offset: 0x00417A9B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraToggle3rdPerson()
	{
		this.RefreshWeaponModel();
	}

	// Token: 0x17001454 RID: 5204
	// (get) Token: 0x0600AFB0 RID: 44976 RVA: 0x004198A4 File Offset: 0x00417AA4
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

	// Token: 0x04008315 RID: 33557
	public GameObject WeaponPrefab;

	// Token: 0x04008316 RID: 33558
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CharacterController Controller;

	// Token: 0x04008317 RID: 33559
	public float RenderingZoomDamping = 0.5f;

	// Token: 0x04008318 RID: 33560
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FinalZoomTime;

	// Token: 0x04008319 RID: 33561
	public float RenderingFieldOfView = 75f;

	// Token: 0x0400831A RID: 33562
	public float originalRenderingFieldOfView;

	// Token: 0x0400831B RID: 33563
	public Vector2 RenderingClippingPlanes = new Vector2(0.01f, 10f);

	// Token: 0x0400831C RID: 33564
	public float RenderingZScale = 1f;

	// Token: 0x0400831D RID: 33565
	public float PositionSpringStiffness = 0.01f;

	// Token: 0x0400831E RID: 33566
	public float PositionSpringDamping = 0.25f;

	// Token: 0x0400831F RID: 33567
	public float PositionFallRetract = 1f;

	// Token: 0x04008320 RID: 33568
	public float PositionPivotSpringStiffness = 0.01f;

	// Token: 0x04008321 RID: 33569
	public float PositionPivotSpringDamping = 0.25f;

	// Token: 0x04008322 RID: 33570
	public float PositionKneeling = 0.06f;

	// Token: 0x04008323 RID: 33571
	public int PositionKneelingSoftness = 1;

	// Token: 0x04008324 RID: 33572
	public Vector3 PositionWalkSlide = new Vector3(0.5f, 0.75f, 0.5f);

	// Token: 0x04008325 RID: 33573
	public Vector3 PositionPivot = Vector3.zero;

	// Token: 0x04008326 RID: 33574
	public Vector3 RotationPivot = Vector3.zero;

	// Token: 0x04008327 RID: 33575
	public float PositionInputVelocityScale = 1f;

	// Token: 0x04008328 RID: 33576
	public float PositionMaxInputVelocity = 25f;

	// Token: 0x04008329 RID: 33577
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_PositionSpring;

	// Token: 0x0400832A RID: 33578
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_PositionPivotSpring;

	// Token: 0x0400832B RID: 33579
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_RotationPivotSpring;

	// Token: 0x0400832C RID: 33580
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CameraMatrixOverride m_CameraMatrixOverride;

	// Token: 0x0400832D RID: 33581
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_WeaponGroup;

	// Token: 0x0400832E RID: 33582
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_Pivot;

	// Token: 0x0400832F RID: 33583
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_WeaponGroupTransform;

	// Token: 0x04008330 RID: 33584
	public float RotationSpringStiffness = 0.01f;

	// Token: 0x04008331 RID: 33585
	public float RotationSpringDamping = 0.25f;

	// Token: 0x04008332 RID: 33586
	public float RotationPivotSpringStiffness = 0.01f;

	// Token: 0x04008333 RID: 33587
	public float RotationPivotSpringDamping = 0.25f;

	// Token: 0x04008334 RID: 33588
	public float RotationKneeling;

	// Token: 0x04008335 RID: 33589
	public int RotationKneelingSoftness = 1;

	// Token: 0x04008336 RID: 33590
	public Vector3 RotationLookSway = new Vector3(1f, 0.7f, 0f);

	// Token: 0x04008337 RID: 33591
	public Vector3 RotationStrafeSway = new Vector3(0.3f, 1f, 1.5f);

	// Token: 0x04008338 RID: 33592
	public Vector3 RotationFallSway = new Vector3(1f, -0.5f, -3f);

	// Token: 0x04008339 RID: 33593
	public float RotationSlopeSway = 0.5f;

	// Token: 0x0400833A RID: 33594
	public float RotationInputVelocityScale = 1f;

	// Token: 0x0400833B RID: 33595
	public float RotationMaxInputVelocity = 15f;

	// Token: 0x0400833C RID: 33596
	public Vector3 RotationFallSwayAbsoluteSpringLimit = new Vector3(0f, 0f, 3f);

	// Token: 0x0400833D RID: 33597
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_RotationSpring;

	// Token: 0x0400833E RID: 33598
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_SwayVel = Vector3.zero;

	// Token: 0x0400833F RID: 33599
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_FallSway = Vector3.zero;

	// Token: 0x04008340 RID: 33600
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_FallSwaySpringLimit = Vector3.zero;

	// Token: 0x04008341 RID: 33601
	public float RetractionDistance;

	// Token: 0x04008342 RID: 33602
	public Vector2 RetractionOffset = new Vector2(0f, 0f);

	// Token: 0x04008343 RID: 33603
	public float RetractionRelaxSpeed = 0.25f;

	// Token: 0x04008344 RID: 33604
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_DrawRetractionDebugLine;

	// Token: 0x04008345 RID: 33605
	public float ShakeSpeed = 0.05f;

	// Token: 0x04008346 RID: 33606
	public Vector3 ShakeAmplitude = new Vector3(0.25f, 0f, 2f);

	// Token: 0x04008347 RID: 33607
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Shake = Vector3.zero;

	// Token: 0x04008348 RID: 33608
	public Vector4 BobRate = new Vector4(0.9f, 0.45f, 0f, 0f);

	// Token: 0x04008349 RID: 33609
	public Vector4 BobAmplitude = new Vector4(0.35f, 0.5f, 0f, 0f);

	// Token: 0x0400834A RID: 33610
	public float BobInputVelocityScale = 1f;

	// Token: 0x0400834B RID: 33611
	public float BobMaxInputVelocity = 100f;

	// Token: 0x0400834C RID: 33612
	public bool BobRequireGroundContact = true;

	// Token: 0x0400834D RID: 33613
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastBobSpeed;

	// Token: 0x0400834E RID: 33614
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector4 m_CurrentBobAmp = Vector4.zero;

	// Token: 0x0400834F RID: 33615
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector4 m_CurrentBobVal = Vector4.zero;

	// Token: 0x04008350 RID: 33616
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_BobSpeed;

	// Token: 0x04008351 RID: 33617
	public Vector3 StepPositionForce = new Vector3(0f, -0.0012f, -0.0012f);

	// Token: 0x04008352 RID: 33618
	public Vector3 StepRotationForce = new Vector3(0f, 0f, 0f);

	// Token: 0x04008353 RID: 33619
	public int StepSoftness = 4;

	// Token: 0x04008354 RID: 33620
	public float StepMinVelocity;

	// Token: 0x04008355 RID: 33621
	public float StepPositionBalance;

	// Token: 0x04008356 RID: 33622
	public float StepRotationBalance;

	// Token: 0x04008357 RID: 33623
	public float StepForceScale = 1f;

	// Token: 0x04008358 RID: 33624
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastUpBob;

	// Token: 0x04008359 RID: 33625
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_BobWasElevating;

	// Token: 0x0400835A RID: 33626
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PosStep = Vector3.zero;

	// Token: 0x0400835B RID: 33627
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_RotStep = Vector3.zero;

	// Token: 0x0400835C RID: 33628
	public bool LookDownActive;

	// Token: 0x0400835D RID: 33629
	public float LookDownYawLimit = 60f;

	// Token: 0x0400835E RID: 33630
	public Vector3 LookDownPositionOffsetMiddle = new Vector3(0.32f, -0.37f, 0.78f);

	// Token: 0x0400835F RID: 33631
	public Vector3 LookDownPositionOffsetLeft = new Vector3(0.27f, -0.31f, 0.7f);

	// Token: 0x04008360 RID: 33632
	public Vector3 LookDownPositionOffsetRight = new Vector3(0.6f, -0.41f, 0.86f);

	// Token: 0x04008361 RID: 33633
	public float LookDownPositionSpringPower = 1f;

	// Token: 0x04008362 RID: 33634
	public Vector3 LookDownRotationOffsetMiddle = new Vector3(-3.9f, 2.24f, 4.69f);

	// Token: 0x04008363 RID: 33635
	public Vector3 LookDownRotationOffsetLeft = new Vector3(-7f, -10.5f, 15.6f);

	// Token: 0x04008364 RID: 33636
	public Vector3 LookDownRotationOffsetRight = new Vector3(-9.2f, -9.8f, 48.84f);

	// Token: 0x04008365 RID: 33637
	public float LookDownRotationSpringPower = 1f;

	// Token: 0x04008366 RID: 33638
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentPosRestState = Vector3.zero;

	// Token: 0x04008367 RID: 33639
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentRotRestState = Vector3.zero;

	// Token: 0x04008368 RID: 33640
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AnimationCurve m_LookDownCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.8f, 0.2f, 0.9f, 1.5f),
		new Keyframe(1f, 1f)
	});

	// Token: 0x04008369 RID: 33641
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LookDownPitch;

	// Token: 0x0400836A RID: 33642
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LookDownYaw;

	// Token: 0x0400836B RID: 33643
	public AudioClip SoundWield;

	// Token: 0x0400836C RID: 33644
	public AudioClip SoundUnWield;

	// Token: 0x0400836D RID: 33645
	public AnimationClip AnimationWield;

	// Token: 0x0400836E RID: 33646
	public AnimationClip AnimationUnWield;

	// Token: 0x0400836F RID: 33647
	public List<global::UnityEngine.Object> AnimationAmbient = new List<global::UnityEngine.Object>();

	// Token: 0x04008370 RID: 33648
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<bool> m_AmbAnimPlayed = new List<bool>();

	// Token: 0x04008371 RID: 33649
	public Vector2 AmbientInterval = new Vector2(2.5f, 7.5f);

	// Token: 0x04008372 RID: 33650
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_CurrentAmbientAnimation;

	// Token: 0x04008373 RID: 33651
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_AnimationAmbientTimer = new vp_Timer.Handle();

	// Token: 0x04008374 RID: 33652
	public Vector3 PositionExitOffset = new Vector3(0f, -1f, 0f);

	// Token: 0x04008375 RID: 33653
	public Vector3 RotationExitOffset = new Vector3(40f, 0f, 0f);

	// Token: 0x04008376 RID: 33654
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_LookInput = Vector2.zero;

	// Token: 0x04008377 RID: 33655
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float LOOKDOWNSPEED = 2f;

	// Token: 0x04008378 RID: 33656
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_FPPlayer;
}
