using System;
using UnityEngine;

// Token: 0x020003C4 RID: 964
public class AutoTurretController : MonoBehaviour, IPowerSystemCamera
{
	// Token: 0x1700037F RID: 895
	// (get) Token: 0x06001CFB RID: 7419 RVA: 0x000AE981 File Offset: 0x000ACB81
	// (set) Token: 0x06001CFC RID: 7420 RVA: 0x000AE989 File Offset: 0x000ACB89
	public int UserAccessingId
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = -1;

	// Token: 0x17000380 RID: 896
	// (get) Token: 0x06001CFD RID: 7421 RVA: 0x000AE992 File Offset: 0x000ACB92
	public bool IsTurning
	{
		get
		{
			return this.YawController.IsTurning || this.PitchController.IsTurning;
		}
	}

	// Token: 0x17000381 RID: 897
	// (get) Token: 0x06001CFE RID: 7422 RVA: 0x000AE9AE File Offset: 0x000ACBAE
	// (set) Token: 0x06001CFF RID: 7423 RVA: 0x000AE9B6 File Offset: 0x000ACBB6
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

	// Token: 0x06001D00 RID: 7424 RVA: 0x000AE9CB File Offset: 0x000ACBCB
	public void OnDestroy()
	{
		this.Cleanup();
		if (this.ConeMaterial != null)
		{
			global::UnityEngine.Object.Destroy(this.ConeMaterial);
		}
	}

	// Token: 0x06001D01 RID: 7425 RVA: 0x000AE9EC File Offset: 0x000ACBEC
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

	// Token: 0x06001D02 RID: 7426 RVA: 0x000AEAE4 File Offset: 0x000ACCE4
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

	// Token: 0x06001D03 RID: 7427 RVA: 0x000AEB40 File Offset: 0x000ACD40
	public void SetConeVisible(bool visible)
	{
		if (this.Cone != null)
		{
			this.Cone.gameObject.SetActive(visible);
		}
	}

	// Token: 0x06001D04 RID: 7428 RVA: 0x000AEB61 File Offset: 0x000ACD61
	public void SetLaserVisible(bool visible)
	{
		if (this.Laser != null)
		{
			this.Laser.gameObject.SetActive(visible);
		}
	}

	// Token: 0x06001D05 RID: 7429 RVA: 0x000AEB82 File Offset: 0x000ACD82
	public void SetPitch(float pitch)
	{
		this.TileEntity.CenteredPitch = pitch;
	}

	// Token: 0x06001D06 RID: 7430 RVA: 0x000AEB90 File Offset: 0x000ACD90
	public void SetYaw(float yaw)
	{
		this.TileEntity.CenteredYaw = yaw;
	}

	// Token: 0x06001D07 RID: 7431 RVA: 0x000AEB9E File Offset: 0x000ACD9E
	public float GetPitch()
	{
		return this.TileEntity.CenteredPitch;
	}

	// Token: 0x06001D08 RID: 7432 RVA: 0x000AEBAB File Offset: 0x000ACDAB
	public float GetYaw()
	{
		return this.TileEntity.CenteredYaw;
	}

	// Token: 0x06001D09 RID: 7433 RVA: 0x000AEBB8 File Offset: 0x000ACDB8
	public Transform GetCameraTransform()
	{
		return this.Cone;
	}

	// Token: 0x06001D0A RID: 7434 RVA: 0x000AEBC0 File Offset: 0x000ACDC0
	public void SetUserAccessing(int userAccessingId)
	{
		this.UserAccessingId = userAccessingId;
	}

	// Token: 0x06001D0B RID: 7435 RVA: 0x000AEBC9 File Offset: 0x000ACDC9
	public void Cleanup()
	{
		if (this.Cone != null && WireManager.HasInstance)
		{
			WireManager.Instance.RemovePulseObject(this.Cone.gameObject);
		}
	}

	// Token: 0x06001D0C RID: 7436 RVA: 0x000AEBF6 File Offset: 0x000ACDF6
	public void SetConeColor(Color _color)
	{
		if (this.ConeMaterial != null)
		{
			this.ConeMaterial.SetColor("_Color", _color);
		}
	}

	// Token: 0x06001D0D RID: 7437 RVA: 0x000AEC17 File Offset: 0x000ACE17
	public Color GetOriginalConeColor()
	{
		return this.ConeColor;
	}

	// Token: 0x06001D0E RID: 7438 RVA: 0x000AEB40 File Offset: 0x000ACD40
	public void SetConeActive(bool _active)
	{
		if (this.Cone != null)
		{
			this.Cone.gameObject.SetActive(_active);
		}
	}

	// Token: 0x06001D0F RID: 7439 RVA: 0x000AEC1F File Offset: 0x000ACE1F
	public bool GetConeActive()
	{
		return this.Cone != null && this.Cone.gameObject.activeSelf;
	}

	// Token: 0x06001D10 RID: 7440 RVA: 0x000AEC41 File Offset: 0x000ACE41
	public bool HasCone()
	{
		return this.Cone != null;
	}

	// Token: 0x06001D11 RID: 7441 RVA: 0x000AEC4F File Offset: 0x000ACE4F
	public bool HasLaser()
	{
		return this.Laser != null;
	}

	// Token: 0x06001D12 RID: 7442 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetLaserColor(Color _color)
	{
	}

	// Token: 0x06001D13 RID: 7443 RVA: 0x000AEC5D File Offset: 0x000ACE5D
	public Color GetOriginalLaserColor()
	{
		return Color.black;
	}

	// Token: 0x06001D14 RID: 7444 RVA: 0x000AEB61 File Offset: 0x000ACD61
	public void SetLaserActive(bool _active)
	{
		if (this.Laser != null)
		{
			this.Laser.gameObject.SetActive(_active);
		}
	}

	// Token: 0x06001D15 RID: 7445 RVA: 0x000AEC64 File Offset: 0x000ACE64
	public bool GetLaserActive()
	{
		return this.Laser != null && this.Laser.gameObject.activeSelf;
	}

	// Token: 0x040012EE RID: 4846
	public AutoTurretYawLerp YawController;

	// Token: 0x040012EF RID: 4847
	public AutoTurretPitchLerp PitchController;

	// Token: 0x040012F0 RID: 4848
	public AutoTurretFireController FireController;

	// Token: 0x040012F1 RID: 4849
	public Transform Laser;

	// Token: 0x040012F2 RID: 4850
	public Transform Cone;

	// Token: 0x040012F3 RID: 4851
	public Material ConeMaterial;

	// Token: 0x040012F4 RID: 4852
	public Color ConeColor;

	// Token: 0x040012F5 RID: 4853
	public bool IsOn;

	// Token: 0x040012F7 RID: 4855
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public TileEntityPoweredRangedTrap tileEntity;
}
