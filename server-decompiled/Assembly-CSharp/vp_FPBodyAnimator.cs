using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001610 RID: 5648
public class vp_FPBodyAnimator : vp_BodyAnimator
{
	// Token: 0x1700147C RID: 5244
	// (get) Token: 0x0600AFC9 RID: 45001 RVA: 0x00418ADE File Offset: 0x00416CDE
	public vp_FPCamera FPCamera
	{
		get
		{
			if (this.m_FPCamera == null)
			{
				this.m_FPCamera = base.transform.root.GetComponentInChildren<vp_FPCamera>();
			}
			return this.m_FPCamera;
		}
	}

	// Token: 0x1700147D RID: 5245
	// (get) Token: 0x0600AFCA RID: 45002 RVA: 0x00418B0A File Offset: 0x00416D0A
	public vp_FPController FPController
	{
		get
		{
			if (this.m_FPController == null)
			{
				this.m_FPController = base.transform.root.GetComponent<vp_FPController>();
			}
			return this.m_FPController;
		}
	}

	// Token: 0x1700147E RID: 5246
	// (get) Token: 0x0600AFCB RID: 45003 RVA: 0x00418B38 File Offset: 0x00416D38
	public vp_FPWeaponShooter CurrentShooter
	{
		get
		{
			if ((this.m_CurrentShooter == null || (this.m_CurrentShooter != null && (!this.m_CurrentShooter.enabled || !vp_Utility.IsActive(this.m_CurrentShooter.gameObject)))) && base.WeaponHandler != null && base.WeaponHandler.CurrentWeapon != null)
			{
				this.m_CurrentShooter = base.WeaponHandler.CurrentWeapon.GetComponentInChildren<vp_FPWeaponShooter>();
			}
			return this.m_CurrentShooter;
		}
	}

