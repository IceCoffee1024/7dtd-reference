using System;
using UnityEngine;

// Token: 0x020003DA RID: 986
public class SpotlightController : MonoBehaviour, IPowerSystemCamera
{
	// Token: 0x1700039D RID: 925
	// (get) Token: 0x06001DDA RID: 7642 RVA: 0x000B5B71 File Offset: 0x000B3D71
	// (set) Token: 0x06001DDB RID: 7643 RVA: 0x000B5B79 File Offset: 0x000B3D79
	public int UserAccessingId
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = -1;

	// Token: 0x06001DDC RID: 7644 RVA: 0x000B5B84 File Offset: 0x000B3D84
	public void Init(DynamicProperties _properties)
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
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
			this.Cone.gameObject.SetActive(false);
		}
	}

	// Token: 0x06001DDD RID: 7645 RVA: 0x000B5C3C File Offset: 0x000B3E3C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.TileEntity == null)
		{
			return;
		}
		if (this.TileEntity.IsPowered && this.UserAccessingId == -1)
		{
			if (this.TileEntity.IsPowered)
			{
				if (this.YawController.Yaw != this.TileEntity.CenteredYaw)
				{
					this.YawController.Yaw = Mathf.Lerp(this.YawController.Yaw, this.TileEntity.CenteredYaw, Time.deltaTime * this.degreesPerSecond);
					this.YawController.UpdateYaw();
				}
				if (this.PitchController.Pitch != this.TileEntity.CenteredPitch)
				{
					this.PitchController.Pitch = Mathf.Lerp(this.PitchController.Pitch, this.TileEntity.CenteredPitch, Time.deltaTime * this.degreesPerSecond);
					this.PitchController.UpdatePitch();
				}
			}
			this.IsOn &= this.TileEntity.IsPowered;
			if (this.LightScript.bSwitchedOn != this.IsOn)
			{
				this.UpdateEmissionColor(this.IsOn);
				this.LightScript.bSwitchedOn = this.IsOn;
			}
			return;
		}
		if (this.UserAccessingId != -1)
		{
			this.YawController.Yaw = this.TileEntity.CenteredYaw;
			this.YawController.UpdateYaw();
			this.PitchController.Pitch = this.TileEntity.CenteredPitch;
			this.PitchController.UpdatePitch();
			return;
		}
		if (!this.TileEntity.IsPowered)
		{
			if (this.YawController.Yaw != this.TileEntity.CenteredYaw)
			{
				this.YawController.Yaw = this.TileEntity.CenteredYaw;
				this.YawController.SetYaw();
			}
			if (this.PitchController.Pitch != this.TileEntity.CenteredPitch)
			{
				this.PitchController.Pitch = this.TileEntity.CenteredPitch;
				this.PitchController.SetPitch();
			}
		}
	}

	// Token: 0x06001DDE RID: 7646 RVA: 0x000B5E3C File Offset: 0x000B403C
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateEmissionColor(bool isPowered)
	{
		Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>();
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].material != componentsInChildren[i].sharedMaterial)
				{
					componentsInChildren[i].material = new Material(componentsInChildren[i].sharedMaterial);
				}
				if (isPowered)
				{
					componentsInChildren[i].material.SetColor("_EmissionColor", Color.white);
				}
				else
				{
					componentsInChildren[i].material.SetColor("_EmissionColor", Color.black);
				}
				componentsInChildren[i].sharedMaterial = componentsInChildren[i].material;
			}
		}
	}

	// Token: 0x06001DDF RID: 7647 RVA: 0x000B5ED5 File Offset: 0x000B40D5
	public void SetPitch(float pitch)
	{
		this.TileEntity.CenteredPitch = pitch;
	}

	// Token: 0x06001DE0 RID: 7648 RVA: 0x000B5EE3 File Offset: 0x000B40E3
	public void SetYaw(float yaw)
	{
		this.TileEntity.CenteredYaw = yaw;
	}

	// Token: 0x06001DE1 RID: 7649 RVA: 0x000B5EF1 File Offset: 0x000B40F1
	public float GetPitch()
	{
		return this.TileEntity.CenteredPitch;
	}

	// Token: 0x06001DE2 RID: 7650 RVA: 0x000B5EFE File Offset: 0x000B40FE
	public float GetYaw()
	{
		return this.TileEntity.CenteredYaw;
	}

	// Token: 0x06001DE3 RID: 7651 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public Transform GetCameraTransform()
	{
		return null;
	}

	// Token: 0x06001DE4 RID: 7652 RVA: 0x000B5F0B File Offset: 0x000B410B
	public void SetUserAccessing(int userAccessingId)
	{
		this.UserAccessingId = userAccessingId;
	}

	// Token: 0x06001DE5 RID: 7653 RVA: 0x000B5F14 File Offset: 0x000B4114
	public void SetConeColor(Color _color)
	{
		if (this.ConeMaterial != null)
		{
			this.ConeMaterial.SetColor("_Color", _color);
		}
	}

	// Token: 0x06001DE6 RID: 7654 RVA: 0x000B5F35 File Offset: 0x000B4135
	public Color GetOriginalConeColor()
	{
		return this.ConeColor;
	}

	// Token: 0x06001DE7 RID: 7655 RVA: 0x000B5F3D File Offset: 0x000B413D
	public void SetConeActive(bool _active)
	{
		if (this.Cone != null)
		{
			this.Cone.gameObject.SetActive(_active);
		}
	}

	// Token: 0x06001DE8 RID: 7656 RVA: 0x000B5F5E File Offset: 0x000B415E
	public bool GetConeActive()
	{
		return this.Cone != null && this.Cone.gameObject.activeSelf;
	}

	// Token: 0x06001DE9 RID: 7657 RVA: 0x000B5F80 File Offset: 0x000B4180
	public bool HasCone()
	{
		return this.Cone != null;
	}

	// Token: 0x06001DEA RID: 7658 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool HasLaser()
	{
		return false;
	}

	// Token: 0x06001DEB RID: 7659 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetLaserColor(Color _color)
	{
	}

	// Token: 0x06001DEC RID: 7660 RVA: 0x000AF39D File Offset: 0x000AD59D
	public Color GetOriginalLaserColor()
	{
		return Color.black;
	}

	// Token: 0x06001DED RID: 7661 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetLaserActive(bool _active)
	{
	}

	// Token: 0x06001DEE RID: 7662 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool GetLaserActive()
	{
		return false;
	}

	// Token: 0x040013F1 RID: 5105
	public AutoTurretYawLerp YawController;

	// Token: 0x040013F2 RID: 5106
	public AutoTurretPitchLerp PitchController;

	// Token: 0x040013F3 RID: 5107
	public LightLOD LightScript;

	// Token: 0x040013F4 RID: 5108
	public Transform Cone;

	// Token: 0x040013F5 RID: 5109
	public Material ConeMaterial;

	// Token: 0x040013F6 RID: 5110
	public Color ConeColor;

	// Token: 0x040013F7 RID: 5111
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float degreesPerSecond = 11.25f;

	// Token: 0x040013F8 RID: 5112
	public bool IsOn;

	// Token: 0x040013F9 RID: 5113
	public TileEntityPowered TileEntity;

	// Token: 0x040013FB RID: 5115
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool initialized;
}
