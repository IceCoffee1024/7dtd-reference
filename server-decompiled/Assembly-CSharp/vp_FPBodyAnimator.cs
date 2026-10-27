using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001622 RID: 5666
public class vp_FPBodyAnimator : vp_BodyAnimator
{
	// Token: 0x17001488 RID: 5256
	// (get) Token: 0x0600B021 RID: 45089 RVA: 0x0041AEE2 File Offset: 0x004190E2
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

	// Token: 0x17001489 RID: 5257
	// (get) Token: 0x0600B022 RID: 45090 RVA: 0x0041AF0E File Offset: 0x0041910E
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

	// Token: 0x1700148A RID: 5258
	// (get) Token: 0x0600B023 RID: 45091 RVA: 0x0041AF3C File Offset: 0x0041913C
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

	// Token: 0x1700148B RID: 5259
	// (get) Token: 0x0600B024 RID: 45092 RVA: 0x0041AFC4 File Offset: 0x004191C4
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

	// Token: 0x0600B025 RID: 45093 RVA: 0x0041B048 File Offset: 0x00419248
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.InitMaterials();
		this.m_WasFirstPersonLastFrame = base.Player.IsFirstPerson.Get();
		this.FPCamera.HasCollision = true;
		base.Player.IsFirstPerson.Set(true);
	}

	// Token: 0x0600B026 RID: 45094 RVA: 0x0041B09E File Offset: 0x0041929E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		base.OnEnable();
		this.RefreshMaterials();
	}

	// Token: 0x0600B027 RID: 45095 RVA: 0x0041B0AC File Offset: 0x004192AC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
	}

	// Token: 0x0600B028 RID: 45096 RVA: 0x0041B0B4 File Offset: 0x004192B4
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

	// Token: 0x0600B029 RID: 45097 RVA: 0x0041B110 File Offset: 0x00419310
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator RefreshMaterialsOnEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		this.RefreshMaterials();
		yield break;
	}

	// Token: 0x0600B02A RID: 45098 RVA: 0x0041B120 File Offset: 0x00419320
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

	// Token: 0x0600B02B RID: 45099 RVA: 0x0041B224 File Offset: 0x00419424
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

	// Token: 0x0600B02C RID: 45100 RVA: 0x0041B3B8 File Offset: 0x004195B8
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

	// Token: 0x0600B02D RID: 45101 RVA: 0x0041B4E4 File Offset: 0x004196E4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateCameraCollision()
	{
		this.FPCamera.DoCameraCollision();
		if (this.FPCamera.CollisionVector != Vector3.zero)
		{
			base.Transform.position += this.FPCamera.CollisionVector;
		}
	}

	// Token: 0x0600B02E RID: 45102 RVA: 0x0041B534 File Offset: 0x00419734
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateGrounding()
	{
		this.m_Grounded = this.FPController.Grounded;
	}

	// Token: 0x0600B02F RID: 45103 RVA: 0x0041B548 File Offset: 0x00419748
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

	// Token: 0x0600B030 RID: 45104 RVA: 0x0041B600 File Offset: 0x00419800
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

	// Token: 0x0600B031 RID: 45105 RVA: 0x0041B650 File Offset: 0x00419850
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

	// Token: 0x0600B032 RID: 45106 RVA: 0x0041B7D0 File Offset: 0x004199D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetIsMoving()
	{
		return Vector3.Scale(base.Player.MotorThrottle.Get(), Vector3.right + Vector3.forward).magnitude > 0.01f;
	}

	// Token: 0x0600B033 RID: 45107 RVA: 0x0041B815 File Offset: 0x00419A15
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Vector3 GetLookPoint()
	{
		return this.FPCamera.LookPoint;
	}

	// Token: 0x1700148C RID: 5260
	// (get) Token: 0x0600B034 RID: 45108 RVA: 0x0041B822 File Offset: 0x00419A22
	public override Vector3 OnValue_LookPoint
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.GetLookPoint();
		}
	}

	// Token: 0x0600B035 RID: 45109 RVA: 0x0041B82A File Offset: 0x00419A2A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnMessage_CameraToggle3rdPerson()
	{
		base.OnMessage_CameraToggle3rdPerson();
		base.StartCoroutine(this.RefreshMaterialsOnEndOfFrame());
	}

	// Token: 0x0600B036 RID: 45110 RVA: 0x0041B83F File Offset: 0x00419A3F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_SetWeapon()
	{
		this.RefreshMaterials();
	}

	// Token: 0x0600B037 RID: 45111 RVA: 0x0041B847 File Offset: 0x00419A47
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnStart_Climb()
	{
		base.OnStart_Climb();
		this.RefreshMaterials();
	}

	// Token: 0x0600B038 RID: 45112 RVA: 0x0041B83F File Offset: 0x00419A3F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Climb()
	{
		this.RefreshMaterials();
	}

	// Token: 0x0600B039 RID: 45113 RVA: 0x0041B855 File Offset: 0x00419A55
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnStart_Dead()
	{
		base.OnStart_Dead();
		this.RefreshMaterials();
	}

	// Token: 0x04008384 RID: 33668
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPController m_FPController;

	// Token: 0x04008385 RID: 33669
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_FPCamera;

	// Token: 0x04008386 RID: 33670
	public Vector3 EyeOffset = new Vector3(0f, -0.08f, -0.1f);

	// Token: 0x04008387 RID: 33671
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_WasFirstPersonLastFrame;

	// Token: 0x04008388 RID: 33672
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_DefaultCamHeight;

	// Token: 0x04008389 RID: 33673
	public float LookDownZoomFactor = 15f;

	// Token: 0x0400838A RID: 33674
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float LookDownForwardOffset = 0.05f;

	// Token: 0x0400838B RID: 33675
	public bool ShowUnarmedArms = true;

	// Token: 0x0400838C RID: 33676
	public Material InvisibleMaterial;

	// Token: 0x0400838D RID: 33677
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material[] m_FirstPersonMaterials;

	// Token: 0x0400838E RID: 33678
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material[] m_FirstPersonWithArmsMaterials;

	// Token: 0x0400838F RID: 33679
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material[] m_ThirdPersonMaterials;

	// Token: 0x04008390 RID: 33680
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material[] m_InvisiblePersonMaterials;

	// Token: 0x04008391 RID: 33681
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPWeaponShooter m_CurrentShooter;
}
