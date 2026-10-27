using System;
using UnityEngine;

// Token: 0x020003DF RID: 991
public class SpotlightController : MonoBehaviour, IPowerSystemCamera
{
	// Token: 0x17000395 RID: 917
	// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x000B5F8D File Offset: 0x000B418D
	// (set) Token: 0x06001DD7 RID: 7639 RVA: 0x000B5F95 File Offset: 0x000B4195
	public int UserAccessingId
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = -1;

	// Token: 0x06001DD8 RID: 7640 RVA: 0x000B5FA0 File Offset: 0x000B41A0
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

	// Token: 0x06001DD9 RID: 7641 RVA: 0x000B6058 File Offset: 0x000B4258
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

	// Token: 0x06001DDA RID: 7642 RVA: 0x000B6258 File Offset: 0x000B4458
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

	// Token: 0x06001DDB RID: 7643 RVA: 0x000B62F1 File Offset: 0x000B44F1
	public void SetPitch(float pitch)
	{
		this.TileEntity.CenteredPitch = pitch;
	}

	// Token: 0x06001DDC RID: 7644 RVA: 0x000B62FF File Offset: 0x000B44FF
	public void SetYaw(float yaw)
	{
		this.TileEntity.CenteredYaw = yaw;
	}

	// Token: 0x06001DDD RID: 7645 RVA: 0x000B630D File Offset: 0x000B450D
	public float GetPitch()
	{
		return this.TileEntity.CenteredPitch;
	}

	// Token: 0x06001DDE RID: 7646 RVA: 0x000B631A File Offset: 0x000B451A
	public float GetYaw()
	{
		return this.TileEntity.CenteredYaw;
	}

	// Token: 0x06001DDF RID: 7647 RVA: 0x000200DA File Offset: 0x0001E2DA
	public Transform GetCameraTransform()
	{
		return null;
	}

	// Token: 0x06001DE0 RID: 7648 RVA: 0x000B6327 File Offset: 0x000B4527
	public void SetUserAccessing(int userAccessingId)
	{
		this.UserAccessingId = userAccessingId;
	}

	// Token: 0x06001DE1 RID: 7649 RVA: 0x000B6330 File Offset: 0x000B4530
	public void SetConeColor(Color _color)
	{
		if (this.ConeMaterial != null)
		{
			this.ConeMaterial.SetColor("_Color", _color);
		}
	}

	// Token: 0x06001DE2 RID: 7650 RVA: 0x000B6351 File Offset: 0x000B4551
	public Color GetOriginalConeColor()
	{
		return this.ConeColor;
	}

	// Token: 0x06001DE3 RID: 7651 RVA: 0x000B6359 File Offset: 0x000B4559
	public void SetConeActive(bool _active)
	{
		if (this.Cone != null)
		{
			this.Cone.gameObject.SetActive(_active);
		}
	}

	// Token: 0x06001DE4 RID: 7652 RVA: 0x000B637A File Offset: 0x000B457A
	public bool GetConeActive()
	{
		return this.Cone != null && this.Cone.gameObject.activeSelf;
	}

	// Token: 0x06001DE5 RID: 7653 RVA: 0x000B639C File Offset: 0x000B459C
	public bool HasCone()
	{
		return this.Cone != null;
	}

	// Token: 0x06001DE6 RID: 7654 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool HasLaser()
	{
		return false;
	}

	// Token: 0x06001DE7 RID: 7655 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetLaserColor(Color _color)
	{
	}

	// Token: 0x06001DE8 RID: 7656 RVA: 0x000AF7B9 File Offset: 0x000AD9B9
	public Color GetOriginalLaserColor()
	{
		return Color.black;
	}

	// Token: 0x06001DE9 RID: 7657 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetLaserActive(bool _active)
	{
	}

	// Token: 0x06001DEA RID: 7658 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool GetLaserActive()
	{
		return false;
	}

	// Token: 0x040013FA RID: 5114
	public AutoTurretYawLerp YawController;

	// Token: 0x040013FB RID: 5115
	public AutoTurretPitchLerp PitchController;

	// Token: 0x040013FC RID: 5116
	public LightLOD LightScript;

	// Token: 0x040013FD RID: 5117
	public Transform Cone;

	// Token: 0x040013FE RID: 5118
	public Material ConeMaterial;

	// Token: 0x040013FF RID: 5119
	public Color ConeColor;

	// Token: 0x04001400 RID: 5120
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float degreesPerSecond = 11.25f;

	// Token: 0x04001401 RID: 5121
	public bool IsOn;

	// Token: 0x04001402 RID: 5122
	public TileEntityPowered TileEntity;

	// Token: 0x04001404 RID: 5124
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool initialized;
}