	// Token: 0x1700147F RID: 5247
	// (get) Token: 0x0600AFCC RID: 45004 RVA: 0x00418BC0 File Offset: 0x00416DC0
	public float DefaultCamHeight
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_DefaultCamHeight == 0f)
			{
				if (this.FPCamera != null && this.FPCamera.DefaultState != null && this.FPCamera.DefaultState.Preset != null)
				{
					this.m_DefaultCamHeight = ((Vector3)this.FPCamera.DefaultState.Preset.GetFieldValue("PositionOffset")).y;
				}
				else
				{
					this.m_DefaultCamHeight = 1.75f;
				}
			}
			return this.m_DefaultCamHeight;
		}
	}

	// Token: 0x0600AFCD RID: 45005 RVA: 0x00418C44 File Offset: 0x00416E44
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.InitMaterials();
		this.m_WasFirstPersonLastFrame = base.Player.IsFirstPerson.Get();
		this.FPCamera.HasCollision = true;
		base.Player.IsFirstPerson.Set(true);
	}

	// Token: 0x0600AFCE RID: 45006 RVA: 0x00418C9A File Offset: 0x00416E9A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		base.OnEnable();
		this.RefreshMaterials();
	}

	// Token: 0x0600AFCF RID: 45007 RVA: 0x00418CA8 File Offset: 0x00416EA8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
	}

	// Token: 0x0600AFD0 RID: 45008 RVA: 0x00418CB0 File Offset: 0x00416EB0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void LateUpdate()
	{
		base.LateUpdate();
		if (Time.timeScale == 0f)
		{
			return;
		}
		if (base.Player.IsFirstPerson.Get())
		{
			this.UpdatePosition();
			this.UpdateCameraPosition();
			this.UpdateCameraCollision();
		}
		else
		{
			this.FPCamera.DoCameraCollision();
		}
		this.UpdateFirePosition();
	}

	// Token: 0x0600AFD1 RID: 45009 RVA: 0x00418D0C File Offset: 0x00416F0C
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator RefreshMaterialsOnEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		this.RefreshMaterials();
		yield break;
	}

	// Token: 0x0600AFD2 RID: 45010 RVA: 0x00418D1C File Offset: 0x00416F1C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void RefreshMaterials()
	{
		if (this.InvisibleMaterial == null)
		{
			Debug.LogWarning("Warning (" + ((this != null) ? this.ToString() : null) + ") No invisible material has been set. Head and arms will look buggy in first person.");
			return;
		}
		if (!base.Player.IsFirstPerson.Get())
		{
			if (this.m_ThirdPersonMaterials != null)
			{
				base.Renderer.materials = this.m_ThirdPersonMaterials;
				return;
			}
		}
		else if (!base.Player.Dead.Active)
		{
			if (this.ShowUnarmedArms && base.Player.CurrentWeaponIndex.Get() < 1 && !base.Player.Climb.Active)
			{
				if (this.m_FirstPersonWithArmsMaterials != null)
				{
					base.Renderer.materials = this.m_FirstPersonWithArmsMaterials;
					return;
				}
			}
			else if (this.m_FirstPersonMaterials != null)
			{
				base.Renderer.materials = this.m_FirstPersonMaterials;
				return;
			}
		}
		else if (this.m_InvisiblePersonMaterials != null)
		{
			base.Renderer.materials = this.m_InvisiblePersonMaterials;
		}
	}

	// Token: 0x0600AFD3 RID: 45011 RVA: 0x00418E20 File Offset: 0x00417020
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdatePosition()
	{
		base.Transform.position = this.FPController.SmoothPosition + this.FPController.SkinWidth * Vector3.down;
		if (base.Player.IsFirstPerson.Get() && !base.Player.Climb.Active)
		{
			if (this.m_HeadLookBones != null && this.m_HeadLookBones.Count > 0)
			{
				base.Transform.position = Vector3.Lerp(base.Transform.position, base.Transform.position + (this.FPCamera.Transform.position - this.m_HeadLookBones[0].transform.position), Mathf.Lerp(1f, 0f, Mathf.Max(0f, base.Player.Rotation.Get().x / 60f)));
			}
			else
			{
				Debug.LogWarning("Warning (" + ((this != null) ? this.ToString() : null) + ") No headlookbones have been assigned!");
			}
		}
		else
		{
			base.Transform.localPosition = Vector3.Scale(base.Transform.localPosition, Vector3.right + Vector3.up);
		}
		if (base.Player.Climb.Active)
		{
			base.Transform.localPosition += this.ClimbOffset;
		}
	}

	// Token: 0x0600AFD4 RID: 45012 RVA: 0x00418FB4 File Offset: 0x004171B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateCameraPosition()
	{
		this.FPCamera.transform.position = this.m_HeadLookBones[0].transform.position;
		float num = Mathf.Max(0f, (base.Player.Rotation.Get().x - 45f) / 45f);
		num = Mathf.SmoothStep(0f, 1f, num);
		this.FPCamera.transform.localPosition = new Vector3(this.FPCamera.transform.localPosition.x, this.FPCamera.transform.localPosition.y, this.FPCamera.transform.localPosition.z + num * (base.Player.Crouch.Active ? 0f : this.LookDownForwardOffset));
		this.FPCamera.Transform.localPosition -= this.EyeOffset;
		this.FPCamera.ZoomOffset = -this.LookDownZoomFactor * num;
		this.FPCamera.RefreshZoom();
	}

	// Token: 0x0600AFD5 RID: 45013 RVA: 0x004190E0 File Offset: 0x004172E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateCameraCollision()
	{
		this.FPCamera.DoCameraCollision();
		if (this.FPCamera.CollisionVector != Vector3.zero)
		{
			base.Transform.position += this.FPCamera.CollisionVector;
		}
	}

	// Token: 0x0600AFD6 RID: 45014 RVA: 0x00419130 File Offset: 0x00417330
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateGrounding()
	{
		this.m_Grounded = this.FPController.Grounded;
	}

	// Token: 0x0600AFD7 RID: 45015 RVA: 0x00419144 File Offset: 0x00417344
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateDebugInfo()
	{
		if (this.ShowDebugObjects)
		{
			base.DebugLookTarget.transform.position = this.FPCamera.LookPoint;
			base.DebugLookArrow.transform.LookAt(base.DebugLookTarget.transform.position);
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

	// Token: 0x0600AFD8 RID: 45016 RVA: 0x004191FC File Offset: 0x004173FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void UpdateFirePosition()
	{
		if (this.CurrentShooter == null)
		{
			return;
		}
		if (this.CurrentShooter.ProjectileSpawnPoint == null)
		{
			return;
		}
		this.CurrentShooter.FirePosition = this.CurrentShooter.ProjectileSpawnPoint.transform.position;
	}

	// Token: 0x0600AFD9 RID: 45017 RVA: 0x0041924C File Offset: 0x0041744C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InitMaterials()
	{
		if (this.InvisibleMaterial == null)
		{
			Debug.LogWarning("Warning () No invisible material has been set.");
			return;
		}
		this.m_FirstPersonMaterials = new Material[base.Renderer.materials.Length];
		this.m_FirstPersonWithArmsMaterials = new Material[base.Renderer.materials.Length];
		this.m_ThirdPersonMaterials = new Material[base.Renderer.materials.Length];
		this.m_InvisiblePersonMaterials = new Material[base.Renderer.materials.Length];
		for (int i = 0; i < base.Renderer.materials.Length; i++)
		{
			this.m_ThirdPersonMaterials[i] = base.Renderer.materials[i];
			if (base.Renderer.materials[i].name.ToLower().Contains("head") || base.Renderer.materials[i].name.ToLower().Contains("arm"))
			{
				this.m_FirstPersonMaterials[i] = this.InvisibleMaterial;
			}
			else
			{
				this.m_FirstPersonMaterials[i] = base.Renderer.materials[i];
			}
			if (base.Renderer.materials[i].name.ToLower().Contains("head"))
			{
				this.m_FirstPersonWithArmsMaterials[i] = this.InvisibleMaterial;
			}
			else
			{
				this.m_FirstPersonWithArmsMaterials[i] = base.Renderer.materials[i];
			}
			this.m_InvisiblePersonMaterials[i] = this.InvisibleMaterial;
		}
		this.RefreshMaterials();
	}

	// Token: 0x0600AFDA RID: 45018 RVA: 0x004193CC File Offset: 0x004175CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetIsMoving()
	{
		return Vector3.Scale(base.Player.MotorThrottle.Get(), Vector3.right + Vector3.forward).magnitude > 0.01f;
	}

	// Token: 0x0600AFDB RID: 45019 RVA: 0x00419411 File Offset: 0x00417611
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Vector3 GetLookPoint()
	{
		return this.FPCamera.LookPoint;
	}

	// Token: 0x17001480 RID: 5248
	// (get) Token: 0x0600AFDC RID: 45020 RVA: 0x0041941E File Offset: 0x0041761E
	public override Vector3 OnValue_LookPoint
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.GetLookPoint();
		}
	}

	// Token: 0x0600AFDD RID: 45021 RVA: 0x00419426 File Offset: 0x00417626
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnMessage_CameraToggle3rdPerson()
	{
		base.OnMessage_CameraToggle3rdPerson();
		base.StartCoroutine(this.RefreshMaterialsOnEndOfFrame());
	}

	// Token: 0x0600AFDE RID: 45022 RVA: 0x0041943B File Offset: 0x0041763B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_SetWeapon()
	{
		this.RefreshMaterials();
	}

	// Token: 0x0600AFDF RID: 45023 RVA: 0x00419443 File Offset: 0x00417643
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnStart_Climb()
	{
		base.OnStart_Climb();
		this.RefreshMaterials();
	}

	// Token: 0x0600AFE0 RID: 45024 RVA: 0x0041943B File Offset: 0x0041763B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Climb()
	{
		this.RefreshMaterials();
	}

	// Token: 0x0600AFE1 RID: 45025 RVA: 0x00419451 File Offset: 0x00417651
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnStart_Dead()
	{
		base.OnStart_Dead();
		this.RefreshMaterials();
	}

	// Token: 0x04008343 RID: 33603
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPController m_FPController;

	// Token: 0x04008344 RID: 33604
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_FPCamera;

	// Token: 0x04008345 RID: 33605
	public Vector3 EyeOffset = new Vector3(0f, -0.08f, -0.1f);

	// Token: 0x04008346 RID: 33606
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_WasFirstPersonLastFrame;

	// Token: 0x04008347 RID: 33607
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_DefaultCamHeight;

	// Token: 0x04008348 RID: 33608
	public float LookDownZoomFactor = 15f;

	// Token: 0x04008349 RID: 33609
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float LookDownForwardOffset = 0.05f;

	// Token: 0x0400834A RID: 33610
	public bool ShowUnarmedArms = true;

	// Token: 0x0400834B RID: 33611
	public Material InvisibleMaterial;

	// Token: 0x0400834C RID: 33612
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material[] m_FirstPersonMaterials;

	// Token: 0x0400834D RID: 33613
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material[] m_FirstPersonWithArmsMaterials;

	// Token: 0x0400834E RID: 33614
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material[] m_ThirdPersonMaterials;

	// Token: 0x0400834F RID: 33615
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material[] m_InvisiblePersonMaterials;

	// Token: 0x04008350 RID: 33616
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPWeaponShooter m_CurrentShooter;
}
