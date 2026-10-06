using System;
using UnityEngine;

// Token: 0x020003CB RID: 971
public class AutoTurretController : MonoBehaviour, IPowerSystemCamera
{
	// Token: 0x1700037B RID: 891
	// (get) Token: 0x06001D10 RID: 7440 RVA: 0x000AF2CD File Offset: 0x000AD4CD
	// (set) Token: 0x06001D11 RID: 7441 RVA: 0x000AF2D5 File Offset: 0x000AD4D5
	public int UserAccessingId
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = -1;

	// Token: 0x1700037C RID: 892
	// (get) Token: 0x06001D12 RID: 7442 RVA: 0x000AF2DE File Offset: 0x000AD4DE
	public bool IsTurning
	{
		get
		{
			return this.YawController.IsTurning || this.PitchController.IsTurning;
		}
	}

	// Token: 0x1700037D RID: 893
	// (get) Token: 0x06001D13 RID: 7443 RVA: 0x000AF2FA File Offset: 0x000AD4FA
	// (set) Token: 0x06001D14 RID: 7444 RVA: 0x000AF302 File Offset: 0x000AD502
	public TileEntityPoweredRangedTrap TileEntity
	{
		get
		{
			return this.tileEntity;
		}
		set
		{
			this.tileEntity = value;
			this.FireController.TileEntity = value;
		}
	}

	// Token: 0x06001D15 RID: 7445 RVA: 0x000AF317 File Offset: 0x000AD517
	public void OnDestroy()
	{
		this.Cleanup();
		if (this.ConeMaterial != null)
		{
			global::UnityEngine.Object.Destroy(this.ConeMaterial);
		}
	}

	// Token: 0x06001D16 RID: 7446 RVA: 0x000AF338 File Offset: 0x000AD538
	public void Init(DynamicProperties _properties)
	{
		this.IsOn = false;
		this.FireController.Cone = this.Cone;
		this.FireController.Laser = this.Laser;
		this.FireController.Init(_properties, this);
		this.PitchController.Init(_properties);
		this.YawController.Init(_properties);
		if (this.Cone != null)
		{
			MeshRenderer component = this.Cone.GetComponent<MeshRenderer>();
			if (component != null)
			{
				if (component.material != null)
				{
					this.ConeMaterial = component.material;
					this.ConeColor = this.ConeMaterial.GetColor("_Color");
				}
				else if (component.sharedMaterial != null)
				{
					this.ConeMaterial = component.sharedMaterial;
					this.ConeColor = this.ConeMaterial.GetColor("_Color");
				}
			}
		}
		WireManager.Instance.AddPulseObject(this.Cone.gameObject);
	}

