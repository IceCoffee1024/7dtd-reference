using System;
using System.Collections;
using UnityEngine;

// Token: 0x020015D6 RID: 5590
public class vp_FPBodyAnimator : vp_BodyAnimator
{
	// Token: 0x17001400 RID: 5120
	// (get) Token: 0x0600AE3A RID: 44602 RVA: 0x0040FAC2 File Offset: 0x0040DCC2
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

	// Token: 0x17001401 RID: 5121
	// (get) Token: 0x0600AE3B RID: 44603 RVA: 0x0040FAEE File Offset: 0x0040DCEE
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

	// Token: 0x17001402 RID: 5122
	// (get) Token: 0x0600AE3C RID: 44604 RVA: 0x0040FB1C File Offset: 0x0040DD1C
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

	// Token: 0x17001403 RID: 5123
	// (get) Token: 0x0600AE3D RID: 44605 RVA: 0x0040FBA4 File Offset: 0x0040DDA4
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

	// Token: 0x0600AE3E RID: 44606 RVA: 0x0040FC28 File Offset: 0x0040DE28
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.InitMaterials();
		this.m_WasFirstPersonLastFrame = base.Player.IsFirstPerson.Get();
		this.FPCamera.HasCollision = true;
		base.Player.IsFirstPerson.Set(true);
	}

	// Token: 0x0600AE3F RID: 44607 RVA: 0x0040FC7E File Offset: 0x0040DE7E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		base.OnEnable();
		this.RefreshMaterials();
	}

	// Token: 0x0600AE40 RID: 44608 RVA: 0x0040FC8C File Offset: 0x0040DE8C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
	}

	// Token: 0x0600AE41 RID: 44609 RVA: 0x0040FC94 File Offset: 0x0040DE94
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

	// Token: 0x0600AE42 RID: 44610 RVA: 0x0040FCF0 File Offset: 0x0040DEF0
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator RefreshMaterialsOnEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		this.RefreshMaterials();
		yield break;
	}

	// Token: 0x0600AE43 RID: 44611 RVA: 0x0040FD00 File Offset: 0x0040DF00
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

	// Token: 0x0600AE44 RID: 44612 RVA: 0x0040FE04 File Offset: 0x0040E004
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

	// Token: 0x0600AE45 RID: 44613 RVA: 0x0040FF98 File Offset: 0x0040E198
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

	// Token: 0x0600AE46 RID: 44614 RVA: 0x004100C4 File Offset: 0x0040E2C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateCameraCollision()
	{
		this.FPCamera.DoCameraCollision();
		if (this.FPCamera.CollisionVector != Vector3.zero)
		{
			base.Transform.position += this.FPCamera.CollisionVector;
		}
	}

	// Token: 0x0600AE47 RID: 44615 RVA: 0x00410114 File Offset: 0x0040E314
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateGrounding()
	{
		this.m_Grounded = this.FPController.Grounded;
	}

	// Token: 0x0600AE48 RID: 44616 RVA: 0x00410128 File Offset: 0x0040E328
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

	// Token: 0x0600AE49 RID: 44617 RVA: 0x004101E0 File Offset: 0x0040E3E0
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

	// Token: 0x0600AE4A RID: 44618 RVA: 0x00410230 File Offset: 0x0040E430
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

	// Token: 0x0600AE4B RID: 44619 RVA: 0x004103B0 File Offset: 0x0040E5B0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetIsMoving()
	{
		return Vector3.Scale(base.Player.MotorThrottle.Get(), Vector3.right + Vector3.forward).magnitude > 0.01f;
	}

	// Token: 0x0600AE4C RID: 44620 RVA: 0x004103F5 File Offset: 0x0040E5F5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Vector3 GetLookPoint()
	{
		return this.FPCamera.LookPoint;
	}

	// Token: 0x17001404 RID: 5124
	// (get) Token: 0x0600AE4D RID: 44621 RVA: 0x00410402 File Offset: 0x0040E602
	public override Vector3 OnValue_LookPoint
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.GetLookPoint();
		}
	}

	// Token: 0x0600AE4E RID: 44622 RVA: 0x0041040A File Offset: 0x0040E60A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnMessage_CameraToggle3rdPerson()
	{
		base.OnMessage_CameraToggle3rdPerson();
		base.StartCoroutine(this.RefreshMaterialsOnEndOfFrame());
	}

	// Token: 0x0600AE4F RID: 44623 RVA: 0x0041041F File Offset: 0x0040E61F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_SetWeapon()
	{
		this.RefreshMaterials();
	}

	// Token: 0x0600AE50 RID: 44624 RVA: 0x00410427 File Offset: 0x0040E627
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnStart_Climb()
	{
		base.OnStart_Climb();
		this.RefreshMaterials();
	}

	// Token: 0x0600AE51 RID: 44625 RVA: 0x0041041F File Offset: 0x0040E61F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Climb()
	{
		this.RefreshMaterials();
	}

	// Token: 0x0600AE52 RID: 44626 RVA: 0x00410435 File Offset: 0x0040E635
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnStart_Dead()
	{
		base.OnStart_Dead();
		this.RefreshMaterials();
	}

	// Token: 0x04008214 RID: 33300
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPController m_FPController;

	// Token: 0x04008215 RID: 33301
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_FPCamera;

	// Token: 0x04008216 RID: 33302
	public Vector3 EyeOffset = new Vector3(0f, -0.08f, -0.1f);

	// Token: 0x04008217 RID: 33303
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_WasFirstPersonLastFrame;

	// Token: 0x04008218 RID: 33304
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_DefaultCamHeight;

	// Token: 0x04008219 RID: 33305
	public float LookDownZoomFactor = 15f;

	// Token: 0x0400821A RID: 33306
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float LookDownForwardOffset = 0.05f;

	// Token: 0x0400821B RID: 33307
	public bool ShowUnarmedArms = true;

	// Token: 0x0400821C RID: 33308
	public Material InvisibleMaterial;

	// Token: 0x0400821D RID: 33309
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material[] m_FirstPersonMaterials;

	// Token: 0x0400821E RID: 33310
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material[] m_FirstPersonWithArmsMaterials;

	// Token: 0x0400821F RID: 33311
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material[] m_ThirdPersonMaterials;

	// Token: 0x04008220 RID: 33312
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material[] m_InvisiblePersonMaterials;

	// Token: 0x04008221 RID: 33313
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPWeaponShooter m_CurrentShooter;
}
