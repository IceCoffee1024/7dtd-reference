using System;
using UnityEngine;

// Token: 0x020003CD RID: 973
public class AutoTurretController : MonoBehaviour, IPowerSystemCamera
{
	// Token: 0x1700037B RID: 891
	// (get) Token: 0x06001D14 RID: 7444 RVA: 0x000AF4DD File Offset: 0x000AD6DD
	// (set) Token: 0x06001D15 RID: 7445 RVA: 0x000AF4E5 File Offset: 0x000AD6E5
	public int UserAccessingId
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = -1;

	// Token: 0x1700037C RID: 892
	// (get) Token: 0x06001D16 RID: 7446 RVA: 0x000AF4EE File Offset: 0x000AD6EE
	public bool IsTurning
	{
		get
		{
			return this.YawController.IsTurning || this.PitchController.IsTurning;
		}
	}

	// Token: 0x1700037D RID: 893
	// (get) Token: 0x06001D17 RID: 7447 RVA: 0x000AF50A File Offset: 0x000AD70A
	// (set) Token: 0x06001D18 RID: 7448 RVA: 0x000AF512 File Offset: 0x000AD712
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

	// Token: 0x06001D19 RID: 7449 RVA: 0x000AF527 File Offset: 0x000AD727
	public void OnDestroy()
	{
		this.Cleanup();
		if (this.ConeMaterial != null)
		{
			global::UnityEngine.Object.Destroy(this.ConeMaterial);
		}
	}

	// Token: 0x06001D1A RID: 7450 RVA: 0x000AF548 File Offset: 0x000AD748
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

	// Token: 0x06001D1B RID: 7451 RVA: 0x000AF640 File Offset: 0x000AD840
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

	// Token: 0x06001D1C RID: 7452 RVA: 0x000AF69C File Offset: 0x000AD89C
	public void SetConeVisible(bool visible)
	{
		if (this.Cone != null)
		{
			this.Cone.gameObject.SetActive(visible);
		}
	}

	// Token: 0x06001D1D RID: 7453 RVA: 0x000AF6BD File Offset: 0x000AD8BD
	public void SetLaserVisible(bool visible)
	{
		if (this.Laser != null)
		{
			this.Laser.gameObject.SetActive(visible);
		}
	}

	// Token: 0x06001D1E RID: 7454 RVA: 0x000AF6DE File Offset: 0x000AD8DE
	public void SetPitch(float pitch)
	{
		this.TileEntity.CenteredPitch = pitch;
	}

	// Token: 0x06001D1F RID: 7455 RVA: 0x000AF6EC File Offset: 0x000AD8EC
	public void SetYaw(float yaw)
	{
		this.TileEntity.CenteredYaw = yaw;
	}

	// Token: 0x06001D20 RID: 7456 RVA: 0x000AF6FA File Offset: 0x000AD8FA
	public float GetPitch()
	{
		return this.TileEntity.CenteredPitch;
	}

	// Token: 0x06001D21 RID: 7457 RVA: 0x000AF707 File Offset: 0x000AD907
	public float GetYaw()
	{
		return this.TileEntity.CenteredYaw;
	}

	// Token: 0x06001D22 RID: 7458 RVA: 0x000AF714 File Offset: 0x000AD914
	public Transform GetCameraTransform()
	{
		return this.Cone;
	}

	// Token: 0x06001D23 RID: 7459 RVA: 0x000AF71C File Offset: 0x000AD91C
	public void SetUserAccessing(int userAccessingId)
	{
		this.UserAccessingId = userAccessingId;
	}

	// Token: 0x06001D24 RID: 7460 RVA: 0x000AF725 File Offset: 0x000AD925
	public void Cleanup()
	{
		if (this.Cone != null && WireManager.HasInstance)
		{
			WireManager.Instance.RemovePulseObject(this.Cone.gameObject);
		}
	}

	// Token: 0x06001D25 RID: 7461 RVA: 0x000AF752 File Offset: 0x000AD952
	public void SetConeColor(Color _color)
	{
		if (this.ConeMaterial != null)
		{
			this.ConeMaterial.SetColor("_Color", _color);
		}
	}

	// Token: 0x06001D26 RID: 7462 RVA: 0x000AF773 File Offset: 0x000AD973
	public Color GetOriginalConeColor()
	{
		return this.ConeColor;
	}

	// Token: 0x06001D27 RID: 7463 RVA: 0x000AF69C File Offset: 0x000AD89C
	public void SetConeActive(bool _active)
	{
		if (this.Cone != null)
		{
			this.Cone.gameObject.SetActive(_active);
		}
	}

	// Token: 0x06001D28 RID: 7464 RVA: 0x000AF77B File Offset: 0x000AD97B
	public bool GetConeActive()
	{
		return this.Cone != null && this.Cone.gameObject.activeSelf;
	}

	// Token: 0x06001D29 RID: 7465 RVA: 0x000AF79D File Offset: 0x000AD99D
	public bool HasCone()
	{
		return this.Cone != null;
	}

	// Token: 0x06001D2A RID: 7466 RVA: 0x000AF7AB File Offset: 0x000AD9AB
	public bool HasLaser()
	{
		return this.Laser != null;
	}

	// Token: 0x06001D2B RID: 7467 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetLaserColor(Color _color)
	{
	}

	// Token: 0x06001D2C RID: 7468 RVA: 0x000AF7B9 File Offset: 0x000AD9B9
	public Color GetOriginalLaserColor()
	{
		return Color.black;
	}

	// Token: 0x06001D2D RID: 7469 RVA: 0x000AF6BD File Offset: 0x000AD8BD
	public void SetLaserActive(bool _active)
	{
		if (this.Laser != null)
		{
			this.Laser.gameObject.SetActive(_active);
		}
	}

	// Token: 0x06001D2E RID: 7470 RVA: 0x000AF7C0 File Offset: 0x000AD9C0
	public bool GetLaserActive()
	{
		return this.Laser != null && this.Laser.gameObject.activeSelf;
	}

	// Token: 0x0400130C RID: 4876
	public AutoTurretYawLerp YawController;

	// Token: 0x0400130D RID: 4877
	public AutoTurretPitchLerp PitchController;

	// Token: 0x0400130E RID: 4878
	public AutoTurretFireController FireController;

	// Token: 0x0400130F RID: 4879
	public Transform Laser;

	// Token: 0x04001310 RID: 4880
	public Transform Cone;

	// Token: 0x04001311 RID: 4881
	public Material ConeMaterial;

	// Token: 0x04001312 RID: 4882
	public Color ConeColor;

	// Token: 0x04001313 RID: 4883
	public bool IsOn;

	// Token: 0x04001315 RID: 4885
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public TileEntityPoweredRangedTrap tileEntity;
}