	// Token: 0x06001D17 RID: 7447 RVA: 0x000AF430 File Offset: 0x000AD630
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.FireController.IsOn && !this.IsOn)
		{
			this.FireController.OnPoweredOff();
		}
		this.FireController.IsOn = this.IsOn;
		if (this.IsOn)
		{
			this.YawController.UpdateYaw();
			this.PitchController.UpdatePitch();
		}
	}

	// Token: 0x06001D18 RID: 7448 RVA: 0x000AF48C File Offset: 0x000AD68C
	public void SetConeVisible(bool visible)
	{
		if (this.Cone != null)
		{
			this.Cone.gameObject.SetActive(visible);
		}
	}

	// Token: 0x06001D19 RID: 7449 RVA: 0x000AF4AD File Offset: 0x000AD6AD
	public void SetLaserVisible(bool visible)
	{
		if (this.Laser != null)
		{
			this.Laser.gameObject.SetActive(visible);
		}
	}

	// Token: 0x06001D1A RID: 7450 RVA: 0x000AF4CE File Offset: 0x000AD6CE
	public void SetPitch(float pitch)
	{
		this.TileEntity.CenteredPitch = pitch;
	}

	// Token: 0x06001D1B RID: 7451 RVA: 0x000AF4DC File Offset: 0x000AD6DC
	public void SetYaw(float yaw)
	{
		this.TileEntity.CenteredYaw = yaw;
	}

	// Token: 0x06001D1C RID: 7452 RVA: 0x000AF4EA File Offset: 0x000AD6EA
	public float GetPitch()
	{
		return this.TileEntity.CenteredPitch;
	}

	// Token: 0x06001D1D RID: 7453 RVA: 0x000AF4F7 File Offset: 0x000AD6F7
	public float GetYaw()
	{
		return this.TileEntity.CenteredYaw;
	}

	// Token: 0x06001D1E RID: 7454 RVA: 0x000AF504 File Offset: 0x000AD704
	public Transform GetCameraTransform()
	{
		return this.Cone;
	}

	// Token: 0x06001D1F RID: 7455 RVA: 0x000AF50C File Offset: 0x000AD70C
	public void SetUserAccessing(int userAccessingId)
	{
		this.UserAccessingId = userAccessingId;
	}

	// Token: 0x06001D20 RID: 7456 RVA: 0x000AF515 File Offset: 0x000AD715
	public void Cleanup()
	{
		if (this.Cone != null && WireManager.HasInstance)
		{
			WireManager.Instance.RemovePulseObject(this.Cone.gameObject);
		}
	}

	// Token: 0x06001D21 RID: 7457 RVA: 0x000AF542 File Offset: 0x000AD742
	public void SetConeColor(Color _color)
	{
		if (this.ConeMaterial != null)
		{
			this.ConeMaterial.SetColor("_Color", _color);
		}
	}

	// Token: 0x06001D22 RID: 7458 RVA: 0x000AF563 File Offset: 0x000AD763
	public Color GetOriginalConeColor()
	{
		return this.ConeColor;
	}

	// Token: 0x06001D23 RID: 7459 RVA: 0x000AF48C File Offset: 0x000AD68C
	public void SetConeActive(bool _active)
	{
		if (this.Cone != null)
		{
			this.Cone.gameObject.SetActive(_active);
		}
	}

	// Token: 0x06001D24 RID: 7460 RVA: 0x000AF56B File Offset: 0x000AD76B
	public bool GetConeActive()
	{
		return this.Cone != null && this.Cone.gameObject.activeSelf;
	}

	// Token: 0x06001D25 RID: 7461 RVA: 0x000AF58D File Offset: 0x000AD78D
	public bool HasCone()
	{
		return this.Cone != null;
	}

	// Token: 0x06001D26 RID: 7462 RVA: 0x000AF59B File Offset: 0x000AD79B
	public bool HasLaser()
	{
		return this.Laser != null;
	}

	// Token: 0x06001D27 RID: 7463 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetLaserColor(Color _color)
	{
	}

	// Token: 0x06001D28 RID: 7464 RVA: 0x000AF5A9 File Offset: 0x000AD7A9
	public Color GetOriginalLaserColor()
	{
		return Color.black;
	}

	// Token: 0x06001D29 RID: 7465 RVA: 0x000AF4AD File Offset: 0x000AD6AD
	public void SetLaserActive(bool _active)
	{
		if (this.Laser != null)
		{
			this.Laser.gameObject.SetActive(_active);
		}
	}

	// Token: 0x06001D2A RID: 7466 RVA: 0x000AF5B0 File Offset: 0x000AD7B0
	public bool GetLaserActive()
	{
		return this.Laser != null && this.Laser.gameObject.activeSelf;
	}

	// Token: 0x040012FC RID: 4860
	public AutoTurretYawLerp YawController;

	// Token: 0x040012FD RID: 4861
	public AutoTurretPitchLerp PitchController;

	// Token: 0x040012FE RID: 4862
	public AutoTurretFireController FireController;

	// Token: 0x040012FF RID: 4863
	public Transform Laser;

	// Token: 0x04001300 RID: 4864
	public Transform Cone;

	// Token: 0x04001301 RID: 4865
	public Material ConeMaterial;

	// Token: 0x04001302 RID: 4866
	public Color ConeColor;

	// Token: 0x04001303 RID: 4867
	public bool IsOn;

	// Token: 0x04001305 RID: 4869
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public TileEntityPoweredRangedTrap tileEntity;
}
