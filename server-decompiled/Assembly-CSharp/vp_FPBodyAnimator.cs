using System;
using System.Collections;
using UnityEngine;

// Token: 0x020015F1 RID: 5617
public class vp_FPBodyAnimator : vp_BodyAnimator
{
	// Token: 0x17001439 RID: 5177
	// (get) Token: 0x0600AF37 RID: 44855 RVA: 0x00414E02 File Offset: 0x00413002
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

	// Token: 0x1700143A RID: 5178
	// (get) Token: 0x0600AF38 RID: 44856 RVA: 0x00414E2E File Offset: 0x0041302E
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

	// Token: 0x1700143B RID: 5179
	// (get) Token: 0x0600AF39 RID: 44857 RVA: 0x00414E5C File Offset: 0x0041305C
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

	// Token: 0x1700143C RID: 5180
	// (get) Token: 0x0600AF3A RID: 44858 RVA: 0x00414EE4 File Offset: 0x004130E4
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

	// Token: 0x0600AF3B RID: 44859 RVA: 0x00414F68 File Offset: 0x00413168
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.InitMaterials();
		this.m_WasFirstPersonLastFrame = base.Player.IsFirstPerson.Get();
		this.FPCamera.HasCollision = true;
		base.Player.IsFirstPerson.Set(true);
	}

	// Token: 0x0600AF3C RID: 44860 RVA: 0x00414FBE File Offset: 0x004131BE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		base.OnEnable();
		this.RefreshMaterials();
	}

	// Token: 0x0600AF3D RID: 44861 RVA: 0x00414FCC File Offset: 0x004131CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
	}

	// Token: 0x0600AF3E RID: 44862 RVA: 0x00414FD4 File Offset: 0x004131D4
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

	// Token: 0x0600AF3F RID: 44863 RVA: 0x00415030 File Offset: 0x00413230
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator RefreshMaterialsOnEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		this.RefreshMaterials();
		yield break;
	}

	// Token: 0x0600AF40 RID: 44864 RVA: 0x00415040 File Offset: 0x00413240
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

	// Token: 0x0600AF41 RID: 44865 RVA: 0x00415144 File Offset: 0x00413344
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

	// Token: 0x0600AF42 RID: 44866 RVA: 0x004152D8 File Offset: 0x004134D8
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

	// Token: 0x0600AF43 RID: 44867 RVA: 0x00415404 File Offset: 0x00413604
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateCameraCollision()
	{
		this.FPCamera.DoCameraCollision();
		if (this.FPCamera.CollisionVector != Vector3.zero)
		{
			base.Transform.position += this.FPCamera.CollisionVector;
		}
	}

	// Token: 0x0600AF44 RID: 44868 RVA: 0x00415454 File Offset: 0x00413654
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateGrounding()
	{
		this.m_Grounded = this.FPController.Grounded;
	}

	// Token: 0x0600AF45 RID: 44869 RVA: 0x00415468 File Offset: 0x00413668
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

	// Token: 0x0600AF46 RID: 44870 RVA: 0x00415520 File Offset: 0x00413720
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

	// Token: 0x0600AF47 RID: 44871 RVA: 0x00415570 File Offset: 0x00413770
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

	// Token: 0x0600AF48 RID: 44872 RVA: 0x004156F0 File Offset: 0x004138F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetIsMoving()
	{
		return Vector3.Scale(base.Player.MotorThrottle.Get(), Vector3.right + Vector3.forward).magnitude > 0.01f;
	}

	// Token: 0x0600AF49 RID: 44873 RVA: 0x00415735 File Offset: 0x00413935
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Vector3 GetLookPoint()
	{
		return this.FPCamera.LookPoint;
	}

	// Token: 0x1700143D RID: 5181
	// (get) Token: 0x0600AF4A RID: 44874 RVA: 0x00415742 File Offset: 0x00413942
	public override Vector3 OnValue_LookPoint
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.GetLookPoint();
		}
	}

	// Token: 0x0600AF4B RID: 44875 RVA: 0x0041574A File Offset: 0x0041394A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnMessage_CameraToggle3rdPerson()
	{
		base.OnMessage_CameraToggle3rdPerson();
		base.StartCoroutine(this.RefreshMaterialsOnEndOfFrame());
	}

	// Token: 0x0600AF4C RID: 44876 RVA: 0x0041575F File Offset: 0x0041395F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_SetWeapon()
	{
		this.RefreshMaterials();
	}

	// Token: 0x0600AF4D RID: 44877 RVA: 0x00415767 File Offset: 0x00413967
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnStart_Climb()
	{
		base.OnStart_Climb();
		this.RefreshMaterials();
	}

	// Token: 0x0600AF4E RID: 44878 RVA: 0x0041575F File Offset: 0x0041395F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Climb()
	{
		this.RefreshMaterials();
	}

	// Token: 0x0600AF4F RID: 44879 RVA: 0x00415775 File Offset: 0x00413975
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnStart_Dead()
	{
		base.OnStart_Dead();
		this.RefreshMaterials();
	}

	// Token: 0x040082ED RID: 33517
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPController m_FPController;

	// Token: 0x040082EE RID: 33518
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_FPCamera;

	// Token: 0x040082EF RID: 33519
	public Vector3 EyeOffset = new Vector3(0f, -0.08f, -0.1f);

	// Token: 0x040082F0 RID: 33520
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_WasFirstPersonLastFrame;

	// Token: 0x040082F1 RID: 33521
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_DefaultCamHeight;

	// Token: 0x040082F2 RID: 33522
	public float LookDownZoomFactor = 15f;

	// Token: 0x040082F3 RID: 33523
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float LookDownForwardOffset = 0.05f;

	// Token: 0x040082F4 RID: 33524
	public bool ShowUnarmedArms = true;

	// Token: 0x040082F5 RID: 33525
	public Material InvisibleMaterial;

	// Token: 0x040082F6 RID: 33526
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material[] m_FirstPersonMaterials;

	// Token: 0x040082F7 RID: 33527
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material[] m_FirstPersonWithArmsMaterials;

	// Token: 0x040082F8 RID: 33528
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material[] m_ThirdPersonMaterials;

	// Token: 0x040082F9 RID: 33529
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material[] m_InvisiblePersonMaterials;

	// Token: 0x040082FA RID: 33530
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPWeaponShooter m_CurrentShooter;
}
